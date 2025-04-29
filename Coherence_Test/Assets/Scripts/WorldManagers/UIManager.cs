using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Serialized Variables

    [Header("UI Panels")]
    [SerializeField] GameObject infoPanel;
    [SerializeField] GameObject roomPanel;

    #endregion

    #region Private Variables

    private TextMeshProUGUI infoText;

    #endregion

    #region Unity Callback Methods

    private void Start()
    {
        infoText = infoPanel.GetComponentInChildren<TextMeshProUGUI>();

        ToggleAllPanels(false);
    }


    #endregion

    #region Public Methods

    public void ToggleInfoPanel(bool enable)
    {
        infoPanel.SetActive(enable);
    }

    public void SetInfoPanel(string infoMessage)
    {
        infoText.text = infoMessage;
    }

    public void ToggleInRoomPanel(bool enable)
    {
        roomPanel.SetActive(enable);
    }

    public void ToggleAllPanels(bool enable)
    {
        infoPanel.SetActive(enable);
        roomPanel.SetActive(enable);
    }

    #endregion

}
