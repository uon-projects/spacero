using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[RequireComponent(typeof(Text))]
public class GameScore : MonoBehaviour
{
    public static bool running;
    public static float score;

    public static string FormatScore(float score)
    {
        if (score < 0f)
            score = 0f;

        return string.Format("{0:#0}", Mathf.Floor(score));
    }

    public bool autoStart = true;
    public float startDelay = 0f;

    Text _text;
    float _startTimer;

    // Use this for initialization
    void Start()
    {
        _text = GetComponent<Text>();
        _startTimer = 0f;

        Messenger.AddListener("GamePause", Deactivate);
        Messenger.AddListener("GameResume", Activate);

        if (startDelay <= 0f)
        {
            StartGame();
        }
        else
        {
            _text.enabled = false;
        }
    }

    void Deactivate()
    {
        this.enabled = false;
    }

    void Activate()
    {
        this.enabled = true;
    }

    void Awake()
    {
        if (autoStart)
        {
            GameScore.running = true;
            GameScore.score = 0f;
        }
    }

    void StartGame()
    {
        Messenger.Broadcast("GameStart");
        _text.enabled = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_startTimer < startDelay)
        {
            _startTimer += Time.deltaTime;
            if (_startTimer >= startDelay)
            {
                StartGame();
            }

            return;
        }

        if (GameScore.running)
            GameScore.score += Time.fixedDeltaTime * 60f;

        _text.text = GameScore.FormatScore(GameScore.score);
    }
}