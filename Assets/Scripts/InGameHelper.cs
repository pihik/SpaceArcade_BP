using System;
using UnityEngine;

public class InGameHelper : MonoBehaviour
{
	#region Singleton
	public static InGameHelper instance;

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

	public Action<Player> onNewPlayer;

	[Header("Script References")]
	[SerializeField] Player player;
	[Tooltip("In order of asteroid size in object pool component")]
	[SerializeField] Asteroid_ObjectPool[] asteroidPool;

	[Header("File References")]
	[SerializeField] GameObject DefaultStorage;
	[SerializeField] GameObject ProjectilesParent;
	[SerializeField] GameObject AsteroidsParent;

	[Header("Layers")]
	[SerializeField] LayerMask playerLayer;
	[SerializeField] LayerMask projectileLayer;
	[SerializeField] LayerMask asteroidLayer;
	[SerializeField] LayerMask shredderLayer;
	[SerializeField] LayerMask enemyLayer;
	[SerializeField] LayerMask upgradeLayer;

	public void SetPlayer(Player newPlayer)
	{
		player = newPlayer;
		onNewPlayer?.Invoke(newPlayer);
	}

	public Player GetPlayer()
	{
		return player;
	}

	public Asteroid_ObjectPool[] GetAsteroidPools()
	{
		return asteroidPool;
	}

	public Asteroid_ObjectPool GetAsteroidPool(int index)
	{
		return asteroidPool[index];
	}

	public Transform GetDefaultStorage()
	{
		return DefaultStorage.transform;
	}

	public Transform GetProjectilesParent()
	{
		return ProjectilesParent.transform;
	}

	public Transform GetAsteroidsParent()
	{
		return AsteroidsParent.transform;
	}

	public LayerMask GetPlayerLayer()
	{
		return playerLayer;
	}

	public LayerMask GetProjectileLayer()
	{
		return projectileLayer;
	}

	public LayerMask GetAsteroidLayer()
	{
		return asteroidLayer;
	}

	public LayerMask GetShredderLayer()
	{
		return shredderLayer;
	}

	public LayerMask GetEnemyLayer()
	{
		return enemyLayer;
	}

	public LayerMask GetUpgradeLayer()
	{
		return upgradeLayer;
	}
}