using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreenBehavior : MonoBehaviour
{
    void Start()
    {
        AudioHandler.PlayMusic("bgm_title");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void Help()
    {
        SceneManager.LoadScene("HelpScreen");
    }

    public void About()
    {
        SceneManager.LoadScene("AboutScreen");
    }
}