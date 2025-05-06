using UnityEngine;

public class GameState
{
    protected GameManager gameManager;
    protected GameStateMachine StateMachine;
    protected GameData gameData;

    protected bool isExitingState;

    protected float startTime;

    public GameState(GameManager gameManager, GameStateMachine stateMachine, GameData gameData)
    {
        this.gameManager = gameManager;
        StateMachine = stateMachine;
        this.gameData = gameData;
    }

    public virtual void Enter()
    {
        DoChecks();
        startTime = Time.time;
        isExitingState = false;
    }

    public virtual void Exit()
    {
        isExitingState = true;
    }

    public virtual void LogicUpdate() { }

    public virtual void PhysicsUpdate() => DoChecks();

    public virtual void DoChecks() { }
}
