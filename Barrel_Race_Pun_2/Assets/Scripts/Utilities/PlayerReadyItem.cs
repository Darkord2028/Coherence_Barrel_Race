using TMPro;
using UnityEngine;

public class PlayerReadyItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] TextMeshProUGUI readyText;

    [Header("Player Runtime Info")]
    [SerializeField] int viewID;
    [SerializeField] bool isReady;

    #region Set Methods

    public void SetPlayer(string name, int viewID)
    {
        playerNameText.text = name;
        this.viewID = viewID;
    }

    public void SetPlayerReady(bool isReady)
    {
        this.isReady = isReady;
        readyText.text = isReady ? "Ready" : "Not Ready";
        
    }

    #endregion

    #region Get Methods

    public string GetPlayerName()
    {
        return playerNameText.text;
    }

    public int GetViewID()
    {
        return viewID;
    }

    public bool CheckIfReady()
    {
        return isReady;
    }

    #endregion

}
