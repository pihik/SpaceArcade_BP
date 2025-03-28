using System.Collections.Generic;
using UnityEngine;

public class Shoot_ObjectPool : MonoBehaviour
{
	[SerializeField] GameObject objectPrefab;
	[SerializeField] int poolSize = 20;
	[SerializeField] ProjectileType projectileType;

	Queue<Projectile> projectilePool = new Queue<Projectile>();

	void Start()
	{
		for (int i = 0; i < poolSize; i++)
		{
			GameObject obj = Instantiate(objectPrefab, GetPoolStorage());
			obj.SetActive(false);

			Projectile projectile = obj.GetComponent<Projectile>();
			projectile.SetObjectPool(this);
			projectilePool.Enqueue(projectile);
		}
	}

	public Projectile GetObjectFromPool()
	{
		if (projectilePool.Count > 0)
		{
			Projectile projectile = projectilePool.Dequeue();
			projectile.gameObject.SetActive(true);

			return projectile;
		}
		else
		{
			GameObject obj = Instantiate(objectPrefab, GetPoolStorage());
			obj.SetActive(true);

			Projectile projectile = obj.GetComponent<Projectile>();
			projectile.SetObjectPool(this);
			projectilePool.Enqueue(projectile);

			return projectile;
		}
	}

	public void ReturnObjectToPool(Projectile projectile)
	{
		projectile.gameObject.SetActive(false);
		projectilePool.Enqueue(projectile);
	}

	Transform GetPoolStorage()
	{
		return InGameHelper.instance.GetProjectilesParent();
	}

	public ProjectileType GetProjectileType()
	{
		return projectileType;
	}
}
