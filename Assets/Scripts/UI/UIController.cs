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
    [SerializeField] private float _fadeInDuration;
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
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _shieldText;
    [SerializeField] private TextMeshProUGUI _bottlesText;

    public void SetLife(float life)
    {
        _lifeText.text = life.ToString();
    }

    public void SetScore(int score)
    {
        _scoreText.text = score.ToString();
    }

    public void SetShield(int shield)
    {
        _shieldText.text = shield.ToString();
    }

    public int GetShield()
    {
        return int.Parse(_shieldText.text); 
    }
    public void SetBottles(int bottles)
    {
        _bottlesText.text = bottles.ToString();
    }

    public int GetBottles()
    {
        return int.Parse(_bottlesText.text);
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
    [SerializeField] private GameObject _selectedOptionDeath;
    private bool _isDeathScreen;

    /// <summary>
    /// Shows the death screen
    /// </summary>
    public void ShowDeathScreen()
    {
        _isDeathScreen = true;
        _deathScreen.SetActive(true);
        _statsPanel.SetActive(false);

        StartCoroutine(FadeInScreen(_deathScreen.GetComponent<CanvasGroup>()));

        EventSystem.current.SetSelectedGameObject(_selectedOptionDeath);
    }

    /// <summary>
    /// Reloads the game after dying
    /// </summary>
    public void HideDeathScreen()
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
    public void LevelCompleted()
    {
        _levelCompleteScreen.SetActive(true);
        _statsPanel.SetActive(false);

        StartCoroutine(FadeInScreen(_levelCompleteScreen.GetComponent<CanvasGroup>()));

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

    private IEnumerator FadeInScreen(CanvasGroup screen)
    {
        float timeElapsed = 0;

        while (timeElapsed < _fadeInDuration)
        {
            screen.alpha = Mathf.Lerp(0, 1, timeElapsed / _fadeInDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        screen.alpha = 1;

        Time.timeScale = 0;
    }
}