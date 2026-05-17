using UnityEngine;

public class Asteroid : MonoBehaviour
{
	[Tooltip("Score will increase after hit to the asteroid")]
	[SerializeField] int scoreToIncrease = 35;
	[Tooltip("Bigger number = bigger asteroid")]
	[SerializeField, Range(1, 3)] int asteroidSize = 1;

	[SerializeField] float selfDestructionTime = 100f;

	[SerializeField] ParticleSystem destroyEffect;

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

		if (((InGameHelper.instance.GetProjectileLayer() & 1 << collisionLayerIndex) == 1 << collisionLayerIndex) &&
			collision.TryGetComponent(out Projectile projectileComp))
		{
			if (projectileComp.GetInstigator().gameObject == InGameHelper.instance.GetPlayer().gameObject)
			{
				GameManager.instance.ScoreIncrease(scoreToIncrease);
			}
		}

		if (!((InGameHelper.instance.GetShredderLayer() & 1 << collisionLayerIndex) == 1 << collisionLayerIndex))
		{
			if (asteroidSize > 1)
			{
				Scatter();
			}
		}

		PlayDestroyClip();
		Deactivate();
	}
	
	void Scatter()
	{
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

		if (Asteroid.TryGetComponent(out AsteroidMove asteroidMoveComp))
		{
			asteroidMoveComp.RandomMove(true);
		}
	}

	public void OnClicked()
	{
		if (asteroidSize > 1)
		{
			Scatter();
		}
		PlayDestroyClip();
		Deactivate();
	}

	public void SetObjectPool(Asteroid_ObjectPool objectPool)
	{
		this.objectPool = objectPool;
	}

	void Deactivate()
	{
		CancelInvoke();
		objectPool?.ReturnObjectToPool(gameObject);
	}

	void SelfDestruction()
	{
		Destroy(gameObject);
	}

    void PlayDestroyClip()
    {
        AudioManager.instance.PlayAsteroidDestruction();

        GameObject effectObj = Instantiate(
            destroyEffect.gameObject,
            transform.position,
            Quaternion.identity
        );

        float multiplier = asteroidSize switch
        {
            1 => 0.6f,
            2 => 1.2f,
            3 => 2f,
            _ => 1f
        };

        ParticleSystem[] systems = effectObj.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem ps in systems)
        {
            var main = ps.main;

            main.startSizeMultiplier *= multiplier;
            main.startSpeedMultiplier *= multiplier;
        }
    }
}
