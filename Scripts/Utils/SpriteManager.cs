using Base;
using UnityEngine;

public class SpriteManager : MonoSingleton<SpriteManager>
{
    [SerializeField] private Sprite[] rankIcons;
    [SerializeField] private Sprite[] arenaIcons;
    [SerializeField] private Sprite[] headImgs;
    [SerializeField] private Sprite[] rankBgs;

    public Sprite GetRankIcon(int index)
    {
        return rankIcons[index];
    }
    
    public Sprite GetArenaIcon(int index)
    {
        return arenaIcons[index];
    }
    
    public Sprite GetHeadImg(int index)
    {
        return headImgs[index];
    }
    
    public Sprite GetRankBg(int index)
    {
        return rankBgs[index];
    }
}