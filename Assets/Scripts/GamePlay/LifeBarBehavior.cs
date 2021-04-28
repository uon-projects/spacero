using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LifeBarBehavior : MonoBehaviour
{
    public ShipBehavior Ship;
    public Sprite[] Frames;

    SpriteRenderer _spriteRenderer;
    Image _image;
    int _lastLife = -1;

    // Use this for initialization
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _image = GetComponent<Image>();

        UpdateFrame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Ship.Life == _lastLife)
            return;

        _lastLife = Ship.Life;

        UpdateFrame();
    }

    void UpdateFrame()
    {
        var index = Mathf.RoundToInt(((float) Ship.Life / (float) Ship.MaxLife) * (Frames.Length - 1));

        if (index >= Frames.Length)
            return;

        if (_spriteRenderer != null)
            _spriteRenderer.sprite = Frames[index];

        if (_image != null)
            _image.sprite = Frames[index];
    }
}