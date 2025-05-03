using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Variables

    public PlayerIdleState IdleState { get; private set; }

    #endregion

    #region Public Variables

    public bool debugAnimationBoolName;

    #endregion

    #region Component and Data References

    [SerializeField] Animator animator;
    [SerializeField] PlayerData playerData;

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
    }

    private void Start()
    {
        StateMachine.InitializeState(IdleState);
        StateMachine.CurrentState.Enter();
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

    public void SetMovement()
    {
        Debug.Log(InputManager.MovementInput);
    }

    #endregion

    #region Get Methods

    public Animator GetAnimator() { return animator; }

    #endregion
}
