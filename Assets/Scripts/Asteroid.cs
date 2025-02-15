using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [Tooltip("Score will increase after hit to the asteroid")]
    [SerializeField] int scoreToIncrease = 35;
    [Tooltip("Bigger number = bigger asteroid")]
    [SerializeField, Range(1, 3)] int asteroidSize = 1;

    [SerializeField] float selfDestructionTime = 100f;

    Asteroid_ObjectPool objectPool;
    Asteroid_ObjectPool nextObjectPool;

    void Start()
    {
        if (asteroidSize > 1)
        {
            nextObjectPool = InGameHelper.instance.GetAsteroidPool(asteroidSize - 2);
        }

        Invoke(nameof(Deactivate), selfDestructionTime);
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        int collisionLayerIndex = collision.gameObject.layer;

        if (collision.TryGetComponent<Projectile>(out Projectile projectileComp))
        {
            if (projectileComp.GetInstigator().gameObject == InGameHelper.instance.GetPlayer().gameObject)
            {
                GameManager.instance.ScoreIncrease(scoreToIncrease);
            }
        }

        if (!((InGameHelper.instance.GetShredderLayer() & 1 << collisionLayerIndex) == 1 << collisionLayerIndex))
        {
            PlayDestroyClip();

            if (asteroidSize > 1)
            {
                Scatter();
                return;
            }
        }

        Deactivate();
    }
    
    void Scatter()
    {
        PlayDestroyClip();
        Deactivate();

        int spawnAmount = 0;

        switch (asteroidSize)
        {
            case 1:
                return;
            case 2:
                spawnAmount = 3;
                break;
            case 3:
                spawnAmount = 2;
                break;
            default:
                break;
        }
        for (int i = 0; i < spawnAmount; i++)
        {
            ActivateSmallerAsteroid();
        }
    }

    void ActivateSmallerAsteroid()
    {
        if (nextObjectPool == null)
        {
            Debug.Log("nextObjectPool is null...... index = " + asteroidSize);
            return;
        }

        GameObject Asteroid = nextObjectPool.GetObjectFromPool();
        Asteroid.transform.position = transform.position;

        AsteroidMove AsteroidMoveComp = Asteroid.GetComponent<AsteroidMove>();
        if (AsteroidMoveComp != null)
        {
            AsteroidMoveComp.RandomMove(true);
        }
    }

    public void SetObjectPool(Asteroid_ObjectPool objectPool)
    {
        this.objectPool = objectPool;
    }

    void Deactivate()
    {
        CancelInvoke();
        objectPool.ReturnObjectToPool(gameObject);
    }

    void SelfDestruction()
    {
        Destroy(gameObject);
    }

    void PlayDestroyClip()
    {
        AudioManager.instance.PlayAsteroidDestruction();
    }
}
