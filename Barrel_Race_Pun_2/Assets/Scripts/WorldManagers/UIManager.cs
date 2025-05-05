using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Serialized Variables

    [Header("Player UI")]
    [SerializeField] TextMeshProUGUI playerName;

    [Header("Room UI")]
    [SerializeField] GameObject roomItemPrefab;
    [SerializeField] Transform roomParent;
    [SerializeField] GameObject playerReadyPrefab;
    [SerializeField] Transform playerReadyParent;

    [Header("UI Panels")]
    [SerializeField] GameObject connectPanel;
    [SerializeField] GameObject nameInputPanel;
    [SerializeField] GameObject createJoinPanel;
    [SerializeField] GameObject createRoomPanel;
    [SerializeField] GameObject roomPanel;
    [SerializeField] GameObject playerReadyPanel;

    [Header("TMPro References")]
    [SerializeField] TextMeshProUGUI infoText;
    public TMP_InputField roomNameText;
    [SerializeField] TextMeshProUGUI maxPlayersText;

    [Header("Button References")]
    [SerializeField] Button reconnectButton;
    [SerializeField] Button createRoomButton;

    private int maxPlayers = 6;

    #endregion

    #region Private Variables

    private string defaultName;

    #endregion

    #region Unity Callback Methods

    private void Start()
    {
        playerName.transform.parent.gameObject.SetActive(false);
        maxPlayersText.text = maxPlayers.ToString();
    }


    #endregion

    #region Public Methods

    public void RefreshRoom()
    {
        PhotonNetwork.JoinLobby();
    }

    public void AddPlayers()
    {
        if (maxPlayers < 6)
        {
            maxPlayers++;
        }
        maxPlayersText.text = maxPlayers.ToString();
    }

    public void SubtractPlayers()
    {
        if (maxPlayers > 1)
        {
            maxPlayers--;
        }
        maxPlayersText.text = maxPlayers.ToString();
    }

    #endregion

    #region Toggle UI Methods

    public void ToggleNameInputPanel(bool enable)
    {
        nameInputPanel.SetActive(enable);
    }

    public void ToggleInfoPanel(bool enable)
    {
        connectPanel.SetActive(enable);
    }

    public void SetInfoPanel(string infoMessage)
    {
        infoText.text = infoMessage;
    }

    public void ToggleCreateJoinPanel(bool enable)
    {
        createJoinPanel.SetActive(enable);
    }

    public void ToggleInRoomPanel(bool enable)
    {
        roomPanel.SetActive(enable);
    }

    public void TogglePlayerReadyPanel(bool enable)
    {
        playerReadyPanel.SetActive(enable);
    }

    public void SetText(TextMeshProUGUI textMeshPro, string message)
    {
        textMeshPro.text = message;
    }

    public void ToggleCreateRoomPanel(bool enable)
    {
        createRoomPanel.SetActive(enable);
    }

    public void ToggleAllPanels(bool enable)
    {
        connectPanel.SetActive(enable);
        roomPanel.SetActive(enable);
        nameInputPanel.SetActive(enable);
        createJoinPanel.SetActive(enable);
        createRoomPanel.SetActive(enable);
        playerReadyPanel.SetActive(enable);
    }

    #endregion

    #region Non Toggle Method

    public void SetPlayerName(string name)
    {
        playerName.text = name;
        playerName.transform.parent.gameObject.SetActive(true);
    }

    public void SetReconnectButton(bool enable)
    {
        SetInfoPanel("Disconnected");
        ToggleInfoPanel(enable);
        reconnectButton.gameObject.SetActive(enable);
        reconnectButton.onClick.AddListener(HandleSceneReload);

    }

    private void HandleSceneReload()
    {
        SceneManager.LoadScene(0);
        reconnectButton.gameObject.SetActive(false);
    }

    #endregion

    #region Set Methods

    public void DisplayRoomList(Dictionary<string, RoomInfo> roomList)
    {
        foreach (Transform child in roomParent)
        {
            RoomItemUI.Destroy(child.gameObject);
        }

        //Display List
        foreach (KeyValuePair<string, RoomInfo> roomInfo in roomList)
        {
            var item = Instantiate(roomItemPrefab.gameObject, roomParent);
            item.GetComponent<RoomItemUI>().SetRoom(roomInfo.Value.Name, roomInfo.Value.MaxPlayers, roomInfo.Value.PlayerCount);
        }
    }

    #endregion

    #region Get Methods

    public string GetRoomName() { return roomNameText.text; }
    public int GetMaxPlayers() { return maxPlayers; }
    public Button GetCreateRoomButton() { return createRoomButton; }
    public GameObject GetPlayerReadyPrefab() { return playerReadyPrefab; }
    public Transform GetPlayerReadyParent() { return playerReadyParent; }

    #endregion

}
