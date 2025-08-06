using Cinemachine;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviourPun, IPunObservable
{
    #region State Variables

    public PlayerIdleState IdleState { get; private set; }
    public PlayerAccelerateState AccelerateState { get; private set; }
    public PlayerDecelerateState DecelerateState { get; private set; }
    public PlayerFinishState FinishState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    private Dictionary<E_PlayerState, PlayerState> StateEnumMap = new();

#endregion

    #region Inspector Variabes

    [SerializeField] bool debugAnimationBoolName;
    [SerializeField] string _currentState;

    #endregion

    #region Private Variables

    private CinemachineVirtualCamera playerCamera;
    private PlayerState currentState;

    #endregion

    #region Network Variables

    private PlayerState networkState;

    #endregion

    #region Component and Data References

    [SerializeField] PlayerData playerData;
    [SerializeField] Transform groundCheckTransform;

    public Animator animator { get; private set; }
    public Rigidbody RB { get; private set; }
    public Collider playerCollider { get; private set; }
    public Vector3 spawnPosition { get; private set; }

    #endregion

    #region Get References

    public PlayerStateMachine StateMachine;
    public PlayerInputManager InputManager { get; private set; }
    public UIManager UIManager => GameManager.Instance.GetUIManager();

    #endregion

    #region Unity Callback Methods

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        InputManager = GetComponent<PlayerInputManager>();
        RB = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider>();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        AccelerateState = new PlayerAccelerateState(this, StateMachine, playerData, "accelerate");
        DecelerateState = new PlayerDecelerateState(this, StateMachine, playerData, "decelerate");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        FinishState = new PlayerFinishState(this, StateMachine, playerData, "finish");

        //TBD --> should be cleaned afterwards
        StateEnumMap.Add(E_PlayerState.IDLE, IdleState);
        StateEnumMap.Add(E_PlayerState.ACCELERATION, AccelerateState);
        StateEnumMap.Add(E_PlayerState.DECELERATION, DecelerateState);
        StateEnumMap.Add(E_PlayerState.INAIR, InAirState);
        StateEnumMap.Add(E_PlayerState.FINISH, FinishState);
    }

    private void Start()
    {
        StateMachine.InitializeState(IdleState);
        StateMachine.CurrentState.Enter();

        if (photonView.IsMine)
        {
            SetPlayerCamera();
        }
        else if (!photonView.IsMine)
        {
            SetClient();
        }
    }

    private void Update()
    {
        if(photonView.IsMine){
        StateMachine.CurrentState.LogicUpdate();
            _currentState = StateMachine.CurrentState.AnimBoolName;
        }
    }

    private void FixedUpdate()
    {
        if(photonView.IsMine)
           { StateMachine.CurrentState.PhysicsUpdate(); }
    }

    #endregion

    #region Set Methods

    public void OnStateUpdate(byte state)
    {
        if(photonView.IsMine)photonView.RPC(nameof(StateUpdateRPC), RpcTarget.Others, state);
    }

    public void SetSpawnPosition(Vector3 position)
    {
        spawnPosition = position;
    }

    public void SetPlayerCamera()
    {
        playerCamera = Instantiate(playerData.playerCinmachineCameraPrefab);

        playerCamera.m_Follow = transform;
        playerCamera.m_LookAt = transform;
    }

    public void SetClient()
    {
        RB.isKinematic = true;
    }

    public void HandleTurning()
    {
        float turn = InputManager.MovementInput.x;
        float currentSpeed = RB.linearVelocity.magnitude;
        float speedFactor = Mathf.Clamp01(currentSpeed / playerData.acceleration); // Normalize 0–1
        float scaledTurnSpeed = playerData.turnSpeed * speedFactor;

        transform.Rotate(Vector3.up, turn * scaledTurnSpeed * Time.fixedDeltaTime);
    }

    public void SetPlayerRank(int Rank)
    {
        UIManager.SetRank(Rank, photonView.Owner.NickName);
        Debug.Log($"Player {photonView.Owner.NickName} has rank {Rank}");
    }

    #endregion

    #region Get Methods

    public Animator GetAnimator() { return animator; }

    #endregion

    #region Event Methods

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Interactable interactable))
        {
            switch (interactable.interactableType)
            {
                case InteractableTypes.Nitro:
                    AccelerateState.SetBoost();
                    break;

                case InteractableTypes.Mud:
                    AccelerateState.SetMudSpeed();
                    break;

                case InteractableTypes.Wall:
                    if (spawnPosition != Vector3.zero)
                    {
                        transform.position = spawnPosition;
                    }
                    else { transform.position = Vector3.zero; }
                    break;

                case InteractableTypes.Ink:
                    // Ink Spots on the canvas
                    break;

                case InteractableTypes.Checkpoint:
                    spawnPosition = other.transform.position;
                    break;

                case InteractableTypes.FinishLine:
                    StateMachine.ChangeState(FinishState);
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Interactable interactable))
        {
            if (interactable.interactableType == InteractableTypes.Nitro)
            {
                AccelerateState.SetBoost();
            }
            switch (interactable.interactableType)
            {
                case InteractableTypes.Mud:
                    AccelerateState.StopMudSpeed();
                    break;
            }
        }
    }

    #endregion

    #region Check Methods

    public bool CheckIfGrounded()
    {
        return Physics.Raycast(groundCheckTransform.position, -groundCheckTransform.up, playerData.groundCheckDistance, playerData.whatIsGround);
    }

    #endregion

    #region Photon Serializable

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {   
        if (stream.IsWriting)
        {
            
        }
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(groundCheckTransform.position, -groundCheckTransform.up * playerData.groundCheckDistance);
    }

    #endregion

    #region PUN RPC's

    [PunRPC]
    private void StateUpdateRPC(byte state)
    {
        E_PlayerState _state = (E_PlayerState)state;
        StateMachine.ChangeState(StateEnumMap[_state]);
    }

    #endregion

}
