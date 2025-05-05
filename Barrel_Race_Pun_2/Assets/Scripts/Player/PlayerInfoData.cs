using UnityEngine;

public class PlayerInfoData : MonoBehaviour
{
    [SerializeField] string playerName;
    [SerializeField] int playerID;
    //Player Current Skin
    //Player Current Barrel Skin

    #region Set Functions

    public void SetPlayer(string name, int id)
    {
        playerName = name;
        playerID = id;
    }

    #endregion

    public string GetPlayerName()
    {
        return playerName;
    }

    public int GetPlayerID()
    {
        return playerID;
    }

}
