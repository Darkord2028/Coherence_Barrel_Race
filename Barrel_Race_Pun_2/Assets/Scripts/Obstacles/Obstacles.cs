using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public ObstacleType obstacleType;
}
public enum ObstacleType
{
    Boost,
    SpeedSlow,
    ReturnToCheckpoint
}
