using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : AdvancedEnemy
{
	[SerializeField] float moveSpeed = 3f;

	[Header ("Move Intervals")]
	[SerializeField] int maxMoveInterval = 6;
	[SerializeField] int minMoveInterval = 2;

	[Header("Shield Intervals")]
	[SerializeField] int maxShieldInterval = 15;
	[SerializeField] int minShieldInterval = 6;

	// guns with time to shoot
	List<Gun[]> gunsList = new List<Gun[]>();

	Camera mainCamera;
	Shield shieldComponent;

	protected override void Start()
	{
		base.Start();

		mainCamera = Camera.main;
		shieldComponent = GetComponentInChildren<Shield>();

		if (!mainCamera || !shieldComponent)
		{
			Debug.LogError("[Boss::Start] Main camera or shield component not found on the object: " + gameObject.name);
		}

		InitializeGuns();

		StartCoroutine(MoveAtIntervals());
		StartCoroutine(ShieldRecharging());
	}

	void InitializeGuns()
	{
		gunsList.Add(guns);

		foreach (ProjectileType type in Enum.GetValues(typeof(ProjectileType)))
		{
			AddGunsToList(GetGunOnType(type));
		}
	}

	void AddGunsToList(Gun[] guns)
	{
		if (guns.Length > 0)
		{
			gunsList.Add(guns);
		}
	}

	protected override void Shoot()
	{
		int randomGunTypeIndex = UnityEngine.Random.Range(0, gunsList.Count);
		guns = gunsList[randomGunTypeIndex];

		base.Shoot();
	}

	IEnumerator MoveAtIntervals()
	{
		while (true)
		{
			Vector2 targetPosition = GetRandomPosition();

			// Move to the target position smoothly
			yield return StartCoroutine(MoveToPosition(targetPosition));

			// Wait for a random interval before moving again
			float waitTime = UnityEngine.Random.Range(minMoveInterval, maxMoveInterval);
			yield return new WaitForSeconds(waitTime);
		}
	}

	IEnumerator ShieldRecharging()
	{
		while (true)
		{
			float shieldTime = UnityEngine.Random.Range(minShieldInterval, maxShieldInterval);
			yield return new WaitForSeconds(shieldTime);

			shieldComponent.ActivateShield(shieldTime);
		}
	}

	IEnumerator MoveToPosition(Vector2 targetPosition)
	{
		while ((Vector2)transform.position != targetPosition)
		{
			transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
			yield return null;
		}
	}

	Vector2 GetRandomPosition()
	{
		float camHeight = mainCamera.orthographicSize;
		float camWidth = camHeight * mainCamera.aspect;

		float randomX = UnityEngine.Random.Range(mainCamera.transform.position.x - camWidth, mainCamera.transform.position.x + camWidth);
		float randomY = UnityEngine.Random.Range(mainCamera.transform.position.y - camHeight, mainCamera.transform.position.y + camHeight);

		return new Vector2(randomX, randomY);
	}

	Gun[] GetGunOnType(ProjectileType gunType)
	{
		return Array.FindAll(guns, gun => gun.GetProjectileType() == gunType);
	}
}
