using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public static class GameState
{
    const string MAX_REACHED_LEVEL_KEY = "maxReachedLevel";
    const string LEVEL_SCORE_KEY = "levelScore";

    public static int MaxReachedLevel
    {
        get
        {
            var maxReachedLevel = PlayerPrefs.GetInt(MAX_REACHED_LEVEL_KEY, 1);
            if (maxReachedLevel > MaxLevel)
                maxReachedLevel = MaxLevel;
            return maxReachedLevel;
        }
        set
        {
            var maxReachedLevel = value;
            if (maxReachedLevel > MaxLevel)
                maxReachedLevel = MaxLevel;
            PlayerPrefs.SetInt(MAX_REACHED_LEVEL_KEY, maxReachedLevel);
            PlayerPrefs.Save();
        }
    }

    public const int MaxLevel = 1;
    public static int CurrentLevel = 1;
    public static bool IsNewRecord = false;

    public static void Reset()
    {
        CurrentLevel = 1;
        IsNewRecord = false;
    }

    public static void FinishedLevel(float score)
    {
        var nextLevel = CurrentLevel + 1;
        if (nextLevel <= MaxLevel && nextLevel > MaxReachedLevel)
        {
            MaxReachedLevel = nextLevel;
        }

        IsNewRecord = IsRecord(CurrentLevel, score);
    }

    public static void LoadNextLevel()
    {
        CurrentLevel++;
        LoadLevel();
    }

    public static void LoadLevel(int level)
    {
        CurrentLevel = Mathf.Clamp(level, 1, MaxLevel);
        LoadLevel();
    }

    public static void LoadLevel()
    {
        if (CurrentLevel > MaxLevel)
        {
            Reset();
            SceneManager.LoadScene("GameOver");
            return;
        }

        if (CurrentLevel > MaxReachedLevel)
            MaxReachedLevel = CurrentLevel;

        IsNewRecord = false;

        SceneManager.LoadScene("Level" + CurrentLevel);
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);
    }

    private static string GetLevelScoreKey(int level)
    {
        return LEVEL_SCORE_KEY + level;
    }

    public static float GetLevelScore(int level)
    {
        return PlayerPrefs.GetFloat(GetLevelScoreKey(level), 0f);
    }

    public static float GetAllLevelsScore()
    {
        var totalScore = 0f;
        for (var level = 1; level <= MaxLevel; level++)
        {
            var score = GetLevelScore(level);
            if (score > 0f)
                totalScore += score;
        }

        return totalScore;
    }

    public static void SaveLevelScore(float score)
    {
        SaveLevelScore(CurrentLevel, score);
    }

    public static void SaveLevelScore(int level, float score)
    {
        PlayerPrefs.SetFloat(GetLevelScoreKey(level), score);
        PlayerPrefs.Save();
    }

    public static bool IsRecord(float score)
    {
        return IsRecord(CurrentLevel, score);
    }

    public static bool IsRecord(int level, float score)
    {
        if (score <= 0f)
            return false;

        var levelScore = GetLevelScore(level);
        var isRecord = score > levelScore;

        if (isRecord)
            SaveLevelScore(level, score);

        return isRecord;
    }
}