using UnityEngine;

public class RankDialog : MonoBehaviour
{
    [SerializeField] private RankPanel rankPanel;

    private void OnEnable()
    {
        rankPanel.Init();
    }

    public void Close()
    {
        rankPanel.DestroyRankItems();
        this.gameObject.SetActive(false);
    }
}
