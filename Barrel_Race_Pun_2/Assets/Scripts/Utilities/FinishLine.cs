using Photon.Pun;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public InteractableTypes interactableTypes;
    
    PhotonView photonView;
    private int currentRank = 0;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PhotonView>(out PhotonView photonView))
        {
            this.photonView.RPC(nameof(IncreaseRank), RpcTarget.All, photonView.ViewID);
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.SetPlayerRank(currentRank);
            }
        }
    }

    [PunRPC]
    private void IncreaseRank(int viewID)
    {
        currentRank++;
    }
}
