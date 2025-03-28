using System.IO;
using TMPro;
using UnityEngine;

public class FPSChecker : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI fpsText;

	private string filePath;
	private float timer = 0f;
	private float logInterval = 1f;

	void Awake()
	{
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = -1;
	}

	void Start()
    {
		filePath = Path.Combine(Application.persistentDataPath, "fps_log.csv");
		Debug.Log("FPS Log Path: " + filePath); // Skontroluj cestu v konzole

		// Vytvorí súbor, ak neexistuje, a pridá hlavièku
		if (!File.Exists(filePath))
		{
			using (StreamWriter writer = new StreamWriter(filePath, false))
			{
				writer.WriteLine("Time,FPS");
			}
		}
	}

    void Update()
    {
		timer += Time.deltaTime;

		if (timer >= logInterval)
		{
			timer = 0f;

			float fps = 1.0f / Time.deltaTime;
			fpsText.text = "FPS: " + Mathf.RoundToInt(fps).ToString();
			string logEntry = $"{Time.time:F2},{Mathf.RoundToInt(fps)}\n";

			File.AppendAllText(filePath, logEntry);
		}
	}
}
