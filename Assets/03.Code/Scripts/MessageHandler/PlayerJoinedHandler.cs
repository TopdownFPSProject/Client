using MessagePack;
using SharedPacket;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoinedHandler : IMessageHandler
{
    public void Handle(PacketBase basePacket)
    {
        if (basePacket is S_PlayerJoinedPacket joinedPacket)
        {
            string id = joinedPacket.Id;
            float x = joinedPacket.X;
            float y = joinedPacket.Y;
            float z = joinedPacket.Z;

            Vector3 pos = new Vector3(x, y, z);

            PlayerSpawnManager.Instance.MakePlayerPrefab(id, pos);

            DebugManager.Instance.Debug($"플레이어 {id} 위치: ({x}, {y}, {z})");
        }
    }
}
