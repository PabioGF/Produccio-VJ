using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [HideInInspector] public static CheckpointManager Instance;

    private Vector2 _spawnPoint = new Vector2(-38, 2);

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
        _spawnPoint = position;
    }

    public Vector2 SpawnPoint => _spawnPoint;
}