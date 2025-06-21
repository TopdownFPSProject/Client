using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//새로운 클라이언트가 들어올때 기존 클라이언트의 정보를 보내는 핸들러
public class PlayerListHandler : IMessageHandler
{
    public void Handle(NetworkMessage msg)
    {
        if (msg.data.TryGetValue("players", out var playersObj) && playersObj is List<object> playerList)
        {
            foreach (var player in playerList)
            {
                if (player is Dictionary<string, object> dict)
                {
                    string id = dict["id"].ToString();
                    float x = Convert.ToSingle(dict["x"]);
                    float y = Convert.ToSingle(dict["y"]);
                    float z = Convert.ToSingle(dict["z"]);
                    Vector3 pos = new Vector3(x, y, z);
                    PlayerSpawnManager.Instance.MakePlayerPrefab(id, pos);
                }
            }
        }
    }
}
