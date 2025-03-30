using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Gun : MonoBehaviour
{
	[Header("Gun References")]
	[SerializeField] float yOffset;

	[Header("Gun Type")]
	[SerializeField] ProjectileType projectileType;

	[Header("Gun Properties")]
	[SerializeField] float fireRate = 0.5f;
	[SerializeField] bool isActive = true;

	[SerializeField] AudioClip shootSound;

	Shoot_ObjectPool shootingObjectPool;
	SpriteRenderer spriteRenderer;

	bool canShoot = true;

	void Awake()
	{
		shootingObjectPool = ProjectilePoolsStorage.instance.GetProjectilePool(projectileType);
		spriteRenderer = GetComponent<SpriteRenderer>();

		ActiveSwitch(isActive);
	}

	public void ActiveSwitch(bool isActive)
	{
		if (this.isActive != isActive)
		{
			this.isActive = isActive;
			canShoot = isActive;
		}
		spriteRenderer.enabled = isActive;
	}

	public void Shoot()
	{
		if (isActive && canShoot && shootingObjectPool)
		{
			StartCoroutine(ShootRoutine());
		}
	}

	IEnumerator ShootRoutine()
	{
		canShoot = false;

		Projectile projectile = shootingObjectPool.GetObjectFromPool();
		projectile.SetInstigator(transform.parent.gameObject);

		if (projectile)
		{
			Vector3 positionOffset = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);

			projectile.transform.position = positionOffset;
			projectile.transform.rotation = transform.rotation;

			PlayShootAudio();
		}

		yield return new WaitForSeconds(fireRate);
		canShoot = true;
	}

	void PlayShootAudio()
	{
		if (shootSound)
		{
			AudioManager.instance.PlaySFX(shootSound);
		}
	}

	void OnDisable()
	{
		StopAllCoroutines();
	}

	public bool IsActivate()
	{
		return isActive;
	}

	public ProjectileType GetProjectileType()
	{
		return projectileType;
	}	

	public void IncreaseFireRate(float value) // ah it's flipped, so we actually need to decrease the fire rate
	{
		float newFireRate = fireRate - value;
		fireRate = (newFireRate < 0.02f) ? 0.02f : newFireRate;
	}
}
public enum ProjectileType 
{
	BlueLaser_slow,
	BlueLaser_fast,
	BlueLaser_Big,
	OrangeLaser_slow,
	OrangeLaser_fast,
	OrangeLaser_Big,
	RedLaser_slow,
	RedLaser_fast,
	RedLaser_Big
}