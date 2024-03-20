using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProgressData
{
    private int _levelIndex;
    private float[] _spawnPoint;
    private bool _isCompletedScreen;
    private int _score;

    public ProgressData(LevelProgressController levelProgressController)
    {
        _levelIndex = levelProgressController.LevelIndex;

        _spawnPoint = new float[2];
        _spawnPoint[0] = levelProgressController.SpawnPoint.x;
        _spawnPoint[1] = levelProgressController.SpawnPoint.y;

        _isCompletedScreen = levelProgressController.IsCompleteScreen;
        _score = levelProgressController.Score;
    }

    public int LevelIndex => _levelIndex;
    public float[] SpawnPoint => _spawnPoint;
    public bool IsCompletedScreen => _isCompletedScreen;
    public int Score => _score;
}