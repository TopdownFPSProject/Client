using MessagePack;
using SharedPacketLib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPositionHandler : IMessageHandler
{
    public void Handle(PacketBase basePacket)
    {
        if (basePacket is C_PositionPacket pac)
        {
            string id = pac.Id;
            Vector3 pos = new Vector3(pac.X, pac.Y, pac.Z);

            if (PlayerSpawnManager.Instance.Players.TryGetValue(id, out Players p))
            {
                p.SetServerPosition(pos);
            }
        }
    }
}
