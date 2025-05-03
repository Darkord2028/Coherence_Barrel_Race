public class GameStateMachine
{
    public GameState CurrentState { get; private set; }

    /// <summary>
    /// It is used to Initialize state during start
    /// </summary>
    /// <param name="startingState"></param>
    public void InitializeState(GameState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    /// <summary>
    /// It is used to Change State
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(GameState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
