using System;
using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

[RequireComponent(typeof(AttributeComponent))]
public class Enemy : SpaceshipBase
{
    [Tooltip("How much score will increase for me!")]
    [SerializeField] int scoreValue = 50;

    [Tooltip("This two variables represents time. It will canculate random number between this two time variables and sets timer for next shot")]
    [SerializeField] float minShootInterval = 2f;
    [SerializeField] float maxShootInterval = 6f;

    AttributeComponent attributeComponent;

    // TODO
    // kamikazze enemy
    // Mining enemy
    // Heavy enemy
    // boss fight with enemy stationary space station

    protected override void Awake()
    {
        base.Awake();
        attributeComponent = GetComponent<AttributeComponent>();

        if (!attributeComponent)
        {
            Debug.LogError("AttributeComponent not found on the object: " + gameObject.name);
            return;
        }

        attributeComponent.OnZeroHealth += ZeroHealth;
    }

    protected override void Start()
    {
        RotateToPlayer();
        StartCoroutine(ShootingRoutine());
    }
    protected IEnumerator ShootingRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(ShootTimeDelay());
            Shoot();
        }
    }

    void Shoot()
    {
        foreach (Gun gun in guns)
        {
            gun.Shoot();
        }
    }

    float ShootTimeDelay()
    {
        return UnityEngine.Random.Range(minShootInterval, maxShootInterval);
    }

    void RotateToPlayer()
    {
        Player player = InGameHelper.instance.GetPlayer();

        if (!player)
        {
            Debug.LogError("Player not found");
            return;
        }

        transform.rotation = Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position);
    }

    protected override void ZeroHealth()
    {
        base.ZeroHealth();

        GameManager.instance.DecreaseNumberOfEnemies();
        GameManager.instance.ScoreIncrease(scoreValue);

        Destroy(gameObject);
    }

    protected void OnDestroy()
    {
        StopAllCoroutines();
    }

    void OnDisable()
    {
        attributeComponent.OnZeroHealth -= ZeroHealth;
    }
}
