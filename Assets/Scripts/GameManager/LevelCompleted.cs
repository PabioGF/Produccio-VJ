using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleted : MonoBehaviour
{
    private bool _hasEntered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasEntered || !collision.CompareTag("Player")) return;

        _hasEntered = true;

        UIController.Instance.LevelCompleted();

        Debug.Log("Level complete previous index: " + ScenesController.Instance.CurrentLevelIndex);

        ScenesController.Instance.CurrentLevelIndex += 1;

        Debug.Log("Level complete after index: " + ScenesController.Instance.CurrentLevelIndex);

        LevelProgressController.Instance.LevelIndex += 1;
        LevelProgressController.Instance.IsCompleteScreen = true;
        LevelProgressController.Instance.HasSpawnPoint = false;
    }
}