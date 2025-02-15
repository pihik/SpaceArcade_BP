using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class AstronautHelper : MonoBehaviour
{
    public static AstronautHelper instance;

    [SerializeField] string[] texts;

    Text myText;
    Canvas canvas;

    int textIndex = 0;
    int amountOfTexts;

    bool canPressContinue = true;

    void OnEnable()
    {
        GameManager.instance.OnEnemiesDestroyed += ShowNewText;
    }

    void Awake()
    {
        instance = this;

        canvas = GetComponentInParent<Canvas>();
        myText = GetComponentInChildren<Text>();

        if (!canvas || !myText)
        {
            Debug.LogError("[AsteroidHelper::Start] Something went wrong, check on: " + gameObject.name);
        }
    }

    void Start()
    {
        amountOfTexts = texts.Length;

        CheckForText(textIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !canPressContinue)
        {
            canPressContinue = true;
            OnContinuePressed();
            return;
        }
        canPressContinue = false;
    }

    public void ShowNewText()
    {
        Debug.Log("[AsteroidHelper::ShowNewText] Showing new text");
        textIndex++;
        CheckForText(textIndex);
    }

    void CheckForText(int index)
    {
        // if there is no text, disable canvas, otherwise show the first text
        if (!IsValidIndex(index))
        {
            Debug.Log("[AsteroidHelper::CheckForText] Index is not valid, loading next scene");
            LevelLoader.instance.LoadNextScene();
            return;
        }

        if (texts[index].Length == 0)
        {
            Time.timeScale = 1;
            canvas.gameObject.SetActive(false);
            return;
        }

        Time.timeScale = 0;
        canvas.gameObject.SetActive(true);
        myText.text = texts[index];

        textIndex++;
    }

    bool IsValidIndex(int index)
    {
        bool isValid = (index > amountOfTexts) ? false : true;

        return isValid;
    }

    void OnContinuePressed()
    {
        if (canvas.enabled)
        {
            CheckForText(textIndex);
        }
    }

    void OnDisable()
    {
        GameManager.instance.OnEnemiesDestroyed -= ShowNewText;
    }
}
