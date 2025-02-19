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

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        int collisionLayerIndex = collision.gameObject.layer;

        if ((InGameHelper.instance.GetEnemyLayer() & 1 << collisionLayerIndex) == 1 << collisionLayerIndex)
        {
            HealthDecrease(3, true);
        }
    }
}
