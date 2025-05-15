using System.Collections;
using UnityEngine;

public class PlayerAccelerateState : PlayerGroundedState
{
    private float boostSpeed;

    public PlayerAccelerateState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

        this.playerState = E_PlayerState.ACCELERATION;
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
        player.HandleTurning();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.RB.linearVelocity = player.transform.forward * (playerData.acceleration + boostSpeed);
    }

    public void SetBoost()
    {
        boostSpeed = playerData.boostSpeed;
        player.StartCoroutine(StopSpeed(playerData.boostSpeedTime));
    }

    public void SetMudSpeed()
    {
        boostSpeed = -playerData.mudSpeed;

        if (boostSpeed < playerData.acceleration) { boostSpeed = -playerData.acceleration + 5f; }
        player.StartCoroutine(StopSpeed(playerData.mudSpeedTime));
    }

    private IEnumerator StopSpeed(float time)
    {
        yield return new WaitForSeconds(time);
        boostSpeed = 0;
    }

}
