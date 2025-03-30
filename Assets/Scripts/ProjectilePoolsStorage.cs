using UnityEngine;

public class ProjectilePoolsStorage : MonoBehaviour
{
	#region Singleton
	public static ProjectilePoolsStorage instance;
	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;
	}
	#endregion

	[SerializeField] Shoot_ObjectPool[] projectilePools;

	public Shoot_ObjectPool GetProjectilePool(ProjectileType projectileType)
	{
		foreach (Shoot_ObjectPool pool in projectilePools)
		{
			if (pool.GetProjectileType() == projectileType)
			{
				return pool;
			}
		}
		return null;
	}
}