using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgressController : MonoBehaviour
{
    [HideInInspector] public static LevelProgressController Instance;

    [SerializeField] private Transform _spawnPoint;

    private int _levelIndex;
    private bool _isCompletedScreen;

    void Awake()
    {
        Instance = this;
        _levelIndex = ScenesController.Instance.CurrentLevelIndex;
        _isCompletedScreen = false;
    }

    public void SetSpawnPoint(Vector2 position)
    {
        _spawnPoint.position = position;
    }

    public Vector2 SpawnPoint { get { return _spawnPoint.position; } set { _spawnPoint.position = value; } }
    public int LevelIndex { get { return _levelIndex; } set { _levelIndex = value; } }
    public bool IsCompleteScreen { get { return _isCompletedScreen; } set { _isCompletedScreen = value; } }
}