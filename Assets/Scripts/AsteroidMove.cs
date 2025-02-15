using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMove : MonoBehaviour
{
    Rigidbody2D myRB;

    [SerializeField] float Speed = 5f;
    float randomRotation = 50f;

    float speedMultiplier = 1f;

    void Awake()
    {
        myRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        RandomRotation();
    }

    public void RandomMove(bool debuffedSpeed)
    {
        float randomSpeed = Random.Range(-Speed * speedMultiplier, Speed * speedMultiplier);
        if (debuffedSpeed)
        {
            randomSpeed /= 2;
        }
        float randomNumber = Random.Range(1f, 0.2f);
        Vector2 randomMove = new Vector2(Random.Range(-randomNumber, randomNumber), Random.Range(-randomNumber, randomNumber));
        
        myRB.linearVelocity = randomMove * randomSpeed;
    }

    public void SetDirection(Direction direction)
    {
        float randomSpeed = Random.Range(Speed * 0.5f * speedMultiplier, Speed * 1.5f * speedMultiplier); // Ensure positive speed

        if (direction == Direction.None)
        {
            RandomMove(false);
            return;
        }

        float angle = GetRandomAngle(direction);
        Vector2 moveDirection = AngleToVector2(angle);

        myRB.linearVelocity = moveDirection * randomSpeed;
    }

    float GetRandomAngle(Direction direction)
    {
        return direction switch
        {
            Direction.Up_Left => Random.Range(90f, 180f),
            Direction.Up_Right => Random.Range(0f, 90f),
            Direction.Down_Left => Random.Range(180f, 270f),
            Direction.Down_Right => Random.Range(270f, 360f),
            _ => 0f
        };
    }

    Vector2 AngleToVector2(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    void RandomRotation()
    {
        transform.Rotate(0, 0, randomRotation * Time.deltaTime);
    }

    public void SetSpeedMultiplier(float multiplier = 1f)
    {
        speedMultiplier = multiplier;
    }
}
