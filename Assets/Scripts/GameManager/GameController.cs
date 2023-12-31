using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private LevelProgressController _levelProgressController;

    public void SaveProgress()
    {
        SaveSystem.SaveProgress(_levelProgressController);
    }

    public void LoadProgress()
    {
        ProgressData data = SaveSystem.LoadProgress();
        _levelProgressController.LevelIndex = data.LevelIndex;

        if (!data.IsCompletedScreen)
        {
            Vector2 spawnPoint = new(data.SpawnPoint[0], data.SpawnPoint[1]);
            _levelProgressController.SpawnPoint = spawnPoint;
        }

        ScenesController.Instance.LoadSceneByIndex(data.LevelIndex);
    }
}