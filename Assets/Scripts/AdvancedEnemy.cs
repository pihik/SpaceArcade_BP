using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedEnemy : Enemy
{
    [SerializeField] float speed = 3f;

    Transform playerTransform;

    void OnEnable()
    {
        InGameHelper.instance.onNewPlayer += OnNewPlayer;
    }

    protected override void Start()
    {
        base.Start();

        playerTransform = InGameHelper.instance.GetPlayer().transform;

        if (!playerTransform)
        {
            Debug.Log("Player not found! on the object: " + gameObject.name);
        }
    }

    void Update()
    {
        RotateToTarget();
    }

    void OnNewPlayer(Player newPlayer)
    {
        playerTransform = newPlayer.transform;
    }

    void RotateToTarget()
    {
        Vector3 direction = playerTransform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, Time.deltaTime * speed);
    }

    void OnDisable()
    {
        InGameHelper.instance.onNewPlayer -= OnNewPlayer;
    }
}
