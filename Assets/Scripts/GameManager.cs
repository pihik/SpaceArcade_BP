using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	#region Singleton
	public static GameManager instance;
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

	#region Actions
	public Action<int> OnScoreChange;
	public Action OnEnemiesDestroyed;
	public Action<float> StartSpawning;
	public Action<float> StopSpawning;
	public Action<float, float> AsteroidStopEvent;

	#endregion

	int numberOfEnemies;
	int scoreAmount = 0;

	void Start()
	{
		LevelLoader.instance.OnLevelLoaded += SaveScoreToCurrentPlayer;
	}

	public void SetScore(int scoreAmount)
	{
		this.scoreAmount = scoreAmount;
	}

	public void ScoreIncrease(int howMuch)
	{
		scoreAmount += LevelLoader.instance.GetActiveSceneInt() * howMuch;
		OnScoreChange?.Invoke(scoreAmount);
	}

	public int GetScore()
	{
		return scoreAmount;
	}

	public int GetPlayerMaxScore()
	{
		string playerName = PlayerPrefs.GetString("PlayerName", "");
		if (string.IsNullOrEmpty(playerName))
		{
			return 0;
		}
		string scoreKey = "Score_" + playerName;

		return PlayerPrefs.GetInt(scoreKey, 0);
	}

	public void SetNumberOfEnemies(int number)
	{
		numberOfEnemies = number;
	}

	public void DecreaseNumberOfEnemies()
	{
		numberOfEnemies--;
		
		if (numberOfEnemies < 0)
		{
			Debug.Log("All enemies destroyed!");
			OnEnemiesDestroyed?.Invoke();
		}
	}

	void SaveScoreToCurrentPlayer(int levelIndex)
	{
		string playerName = PlayerPrefs.GetString("PlayerName", "");
		if (string.IsNullOrEmpty(playerName)) return;

		string scoreKey = "Score_" + playerName;

		int savedScore = PlayerPrefs.GetInt(scoreKey, 0);
		if (scoreAmount > savedScore)
		{
			PlayerPrefs.SetInt(scoreKey, scoreAmount);
			PlayerPrefs.Save();
		}

		List<string> keys = PlayerPrefsKeys();
		if (!keys.Contains(scoreKey))
		{
			keys.Add(scoreKey);
			PlayerPrefs.SetString("PlayerPrefsKeys", string.Join("|", keys));
			PlayerPrefs.Save();
		}
	}

	public List<string> PlayerPrefsKeys()
	{
		List<string> keys = new List<string>();
		foreach (var key in PlayerPrefs.GetString("PlayerPrefsKeys", "").Split('|'))
		{
			if (!string.IsNullOrEmpty(key))
				keys.Add(key);
		}
		return keys;
	}

	void OnDisable()
	{
		LevelLoader.instance.OnLevelLoaded -= SaveScoreToCurrentPlayer;
	}
}
