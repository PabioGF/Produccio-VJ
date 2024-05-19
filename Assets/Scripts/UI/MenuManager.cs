using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _selectedOption;
    [SerializeField] private GameObject _mainOptions;
    [SerializeField] private GameObject _levelSelection;

    private void Awake()
    {
        AudioManager.Instance.StopMusic();
        EventSystem.current.SetSelectedGameObject(_selectedOption);
    }

    public void StartGame()
    {
        LevelProgressController.Instance.HasSpawnPoint = false;
        ScenesController.Instance.LoadSceneByIndex(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowLevelSelection()
    {
        _levelSelection.SetActive(true);
        _mainOptions.SetActive(false);
    }

    public void HideLevelSelection()
    {
        _levelSelection.SetActive(false);
        _mainOptions.SetActive(true);
    }

    public void LoadLevel(int index)
    {
        LevelProgressController.Instance.HasSpawnPoint = false;
        ScenesController.Instance.LoadSceneByIndex(index);
    }
}