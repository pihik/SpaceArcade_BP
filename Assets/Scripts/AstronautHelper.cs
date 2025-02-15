using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class AstronautHelper : MonoBehaviour
{
    public static AstronautHelper instance;

    [SerializeField] string[] texts;

    Text myText;
    Canvas canvas;
    //Player player;

    int textIndex = 0;
    int amountOfTexts;

    bool canPressContinue = true;
    void OnEnable()
    {
        //InGameHelper.instance.onNewPlayer += OnNewPlayer;
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
        /*
        player = InGameHelper.instance.GetPlayer();

        if (!player)
        {
            Debug.LogError("[AsteroidHelper::Start] Player not found!");
            return;
        }

        player.onSpacePressed += OnContinuePressed;*/
        amountOfTexts = texts.Length;

        CheckForText(textIndex);
    }

    public void ShowNewText()
    {
        textIndex++;
        CheckForText(textIndex);
    }
    /*
    void OnNewPlayer(Player newPlayer)
    {
        player = newPlayer;
    }*/

    void CheckForText(int index)
    {
        Debug.Log("checking index = " + index);
        // if there is no text, disable canvas, otherwise show the first text
        if (!IsValidIndex(index))
        {
            Debug.Log("[AsteroidHelper::CheckForText] Index is not valid");
            return;
        }

        if (texts[index].Length == 0)
        {
            Time.timeScale = 1;
            canvas.enabled = false;
            return;
        }

        Time.timeScale = 0;
        canvas.enabled = true;
        myText.text = texts[index];

        textIndex++;
    }

    bool IsValidIndex(int index)
    {
        bool isValid = (index > amountOfTexts) ? false : true;

        return isValid;
    }
    //rebuild this for any key and on key release activate canContinue bool
    void OnContinuePressed()
    {
        if (canvas.enabled)
        {
            CheckForText(textIndex);
        }
    }

    void Update()
    {
        if (Input.anyKeyDown && !canPressContinue)
        {
            canPressContinue = true;
            OnContinuePressed();
            return;
        }
        canPressContinue = false;
    }

    void OnDisable()
    {
        /*
        InGameHelper.instance.onNewPlayer -= OnNewPlayer;
        
        if (player)
        {
            player.onSpacePressed -= OnContinuePressed;
        }*/
    }
}
