using UnityEngine;

public class HomePageDialog : MonoBehaviour
{
    [SerializeField] private RankDialog rankDialog;
    
    public void OnStartClick()
    {
        rankDialog.gameObject.SetActive(true);
    }
}