using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//새로운 클라이언트가 들어올때 기존 클라이언트에게 보내는 핸들러
public class PlayerJoinedHandler : IMessageHandler
{
    public void Handle(NetworkMessage msg)
    {
        DebugManager.Instance.Debug(msg.command);
        string id = msg.id;
        float x = msg.data.ContainsKey("x") ? Convert.ToSingle(msg.data["x"]) : 0;
        float y = msg.data.ContainsKey("y") ? Convert.ToSingle(msg.data["y"]) : 0;
        float z = msg.data.ContainsKey("z") ? Convert.ToSingle(msg.data["z"]) : 0;
        Vector3 pos = new Vector3(x, y, z);

        PlayerSpawnManager.Instance.MakePlayerPrefab(id, pos);
    }
}
