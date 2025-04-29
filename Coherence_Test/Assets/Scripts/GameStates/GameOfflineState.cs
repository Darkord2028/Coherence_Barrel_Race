using Coherence.Toolkit;
using UnityEngine;

public class GameOfflineState : GameState
{
    CoherenceBridge coherenceBridge;
    UIManager uiManager;

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

        coherenceBridge = gameManager.GetCoherenceBridge();
        uiManager = gameManager.GetUIManager();

        uiManager.ToggleAllPanels(false);
        uiManager.ToggleInfoPanel(true);
        uiManager.ToggleInRoomPanel(true);
        uiManager.SetInfoPanel("Connecting");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (coherenceBridge.IsConnecting)
        {
            uiManager.SetInfoPanel("Connecting");
        }
        else if (coherenceBridge.IsConnected)
        {
            uiManager.ToggleAllPanels(false);
            StateMachine.ChangeState(gameManager.OnlineState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
