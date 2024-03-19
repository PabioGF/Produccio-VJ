using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [HideInInspector] public static GameController Instance;
    [SerializeField] private HitStopController _hitStopController;
    private void Awake()
    {
        Instance = this;
    }

    public void SaveProgress()
    {
        SaveSystem.SaveProgress(LevelProgressController.Instance);
    }

    public void LoadProgress()
    {
        ProgressData data = SaveSystem.LoadProgress();
        LevelProgressController.Instance.LevelIndex = data.LevelIndex;
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

    public void StopTime(float timeChange, float duration)
    {
        _hitStopController.StopTime(timeChange, duration);
    }
}