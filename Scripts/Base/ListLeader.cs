using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DirType
{
    Vertical,
    Horizontal
}

public abstract class ListLeader<TCell, TSource> : MonoBehaviour
    where TCell : ListCell<TSource>
{
    [SerializeField] private DirType dirType;
    [SerializeField] protected TCell cellPrefab;
    [SerializeField] protected ScrollRect scrollRect;
    [SerializeField] protected List<RectTransform> scrollContents;
    [SerializeField] protected ListCellContainer cellContainerPrefab;

    protected ListCellContainer<TCell> containerPrefab;
    public List<ListCellContainer<TCell>> CellContainers { get; protected set; }
    protected List<PlayerInfo> models;
    protected Stack<TCell> cellReusePool;
    protected int minCellRow;
    protected int maxCellRow;
    protected int minRowIndex;
    protected int maxRowIndex;
    protected List<int> groupContentCap;

    protected abstract float CellHeight { get; }
    protected abstract float CellWidth { get; }
    protected virtual float GroupSpacing => 100;
    public abstract int CellColumnCount { get; }

    protected float cellHeight;
    protected float cellWidth;
    protected float cellSize => dirType == DirType.Vertical ? CellHeight : CellWidth;

    private bool isLoaded;

    protected Dictionary<int, List<int>> allCellsByRow;

    public virtual void SetupModels(List<PlayerInfo> models)
    {
        this.models = models;
    }

    private void TryInit()
    {
        CellContainers = new List<ListCellContainer<TCell>>();
        cellReusePool = new Stack<TCell>();
        scrollRect.onValueChanged.AddListener(OnValueChanged);

        containerPrefab = cellContainerPrefab as ListCellContainer<TCell>;
        var size = cellPrefab.GetComponent<RectTransform>().sizeDelta;
        cellHeight = size.y;
        cellWidth = size.x;
    }

    private void OnDestroy()
    {
        scrollRect.onValueChanged.RemoveAllListeners();
    }

    public virtual void LoadCells()
    {
        TryInit();
        SetData(true);
    }

    private void SetData(bool isRefresh)
    {
        InitCellContainers();
        CalculateVisibleRowRange();
        AdjustCurrentCells(isRefresh);
    }

    private void InitCellContainers()
    {
        if (models.Count > CellContainers.Count)
        {
            CalculateGroupContentCap();

            for (int i = CellContainers.Count; i < models.Count; i++)
            {
                var cellContainer = Instantiate(containerPrefab, GetScrollContent(i));
                cellContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(cellWidth, cellHeight);
                CellContainers.Add(cellContainer);

                // set cell by row
                allCellsByRow = allCellsByRow ?? new Dictionary<int, List<int>>();
                int row = CalculateCellRow(i);

                if (!allCellsByRow.TryGetValue(row, out var result))
                {
                    result = new List<int>();
                    allCellsByRow.Add(row, result);
                }

                result.Add(i);
            }
        }
        else if (models.Count < CellContainers.Count)
        {
            int count = CellContainers.Count - models.Count;
            for (int i = models.Count; i < CellContainers.Count; i++)
            {
                int row = CalculateCellRow(i);

                if (allCellsByRow.TryGetValue(row, out var result))
                {
                    result.Remove(i);

                    if (result.Count <= 0)
                    {
                        allCellsByRow.Remove(row);
                    }
                }

                if (CellContainers[i].gameObject)
                {
                    RecycleCell(i);
                    Destroy(CellContainers[i].gameObject);
                }
            }

            CellContainers.RemoveRange(models.Count, count);
            CalculateGroupContentCap();
        }
    }

    public void OnValueChanged(Vector2 value)
    {
        CalculateVisibleRowRange();
        AdjustCurrentCells(false);
    }

    protected virtual void AdjustCurrentCells(bool isRefresh)
    {
        if (!isRefresh && minRowIndex == minCellRow && maxRowIndex == maxCellRow)
        {
            return;
        }

        // try to recycle cells
        for (int i = minCellRow; i < minRowIndex; i++)
        {
            RecycleCellByRow(i);
        }

        for (int i = maxRowIndex; i <= maxCellRow; i++)
        {
            RecycleCellByRow(i);
        }

        // set visible cells
        for (int row = minRowIndex; row <= maxRowIndex; row++)
        {
            if (allCellsByRow.TryGetValue(row, out var result))
            {
                foreach (var index in result)
                {
                    var container = CellContainers[index];
                    if (container.Element == null)
                    {
                        var element = GetCellElement(container);
                        container.Element = element;
                    }

                    var playerInfo = models[index];
                    bool isSelf = index == 0;
                    ElementInit(container, playerInfo, index);
                }
            }
        }

        minCellRow = minRowIndex;
        maxCellRow = maxRowIndex;
    }

    protected virtual void ElementInit(ListCellContainer<TCell> cellContainer, PlayerInfo dataSource, int index)
    {
        cellContainer.Element.SetData(dataSource);
    }

    protected virtual void CalculateGroupContentCap()
    {
        groupContentCap = new List<int> {models.Count};
    }

    protected virtual int CalculateCellRow(int index)
    {
        int additionValue = 0;
        int calculator = index;
        foreach (var value in groupContentCap)
        {
            if (calculator >= value)
            {
                calculator -= value;
                additionValue++;
            }
            else
            {
                break;
            }
        }

        var currentRow = (index - additionValue) / CellColumnCount;
        return currentRow;
    }

    protected virtual void CalculateVisibleRowRange()
    {
        var rect = scrollRect.viewport.rect;
        var anchoredPosition = scrollRect.content.anchoredPosition;

        float offset = dirType == DirType.Vertical ? anchoredPosition.y : -anchoredPosition.x;
        var viewportHeight = dirType == DirType.Vertical ? rect.height : rect.width;

        minRowIndex = CalculateContentRow(offset);
        maxRowIndex = CalculateContentRow(offset + viewportHeight);
    }

    protected virtual int CalculateContentRow(float offset)
    {
        int row = (int) (offset / cellSize);
        int group = GroupIndexByCellRow(row);

        if (group > 0)
        {
            offset -= (group + 1) * GroupSpacing; // 减去每个分组的标题
            row = (int) (offset / cellSize);
        }

        return row;
    }

    protected virtual Transform GetScrollContent(int index)
    {
        return scrollContents[GroupIndexByCellIndex(index)];
    }

    protected virtual int GroupIndexByCellIndex(int index)
    {
        int groupIndex = groupContentCap.FindIndex(x =>
        {
            if (index >= x)
            {
                index -= x;
                return false;
            }

            return true;
        });

        return groupIndex;
    }

    protected virtual int GroupIndexByCellRow(int row)
    {
        int groupIndex = 0;
        foreach (var value in groupContentCap)
        {
            int rowCap = CalculateCellRow(value);
            if (row > rowCap)
            {
                row -= rowCap;
                groupIndex++;
            }
        }

        return groupIndex;
    }

    protected TCell GetCellElement(ListCellContainer<TCell> container)
    {
        if (cellReusePool.Count > 0)
        {
            TCell cell = cellReusePool.Pop();
            return cell;
        }

        return Instantiate(cellPrefab, container.transform);
    }

    private void RecycleCell(int cellIndex)
    {
        if (cellIndex >= CellContainers.Count)
        {
            return;
        }

        var cellContainer = CellContainers[cellIndex];
        if (cellContainer.Element != null)
        {
            cellReusePool.Push(cellContainer.Element);
            cellContainer.Element = null;
        }
    }

    protected void RecycleCellByRow(int row)
    {
        if (allCellsByRow.TryGetValue(row, out var result))
        {
            foreach (var index in result)
            {
                RecycleCell(index);
            }
        }
    }
}