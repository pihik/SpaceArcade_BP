using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Tooltip("Asteroids will have starting direction after spawn and this direction will be between this two directions")]
    [SerializeField] Direction direction;

    public Direction GetCurrentDirection()
    {
        return direction;
    }
}

public enum Direction
{
    None,
    Up_Left,
    Up_Right,
    Down_Left,
    Down_Right
}