using System.IO;
using TMPro;
using UnityEngine;

public class FPSChecker : MonoBehaviour
{
	#region Singleton
	public static FPSChecker instance;

	void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad(gameObject);
	}
	#endregion

	[SerializeField] TextMeshProUGUI fpsText;

	private string filePath;
	private float timer = 0f;
	private float logInterval = 1f;

	void Start()
	{
		LoadSettings();

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

	public void VSyncSwitch()
	{
		bool vSyncOff = (QualitySettings.vSyncCount == 0) ? true : false;

		QualitySettings.vSyncCount = (vSyncOff) ? 1 : 0;
		Application.targetFrameRate = (vSyncOff) ? 144 : -1;

		SaveSettings();
	}

	public void TextSwitch(bool bActivate)
	{
		fpsText.enabled = bActivate;
	}

	void SaveSettings()
	{
		PlayerPrefs.SetInt("VSync", QualitySettings.vSyncCount);
		PlayerPrefs.SetInt("TargetFPS", Application.targetFrameRate);
		PlayerPrefs.Save();
	}

	void LoadSettings()
	{
		int vsync = PlayerPrefs.GetInt("VSync", 1);
		int targetFPS = PlayerPrefs.GetInt("TargetFPS", 144);

		QualitySettings.vSyncCount = vsync;
		Application.targetFrameRate = targetFPS;
	}

	public bool IsVSyncEnabled()
	{
		return QualitySettings.vSyncCount == 1;
	}
}
