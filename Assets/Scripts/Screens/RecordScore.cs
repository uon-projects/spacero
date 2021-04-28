using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class RecordScore : MonoBehaviour
{
    Text _text;

    // Use this for initialization
    void Start()
    {
        _text = GetComponent<Text>();

        var score = GameState.GetLevelScore(GameState.CurrentLevel);

        _text.text = score >= 0f ? GameScore.FormatScore(score) : "-";
    }
}