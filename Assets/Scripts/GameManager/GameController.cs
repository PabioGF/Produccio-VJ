using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [HideInInspector] public static GameController Instance;
    [SerializeField] private HitStopController _hitStopController;

    private int _score;

    private void Awake()
    {
        Instance = this;
    }

    #region Data Persistance
    public void SaveProgress()
    {
        SaveSystem.SaveProgress(LevelProgressController.Instance);
    }

    public void LoadProgress()
    {
        ProgressData data = SaveSystem.LoadProgress();
        LevelProgressController.Instance.LevelIndex = data.LevelIndex;
        LevelProgressController.Instance.Score = data.Score;
        Debug.Log("Level Index Loaded: " + data.LevelIndex);

        if (!data.IsCompletedScreen)
        {
            Vector2 spawnPoint = new(data.SpawnPoint[0], data.SpawnPoint[1]);
            LevelProgressController.Instance.SpawnPoint = spawnPoint;
            LevelProgressController.Instance.HasSpawnPoint = true;
            Debug.Log("Spawn Point Loaded: " + spawnPoint);
        }

        ScenesController.Instance.LoadSceneByIndex(data.LevelIndex);
    }
    #endregion

    #region Score
    public void AddScore(int score)
    {
        _score += score;
        UIController.Instance.SetScore(_score);
    }

    public void SetScore(int score)
    {
        _score = score;
        UIController.Instance.SetScore(_score);
    }

    public void SubstractScore(int score)
    {
        _score -= score;
        if (_score < 0) _score = 0;
        UIController.Instance.SetScore(_score);
    }
    #endregion

    public void StopTime(float timeChange, float duration)
    {
        _hitStopController.StopTime(timeChange, duration);
    }

    public int Score { get { return _score; } set { _score = value; } }
}