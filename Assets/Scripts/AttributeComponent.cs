using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CircleCollider2D))]
public class AttributeComponent : MonoBehaviour
{
    public Action OnZeroHealth;

    [SerializeField] protected int healthAmount = 3;

    Shield shield;

    bool isImmortal = false;

    void Awake()
    {
        shield = GetComponentInChildren<Shield>();
        
        if (shield == null)
        {
            Debug.LogError("Shield component not found");
        }
    }

    virtual protected void HealthDecrease(int amount, bool activateShield)
    {
        if (isImmortal)
        {
            return;
        }

        if (!shield.isActiveAndEnabled)
        {
            healthAmount -= amount;

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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Projectile>(out Projectile projectileComp))
        {
            if (projectileComp.GetInstigator() == gameObject)
            {
                return;
            }
            HealthDecrease(projectileComp.GetDamage(), false);
        }
        else if (collision.TryGetComponent<Asteroid>(out Asteroid asteroidComp))
        {
            HealthDecrease(1, true);
        }
    }

    public Shield GetShieldComponent()
    {
        return shield;
    }

    public void SetIsImmortal(bool value)
    {
        isImmortal = value;
    }
}
