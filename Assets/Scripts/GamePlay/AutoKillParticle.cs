using UnityEngine;
using System.Collections;

public class AutoKillParticle : MonoBehaviour
{
    ParticleSystem _audioSource;

    // Use this for initialization
    void Start()
    {
        _audioSource = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_audioSource != null && _audioSource.isStopped)
            GameObject.Destroy(gameObject);
    }
}