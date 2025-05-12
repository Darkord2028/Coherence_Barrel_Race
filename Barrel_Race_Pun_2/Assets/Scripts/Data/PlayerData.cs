using Cinemachine;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Player Camera")]
    public CinemachineVirtualCamera playerCinmachineCameraPrefab;

    [Header("Accelerate State")]
    public float acceleration;
    public float deceleration;
    public float turnSpeed;

    [Header("Check Config")]
    public float groundCheckDistance;
    public LayerMask whatIsGround;

}
