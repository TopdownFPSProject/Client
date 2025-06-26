using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPositionHandler : IMessageHandler
{
    public void Handle(string data)
    {
        string[] parts = data.Split(';', StringSplitOptions.RemoveEmptyEntries);

        string id = parts[1];
        float x = float.Parse(parts[2]);
        float y = float.Parse(parts[3]);
        float z = float.Parse(parts[4]);

        if (PlayerSpawnManager.Instance.Players.TryGetValue(id, out Players p))
        {
            p.SetServerPosition(new Vector3(x, y, z));
        }
    }
}
