using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviourPun, IPunObservable
{
    #region State Variables

    public PlayerIdleState IdleState { get; private set; }
    public PlayerAccelerateState AccelerateState { get; private set; }
    public PlayerDecelerateState DecelerateState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }

    #endregion

    #region Public Variables

    public bool debugAnimationBoolName;
    public string animBoolName;
    public bool isGrounded;

    #endregion

    #region Private Variables

    private CinemachineVirtualCamera playerCamera;

    private Collider[] hitColliders = new Collider[3];

    #endregion

    #region Network Variables

    private string networkAnimBoolName;

    #endregion

    #region Component and Data References

    [SerializeField] PlayerData playerData;
    [SerializeField] Transform groundCheckTransform;

    public Animator animator { get; private set; }
    public Rigidbody RB { get; private set; }
    public Collider playerCollider { get; private set; }

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
        RB = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider>();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        AccelerateState = new PlayerAccelerateState(this, StateMachine, playerData, "accelerate");
        DecelerateState = new PlayerDecelerateState(this, StateMachine, playerData, "decelerate");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
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
            PlayerInput playerInput = GetComponent<PlayerInput>();
            playerInput.enabled = false;
            SetClient();
        }
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
        isGrounded = CheckIfGrounded();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #endregion

    #region Set Methods

    public void SetPlayerCamera()
    {
        playerCamera = Instantiate(playerData.playerCinmachineCameraPrefab);

        playerCamera.m_Follow = transform;
        playerCamera.m_LookAt = transform;
    }

    public void SetClient()
    {
        RB.isKinematic = true;
        playerCollider.enabled = false;
    }

    #endregion

    #region Get Methods

    public Animator GetAnimator() { return animator; }

    #endregion

    #region Event Methods

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Obstacles obstacle))
        {
            //StateMachine.ChangeState();
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
            stream.SendNext(animBoolName);
        }
        else
        {
            networkAnimBoolName = (string)stream.ReceiveNext();
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

}
