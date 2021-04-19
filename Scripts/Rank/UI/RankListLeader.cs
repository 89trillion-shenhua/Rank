using System;
using UnityEngine;
using UnityEngine.UI;

public class RankListLeader : ListLeader<RankItem, PlayerInfo>
{
    [SerializeField] private VerticalLayoutGroup layoutGroup;
    protected override float CellHeight => cellHeight + layoutGroup.spacing;
    protected override float CellWidth => 0;
    public override int CellColumnCount => 1;
    private Action<PlayerInfo> onItemClickEvent;


    public void SetItemClickEvent(Action<PlayerInfo> onItemClickEvent)
    {
        this.onItemClickEvent = onItemClickEvent;
    }
    
    protected override void ElementInit(ListCellContainer<RankItem> cellContainer, PlayerInfo playerInfo,
        int index)
    {
        base.ElementInit(cellContainer, playerInfo, index);
        cellContainer.Element.SetClickEvent(() => { onItemClickEvent?.Invoke(playerInfo); });
    }
}