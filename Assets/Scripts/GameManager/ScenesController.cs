using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : MonoBehaviour
{
    [HideInInspector] public static ScenesController Instance;
    private int _currentLevelIndex;

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

    /// <summary>
    /// Reloads the current scene
    /// </summary>
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Loads the next scene in the build hierarchy
    /// </summary>
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;
    }

    /// <summary>
    /// Loads the scene with the given build index
    /// </summary>
    /// <param name="index">Scene build index</param>
    public void LoadSceneByIndex(int index)
    {
        _currentLevelIndex = index;
        LevelProgressController.Instance.LevelIndex = _currentLevelIndex;

        SceneManager.LoadScene(index);
    }

    public int CurrentLevelIndex { get { return _currentLevelIndex; } set { _currentLevelIndex = value; } }
}
