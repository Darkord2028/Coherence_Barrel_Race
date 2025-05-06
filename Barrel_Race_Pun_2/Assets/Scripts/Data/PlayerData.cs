using Cinemachine;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Player Camera")]
    public GameObject playerCinmachineCameraPrefab;

    [Header("Move State")]
    public float movementSpeed;
    public float rotationSpeed;
    [Tooltip("Do not add negative sign")]
    public float gravity;

    [Header("Turning Settings")]
    public float turnAngle = 45f;
    public float turnSpeed = 5f;
    public float turnCooldown = 0.5f;
    public float maxTurnLimit = 90f;
}
