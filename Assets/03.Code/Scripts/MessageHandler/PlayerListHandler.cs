using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//새로운 클라이언트가 들어올때 기존 클라이언트의 정보를 보내는 핸들러
public class PlayerListHandler : IMessageHandler
{
    public void Handle(string data)
    {
        if (!data.StartsWith("playerList;"))
        {
            DebugManager.Instance.Debug($"[커멘드 이상] : {data}");
            return;
        }

        string body = data.Substring("playerList;".Length);
        string[] playerData = body.Split('|', StringSplitOptions.RemoveEmptyEntries);

        foreach (var p in playerData)
        {
            string[] parts = p.Split(',');
            string id = parts[0];
            float x = float.Parse(parts[1]);
            float y = float.Parse(parts[2]);
            float z = float.Parse(parts[3]);

            Vector3 pos = new Vector3(x, y, z);

            DebugManager.Instance.Debug($"플레이어 {id} 위치: ({x}, {y}, {z})");

            PlayerSpawnManager.Instance.MakePlayerPrefab(id, pos);
        }
    }
}
