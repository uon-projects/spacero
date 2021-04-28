using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(Texture))]
public class FontPointFilter : MonoBehaviour
{
    void Start()
    {
        GetComponent<Text>().font.material.mainTexture.filterMode = FilterMode.Point;
    }

    /*
    void Update() {

        var text = FindObjectOfType<Text> ();
        if (text != null) {
            text.font.material.mainTexture.filterMode = FilterMode.Point;
            this.enabled = false;
        }

    }
    */
}