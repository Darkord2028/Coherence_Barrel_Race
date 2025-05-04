using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    #region Game States

    public GameStateMachine StateMachine { get; private set; }
    public GameOfflineState OfflineState { get; private set; }
    public GameOnlineState OnlineState { get; private set; }

    #endregion

    #region World Manager References

    [SerializeField] UIManager uiManager;

    #endregion

    #region Serialized Variables

    [SerializeField] GameData gameData;

    #endregion

    #region Public Get Variables

    public Player localPlayer;
    public Dictionary<ulong, Player> networkPlayers = new Dictionary<ulong, Player>();

    #endregion

    #region Private Variables

    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

    #endregion

    #region Unity Callback Methods

    private void Awake()
    {
        StateMachine = new GameStateMachine();

        OfflineState = new GameOfflineState(this, StateMachine, gameData);
        OnlineState = new GameOnlineState(this, StateMachine, gameData);
    }

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void Start()
    {
        StateMachine.InitializeState(OfflineState);
        StateMachine.CurrentState.Enter();

        PhotonNetwork.AutomaticallySyncScene = true;
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

    #region Public Methods

    public void OnConnectPress()
    {
        uiManager.ToggleAllPanels(false);
        PhotonNetwork.ConnectUsingSettings();
        uiManager.SetInfoPanel("Connecting...");
        uiManager.ToggleInfoPanel(true);
    }

    public void CreateRoom()
    {
        string roomName = uiManager.roomNameText.text.Trim();

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = uiManager.GetMaxPlayers();
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        if (string.IsNullOrEmpty(roomName))
        {
            StartCoroutine(ShowMessage(1f, "Room Name is Empty!"));
        }
        else
        {
            PhotonNetwork.CreateRoom(roomName, roomOptions);
            uiManager.ToggleAllPanels(false);
            uiManager.SetInfoPanel("Creating Room...");
            uiManager.ToggleInfoPanel(true);
        }
    }

    #endregion

    #region Private Methods

    private void UpdateRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            if (info.RemovedFromList)
            {
                cachedRoomList.Remove(info.Name);
            }
            else
            {
                cachedRoomList[info.Name] = info;
            }
        }
        uiManager.DisplayRoomList(cachedRoomList);
    }

    #endregion

    #region IEnumarators

    private IEnumerator ShowMessage(float infoShowTime, string message)
    {
        uiManager.SetInfoPanel(message);
        uiManager.ToggleInfoPanel(true);

        yield return new WaitForSeconds(infoShowTime);

        uiManager.ToggleInfoPanel(false);
    }

    #endregion

    #region Photon Callbacks

    public override void OnConnected()
    {
        uiManager.ToggleAllPanels(false);
        uiManager.SetReconnectButton(false);
        OfflineState.SetIsConnected(true);
    }

    public override void OnConnectedToMaster()
    {
        uiManager.SetPlayerName(PhotonNetwork.NickName);
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        uiManager.ToggleAllPanels(false);
        uiManager.SetReconnectButton(true);
    }

    public override void OnJoinedLobby()
    {
        cachedRoomList.Clear();
        uiManager.ToggleAllPanels(false);
        uiManager.ToggleCreateJoinPanel(true);
    }

    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
    }

    public override void OnJoinedRoom()
    {
        uiManager.ToggleInRoomPanel(false);
        StartCoroutine(ShowMessage(1f, "Joined Room!"));
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);
    }

    #endregion

    #region Get Methods

    public UIManager GetUIManager() { return uiManager; }

    #endregion

    #region Set Functions

    public void SpawnPlayer(Vector3 position, Quaternion rotation)
    {
        GameObject player = Instantiate(gameData.playerPrefab, position, rotation);
        localPlayer = player.GetComponent<Player>();
    }

    #endregion

    #region Pun Event

    public void OnEvent(EventData photonEvent)
    {
    }

    #endregion

}
