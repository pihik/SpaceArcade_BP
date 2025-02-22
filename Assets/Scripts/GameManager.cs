using System;
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

    public void SetScore(int scoreAmount)
    {
        this.scoreAmount = scoreAmount;
    }

    public void ScoreIncrease(int howMuch)
    {
        scoreAmount += LevelLoader.instance.GetActiveSceneInt() * howMuch;                       //SCORE INCREASE, HOW MUCH - DEPPEND ON LVL
        OnScoreChange?.Invoke(scoreAmount);
    }

    public int GetScore()
    {
        return scoreAmount;
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
}
