using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgressController : MonoBehaviour
{
    [HideInInspector] public static LevelProgressController Instance;

    private Vector2 _spawnPoint;
    private int _levelIndex;
    private bool _isCompletedScreen;
    private bool _hasSpawnPoint;
    private int _score;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!_hasSpawnPoint)
        {
            _score = 0;
            if (GameObject.FindGameObjectWithTag("Spawn") != null)
            {
                _spawnPoint = GameObject.FindGameObjectWithTag("Spawn").transform.position;
                _hasSpawnPoint = true;
            }
        }

        // If it is not the menu scene, sets the current score
        if (!scene.name.Equals("Menu"))
        {
            GameController.Instance.SetScore(_score);
        }

        Debug.Log("Scene " + scene.name + " loaded");
        Debug.Log("Has spawn point: " + _hasSpawnPoint);
        Debug.Log("Score: " + GameController.Instance.Score);
    }

    /// <summary>
    /// Sets the new Spawn Point
    /// </summary>
    /// <param name="position">new spawn point position</param>
    public void SetSpawnPoint(Vector2 position)
    {
        _spawnPoint = position;
        Debug.Log("New Spawn Point: " + position);
    }

    public Vector2 SpawnPoint { get { return _spawnPoint; } set { _spawnPoint = value; } }
    public int LevelIndex { get { return _levelIndex; } set { _levelIndex = value; } }
    public bool IsCompleteScreen { get { return _isCompletedScreen; } set { _isCompletedScreen = value; } }
    public bool HasSpawnPoint { get { return _hasSpawnPoint; } set { _hasSpawnPoint = value; } }
    public int Score { get { return _score; } set { _score = value; } }
}