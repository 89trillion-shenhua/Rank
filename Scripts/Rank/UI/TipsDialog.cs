using UnityEngine;
using UnityEngine.UI;

namespace Rank.UI
{
    public class TipsDialog : MonoBehaviour
    {
        [SerializeField] private Text playerInfo;

        public void ShowTipsDialog(string userName, int userRank)
        {
            playerInfo.text = string.Format("User:{0},Rank:{1}", userName, userRank);
            this.gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}