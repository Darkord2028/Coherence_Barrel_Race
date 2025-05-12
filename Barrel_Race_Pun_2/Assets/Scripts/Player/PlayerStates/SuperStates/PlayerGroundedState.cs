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
        MovementInput = player.InputManager.MovementInput;
        AccelerateInput = player.InputManager.AccelerateInput;

        HandleTurning();

        if (isGrounded)
        {
            if (!AccelerateInput && player.RB.linearVelocity.magnitude < 0.1f && !isExitingState)
            {
                StateMachine.ChangeState(player.IdleState);
            }
            else if (AccelerateInput && !isExitingState)
            {
                StateMachine.ChangeState(player.AccelerateState);
            }
            else if (!AccelerateInput && player.RB.linearVelocity.magnitude > 0.1f && !isExitingState)
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

    private void HandleTurning()
    {
        float turn = player.InputManager.MovementInput.x;
        float currentSpeed = player.RB.linearVelocity.magnitude;
        float speedFactor = Mathf.Clamp01(currentSpeed / playerData.acceleration); // Normalize 0–1
        float scaledTurnSpeed = playerData.turnSpeed * speedFactor;

        player.transform.Rotate(Vector3.up, turn * scaledTurnSpeed * Time.fixedDeltaTime);
    }

}
