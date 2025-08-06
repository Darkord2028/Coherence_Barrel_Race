using TMPro;
using UnityEngine;

public class PlayerWinnerDeclaration : MonoBehaviour
{
    [Header("UI Items")]
    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] TextMeshProUGUI playerRankText;

    public void DeclarePlayerRank(int rank, string PlayerName)
    {
        playerNameText.text = PlayerName;
        playerRankText.text = rank.ToString();
    }

}
