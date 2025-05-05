using UnityEngine;

public class GameInRoomState : GameBaseState
{
    public GameInRoomState(GameManager gameManager, GameStateMachine stateMachine, GameData gameData) : base(gameManager, stateMachine, gameData)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        uiManager.TogglePlayerReadyPanel(true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
