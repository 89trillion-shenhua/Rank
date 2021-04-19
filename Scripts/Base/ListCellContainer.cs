using UnityEngine;

public class ListCellContainer : MonoBehaviour
{
}

public class ListCellContainer<T> : ListCellContainer where T : ListCell
{
    private T element;

    public T Element
    {
        get => element;
        set
        {
            if (element == value)
            {
                return;
            }

            if (element != null)
            {
                element.gameObject.transform.SetParent(transform.root);
                element.gameObject.SetActive(false);
            }

            element = value;

            if (element != null)
            {
                element.gameObject.transform.SetParent(transform);
                element.gameObject.SetActive(true);

                Transform elementTrans = element.transform;
                elementTrans.localPosition = Vector3.zero;
                elementTrans.localScale = Vector3.one;
            }
        }
    }
}