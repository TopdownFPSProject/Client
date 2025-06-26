using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPositionHandler : IMessageHandler
{
    public void Handle(string data)
    {
        string[] posData = data.Split(';', StringSplitOptions.RemoveEmptyEntries);

        string id = posData[1];
        float x = Convert.ToSingle(posData[2]);
        float y = Convert.ToSingle(posData[3]);
        float z = Convert.ToSingle(posData[4]);

        if (id != TcpClientController.Instance.MyId && PlayerSpawnManager.Instance.OtherPlayers.TryGetValue(id, out OtherPlayer p))
        {
            Vector3 newPos = new Vector3(x, y, z);
            p.SetTargetPosition(newPos);
        }
    }
}
