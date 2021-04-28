using UnityEngine;
using System.Collections;
using CC;
using UnityEngine.SceneManagement;

public class SplashLogo : MonoBehaviour
{
    public SpriteRenderer target;
    public float distance = 5.5f;
    public Sprite[] spriteChanges;

    int _changeIndex;

    // Use this for initialization
    void Start()
    {
        _changeIndex = -1;

        FullScreenLocker.ControlEnabled = true;

        ResetPosition();

        StartCoroutine(Animate());
    }

    // Update is called once per frame
    void ResetPosition()
    {
        this.transform.position = new Vector3(0, distance, 0);
    }

    IEnumerator Animate()
    {
        yield return this.RunSequence(
            new DelayTime(0.1f),
            new MoveBy(0.2f, new Vector3(0, -distance, 0)),
            new DelayTime(1.55f),
            new MoveBy(0.2f, new Vector3(0, -distance, 0))
        );

        _changeIndex++;

        if (target != null && _changeIndex + 1 <= spriteChanges.Length)
        {
            ResetPosition();
            target.sprite = spriteChanges[_changeIndex];

            yield return StartCoroutine(Animate());
        }
        else
        {
            SceneManager.LoadScene("TitleScreen");
        }
    }
}