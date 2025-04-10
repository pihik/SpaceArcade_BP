using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainMenuManager : MonoBehaviour
{
	public static Action OnNameSet;

	[SerializeField] TMP_InputField nameInputField;
	[SerializeField] Text messageText;
	[SerializeField] Button playButton;
	[SerializeField] Button exitButton;
	[SerializeField] Button soundButton;
	[SerializeField] Button vSyncButton;

	//******************************************** maybe add functionality for add new player and current player will be set to scoreboard ********************************************

	string soundON = "SOUND <color=green>ON</color>";
	string soundOFF = "SOUND <color=red>OFF</color>";

	string vSyncON = "VSYNC <color=green>ON</color>";
	string vSyncOFF = "VSYNC <color=red>OFF</color>";

	Text soundText;
	Text vSyncText;

	void Start()
	{
		soundText = soundButton.GetComponentInChildren<Text>();
		vSyncText = vSyncButton.GetComponentInChildren<Text>();

		CheckIfNameIsSet();
		InitializeButtons();
	}

	public void CheckIfNameIsSet()
	{
		if (!PlayerPrefs.HasKey("PlayerName"))
		{
			StartCoroutine(ShowMessage("Welcome firstly you need to select your name!"));
		}
		else
		{
			StartCoroutine(ShowMessage("Welcome back " + PlayerPrefs.GetString("PlayerName") + "!"));
		}
	}

	public void SetName()
	{
		string newName = nameInputField.text;

		if (string.IsNullOrEmpty(newName))
		{
			StartCoroutine(WiggleInputField());
			return;
		}

		if (PlayerPrefs.GetString("PlayerName", "") == newName)
		{
			StartCoroutine(ShowMessage("This name is already in use! Please choose another."));
			return;
		}

		PlayerPrefs.SetString("PlayerName", newName);
		PlayerPrefs.Save();

		StartCoroutine(ShowMessage("Welcome " + newName + "!"));

		GameManager.instance.SetScore(0);
		OnNameSet?.Invoke();
	}

	public void SetNameToPlaceholder()
	{
		nameInputField.text = PlayerPrefs.GetString("PlayerName");
	}

	void InitializeButtons()
	{
		playButton.onClick.RemoveAllListeners();
		exitButton.onClick.RemoveAllListeners();
		playButton.onClick.AddListener(LevelLoader.instance.LoadNextScene);
		exitButton.onClick.AddListener(LevelLoader.instance.QuitApplication);

		UpdateSoundText();
		soundButton.onClick.RemoveAllListeners();
		soundButton.onClick.AddListener(AudioManager.instance.ToggleSound);
		soundButton.onClick.AddListener(UpdateSoundText);

		UpdateVsyncText();
		vSyncButton.onClick.RemoveAllListeners();
		vSyncButton.onClick.AddListener(FPSChecker.instance.VSyncSwitch);
		vSyncButton.onClick.AddListener(UpdateVsyncText);
	}

	void UpdateSoundText()
	{
		soundText.text = AudioManager.instance.IsMusicPlaying() ? soundON : soundOFF;
	}

	void UpdateVsyncText()
	{
		vSyncText.text = FPSChecker.instance.IsVSyncEnabled() ? vSyncON : vSyncOFF;
	}

	IEnumerator WiggleInputField()
	{
		Vector3 originalPosition = nameInputField.transform.localPosition;
		float wiggleDuration = 0.3f;
		float wiggleSpeed = 20f;
		float elapsed = 0.0f;

		while (elapsed < wiggleDuration)
		{
			float x = Mathf.Sin(elapsed * wiggleSpeed) * 10f;
			nameInputField.transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y, originalPosition.z);

			elapsed += Time.deltaTime;
			yield return null;
		}

		nameInputField.transform.localPosition = originalPosition; // Reset to original position
	}

	IEnumerator ShowMessage(string message)
	{
		messageText.text = message;
		messageText.gameObject.SetActive(true);
		yield return new WaitForSeconds(3f);
		messageText.gameObject.SetActive(false);
	}
}