using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameManager : MonoBehaviour
{
    #region Singleton
    public static UI_GameManager instance;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        //DontDestroyOnLoad(gameObject); we need object this in every level for new references
    }
    #endregion

    [SerializeField] GameObject gameOverObject;
    [SerializeField] Text levelInfoText;

    [SerializeField] Button[] buttons;

    [Header ("Upper texts")]
    [SerializeField] Text levelText;
    [SerializeField] Text scoreText;
    [SerializeField] Text healthText;

    Canvas canvas;

    void OnEnable()
    {
        GameManager.instance.OnScoreChange += ScoreTextChange;
        PlayersAttributeComponent.OnHealthChange += HealthTextChange;
        PlayersAttributeComponent.OnPlayerDestroy += OnPlayerDestroy;
    }

    void Start()
    {
        canvas = GetComponent<Canvas>();

        if (LevelLoader.instance.IsLastScene())
        {
            HandleLastScene();
            return;
        }

        OnSceneChange();
    }

    void OnSceneChange()
    {
        string level = LevelLoader.instance.GetActiveSceneInt().ToString();

        levelText.text = level;
        
        levelInfoText.text = "Level " + level;
        levelInfoText.enabled = true;
        Invoke(nameof(DeactivateLevelText), 3);
        
        gameOverObject.SetActive(true);
        InitializeButtons();

        gameOverObject.SetActive(false);
    }

    void HandleLastScene()
    {
        OnPlayerDestroy();
        InitializeButtons();
    }

    void OnPlayerDestroy()
    {
        gameOverObject.SetActive(true);
        gameOverObject.transform.GetChild(1).GetComponent<Text>().text = "SCORE: " + GameManager.instance.GetScore().ToString();
        canvas.sortingOrder = 1000;
    }

    void ScoreTextChange(int amount)
    {
        scoreText.text = amount.ToString();
    }

    void HealthTextChange(int amount)
    {
        healthText.text = amount.ToString();
    }

    void DeactivateLevelText()
    {
        levelInfoText.enabled = false;
    }

    void InitializeButtons()
    {
        if (buttons.Length < 3)
        {
            Debug.LogError("Buttons are not assigned in the inspector");
            return;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.RemoveAllListeners();

            switch (i)
            {
                case 0:
                    buttons[i].GetComponentInChildren<Text>().text = "PLAY AGAIN";
                    buttons[i].onClick.AddListener(LevelLoader.instance.Restart);
                    break;
                case 1:
                    buttons[i].GetComponentInChildren<Text>().text = "MAIN MENU";
                    buttons[i].onClick.AddListener(LevelLoader.instance.LoadMainMenu);
                    break;
                case 2:
                    buttons[i].GetComponentInChildren<Text>().text = "QUIT";
                    buttons[i].onClick.AddListener(LevelLoader.instance.QuitApplication);
                    break;
            }
        }
    }

    void OnDisable()
    {
        GameManager.instance.OnScoreChange -= ScoreTextChange;
        PlayersAttributeComponent.OnHealthChange -= HealthTextChange;
        PlayersAttributeComponent.OnPlayerDestroy -= OnPlayerDestroy;
    }
}
