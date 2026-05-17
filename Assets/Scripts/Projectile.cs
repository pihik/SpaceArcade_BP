using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float speed = 300f;
    [SerializeField] float timeToDestroy = 1f;

    Shoot_ObjectPool shootingObjectPool;
    GameObject instigator;
    TrailRenderer trail;

    void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        // Start fresh: disable emitting so it doesn't log the old position
        if (trail != null)
        {
            trail.emitting = false;
            trail.Clear();
        }

        // Use Invoke instead of Start because Start only runs once in a prefab's lifecycle,
        // whereas OnEnable runs every time it leaves the object pool.
        Invoke(nameof(Deactivate), timeToDestroy);
    }

    // Call this AFTER moving the bullet to the gun tip
    public void ResetTrail()
    {
        if (trail != null)
        {
            trail.Clear();
            trail.emitting = true;
        }
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == instigator)
        {
            return;
        }

        Deactivate();
    }

    void Deactivate()
    {
        if (trail != null)
        {
            trail.emitting = false;
            trail.Clear();
        }
        CancelInvoke();
        shootingObjectPool.ReturnObjectToPool(this);
    }

    public void SetObjectPool(Shoot_ObjectPool ObjectPoolComponent)
    {
        shootingObjectPool = ObjectPoolComponent;
    }

    public void SetInstigator(GameObject instigator)
    {
        this.instigator = instigator;
    }

    public GameObject GetInstigator()
    {
        return instigator;
    }

    public int GetDamage()
    {
        return damage;
    }
}