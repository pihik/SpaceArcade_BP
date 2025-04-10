using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour
{
	[SerializeField] float shieldDuration = 3f;
	[SerializeField] float blinkInterval = 0.2f;

	SpriteRenderer spriteRenderer;
	float timer = 0;

	Coroutine shieldBlinkRoutine;

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Start()
	{
		spriteRenderer.enabled = false;
	}

	public void SetShieldTime(float time)
	{
		shieldDuration = time;
	}

	public void ActivateShieldBlink()
	{
		spriteRenderer.enabled = true;
		shieldBlinkRoutine = StartCoroutine(ShieldBlinkRoutine());
	}

	public void ActivateShield(float timeDuration)
	{
		spriteRenderer.enabled = true;

		if (shieldBlinkRoutine != null)
		{
			StopCoroutine(shieldBlinkRoutine);
		}

		StartCoroutine(ShieldRoutine(timeDuration));
	}

	IEnumerator ShieldBlinkRoutine()
	{
		timer = 0;

		while (timer < shieldDuration)
		{
			SetBlinkColor(timer);

			timer += Time.deltaTime;
			yield return null;
		}

		ResetAlphaColor();

		spriteRenderer.enabled = false;
		shieldBlinkRoutine = null;
	}

	IEnumerator ShieldRoutine(float duration)
	{
		timer = 0;
		ResetAlphaColor();

		while (timer < duration)
		{
			timer += Time.deltaTime;
			yield return null;
		}

		spriteRenderer.enabled = false;
	}

	void SetBlinkColor(float timer)
	{
		float alpha = Mathf.PingPong(timer / blinkInterval, 1);
		Color newColor = spriteRenderer.color;
		newColor.a = alpha;
		spriteRenderer.color = newColor;
	}

	public SpriteRenderer GetMySpriteRenderer()
	{
		return spriteRenderer;
	}

	void ResetAlphaColor()
	{
		Color finalColor = spriteRenderer.color;
		finalColor.a = 1;
		spriteRenderer.color = finalColor;
	}

	void OnDisable()
	{
		StopAllCoroutines();
	}
}
