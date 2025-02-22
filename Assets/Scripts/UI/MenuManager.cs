using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _selectedOption;

    private void Awake()
    {
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
}