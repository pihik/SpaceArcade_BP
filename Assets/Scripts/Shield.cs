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
        gameObject.SetActive(false);
    }

    public void SetShieldTime(float time)
    {
        shieldDuration = time;
    }

    public void ActivateShieldBlink()
    {
        gameObject.SetActive(true);
        shieldBlinkRoutine = StartCoroutine(ShieldBlinkRoutine());
    }

    public void ActivateShield(float timeDuration)
    {
        gameObject.SetActive(true);

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

        gameObject.SetActive(false);
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

        gameObject.SetActive(false);
    }

    void SetBlinkColor(float timer)
    {
        float alpha = Mathf.PingPong(timer / blinkInterval, 1);
        Color newColor = spriteRenderer.color;
        newColor.a = alpha;
        spriteRenderer.color = newColor;
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
