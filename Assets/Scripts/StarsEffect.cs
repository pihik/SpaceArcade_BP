using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class StarsEffect : MonoBehaviour
{
	public static StarsEffect instance;

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

	[SerializeField] RawImage[] stars;
	[SerializeField] float fadeDuration = 0.5f;
	[SerializeField] float starDuration = 1f;
	[SerializeField] float minInterval = 2f, maxInterval = 5f;

	RectTransform canvasRect;

	void Start()
	{
		canvasRect = GetComponent<RectTransform>();

		foreach (RawImage star in stars)
		{
			Color c = star.color;
			c.a = 0f;
			star.color = c;
		}

		StartCoroutine(StarEffectLoop());
	}

	IEnumerator StarEffectLoop()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

			RawImage star = stars[Random.Range(0, stars.Length)];
			MoveStarToRandomPosition(star);

			yield return StartCoroutine(FadeStar(star, 0f, 1f, fadeDuration));
			yield return new WaitForSeconds(starDuration);
			yield return StartCoroutine(FadeStar(star, 1f, 0f, fadeDuration));
		}
	}

	void MoveStarToRandomPosition(RawImage star)
	{
		RectTransform starRect = star.GetComponent<RectTransform>();

		float randomX = Random.Range(-canvasRect.rect.width / 2, canvasRect.rect.width / 2);
		float randomY = Random.Range(-canvasRect.rect.height / 2, canvasRect.rect.height / 2);

		starRect.anchoredPosition = new Vector2(randomX, randomY);
	}

	IEnumerator FadeStar(RawImage star, float startAlpha, float endAlpha, float duration)
	{
		float elapsedTime = 0f;
		Color color = star.color;

		while (elapsedTime < duration)
		{
			color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
			star.color = color;
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		color.a = endAlpha;
		star.color = color;
	}
}
