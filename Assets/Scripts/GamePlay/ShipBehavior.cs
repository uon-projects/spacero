using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipBehavior : MonoBehaviour
{
    public TimeCircleBehavior TimeCircle;
    public GameObject ExplosionObject;
    public GameObject[] BreakObjects;
    public int Life = 3;
    public int MaxLife = 3;
    public float Energy = 0f;
    public float EnergyChargeTimeFactor = 1f / 30f;
    public float MoveForce = 0.1f;
    public float MaxSpeed = 2f;

    public UnityEvent OnHit;
    public UnityEvent OnExplosion;
    public UnityEvent OnDeath;

    Rigidbody2D _body;
    float _maxEnergy = 1f;

    void Awake()
    {
        if (OnHit == null)
            OnHit = new UnityEvent();

        if (OnExplosion == null)
            OnExplosion = new UnityEvent();

        if (OnDeath == null)
            OnDeath = new UnityEvent();
    }

    // Use this for initialization
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var stopped = true;

        if (InputExtensions.Holding.Left)
        {
            stopped = false;
            _body.AddForce(new Vector2(-MoveForce, 0), ForceMode2D.Impulse);
        }

        if (InputExtensions.Holding.Right)
        {
            stopped = false;
            _body.AddForce(new Vector2(MoveForce, 0), ForceMode2D.Impulse);
        }

        if (InputExtensions.Holding.Up)
        {
            stopped = false;
            _body.AddForce(new Vector2(0, MoveForce), ForceMode2D.Impulse);
        }

        if (InputExtensions.Holding.Down)
        {
            stopped = false;
            _body.AddForce(new Vector2(0, -MoveForce), ForceMode2D.Impulse);
        }

        if (stopped)
            _body.AddForce(-(_body.mass * _body.velocity) * 0.25f, ForceMode2D.Impulse);

        _body.velocity = new Vector2(
            Mathf.Clamp(_body.velocity.x, -MaxSpeed, MaxSpeed),
            Mathf.Clamp(_body.velocity.y, -MaxSpeed, MaxSpeed)
        );

        if (InputExtensions.Pressed.A || InputExtensions.Pressed.B)
        {
            Explode();
        }

        var oldEnergyMaxedOut = IsEnergyMaxedOut();
        Energy = Mathf.Clamp(Energy + Time.deltaTime * EnergyChargeTimeFactor, 0, _maxEnergy);
        var newEnergyMaxedOut = IsEnergyMaxedOut();

        if (oldEnergyMaxedOut != newEnergyMaxedOut)
            AudioHandler.Play("charge1");

        if (TimeCircle != null)
        {
            TimeCircle.SetRadius(Energy * 1.5f);
            TimeCircle.SetMaximum(newEnergyMaxedOut);
        }
    }

    bool IsEnergyMaxedOut()
    {
        return Energy >= _maxEnergy;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var hitObject = collision.gameObject;

        if (IsDanger(hitObject))
        {
            Life -= 1;

            OnHit.Invoke();

            if (Life <= 0)
            {
                if (BreakObjects != null)
                {
                    foreach (var breakObject in BreakObjects)
                    {
                        Instantiate(breakObject, transform.position + new Vector3(0, 0, -2), transform.rotation);
                    }
                }

                AudioHandler.Play("crash2");

                gameObject.SetActive(false);
                GameObject.Destroy(gameObject);

                GameScore.running = false;
                GameState.FinishedLevel(GameScore.score);

                OnDeath.Invoke();
            }
        }

        HitDanger(hitObject);
    }

    bool IsDanger(GameObject obj)
    {
        return obj.activeSelf && obj.CompareTag("Danger");
    }

    void HitDanger(GameObject obj)
    {
        obj.SendMessage("Hit", SendMessageOptions.DontRequireReceiver);
    }

    void Explode()
    {
        if (Energy < _maxEnergy)
        {
            return;
        }

        Energy = 0;

        AudioHandler.Play("charge2");

        if (ExplosionObject != null)
            Instantiate(ExplosionObject, transform.position + new Vector3(0, 0, -2), transform.rotation);

        StartCoroutine(DestroyAllDanger());
    }

    IEnumerator DestroyAllDanger()
    {
        yield return new WaitForSeconds(0.2f);

        AudioHandler.Play("explode");

        OnExplosion.Invoke();

        //var contacts = Physics2D.CircleCastAll (transform.position, 5, Vector2.up, 0);
        var contacts = Physics2D.BoxCastAll(new Vector2(0f, 0f), new Vector2(5f, 4.5f), 0, Vector2.up, 0);

        foreach (var contact in contacts)
        {
            var obj = contact.collider.gameObject;

            if (IsDanger(obj))
                GameScore.score += 5000;

            HitDanger(obj);
        }
    }
}