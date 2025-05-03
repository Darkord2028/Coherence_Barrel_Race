using UnityEngine;

public class PlayerInfoData : MonoBehaviour
{
    [SerializeField] string playerName;
    [SerializeField] ushort playerID;
    //Player Current Skin
    //Player Current Barrel Skin

    #region Set Functions

    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    public void SetPlayerId(ushort id)
    {
        playerID = id;
    }

    #endregion

}
