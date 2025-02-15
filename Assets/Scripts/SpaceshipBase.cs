using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public abstract class SpaceshipBase : MonoBehaviour
{
    protected ParticleSystem explosionEffect;
    protected Gun[] guns;

    Collider2D myCollider;

    protected virtual void Awake()
    {
        // watch out if there are more then one of these components, then you'll need reference for it
        myCollider = GetComponent<Collider2D>();
        guns = GetComponentsInChildren<Gun>();
        explosionEffect = GetComponentInChildren<ParticleSystem>();

        myCollider.isTrigger = true;

        if (!myCollider || !explosionEffect)
        {
            Debug.LogError("[SpaceshipBase::Awake] Collider or explosion effect not found on the object: " + gameObject.name);
            return;
        }

        if (guns.Length == 0)
        {
            Debug.Log("[SpaceshipBase::Awake] No gun found on the object: " + gameObject.name);
        }
    }
    protected virtual void Start() 
    { 
        explosionEffect.Stop();
    }

    protected virtual void ZeroHealth()
    {
        explosionEffect.transform.parent = null;
        explosionEffect.Play();

        AudioManager.instance.PlayDestroySFX();
    }
}
