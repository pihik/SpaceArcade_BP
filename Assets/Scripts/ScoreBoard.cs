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

	void OnEnable()
	{
		UI_MainMenuManager.OnNameSet += CreateScoreList;
	}

	void Start()
	{
		CreateScoreList();
	}

	public void CreateScoreList()
	{
		ClearList();
		addToBoard = new List<AddToBoard>();
		HashSet<string> usedNames = new HashSet<string>();

		foreach (string key in GameManager.instance.PlayerPrefsKeys())
		{
			if (key.StartsWith("Score_"))
			{
				string playerName = key.Replace("Score_", "");
				int score = PlayerPrefs.GetInt(key, 0);
				addToBoard.Add(new AddToBoard { score = score, name = playerName });
				usedNames.Add(playerName);
			}
		}

		string currentPlayer = GetCurrentPlayerName();
		int currentPlayerScore = GameManager.instance.GetScore();

		if (!usedNames.Contains(currentPlayer))
		{
			addToBoard.Add(new AddToBoard { score = currentPlayerScore, name = currentPlayer });
			PlayerPrefs.SetInt("Score_" + currentPlayer, currentPlayerScore);
			PlayerPrefs.Save();
			usedNames.Add(currentPlayer);
		}

		while (addToBoard.Count < 10)
		{
			string randomName = GetUniqueRandomName(usedNames);
			int randomScore = Random.Range(0, 100000);
			addToBoard.Add(new AddToBoard { score = randomScore, name = randomName });
		}

		addToBoard.Sort((a, b) => b.score.CompareTo(a.score));

		entryToBoard = new List<Transform>();
		foreach (AddToBoard player in addToBoard)
		{
			CreatePlayer(player, texts, entryToBoard);
		}
	}

	string GetUniqueRandomName(HashSet<string> existingNames)
	{
		string[] randomNames = { "Jozef", "Ferko", "Dano", "Pato", "Adam", "Adrian", "Mato", "Denis", "Juro" };

		string newName;
		do
		{
			newName = randomNames[Random.Range(0, randomNames.Length)];
		} while (existingNames.Contains(newName));

		existingNames.Add(newName);
		return newName;
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

	void OnDisable()
	{
		UI_MainMenuManager.OnNameSet -= CreateScoreList;
	}
}
