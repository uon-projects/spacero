using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GoToTitleBehavior : MonoBehaviour
{
    public void GoToTitle()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}