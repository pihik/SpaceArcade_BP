using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayersAttributeComponent : AttributeComponent
{
    public static Action<int> OnHealthChange;
    public static Action OnPlayerDestroy;

    void Start()
    {
        OnHealthChange?.Invoke(healthAmount);
    }

    protected override void HealthDecrease(int amount, bool activateShield)
    {
        base.HealthDecrease(amount, activateShield);
        OnHealthChange?.Invoke(healthAmount);
    }

    public void IncreaseHealth()
    {
        healthAmount++;
        OnHealthChange?.Invoke(healthAmount);
    }
}
