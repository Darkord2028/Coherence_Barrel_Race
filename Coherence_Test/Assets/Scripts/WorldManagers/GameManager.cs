using Coherence;
using Coherence.Connection;
using Coherence.Toolkit;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Game States

    public GameStateMachine StateMachine { get; private set; }
    public GameOfflineState OfflineState { get; private set; }
    public GameOnlineState OnlineState { get; private set; }

    #endregion

    #region World Manager References

    [SerializeField] CoherenceBridge coherenceBridge;
    [SerializeField] UIManager uiManager;

    #endregion

    #region Serialized Variables

    [SerializeField] GameData gameData;

    #endregion

    #region Public Get Variables

    public Dictionary<ClientID, Player> networkPlayers = new Dictionary<ClientID, Player>();

    #endregion

    #region Unity Callback Methods

    private void Awake()
    {
        StateMachine = new GameStateMachine();

        OfflineState = new GameOfflineState(this, StateMachine, gameData);
        OnlineState = new GameOnlineState(this, StateMachine, gameData);
    }

    private void OnEnable()
    {
         
    }

    private void OnDisable()
    {
        
    }

    private void Start()
    {
        StateMachine.InitializeState(OfflineState);
        StateMachine.CurrentState.Enter();
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #endregion

    #region Get Methods

    public CoherenceBridge GetCoherenceBridge() { return coherenceBridge; }
    public UIManager GetUIManager() { return uiManager; }

    #endregion

    #region Set Functions

    public void SpawnPlayer(Vector3 position, Quaternion rotation)
    {
        GameObject player = Instantiate(gameData.playerPrefab, position, rotation);
    }

    #endregion

    #region Public Functions


    #endregion

    #region Coherence Events

    public void OnEntitySpawned()
    {
        Player player = GetComponent<Player>();

        if (player != null)
        {
            foreach (var connections in coherenceBridge.ClientConnections.GetAllClients())
            {
                ClientID clientID = connections.ClientId;

                if (!networkPlayers.ContainsKey(clientID))
                {
                    networkPlayers.Add(clientID, player);
                    Debug.Log($"Added Player with ClientID: {clientID}");
                }
            }
        }
    }

    #endregion

}
