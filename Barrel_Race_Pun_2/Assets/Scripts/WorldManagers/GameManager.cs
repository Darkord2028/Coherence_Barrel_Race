using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    #region Game States

    public GameStateMachine StateMachine { get; private set; }
    public GameOfflineState OfflineState { get; private set; }
    public GameLobbyState OnlineState { get; private set; }
    public GameInRoomState InRoomState { get; private set; }

    #endregion

    #region World Manager References

    [SerializeField] UIManager uiManager;

    #endregion

    #region Serialized Variables

    [SerializeField] GameData gameData;

    [Header("Player Spawn Positions")]
    [SerializeField] Transform[] spawnPositions;

    #endregion

    #region Public Get Variables

    

    #endregion

    #region Private Variables

    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();
    private Dictionary<PhotonView, Player> networkPlayers = new Dictionary<PhotonView, Player>();

    public Player localPlayer;
    private List<PlayerInfoData> networkPlayersInfo = new List<PlayerInfoData>();
    private List<PlayerReadyItem> networkPlayerReadyItem = new List<PlayerReadyItem>();

    private int readyPlayerCount;

    private bool isReady = false;

    #endregion

    #region Unity Callback Methods

    private void Awake()
    {
        StateMachine = new GameStateMachine();

        OfflineState = new GameOfflineState(this, StateMachine, gameData);
        OnlineState = new GameLobbyState(this, StateMachine, gameData);
        InRoomState = new GameInRoomState(this, StateMachine, gameData);
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
        OfflineState.SetIsConnected(true);
    }

    public override void OnConnectedToMaster()
    {
        uiManager.SetPlayerName(PhotonNetwork.NickName);
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (Application.isPlaying)
        {
            uiManager.ToggleAllPanels(false);
            uiManager.SetReconnectButton(true);
        }
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

        GameObject playerGO = PhotonNetwork.Instantiate(gameData.playerPrefab.name, GetPlayerSpawnTransform().position, GetPlayerSpawnTransform().rotation);
        Vector3 spawnPosition = playerGO.transform.position;
        localPlayer = playerGO.GetComponent<Player>();
        localPlayer.SetSpawnPosition(spawnPosition);
        localPlayer.GetComponent<PlayerInput>().enabled = false;

        PhotonView view = playerGO.GetPhotonView();

        photonView.RPC(nameof(SetPayerInfoRPC), RpcTarget.AllBuffered, view.ViewID);
        photonView.RPC(nameof(SetPlayerReadyListRPC), RpcTarget.AllBuffered, view.ViewID);

        StateMachine.ChangeState(InRoomState);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);
    }

    #endregion

    #region Get Methods

    public UIManager GetUIManager() { return uiManager; }

    public Transform GetPlayerSpawnTransform()
    {
        return spawnPositions[PhotonNetwork.CurrentRoom.PlayerCount - 1];
    }

    public PlayerReadyItem GetPlayerReadyItem(int viewID)
    {
        foreach (PlayerReadyItem readyItem in networkPlayerReadyItem)
        {
            if (readyItem.GetViewID() == viewID)
            {
                return readyItem;
            }
        }

        return null;
    }

    #endregion

    #region Set Functions

    public void SetPlayerReady()
    {
        isReady = !isReady;
        object[] content = new object[] { networkPlayersInfo[0].GetPlayerID(), isReady };
        PhotonNetwork.RaiseEvent(EventCode.readyButton, content, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    #endregion

    #region RPC Methods

    [PunRPC]
    public void SetPayerInfoRPC(int viewID)
    {
        PhotonView photonView = PhotonNetwork.GetPhotonView(viewID);
        Player player = photonView.GetComponent<Player>();
        PlayerInfoData infoData = player.gameObject.GetComponent<PlayerInfoData>();

        networkPlayersInfo.Add(infoData);

        if (infoData != null)
        {
            infoData.SetPlayer(photonView.Owner.NickName, viewID);
        }

        networkPlayers.Add(photonView, player);
    }

    [PunRPC]
    private void SetPlayerReadyListRPC(int viewID)
    {
        GameObject instance = Instantiate(uiManager.GetPlayerReadyPrefab(), uiManager.GetPlayerReadyParent());
        PlayerReadyItem readyItem = instance.GetComponent<PlayerReadyItem>();
        networkPlayerReadyItem.Add(readyItem);
        PhotonView photonView = PhotonNetwork.GetPhotonView(viewID);

        readyItem.SetPlayer(photonView.Owner.NickName, photonView.ViewID);
        readyItem.SetPlayerReady(false);
    }

    #endregion

    #region Pun Event

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == EventCode.readyButton)
        {
            object[] data = photonEvent.CustomData as object[];

            if (data == null || data.Length < 2)
            {
                Debug.LogWarning("Received malformed readyButton event data.");
                return;
            }

            int viewID = (int)data[0];
            bool isReady = (bool)data[1];

            PlayerReadyItem readyItem = GetPlayerReadyItem(viewID);
            readyItem.SetPlayerReady(isReady);

            if (isReady) { readyPlayerCount++; }
            else { readyPlayerCount--; }

            if (readyPlayerCount == PhotonNetwork.CurrentRoom.PlayerCount) { InRoomState.StartTimer(); }
        }
    }

    #endregion

}

public static class EventCode
{
    public const byte readyButton = 1;
}
