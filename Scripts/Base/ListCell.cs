using System;
using UnityEngine;

public abstract class ListCell : MonoBehaviour
{
    protected Action onClickEvent;
    
    public virtual void SetClickEvent(Action onClickEvent)
    {
        this.onClickEvent = onClickEvent;
    }

    public virtual void OnClickEvent()
    {
        onClickEvent?.Invoke();
    }
}

public abstract class ListCell<T> : ListCell
{
    public abstract void SetData(PlayerInfo dataSource);
}
