using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverBehavior : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void Exit()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}