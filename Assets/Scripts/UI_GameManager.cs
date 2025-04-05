using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.EventSystems;
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

	[Header("Canvases")]
	[SerializeField] Canvas gameCanvas;
	[SerializeField] Canvas helperCanvas;
	[SerializeField] Canvas importantCanvas;
	[SerializeField] Canvas consoleCanvas;

	[SerializeField] Button[] buttons;

	[Header("Game texts")]
	[SerializeField] Text levelInfoText;

	[Header("Important button texts")]
	[SerializeField] Text levelText;
	[SerializeField] Text scoreText;
	[SerializeField] Text healthText;

	bool canPressEscape;
	TMP_InputField consoleInputField;

	void OnEnable()
	{
		GameManager.instance.OnScoreChange += ScoreTextChange;
		PlayersAttributeComponent.OnHealthChange += HealthTextChange;
		PlayersAttributeComponent.OnPlayerDestroy += OnPlayerDestroy;
	}

	void Start()
	{
		if (LevelLoader.instance.IsLastScene())
		{
			HandleLastScene();
			return;
		}

		OnSceneChange();

		consoleInputField = consoleCanvas.GetComponentInChildren<TMP_InputField>();
		if (!consoleInputField)
		{
			Debug.LogError("Console input field is not assigned in the inspector");
		}
		consoleInputField.onSubmit.AddListener(ProcessCommand);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && canPressEscape)
		{
			EscapeSwitch();
		}

		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
			ConsoleSwitch();
		}
	}

	void OnSceneChange()
	{
		int currentScore = GameManager.instance.GetScore();
		ScoreTextChange(currentScore);

		string level = LevelLoader.instance.GetActiveSceneInt().ToString();

		levelText.text = level;

		levelInfoText.text = "Level " + level;
		levelInfoText.enabled = true;
		Invoke(nameof(DeactivateLevelText), 3);

		importantCanvas.enabled = true;
		InitializeButtons();

		importantCanvas.enabled = false;
		canPressEscape = true;
	}

	void ConsoleSwitch()
	{
		bool isConsoleActive = consoleCanvas.isActiveAndEnabled;

		consoleCanvas.enabled = !isConsoleActive;
		consoleInputField.text = "";
		FPSChecker.instance.TextSwitch(isConsoleActive);

		if (consoleCanvas.isActiveAndEnabled)
		{
			consoleInputField.ActivateInputField();
		}
		else
		{
			consoleInputField.DeactivateInputField();
			EventSystem.current.SetSelectedGameObject(null);
		}

		Time.timeScale = (!consoleCanvas.isActiveAndEnabled && !importantCanvas.isActiveAndEnabled && !helperCanvas.isActiveAndEnabled) ? 1 : 0;
	}

	void HandleLastScene()
	{
		int maxScore = GameManager.instance.GetPlayerMaxScore();
		int currentScore = GameManager.instance.GetScore();

		scoreText.text = (currentScore > maxScore) ? "YOUR NEW HIGH SCORE: " + currentScore.ToString() : "TOTAL SCORE: " + currentScore.ToString();

		OnPlayerDestroy();
		InitializeButtons();
	}

	void OnPlayerDestroy()
	{
		canPressEscape = false;
		if (!importantCanvas)
		{
			return;
		}
		importantCanvas.enabled = true;
		importantCanvas.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "SCORE: " + GameManager.instance.GetScore().ToString();
	}

	void EscapeSwitch()
	{
		bool isEscapeActive = importantCanvas.isActiveAndEnabled;

		importantCanvas.enabled = !isEscapeActive;
		helperCanvas.gameObject.SetActive(isEscapeActive);
		levelInfoText.enabled = isEscapeActive;

		if (consoleCanvas.isActiveAndEnabled)
		{
			consoleCanvas.enabled = false;
		}

		Time.timeScale = (!importantCanvas.isActiveAndEnabled && !helperCanvas.isActiveAndEnabled && !consoleCanvas.isActiveAndEnabled) ? 1 : 0;
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

	void ProcessCommand(string command)
	{
		if (string.IsNullOrWhiteSpace(command))
		{
			ConsoleSwitch();
			return;
		}

		string[] args = command.Split(' ');
		string cmd = args[0].ToLower();

		switch (cmd)
		{
			case "/loadlevel":
				if (args.Length > 1 && int.TryParse(args[1], out int levelIndex))
				{
					Debug.Log("Loading level: " + levelIndex);
					if (levelIndex < 0 || levelIndex >= LevelLoader.instance.GetSceneCount() - 1)
					{
						Debug.Log("Level index out of range!");
						return;
					}
					LevelLoader.instance.LoadLevel(levelIndex);
				}
				else
				{
					Debug.Log("Command not found!");
				}
				break;
			case "/godmode":
				if (args.Length > 1)
				{
					bool isImmortal;

					if (bool.TryParse(args[1], out isImmortal))
					{
						Debug.Log("God mode " + (isImmortal ? "activated!" : "deactivated!"));
						InGameHelper.instance.GetPlayer().GetAttributeComponent().SetIsImmortal(isImmortal);
					}
					else
					{
						Debug.LogWarning("Invalid godmode value! Use: /godmode true or /godmode false");
					}
				}
				else
				{
					Debug.Log("Command not found!");
				}
				break;
			case "/activateweapons":
				InGameHelper.instance.GetPlayer().ActivateAllGuns();
				break;
			default:
				Debug.Log("Unknown command: " + command);
				break;
		}
		ConsoleSwitch();
	}

	public bool IsConsoleActive()
	{
		return consoleCanvas.isActiveAndEnabled;
	}

	void OnDisable()
	{
		GameManager.instance.OnScoreChange -= ScoreTextChange;
		PlayersAttributeComponent.OnHealthChange -= HealthTextChange;
		PlayersAttributeComponent.OnPlayerDestroy -= OnPlayerDestroy;
	}
}
