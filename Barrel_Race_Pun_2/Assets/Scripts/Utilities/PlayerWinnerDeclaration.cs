using TMPro;
using UnityEngine;

public class PlayerWinnerDeclaration : MonoBehaviour
{
    [Header("UI Items")]
    [SerializeField] TextMeshProUGUI playerRankText;
    [SerializeField] TextMeshProUGUI playerNameText;

    public void DeclarePlayerRank(int Rank, string PlayerName)
    {
        playerRankText.text = Rank.ToString();
        playerNameText.text = PlayerName;
    }

}
