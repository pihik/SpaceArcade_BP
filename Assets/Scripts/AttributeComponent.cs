using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class AttributeComponent : MonoBehaviour
{
	public Action OnZeroHealth;

	[SerializeField] protected int healthAmount = 3;

	Shield shield;
	SpriteRenderer spriteRenderer;

	bool isImmortal = false;

	void Awake()
	{
		shield = GetComponentInChildren<Shield>();
		spriteRenderer = GetComponent<SpriteRenderer>();

		if (!shield || !spriteRenderer)
		{
			Debug.LogError("[AttributeComponent::Awake] Somethings wrong on: " + name);
		}
	}

	virtual protected void HealthDecrease(int amount, bool activateShield)
	{
		if (isImmortal)
		{
			return;
		}

		if (!shield.GetMySpriteRenderer().isVisible)
		{
			healthAmount -= amount;

			StartCoroutine(ChangeColorForSplitSecond());
			if (healthAmount <= 0)
			{
				healthAmount = 0;
				OnZeroHealth?.Invoke();

				return;
			}

			if (activateShield)
			{
				shield.ActivateShieldBlink();
			}
		}
	}

	protected virtual void OnTriggerEnter2D(Collider2D collision)
	{
		int collisionLayerIndex = collision.gameObject.layer;

		if (((InGameHelper.instance.GetProjectileLayer() & 1 << collisionLayerIndex) == 1 << collisionLayerIndex) &&
			collision.TryGetComponent(out Projectile projectileComp))
		{
			if (projectileComp.GetInstigator() == gameObject)
			{
				return;
			}

			HealthDecrease(projectileComp.GetDamage(), false);
		}
		else if (((InGameHelper.instance.GetAsteroidLayer() & 1 << collisionLayerIndex) == 1 << collisionLayerIndex) &&
			collision.TryGetComponent(out Asteroid asteroidComp))
		{
			HealthDecrease(1, true);
		}
	}

	IEnumerator ChangeColorForSplitSecond()
	{
		spriteRenderer.color = Color.red;
		yield return new WaitForSeconds(0.1f);
		spriteRenderer.color = Color.white;
	}

	public Shield GetShieldComponent()
	{
		return shield;
	}

	public void SetIsImmortal(bool value)
	{
		isImmortal = value;
	}

	void OnDestroy()
	{
		StopAllCoroutines();
	}
}
