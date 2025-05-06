using Cinemachine;
using Photon.Pun;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;

public class Player : MonoBehaviourPun, IPunObservable
{
    #region State Variables

    public PlayerIdleState IdleState { get; private set; }
    public PlayerClientState ClientState { get; private set; }

    #endregion

    #region Public Variables

    public bool debugAnimationBoolName;
    public string animBoolName;

    #endregion

    #region Private Variables

    private CinemachineVirtualCamera playerCamera;
    private Transform playerCameraTransform;

    private Vector3 moveDirection;
    private Quaternion targetRotation;
    private float currentYaw = 0f;

    #endregion

    #region Network Variables

    private string networkAnimBoolName;

    #endregion

    #region Component and Data References

    [SerializeField] Animator animator;
    [SerializeField] PlayerData playerData;
    [SerializeField] CharacterController characterController;

    #endregion

    #region Get References

    public PlayerStateMachine StateMachine;
    public PlayerInputManager InputManager { get; private set; }

    #endregion

    #region Unity Callback Methods

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        InputManager = GetComponent<PlayerInputManager>();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        ClientState = new PlayerClientState(this, StateMachine, playerData, "client");
    }

    private void Start()
    {
        StateMachine.InitializeState(IdleState);
        StateMachine.CurrentState.Enter();

        if (photonView.IsMine)
        {
            SetCustomCamera();
        }
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #endregion

    #region Set Methods

    public void HandleForwardMovement()
    {
        moveDirection = transform.forward * playerData.movementSpeed;
        moveDirection.y -= playerData.gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);

        // Smooth turning toward currentYaw
        Quaternion desiredRotation = Quaternion.Euler(0f, currentYaw, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * playerData.turnSpeed);
    }

    public void HandleConstrainedTurning()
    {
        float inputX = InputManager.MovementInput.x;

        // Only turn if input is held and significant
        if (Mathf.Abs(inputX) > 0.1f)
        {
            float deltaYaw = inputX * playerData.turnAngle * Time.deltaTime;
            float newYaw = currentYaw + deltaYaw;

            // Clamp the new yaw within limits
            newYaw = Mathf.Clamp(newYaw, -playerData.maxTurnLimit, playerData.maxTurnLimit);

            currentYaw = newYaw;
        }
    }

    public void SetPlayerCamera()
    {
        GameObject instance = Instantiate(playerData.playerCinmachineCameraPrefab, Vector3.zero, Quaternion.identity);
        playerCamera = instance.GetComponentInChildren<CinemachineVirtualCamera>();
        playerCamera.Follow = transform;
        playerCamera.LookAt = transform;
        playerCameraTransform = instance.transform;
    }

    public void SetCustomCamera()
    {
        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        cameraController.SetTarget(transform);
    }

    public void SetMovement(float movementSpeed)
    {
        if (playerCameraTransform == null) return;

        Vector3 moveDirection;
        moveDirection = playerCameraTransform.forward * InputManager.MovementInput.y;
        moveDirection = moveDirection + playerCameraTransform.right * InputManager.MovementInput.x;
        moveDirection.Normalize();
        moveDirection = moveDirection * movementSpeed;

        Vector3 movementVelocity = moveDirection;
        characterController.Move(movementVelocity * Time.deltaTime);
    }

    public void SetRotation(float rotationSpeed)
    {
        if (playerCameraTransform == null) return;

        Vector3 targetDirection = Vector3.zero;
        targetDirection = playerCameraTransform.forward * InputManager.MovementInput.y;
        targetDirection = targetDirection + playerCameraTransform.right * InputManager.MovementInput.x;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = playerRotation;
    }

    #endregion

    #region Get Methods

    public Animator GetAnimator() { return animator; }

    #endregion

    #region Photon Serializable

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(animBoolName);
        }
        else
        {
            networkAnimBoolName = (string)stream.ReceiveNext();
        }
        
    }

    #endregion

}
