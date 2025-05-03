using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerName : MonoBehaviour
{
    [SerializeField]
    TMP_InputField inputField;
    string defaultName;

    void Start()
    {
        if (inputField != null)
        {
            if (PlayerPrefs.HasKey("PlayerName"))
            {
                defaultName = PlayerPrefs.GetString("PlayerName");
                inputField.text = defaultName;
            }
        }
    }

    public void SetPlayerName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            value = "Player" + Random.Range(0, 100);
            return;
        }
        PhotonNetwork.NickName = value;

        PlayerPrefs.SetString("PlayerName", value);
    }
}
