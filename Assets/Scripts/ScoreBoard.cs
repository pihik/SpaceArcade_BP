using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] Transform texts;
    [SerializeField] Transform stats;
    float textsHeight = 20f;

    List<AddToBoard> addToBoard;
    List<Transform> entryToBoard;

    private void OnEnable()
    {
        UI_MainMenuManager.OnNameSet += CreateScoreList;
    }

    private void Start()
    {
        CreateScoreList();
    }
    public void CreateScoreList()
    {
        ClearList();
        addToBoard = new List<AddToBoard>()
        {
            new AddToBoard{ score = GameManager.instance.GetScore(), name = GetCurrentPlayerName() },
            new AddToBoard{ score = Random.Range(0,100000), name = "JOZKO" },
            new AddToBoard{ score = Random.Range(0,100000), name = "FERKO" },
            new AddToBoard{ score = Random.Range(0,100000), name = "DANO" },
            new AddToBoard{ score = Random.Range(0,100000), name = "PATO" },
            new AddToBoard{ score = Random.Range(0,100000), name = "ADAM" },
            new AddToBoard{ score = Random.Range(0,100000), name = "ADRIAN" },
            new AddToBoard{ score = Random.Range(0,100000), name = "MATO" },
            new AddToBoard{ score = Random.Range(0,100000), name = "DENIS" },
            new AddToBoard{ score = Random.Range(0,100000), name = "JURO" }
        };

        for (int i = 0; i < addToBoard.Count; i++)
        {
            for (int j = i; j < addToBoard.Count; j++)
            {
                if (addToBoard[j].score > addToBoard[i].score)
                {
                    AddToBoard tmp = addToBoard[i];
                    addToBoard[i] = addToBoard[j];
                    addToBoard[j] = tmp;
                }
            }
        }

        entryToBoard = new List<Transform>();
        foreach (AddToBoard player in addToBoard)
        {
            CreatePlayer(player, texts, entryToBoard);
        }
    }
    string GetCurrentPlayerName()
    {
        string playerName = (!PlayerPrefs.HasKey("PlayerName")) ? "PLAYER" : PlayerPrefs.GetString("PlayerName");
        return playerName;
    }
    void CreatePlayer(AddToBoard addToBoard, Transform transform, List<Transform> transformList)
    {
        var newPlayer = Instantiate(stats, transform);
        RectTransform newPlayerRectTransform = newPlayer.GetComponent<RectTransform>();
        newPlayerRectTransform.anchoredPosition = new Vector2(0, -textsHeight * transformList.Count);

        int position = transformList.Count + 1;
        string playerPosision;
        switch (position)
        {
            default:
                playerPosision = position + "."; break;
            case 1: playerPosision = "1."; break;
            case 2: playerPosision = "2."; break;
            case 3: playerPosision = "3."; break;
        }


        newPlayer.Find("position").GetComponent<Text>().text = playerPosision;

        newPlayer.Find("score").GetComponent<Text>().text = addToBoard.score.ToString();

        newPlayer.Find("name").GetComponent<Text>().text = addToBoard.name;

        transformList.Add(newPlayer);
    }
    void ClearList()
    {
        if (entryToBoard != null)
        {
            foreach (Transform entry in entryToBoard)
            {
                Destroy(entry.gameObject);
            }
            entryToBoard.Clear();
            addToBoard.Clear();
        }
    }
    class AddToBoard
    {
        public int score;

        public string name;
    }
    private void OnDisable()
    {
        UI_MainMenuManager.OnNameSet -= CreateScoreList;
    }
}
