using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [HideInInspector] public static CheckpointManager Instance;

    [SerializeField] private Transform _spawnPoint;

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

    public void SetSpawnPoint(Vector2 position)
    {
        _spawnPoint.position = position;
    }

    public Vector2 SpawnPoint => _spawnPoint.position;
}