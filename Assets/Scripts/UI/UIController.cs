using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using TMPro;

public class UIController : MonoBehaviour
{
    #region Global Variables
    [SerializeField] private GameObject _statsPanel;
    public static UIController Instance;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _statsPanel.SetActive(true);
    }

    private void OnDisable()
    {
    }
    #endregion

    #region Stats Interface
    [Header("Stats Panel")]
    [SerializeField] private TextMeshProUGUI _lifeText;
    [SerializeField] private TextMeshProUGUI _multiplierText;
    [SerializeField] private TextMeshProUGUI _bottlesText;

    public void SetLife(float life)
    {
        _lifeText.text = life.ToString();
    }

    public void SetMultiplier(int multiplier)
    {
        _multiplierText.text = multiplier.ToString();
    }

    public void SetBottles(float bottles)
    {
        _bottlesText.text = bottles.ToString();
    }
    #endregion

    #region Pause Menu
    [Header ("Pause Menu")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject _selectedOptionPause;
    [SerializeField] private GameObject _comboList;
    private bool _isPaused;

    /// <summary>
    /// Pauses / unpauses the game
    /// </summary>
    /// <param name="context">InputAction</param>
    public void HandlePauseInput()
    {
        if (!_isPaused)
        {
            _isPaused = true;
            pausePanel.SetActive(true);
            Time.timeScale = 0;

            EventSystem.current.SetSelectedGameObject(_selectedOptionPause);
        }
        else
        {
            Resume();
        }
    }

    /// <summary>
    /// Resumes the game
    /// </summary>
    public void Resume()
    {
        _isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void EnterComboList()
    {
        pausePanel.SetActive(false);
        _comboList.SetActive(true);
    }

    public void ExitComboList()
    {
        pausePanel.SetActive(true);
        _comboList.SetActive(false);
    }

    /// <summary>
    /// Goes to the main menu
    /// </summary>
    public void GoMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void SaveGame()
    {
        GameController.Instance.SaveProgress();
    }

    /// <summary>
    /// Exits the game
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion

    #region Death Screen
    [Header ("Death Screen")]
    [SerializeField] private GameObject _deathScreen;
    private bool _isDeathScreen;

    /// <summary>
    /// Shows the death screen
    /// </summary>
    public void ShowDeathScreen()
    {
        _isDeathScreen = true;
        _deathScreen.SetActive(true);
    }

    /// <summary>
    /// Reloads the game after dying
    /// </summary>
    /// <param name="context">InputAction</param>
    public void HideDeathScreen(InputAction.CallbackContext context)
    {
        if (!_isDeathScreen) return;

        _isDeathScreen = false;
        ScenesController.Instance.ReloadScene();
    }
    #endregion

    #region Level Complete Screen
    [Header("Level Complete Screen")]
    [SerializeField] private GameObject _levelCompleteScreen;
    [SerializeField] private GameObject _selectedOptionCompleted;

    /// <summary>
    /// Shows the level complete screen
    /// </summary>
    public IEnumerator LevelCompleted()
    {
        _levelCompleteScreen.SetActive(true);
        _statsPanel.SetActive(false);

        float timeElapsed = 0;
        CanvasGroup group = _levelCompleteScreen.GetComponent<CanvasGroup>();
        //AudioManager.Instance.PlaySFX("LightSwitch");

        float duration = 0.5f;
        while (timeElapsed < duration)
        {
            group.alpha = Mathf.Lerp(0, 1, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        group.alpha = 1;

        Time.timeScale = 0;

        EventSystem.current.SetSelectedGameObject(_selectedOptionCompleted);
    }

    /// <summary>
    /// Loads the level that goes next to the currently completed one
    /// </summary>
    public void LoadNextLevel()
    {
        ScenesController.Instance.LoadNextScene();
    }

    public void SaveAndExit()
    {
        SaveGame();
        GoMainMenu();
    }
    #endregion

    #region Game Ended Screen
    [Header("End Game Screen")]
    [SerializeField] private GameObject _endGameScreen;
    [SerializeField] private GameObject _selectedOptionEnd;
    public void GameEnded()
    {
        _endGameScreen.SetActive(true);
        Time.timeScale = 0;

        EventSystem.current.SetSelectedGameObject(_selectedOptionEnd);
    }
    #endregion
}