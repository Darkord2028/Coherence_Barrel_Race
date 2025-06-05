using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine StateMachine;
    protected PlayerData playerData;

    protected bool isExitingState;
    protected bool isAnimationFinished;

    protected float startTime;

    public string AnimBoolName;

    public E_PlayerState playerState;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        StateMachine = stateMachine;
        this.playerData = playerData;
        AnimBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        DoChecks();
        startTime = Time.time;
        player.GetAnimator().SetBool(AnimBoolName.ToString(), true);
        isExitingState = false;
        isAnimationFinished = false;
        player.OnStateUpdate((byte)playerState);
    }

    public virtual void Exit()
    {
        isExitingState = true;
        player.GetAnimator().SetBool(AnimBoolName.ToString(), false);
    }

    public virtual void LogicUpdate() { }

    public virtual void PhysicsUpdate() => DoChecks();

    public virtual void DoChecks() { }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}

public enum E_PlayerState
{
    IDLE,
    ACCELERATION,
    DECELERATION,
    BOOST,
    INAIR,
    FINISH
}
