using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class PlayerInfo
{
    public string nickName;
    public int avatar;
    public int trophy;
    public int userRank;
    public bool isSelf;
}

public class RankData
{
    public int countDown;
    public List<PlayerInfo> PlayerInfos;
    public int seasonID;
    public int selfRank;
    
    public RankData()
    {
        LoadRankData();
    }

    private void LoadRankData()
    {
        var jsonfile = Resources.Load<TextAsset>("Json/ranklist");
        JSONNode jsondata = JSONNode.Parse(jsonfile.text);
        this.PlayerInfos = new List<PlayerInfo>();
        JSONArray dataArray = jsondata["list"].AsArray;
        foreach (JSONNode node in dataArray)
        {
            PlayerInfo playerInfo = new PlayerInfo();
            playerInfo.nickName = node["nickName"].ToString();
            playerInfo.avatar = node["avatar"].AsInt;
            playerInfo.trophy = node["trophy"].AsInt;
            this.PlayerInfos.Add(playerInfo);
        }

        this.countDown = jsondata["countDown"];
        this.seasonID = jsondata["seasonID"];
        this.selfRank = jsondata["selfRank"];
    }
}