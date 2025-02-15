using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    #region Singleton
    public static LevelLoader instance;

    void OnEnable()
    {
        //GameManager.instance.OnEnemiesDestroyed += LoadNextScene;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public void LoadNextScene()
    {
        SceneManager.LoadScene(GetActiveSceneInt() + 1);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(GetActiveSceneInt());
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public int GetActiveSceneInt()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    void OnDisable()
    {
        //GameManager.instance.OnEnemiesDestroyed -= LoadNextScene;
    }
}
