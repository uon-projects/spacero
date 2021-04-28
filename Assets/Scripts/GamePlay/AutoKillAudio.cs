using UnityEngine;
using System.Collections;

public class AutoKillAudio : MonoBehaviour
{
    AudioSource _audioSource;

    // Use this for initialization
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_audioSource != null && !_audioSource.isPlaying)
            GameObject.Destroy(gameObject);
    }
}