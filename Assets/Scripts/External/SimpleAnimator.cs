// source http://www.gamasutra.com/blogs/JoeStrout/20150807/250646/2D_Animation_Methods_in_Unity.php

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class SimpleAnimator : MonoBehaviour
{
    #region Public Properties

    [System.Serializable]
    public class Anim
    {
        public string name;
        public Sprite[] frames;
        public float framesPerSec = 5;
        public bool loop = true;
        public bool pingPong = false;

        public float duration
        {
            get { return frames.Length * framesPerSec; }
            set { framesPerSec = value / frames.Length; }
        }
    }

    public List<Anim> animations = new List<Anim>();

    [HideInInspector] public int currentFrame;

    [HideInInspector]
    public bool done
    {
        get { return current.pingPong ? currentFrame <= 0 : currentFrame >= current.frames.Length; }
    }

    [HideInInspector]
    public bool playing
    {
        get { return _playing; }
    }

    #endregion

    //--------------------------------------------------------------------------------

    #region Private Properties

    SpriteRenderer spriteRenderer;
    Image uiImage;
    Anim current;
    bool _playing;
    bool _decreasing;
    float secsPerFrame;
    float nextFrameTime;

    #endregion

    //--------------------------------------------------------------------------------

    #region Editor Support

    [ContextMenu("Sort All Frames by Name")]
    void DoSort()
    {
        foreach (Anim anim in animations)
        {
            System.Array.Sort(anim.frames, (a, b) => a.name.CompareTo(b.name));
        }

        Debug.Log(gameObject.name + " animation frames have been sorted alphabetically.");
    }

    #endregion

    //--------------------------------------------------------------------------------

    #region MonoBehaviour Events

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            uiImage = GetComponentInChildren<Image>();
            if (uiImage == null)
            {
                Debug.Log(gameObject.name + ": Couldn't find SpriteRenderer or Image");
            }
        }

        if (animations.Count > 0) PlayByIndex(0);
    }

    void Update()
    {
        if (!_playing || Time.time < nextFrameTime) return;
        if (_decreasing)
            currentFrame--;
        else
            currentFrame++;
        if (currentFrame >= current.frames.Length)
        {
            if (current.pingPong)
            {
                _decreasing = true;
                currentFrame = current.frames.Length - 2;
            }
            else
            {
                if (!current.loop)
                {
                    _playing = false;
                    return;
                }

                currentFrame = 0;
            }
        }
        else if (currentFrame < 0)
        {
            if (current.pingPong)
            {
                if (current.loop)
                {
                    _decreasing = false;
                    currentFrame = 1;
                }
                else
                {
                    _playing = false;
                    return;
                }
            }
            else
            {
                _playing = false;
                return;
            }
        }

        UpdateFrame();
        nextFrameTime += secsPerFrame;
    }

    void UpdateFrame()
    {
        if (spriteRenderer != null)
            spriteRenderer.sprite = current.frames[currentFrame];
        if (uiImage != null)
            uiImage.sprite = current.frames[currentFrame];
    }

    #endregion

    //--------------------------------------------------------------------------------

    #region Public Methods

    public void Play(string name)
    {
        int index = animations.FindIndex(a => a.name == name);
        if (index < 0)
        {
            Debug.LogError(gameObject + ": No such animation: " + name);
        }
        else
        {
            PlayByIndex(index);
        }
    }

    public void PlayByIndex(int index)
    {
        if (index < 0) return;
        Anim anim = animations[index];

        current = anim;

        secsPerFrame = 1f / anim.framesPerSec;
        currentFrame = -1;
        _playing = true;
        _decreasing = false;
        nextFrameTime = Time.time;
    }

    public void Stop()
    {
        _playing = false;
    }

    public void Resume()
    {
        _playing = true;
        nextFrameTime = Time.time + secsPerFrame;
    }

    #endregion
}