using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    // Input Flags
    protected Vector2 MovementInput;
    protected bool AccelerateInput;

    // Get Variables
    protected bool isGrounded;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        isGrounded = player.CheckIfGrounded();
        AccelerateInput = player.InputManager.AccelerateInput;


        if (isGrounded)
        {
            if (!AccelerateInput && player.RB.linearVelocity.magnitude < 0.1f && !isExitingState && StateMachine.CurrentState.playerState!=E_PlayerState.IDLE)
            {
                StateMachine.ChangeState(player.IdleState);
            }
            else if (AccelerateInput && !isExitingState && StateMachine.CurrentState.playerState != E_PlayerState.ACCELERATION)
            {
                StateMachine.ChangeState(player.AccelerateState);
            }
            else if (!AccelerateInput && player.RB.linearVelocity.magnitude > 0.1f && !isExitingState && StateMachine.CurrentState.playerState != E_PlayerState.DECELERATION)
            {
                StateMachine.ChangeState(player.DecelerateState);
            }
        }
        else if (!isGrounded)
        {
            StateMachine.ChangeState(player.InAirState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
