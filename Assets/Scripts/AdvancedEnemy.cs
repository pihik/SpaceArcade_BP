using UnityEngine;

public class AdvancedEnemy : Enemy
{
    [SerializeField] float rotatingSpeed = 3f;

    protected Transform playerTransform;

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
            Debug.Log("[AdvancedEnemy::Start] Player not found! on the object: " + name);
        }
    }

    protected virtual void Update()
    {
        RotateToTarget();
    }

    void OnNewPlayer(Player newPlayer)
    {
        playerTransform = newPlayer.transform;
    }

    protected void RotateToTarget()
    {
        Vector3 direction = playerTransform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, Time.deltaTime * rotatingSpeed);
    }

    void OnDisable()
    {
        InGameHelper.instance.onNewPlayer -= OnNewPlayer;
    }
}
