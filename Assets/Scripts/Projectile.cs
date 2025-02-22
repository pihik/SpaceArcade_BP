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

    void Start()
    {
        Invoke(nameof(Deactivate), timeToDestroy);
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

    void SelfDestruction()
    {
        Destroy(gameObject);
    }

    void Deactivate()
    {
        trail.Clear();
        CancelInvoke();
        shootingObjectPool.ReturnObjectToPool(gameObject);
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