using Photon.Pun;
using TMPro;
using UnityEngine;

public class RoomItemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI roomName_Text;
    [SerializeField] TextMeshProUGUI playerCount_Text;

    public void SetRoom(string _roomName,  int _maxPlayers, int _currentPlayers)
    {
        roomName_Text.text = _roomName;
        playerCount_Text.text = _currentPlayers.ToString() + "/" + _maxPlayers.ToString();
    }

    public void OnJoinedPressed()
    {
        PhotonNetwork.JoinRoom(roomName_Text.text);
    }
}
