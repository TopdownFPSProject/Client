using MessagePack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPositionHandler : IMessageHandler
{
    public void Handle(byte[] body)
    {
        PositionPacket packet = MessagePackSerializer.Deserialize<PositionPacket>(body);

        string id = packet.Id;
        Vector3 pos = new Vector3(packet.X, packet.Y, packet.Z);

        if (PlayerSpawnManager.Instance.Players.TryGetValue(id, out Players p))
        {
            p.SetServerPosition(pos);
        }
    }
}
