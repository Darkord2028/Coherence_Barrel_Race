using Photon.Pun;
using System.Collections;
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

    public void StartTimer()
    {
        uiManager.TogglePlayerReadyPanel(false);
        gameManager.StartCoroutine(CountdownCoroutine());
    }

    public void StopTimer()
    {
        uiManager.ToggleTimerPanel(false);
        gameManager.StopCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        float timeLeft = gameData.countdownDuration;
        string timeText = "";
        uiManager.ToggleTimerPanel(true);

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timeText = Mathf.Ceil(timeLeft).ToString();
            uiManager.SetTimer(timeText);
            yield return null;
        }

        // Trigger your game logic here
        uiManager.ToggleAllPanels(false);
    }

}
