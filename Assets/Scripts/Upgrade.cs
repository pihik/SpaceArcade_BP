using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
public class Upgrade : MonoBehaviour
{
    enum UpgradeType { Health, Gun, Shield, AsteroidStop }//Damage, Speed, }

    [SerializeField] UpgradeType upgradeType;
    [SerializeField] float destroyTime = 5f;

    CircleCollider2D circleCollider;

    void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;
    }

    void Start()
    {
        Invoke(nameof(SelfDestroy), destroyTime);
    }

    protected virtual void ApplyUpgrade(Player player) 
    { 
        switch(upgradeType)
        {
            case UpgradeType.Health:
                player.GetAttributeComponent().IncreaseHealth();
                break;
            case UpgradeType.Shield:
                player.GetAttributeComponent().GetShieldComponent().ActivateShield(5);
                break;
            case UpgradeType.Gun:
                player.AddGun();
                break;
            case UpgradeType.AsteroidStop:
                GameManager.instance.AsteroidStopEvent?.Invoke(0, 8);
                break;
            default:
                break;
                /*
            case UpgradeType.Speed:
                player.GetAttributeComponent().SpeedIncrease(1);
                break;
            case UpgradeType.Damage:
                player.GetAttributeComponent().DamageIncrease(1);
                break;
*/
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        int collisionLayerIndex = collision.gameObject.layer;

        if ((InGameHelper.instance.GetPlayerLayer() & 1 << collisionLayerIndex) == 1 << collisionLayerIndex)
        {
            ApplyUpgrade(InGameHelper.instance.GetPlayer());
            Destroy(gameObject);
        }
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}