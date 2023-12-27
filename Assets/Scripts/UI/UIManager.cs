using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject PausePanel;

    public void PauseMenu()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
    }

    public void Return()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
    }

    public void GoMianMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    } 
}
