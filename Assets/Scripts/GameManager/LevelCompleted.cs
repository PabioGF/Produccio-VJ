using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleted : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UIController.Instance.LevelCompleted();
        ScenesController.Instance.CurrentLevelIndex += 1;
        LevelProgressController.Instance.LevelIndex += 1;
        LevelProgressController.Instance.IsCompleteScreen = true;
    }
}