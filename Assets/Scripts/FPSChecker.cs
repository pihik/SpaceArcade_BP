using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPSChecker : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI fpsText;

	private string filePath;
	private float timer = 0f;
	private float logInterval = 1f;

	void Awake()
	{
		QualitySettings.vSyncCount = 1;
		Application.targetFrameRate = 144;
	}

	void Start()
	{
		filePath = Path.Combine(Application.persistentDataPath, "fps_log.csv");

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

	public void VSyncSwitch(Text text)
	{
		bool vSyncOff = (QualitySettings.vSyncCount == 0) ? true : false;

		QualitySettings.vSyncCount = (vSyncOff) ? 1 : 0;
		Application.targetFrameRate = (vSyncOff) ? 144 : -1;
		text.text = "VSYNC " + ((vSyncOff) ? "<color=green>ON</color>" : "<color=red>OFF</color>");
	}

	public void TextSwitch(bool bActivate)
	{
		fpsText.enabled = bActivate;
	}
}
