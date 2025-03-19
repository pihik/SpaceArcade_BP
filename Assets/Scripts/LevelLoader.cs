using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
	public Action<int> OnLevelLoaded;

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

	public void LoadNextScene()
	{
		int levelIndex = GetActiveSceneInt() + 1;

		SceneManager.LoadScene(GetActiveSceneInt() + 1);
		OnLevelLoaded?.Invoke(levelIndex);
	}

	public void Restart()
	{
		Time.timeScale = 1;
		GameManager.instance.SetScore(0);
		SceneManager.LoadScene(1);
		OnLevelLoaded?.Invoke(1);
	}

	public void LoadMainMenu() 
	{
		Time.timeScale = 1;
		SceneManager.LoadScene("MainMenu");
		OnLevelLoaded?.Invoke(1);
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
}
