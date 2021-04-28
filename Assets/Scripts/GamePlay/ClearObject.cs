using UnityEngine;
using System.Collections;

public class ClearObject : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject.Destroy(collision.gameObject);
    }
}