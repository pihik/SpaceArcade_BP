using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AttributeComponent))]
public class Enemy : SpaceshipBase
{
    [Tooltip("How much score will increase for me!")]
    [SerializeField] int scoreValue = 50;

    [Tooltip("This two variables represents time. It will canculate random number between this two time variables and sets timer for next shot")]
    [SerializeField] float minShootInterval = 2f;
    [SerializeField] float maxShootInterval = 6f;

    protected AttributeComponent attributeComponent;

    float timer = 0;

    protected override void Awake()
    {
        base.Awake();
        attributeComponent = GetComponent<AttributeComponent>();

        attributeComponent.OnZeroHealth += ZeroHealth;
        myCollider.enabled = false;
    }

    protected override void Start()
    {
        RotateToPlayer();
        StartCoroutine(SpawnedRoutine());
    }

    protected IEnumerator ShootingRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(ShootTimeDelay());
            Shoot();
        }
    }

    protected virtual void Shoot()
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

    IEnumerator SpawnedRoutine()
    {
        timer = 0;

        while (timer < 3)
        {
            SetBlinkColor(timer);

            timer += Time.deltaTime;
            yield return null;
        }

        myCollider.enabled = true;

        StartCoroutine(ShootingRoutine());
    }

    void SetBlinkColor(float timer)
    {
        Color color = myRenderer.color;
        color.a = Mathf.PingPong(timer * 3, 1);
        myRenderer.color = color;
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
