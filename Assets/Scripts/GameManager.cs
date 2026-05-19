using System;
using System.Collections;
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
	bool isGodModeEnabled = false;
	bool checkingEnemiesDestroyed = false;
	bool enemiesDestroyedInvoked = false;

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

	public void SetGodMode(bool isEnabled)
	{
		isGodModeEnabled = isEnabled;
		ApplyGodModeToPlayer();
	}

	public bool IsGodModeEnabled()
	{
		return isGodModeEnabled;
	}

	public void ApplyGodModeToPlayer()
	{
		if (!InGameHelper.instance || !InGameHelper.instance.GetPlayer())
		{
			return;
		}

		InGameHelper.instance.GetPlayer().GetAttributeComponent().SetIsImmortal(isGodModeEnabled);
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
		checkingEnemiesDestroyed = false;
		enemiesDestroyedInvoked = false;
	}

	public void DecreaseNumberOfEnemies()
	{
		numberOfEnemies--;
		
		if (numberOfEnemies <= 0)
		{
			CheckEnemiesDestroyed();
		}
	}

	void CheckEnemiesDestroyed()
	{
		if (checkingEnemiesDestroyed || enemiesDestroyedInvoked)
		{
			return;
		}

		checkingEnemiesDestroyed = true;
		StartCoroutine(CheckEnemiesDestroyedAtEndOfFrame());
	}

	IEnumerator CheckEnemiesDestroyedAtEndOfFrame()
	{
		yield return null;

		checkingEnemiesDestroyed = false;

		if (numberOfEnemies > 0 || !EnemySpawnersDepleted() || FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length > 0)
		{
			yield break;
		}

		enemiesDestroyedInvoked = true;
		Debug.Log("All enemies destroyed!");
		StopSpawning?.Invoke(0);
		OnEnemiesDestroyed?.Invoke();
	}

	bool EnemySpawnersDepleted()
	{
		foreach (EnemySpawner enemySpawner in FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None))
		{
			if (!enemySpawner.HasSpawnedAllObjects())
			{
				return false;
			}
		}

		return true;
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
