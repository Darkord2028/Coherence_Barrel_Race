using Photon.Pun;
using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient) {  return; }

        int viewID = other.GetComponent<PhotonView>().ViewID;
        photonView.RPC(nameof(DeclareRank), RpcTarget.All, viewID);
    }

    [PunRPC]
    private void DeclareRank(int viewID)
    {
        print(viewID);
    }

}
