using UnityEngine;
#if UNITY_EDITOR

#endif

public class PaletteSwap : MonoBehaviour
{
    private const string PALETTE_INDEX_KEY = "paletteIndex";

    public bool EnableSwap;
    public bool EnableUp;
    public bool EnableDown = true;

#if UNITY_EDITOR
    private bool _didResetPalette;
#endif
    private Texture2D[] _palettes;

    private Resolutioner _resolutioner;

    public static int PaletteIndex
    {
        get => PlayerPrefs.GetInt(PALETTE_INDEX_KEY, 0);
        set
        {
            PlayerPrefs.SetInt(PALETTE_INDEX_KEY, value);
            PlayerPrefs.Save();
        }
    }

    // Use this for initialization
    private void Start()
    {
#if UNITY_EDITOR
        if (!_didResetPalette)
        {
            ResetPalette();
            _didResetPalette = true;
        }
#endif

        AudioHandler.Load("palette_change");

        _resolutioner = GetComponent<Resolutioner>();

        _palettes = Resources.LoadAll<Texture2D>("Ramps");

        UpdatePalette();

        Messenger.AddListener<bool>("EnablePalleteSwap", SetEnableSwap);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!EnableSwap)
            return;

        if (InputExtensions.Pressed.Up && EnableUp)
            ChangePaletteUp();

        if (InputExtensions.Pressed.Down && EnableDown)
            ChangePaletteDown();
    }

    private void SetEnableSwap(bool enable)
    {
        EnableSwap = enabled;
    }

    public void ChangePaletteUp()
    {
        PaletteIndex++;
        if (PaletteIndex >= _palettes.Length)
            PaletteIndex = 0;
        AudioHandler.Play("palette_change");
        UpdatePalette();
    }

    public void ChangePaletteDown()
    {
        PaletteIndex--;
        if (PaletteIndex < 0)
            PaletteIndex = _palettes.Length - 1;
        AudioHandler.Play("palette_change");
        UpdatePalette();
    }

    private void UpdatePalette()
    {
        if (_resolutioner == null)
            return;
        _resolutioner.postprocessDither.SetPalette(_palettes[PaletteIndex]);
    }

#if UNITY_EDITOR

    private void OnApplicationQuit()
    {
        ResetPalette();
    }

    private void ResetPalette()
    {
        PaletteIndex = 0;
        UpdatePalette();
    }

#endif
}