using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Serialized Variables

    [Header("Player UI")]
    [SerializeField] TextMeshProUGUI playerName;

    [Header("UI Panels")]
    [SerializeField] GameObject connectPanel;
    [SerializeField] GameObject nameInputPanel;
    [SerializeField] GameObject createJoinPanel;
    [SerializeField] GameObject roomPanel;

    [Header("TMPro References")]
    [SerializeField] TextMeshProUGUI infoText;
    public TextMeshProUGUI roomNameText;

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

    public void ToggleAllPanels(bool enable)
    {
        connectPanel.SetActive(enable);
        roomPanel.SetActive(enable);
        nameInputPanel.SetActive(enable);
        createJoinPanel.SetActive(enable);
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

    #endregion

    #region Get Methods

    public string GetRoomName() { return roomNameText.text; }
    public int GetMaxPlayers() { return maxPlayers; }
    public Button GetCreateRoomButton() { return createRoomButton; }

    #endregion

}
