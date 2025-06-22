using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPositionHandler : IMessageHandler
{
    public void Handle(string data)
    {
        DebugManager.Instance.Debug("1");
        string body = data.Substring("syncPosition;".Length);
        string[] posData = body.Split(',', StringSplitOptions.RemoveEmptyEntries);
        string id = posData[0];

        float x = Convert.ToSingle(posData[1]);
        float z = Convert.ToSingle(posData[2]);
        Vector2 pos = new Vector2(x, z);

        if (PlayerSpawnManager.Instance.Players.TryGetValue(id, out Players player))
        {
            Vector3 newPos = new Vector3(x, player.transform.position.y, z);
            player.SetTargetPosition(newPos);
        }
    }
}
