using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Projectile : MonoBehaviour
{
    Rigidbody2D myRB;
    Shoot_ObjectPool shootingObjectPool;

    [SerializeField] int damage = 1;
    [SerializeField] float speed = 300f;
    [SerializeField] float timeToDestroy = 1f;

    GameObject instigator;

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