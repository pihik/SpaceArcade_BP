using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainMenuManager : MonoBehaviour
{
    public static Action OnNameSet;

    [SerializeField] GameObject setNameCanvas;
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] Text messageText;
    [SerializeField] Button playButton;
    [SerializeField] Button SoundButton;

    //******************************************** maybe add functionality for add new player and current player will be set to scoreboard ********************************************

    string soundON = "SOUND <color=green>ON</color>";
    string soundOFF = "SOUND <color=red>OFF</color>";

    Text soundText;

    void Start()
    {
        soundText = SoundButton.GetComponentInChildren<Text>();

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

        ToggleNameSet();
    }

    public void SetName()
    {
        if (string.IsNullOrEmpty(nameInputField.text))
        {
            StartCoroutine(WiggleInputField());
            return;
        }

        PlayerPrefs.SetString("PlayerName", nameInputField.text);
        PlayerPrefs.Save();

        ToggleNameSet();
        StartCoroutine(ShowMessage("Welcome " + nameInputField.text + "!"));

        OnNameSet?.Invoke();
    }

    public void ToggleNameSet()
    {
        bool value = setNameCanvas.activeSelf;
        setNameCanvas.SetActive(!value);
        playButton.gameObject.SetActive(value);
    }

    void InitializeButtons()
    {
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(LevelLoader.instance.LoadNextScene);

        SoundButton.onClick.RemoveAllListeners();
        SoundButton.onClick.AddListener(AudioManager.instance.ToggleSound);
        SoundButton.onClick.AddListener(UpdateSoundText);
    }

    void UpdateSoundText()
    {
        soundText.text = AudioManager.instance.IsMusicPlaying() ? soundON : soundOFF;
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