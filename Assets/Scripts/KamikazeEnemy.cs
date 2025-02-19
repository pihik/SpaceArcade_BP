using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class KamikazeEnemy : AdvancedEnemy
{
    [SerializeField] float speed = 1;

    protected override void Update()
    {
        base.Update();

        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        RotateToTarget();

        Vector2 newPosition = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
        transform.position = newPosition;
    }
}
