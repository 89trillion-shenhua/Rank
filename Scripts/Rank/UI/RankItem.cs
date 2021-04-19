using System;
using Rank.UI;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class RankItem : ListCell<PlayerInfo>
{
     [SerializeField] private Text nameTxt;
     [SerializeField] private Text trophyNumText;
     [SerializeField] private Text topRankTxt;

     [SerializeField] private Image rankBg;
     [SerializeField] private Image rankIcon;
     [SerializeField] private Image arenaIcon;
     [SerializeField] private Image headImg;

     private int userRank;
     private PlayerInfo selfInfo;
     private ListCell<PlayerInfo> listCellImplementation;
     private Action<PlayerInfo> onSelfClickEvent;

     public void SetSelfData(PlayerInfo playerInfo, int index)
     {
         selfInfo = playerInfo;
         int arenaIconIndex = playerInfo.trophy / 1000;
         arenaIcon.sprite = SpriteManager.Instance.GetArenaIcon(arenaIconIndex);
         
         nameTxt.text = playerInfo.nickName;
     
         trophyNumText.text = playerInfo.trophy.ToString();
         
         userRank = index + 1;
         if (userRank <= 3)
         {
             rankIcon.sprite = SpriteManager.Instance.GetRankIcon(index);
             rankIcon.SetNativeSize();
             rankIcon.gameObject.SetActive(true);
             topRankTxt.gameObject.SetActive(false);
         }
         else
         {
             topRankTxt.text = userRank.ToString();
             rankBg.sprite = SpriteManager.Instance.GetRankBg(3);
             topRankTxt.gameObject.SetActive(true);
             rankIcon.gameObject.SetActive(false);
         }
     }

     public void SetSelfClickEvent(Action<PlayerInfo> selfClickEvent)
     {
         onSelfClickEvent = selfClickEvent;
     }

     public void OnSelfClickEvent()
     {
         onSelfClickEvent?.Invoke(selfInfo);
     }

     public override void SetData(PlayerInfo playerInfo)
     {
         int arenaIconIndex = playerInfo.trophy / 1000;
         arenaIcon.sprite = SpriteManager.Instance.GetArenaIcon(arenaIconIndex);
         
         nameTxt.text = playerInfo.nickName;

         trophyNumText.text = playerInfo.trophy.ToString();
         
         userRank = playerInfo.userRank;
         if (userRank <= 3)
         {
             rankIcon.sprite = SpriteManager.Instance.GetRankIcon(userRank-1);
             rankIcon.SetNativeSize();
             rankBg.sprite = SpriteManager.Instance.GetRankBg(userRank-1);
             rankIcon.gameObject.SetActive(true);
             topRankTxt.gameObject.SetActive(false);
         }
         else
         {
             topRankTxt.text = userRank.ToString();
             rankBg.sprite = SpriteManager.Instance.GetRankBg(3);
             topRankTxt.gameObject.SetActive(true);
             rankIcon.gameObject.SetActive(false);
         }
     }
}