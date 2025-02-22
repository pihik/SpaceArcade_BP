using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public static Action OnLevelLoaded;

    #region Singleton
    public static LevelLoader instance;

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

    void OnEnable()
    {
        //GameManager.instance.OnEnemiesDestroyed += LoadNextScene;
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(GetActiveSceneInt() + 1);
        OnLevelLoaded?.Invoke();
    }

    public void Restart() // later put player to starting level 1 and reset his score 
    {
        Time.timeScale = 1;
        GameManager.instance.SetScore(0);
        SceneManager.LoadScene(1);//(GetActiveSceneInt());
        OnLevelLoaded?.Invoke();
    }

    public void LoadMainMenu() 
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
        OnLevelLoaded?.Invoke();
    }

    public int GetActiveSceneInt()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public bool IsLastScene()
    {
        return GetActiveSceneInt() == SceneManager.sceneCountInBuildSettings - 1;
    }

    void OnDisable()
    {
        //GameManager.instance.OnEnemiesDestroyed -= LoadNextScene;
    }
}
