using Coherence.Toolkit;
using UnityEngine;

public class GameOnlineState : GameState
{
    CoherenceBridge coherenceBridge;
    public GameOnlineState(GameManager gameManager, GameStateMachine stateMachine, GameData gameData) : base(gameManager, stateMachine, gameData)
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
        gameManager.SpawnPlayer(Vector3.zero, Quaternion.identity);
        gameManager.OnEntitySpawned();
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
