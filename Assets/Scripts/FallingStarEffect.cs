using System.Collections;
using UnityEngine;

public class FallingStarEffect : MonoBehaviour
{
	//maybe rebuild this to fall to corner of the screen and controll speed and delete falling star duration

	public static FallingStarEffect instance;

	#region Singleton
	private void Awake()
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

	[SerializeField] Gradient[] gradients;
	[SerializeField] SpriteRenderer fallingStar;
	[SerializeField] float minInterval = 2f, maxInterval = 5f;
	[SerializeField] float fallingStarDuration = 1.5f;

	private TrailRenderer trail;

	void Start()
	{
		trail = fallingStar.GetComponent<TrailRenderer>();

		SetStarVisibility(false);
		StartCoroutine(FallingStarEffectLoop());
	}

	IEnumerator FallingStarEffectLoop()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

			MoveFallingStarToCorner();
			SetStarVisibility(true);

			Vector2 targetPosition = GetRandomScreenPoint();
			yield return StartCoroutine(MoveFallingStar(targetPosition));

			SetStarVisibility(false);
		}
	}

	void MoveFallingStarToCorner()
	{
		Vector3 screenCorner = GetRandomScreenCorner();
		fallingStar.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(screenCorner.x, screenCorner.y, 10f));
	}

	Vector3 GetRandomScreenCorner()
	{
		float x = (Random.value > 0.5f) ? 0 : Screen.width;
		float y = (Random.value > 0.5f) ? 0 : Screen.height;
		return new Vector3(x, y, 0);
	}

	Vector2 GetRandomScreenPoint()
	{
		float randomX = Random.Range(0, Screen.width);
		float randomY = Random.Range(0, Screen.height);
		return Camera.main.ScreenToWorldPoint(new Vector3(randomX, randomY, 10f));
	}

	IEnumerator MoveFallingStar(Vector2 targetPosition)
	{
		trail.Clear();
		trail.colorGradient = gradients[Random.Range(0, gradients.Length)];

		float elapsedTime = 0f;
		Vector2 startPosition = fallingStar.transform.position;

		while (elapsedTime < fallingStarDuration)
		{
			float t = elapsedTime / fallingStarDuration;

			Vector2 newPosition = Vector2.Lerp(startPosition, targetPosition, t);

			fallingStar.transform.position = newPosition;

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		fallingStar.transform.position = targetPosition;
	}

	void SetStarVisibility(bool isVisible)
	{
		fallingStar.enabled = isVisible;
	}
}