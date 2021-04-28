using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Starfall : MonoBehaviour
{
    public Vector2 Speed = new Vector2(0f, 0f);
    public ParticleSystem TrailParticle;
    public GameObject DeathObject;

    Vector2 _gravity = new Vector2(0f, -0.5f);
    bool _timeIsChanged = false;
    float _timeMultiplier = 1;
    Rigidbody2D _body;

    // Use this for initialization
    void Start()
    {
        AudioHandler.Load("palette_change");

        _body = GetComponent<Rigidbody2D>();

        UpdateVelocity();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVelocity();
    }

    public void UpdateVelocity()
    {
        var finalSpeed = Speed;
        if (_timeIsChanged)
            finalSpeed *= _timeMultiplier;

        _body.velocity = finalSpeed + _gravity;

        if (TrailParticle != null)
            TrailParticle.gravityModifier = finalSpeed.y;
    }

    public void EnterTimeCircle()
    {
        _timeIsChanged = true;
    }

    public void ExitTimeCircle()
    {
        _timeIsChanged = false;
    }

    public void UpdateTime(float time)
    {
        _timeMultiplier = time;
    }

    void Hit()
    {
        if (DeathObject != null)
            Instantiate(DeathObject, transform.position + new Vector3(0, 0, -2), transform.rotation);

        AudioHandler.Play("crash1", transform.position + new Vector3(0, 0, -10));

        gameObject.SetActive(false);

        GameObject.Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsDanger(collision.gameObject))
        {
            HitDanger(collision.gameObject);
            HitDanger(gameObject);
        }
    }

    bool IsDanger(GameObject obj)
    {
        return obj.activeSelf && obj.CompareTag("Danger");
    }

    void HitDanger(GameObject obj)
    {
        obj.SendMessage("Hit", SendMessageOptions.DontRequireReceiver);
    }
}