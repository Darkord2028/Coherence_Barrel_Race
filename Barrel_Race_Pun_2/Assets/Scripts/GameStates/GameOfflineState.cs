using UnityEngine;

public class GameOfflineState : GameBaseState
{
    private bool isConnected;

    public GameOfflineState(GameManager gameManager, GameStateMachine stateMachine, GameData gameData) : base(gameManager, stateMachine, gameData)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        isConnected = false;
        uiManager.ToggleNameInputPanel(true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isConnected)
        {
            StateMachine.ChangeState(gameManager.OnlineState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SetIsConnected(bool isConnected)
    {
        this.isConnected = isConnected;
    }

}
