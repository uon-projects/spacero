using UnityEngine;
using System.Collections;

public class TimeCircleBehavior : MonoBehaviour
{
    public float TimeMultiplier = 0.25f;
    public float TimeChangeFactor = 0.25f;

    ParticleSystem _particleSystem;
    CircleCollider2D _collider;

    // Use this for initialization
    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _collider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        if (InputExtensions.Holding.Down)
            TimeMultiplier -= Time.fixedDeltaTime * TimeChangeFactor;

        if (InputExtensions.Holding.Up)
            TimeMultiplier += Time.fixedDeltaTime * TimeChangeFactor;
        
        TimeMultiplier = Mathf.Clamp (TimeMultiplier, 0.25f, 2.0f);
        */
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var c = other.GetComponent<Starfall>();
        if (c != null)
        {
            c.EnterTimeCircle();
            c.UpdateTime(TimeMultiplier);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        var c = other.GetComponent<Starfall>();
        if (c != null)
        {
            c.EnterTimeCircle();
            c.UpdateTime(TimeMultiplier);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var c = other.GetComponent<Starfall>();
        if (c != null)
        {
            c.ExitTimeCircle();
        }
    }

    public void SetRadius(float radius)
    {
        if (_particleSystem != null)
        {
            var shapeModule = _particleSystem.shape;
            shapeModule.radius = radius;
        }

        if (_collider != null)
        {
            _collider.radius = radius;
        }
    }

    public void SetMaximum(bool isMaxedOut)
    {
        if (isMaxedOut)
        {
            _particleSystem.startSpeed = 0.5f;
            _particleSystem.startSize = 0.06f;
        }
        else
        {
            _particleSystem.startSpeed = -0.5f;
            _particleSystem.startSize = 0.03f;
        }
    }
}