using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectHandler : IMessageHandler
{
    public void Handle(NetworkMessage msg)
    {
        // playerList 메시지 처리
        //if (msg.command == "playerList")
        //{
        //    if (msg.data.TryGetValue("players", out var playersObj) && playersObj is List<object> playersList)
        //    {
        //        foreach (var p in playersList)
        //        {
        //            if (p is Dictionary<string, object> dict)
        //            {
        //                string id = dict["id"].ToString();
        //                float x = Convert.ToSingle(dict["x"]);
        //                float y = Convert.ToSingle(dict["y"]);
        //                float z = Convert.ToSingle(dict["z"]);
        //                Vector3 pos = new Vector3(x, y, z);
        //                PlayerSpawnManager.Instance.MakePlayerPrefab(id, pos);
        //            }
        //        }
        //    }
        //}
        //// playerJoined 메시지 처리
        //else if (msg.command == "playerJoined")
        //{
        //    string id = msg.id;
        //    float x = msg.data.ContainsKey("x") ? Convert.ToSingle(msg.data["x"]) : 0;
        //    float y = msg.data.ContainsKey("y") ? Convert.ToSingle(msg.data["y"]) : 0;
        //    float z = msg.data.ContainsKey("z") ? Convert.ToSingle(msg.data["z"]) : 0;
        //    Vector3 pos = new Vector3(x, y, z);

        //    PlayerSpawnManager.Instance.MakePlayerPrefab(id, pos);
        //}
    }
}
