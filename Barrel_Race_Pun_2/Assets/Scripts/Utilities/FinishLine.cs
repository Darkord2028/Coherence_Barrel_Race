using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;

public class FinishLine : MonoBehaviour
{
    public InteractableTypes interactableTypes;
    
    PhotonView photonView;
    public int currentRank = 1;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PhotonView>(out PhotonView photonView))
        {
            this.photonView.RPC(nameof(AssignRankToPlayer), RpcTarget.OthersBuffered, photonView.ViewID);
        }
    }

    [PunRPC]
    private void AssignRankToPlayer(int viewID)
    {
        currentRank++;
        PhotonView pv = PhotonView.Find(viewID);
        if (pv != null)
        {
            Player player = pv.GetComponent<Player>();
            PlayerInfoData playerInfo = player.GetComponent<PlayerInfoData>();
            if (player != null)
            {
                player.StateMachine.ChangeState(player.FinishState);

                if (playerInfo != null)
                {
                    player.SetPlayerRank(currentRank);
                }
            }
        }
    }
}
