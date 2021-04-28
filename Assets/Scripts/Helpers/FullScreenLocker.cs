using UnityEngine;
using System;
using System.Collections;

public class FullScreenLockerHelper : MonoBehaviour
{
    private const string RESOLUTION_WIDTH_KEY = "ResWidth";
    private const string RESOLUTION_HEIGHT_KEY = "ResHeight";
    private const int DEFAULT_WIDTH = 480;
    private const int DEFAULT_HEIGHT = 432;

    //int _lastWidth = 0;
    //int _lastHeight = 0;
    int _desiredWidth = DEFAULT_WIDTH;
    int _desiredHeight = DEFAULT_HEIGHT;

    // Use this for initialization
    void Start()
    {
    }

    void Awake()
    {
        SetupResolution();
        DontDestroyOnLoad(gameObject);
    }

    void SetupResolution()
    {
        SetDesiredResolution(
            PlayerPrefs.GetInt(RESOLUTION_WIDTH_KEY, DEFAULT_WIDTH),
            PlayerPrefs.GetInt(RESOLUTION_HEIGHT_KEY, DEFAULT_HEIGHT)
        );
    }

    void SetDesiredResolution(int width, int height)
    {
        //_lastWidth = 0;
        //_lastHeight = 0;
        _desiredWidth = width;
        _desiredHeight = height;
    }

    // Update is called once per frame
    void Update()
    {
        if (FullScreenLocker.ControlEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                SetDesiredResolution(160, 144);

            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                SetDesiredResolution(320, 288);

            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
                SetDesiredResolution(480, 432);

            if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
                SetDesiredResolution(640, 576);

            if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
                SetDesiredResolution(800, 720);

            if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
                SetDesiredResolution(960, 864);

            if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
                SetDesiredResolution(1120, 1008);
        }

        //if (_lastWidth == Screen.width && _lastHeight == Screen.height)
        //	return;

        if (Screen.width != _desiredWidth || Screen.height != _desiredHeight)
        {
            var newWidth = _desiredWidth;
            var newHeight = _desiredHeight;

            /*
            if (Screen.height < Screen.width)
                newWidth = Mathf.CeilToInt((float)_desiredHeight * ((float)Screen.width / (float)Screen.height));
            else
                newHeight = Mathf.CeilToInt((float)_desiredWidth * ((float)Screen.height / (float)Screen.width));
            */

            PlayerPrefs.SetInt(RESOLUTION_WIDTH_KEY, newWidth);
            PlayerPrefs.SetInt(RESOLUTION_HEIGHT_KEY, newHeight);
            PlayerPrefs.Save();

            Screen.SetResolution(newWidth, newHeight, Screen.fullScreen);
        }

        //_lastWidth = Screen.width;
        //_lastHeight = Screen.height;
    }
}


static internal class FullScreenLocker
{
    //Disable the unused variable warning
#pragma warning disable 0414
    static private FullScreenLockerHelper _fullScreenLockerHelper =
        (new GameObject("FullScreenLockerHelper")).AddComponent<FullScreenLockerHelper>();
#pragma warning restore 0414

    public static bool ControlEnabled = false;
}