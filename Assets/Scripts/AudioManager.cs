using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
	#region Singleton
	public static AudioManager instance;

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

	[Header("Audio Sources")]
	[SerializeField] AudioSource backgroundAudioSource;
	[SerializeField] AudioSource SFXSource;
	[Header("Global Audio Clips")]
	[SerializeField] AudioClip backgroundMainMenuAudio;
	[SerializeField] AudioClip backgroundGameAudio;
	[SerializeField] AudioClip buttonsSFX;
	[SerializeField] AudioClip explosionSFX;
	[SerializeField] AudioClip asteroidDestructionSFX;

	void OnEnable()
	{
		LevelLoader.instance.OnLevelLoaded += PlayBackgroundAudio;
	}

	void Start()
	{
		if (backgroundAudioSource == null || SFXSource == null) 
		{ 
			Debug.LogError("No audio source or clip found on Music script"); 
			return;
		}

		LoadAudioSettings();
	}

	public void ToggleSound()
	{
		backgroundAudioSource.mute = !backgroundAudioSource.mute;
		if (!backgroundAudioSource.isPlaying)
		{
			PlayBackgroundAudio(0);
		}

		PlayerPrefs.SetInt("MusicMuted", backgroundAudioSource.mute ? 1 : 0);
		PlayerPrefs.Save();
	}

	void PlayBackgroundAudio(int index)
	{
		AudioClip clip = (index == 0) ? backgroundMainMenuAudio : backgroundGameAudio;

		if (clip == null)
		{
			Debug.LogError("No background audio clip provided.");
			return;
		}

		backgroundAudioSource.Stop();
		backgroundAudioSource.clip = clip;
		backgroundAudioSource.loop = true;
		backgroundAudioSource.Play();
	}

	public void PlaySFX(AudioClip clip)
	{
		if (clip == null) 
		{ 
			Debug.LogError("No audio clip found"); 
			return; 
		}

		if (SFXSource == null)
		{
			Debug.Log(clip.name + " not played because no audio source found");
			return;
		}

		SFXSource.PlayOneShot(clip);
	}

	void LoadAudioSettings()
	{
		int isMuted = PlayerPrefs.GetInt("MusicMuted", 0); // 0 = not muted, 1 = muted
		if (isMuted == 0)
		{
			backgroundAudioSource.mute = false;
			PlayBackgroundAudio(0);
		}
		else
		{
			backgroundAudioSource.mute = true;
		}
	}

	public bool IsMusicPlaying()
	{
		return !backgroundAudioSource.mute && backgroundAudioSource.isPlaying;
	}

	public void PlayDestroySFX()
	{
		PlaySFX(explosionSFX);
	}

	public void PlayAsteroidDestruction()
	{
		PlaySFX(asteroidDestructionSFX);
	}

	void OnDisable()
	{
		LevelLoader.instance.OnLevelLoaded -= PlayBackgroundAudio;
	}
}
