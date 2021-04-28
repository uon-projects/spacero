using UnityEngine;
using System.Collections;

public class ShowOnNewRecord : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        this.gameObject.SetActive(GameState.IsNewRecord);
    }
}