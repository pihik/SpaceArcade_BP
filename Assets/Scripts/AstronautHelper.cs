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
        GameManager.instance.OnEnemiesDestroyed += ShowLastTexts;
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

        CheckForText();
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
        textIndex++;
        CheckForText();
    }

    void ShowLastTexts() // it can be also handled through ShowNextText, but this is safer in case of different events
    {
        for (int i = amountOfTexts - 1; i >= 0; i--)
        {
            if (texts[i].Length == 0) // if we found empty text from end to start, that means we found end level texts
            {
                textIndex = ++i;
                CheckForText();
                break;
            }
        }
    }

    void CheckForText()
    {
        // if there is no text, disable canvas, otherwise show the first text
        if (!IsValidIndex(textIndex))
        {
            Debug.Log("[AsteroidHelper::CheckForText] Index is not valid, loading next scene");
            LevelLoader.instance.LoadNextScene();
            return;
        }

        if (texts[textIndex].Length == 0)
        {
            Time.timeScale = 1;
            canvas.gameObject.SetActive(false);
            return;
        }

        Time.timeScale = 0;
        canvas.gameObject.SetActive(true);
        myText.text = texts[textIndex];

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
            CheckForText();
        }
    }

    void OnDisable()
    {
        //GameManager.instance.OnEnemiesDestroyed -= ShowLastTexts;
    }
}
