using System.Collections;
using System.Collections.Generic;
using Rank.UI;
using UnityEngine;
using UnityEngine.UI;

public class RankPanel : MonoBehaviour
{
    [SerializeField] private RankListLeader rankListLeader;
    [SerializeField] private Text seasonText;
    [SerializeField] private Text countDownTime;
    [SerializeField] private RankItem selfRankItem;
    [SerializeField] private GameObject content;
    [SerializeField] private RankItem rankItemPrefab;
    [SerializeField] private TipsDialog tipsDialog;

    private RankData rankData;
    private List<PlayerInfo> playerInfos = new List<PlayerInfo>();

    public void Init()
    {
        rankData = new RankData();
        seasonText.text = "Season " + rankData.seasonID + " Ranking";
        playerInfos = rankData.PlayerInfos;
        for (int i = 0; i < playerInfos.Count; i++)
        {
            playerInfos[i].userRank = i + 1;
            playerInfos[i].isSelf = playerInfos[i].userRank == 1;
            if (playerInfos[i].isSelf)
            {
                selfRankItem.SetSelfData(playerInfos[i], i);
                selfRankItem.SetSelfClickEvent(selfInfo =>
                {
                    tipsDialog.ShowTipsDialog(selfInfo.nickName, selfInfo.userRank);
                });
            }
        }
        SetListView();
        StartCoroutine(CountDown());
    }

    private void SetListView()
    {
        if (playerInfos != null && playerInfos.Count > 0)
        {
            rankListLeader.SetupModels(playerInfos);
            rankListLeader.SetItemClickEvent(playerInfos =>
            {
                tipsDialog.ShowTipsDialog(playerInfos.nickName, playerInfos.userRank);
            });
            rankListLeader.LoadCells();
        }
    }

    private IEnumerator CountDown()
    {
        while (true)
        {
            countDownTime.text = "Ends in:" + TimeUtil.SecondsToDhmsString(rankData.countDown);
            yield return new WaitForSeconds(1);
            rankData.countDown--;
            if (rankData.countDown <= 0)
            {
                yield break;
            }
        }
    }

    public void DestroyRankItems()
    {
        for (int i = content.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }
}
