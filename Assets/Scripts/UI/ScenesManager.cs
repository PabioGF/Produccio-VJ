using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    #region Unity methods
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    public void StartGame()
    {
        SceneManager.LoadScene("AssetTest");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
