using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainGameBehavior : MonoBehaviour
{
    public Transform ShakeTarget;
    public float ShakeDuration = 0.4f;
    public float ShakeMagnitude = 0.25f;

    bool _isShaking = false;
    float _shakeTime = 0f;

    void Start()
    {
        var musicNumber = Random.Range(1, 4);
        AudioHandler.PlayMusic("bgm" + musicNumber.ToString());
    }

    public void FinishGame()
    {
        AudioHandler.StopMusic();
        StartCoroutine(GoToGameOverScreen());
    }

    IEnumerator GoToGameOverScreen()
    {
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("GameOver");
    }

    public void StartShake()
    {
        if (_isShaking)
        {
            _shakeTime = 0.0f;
            return;
        }

        if (ShakeTarget != null)
            StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        _isShaking = true;
        _shakeTime = 0.0f;

        Vector3 originalCamPos = ShakeTarget.position;

        while (_shakeTime < ShakeDuration)
        {
            _shakeTime += Time.deltaTime;

            float percentComplete = _shakeTime / ShakeDuration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            // map value to [-1, 1]
            float x = Random.value * 2.0f - 1.0f;
            float y = Random.value * 2.0f - 1.0f;
            x *= ShakeMagnitude * damper;
            y *= ShakeMagnitude * damper;

            ShakeTarget.position = new Vector3(originalCamPos.x + x, originalCamPos.y + y, originalCamPos.z);

            yield return null;
        }

        ShakeTarget.position = originalCamPos;
        _isShaking = false;
    }
}