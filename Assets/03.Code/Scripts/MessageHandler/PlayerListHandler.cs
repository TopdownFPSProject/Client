using MessagePack;
using SharedPacket;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 새로운 클라이언트가 들어올때 기존 클라이언트의 정보를 보내는 핸들러
public class PlayerListHandler : IMessageHandler
{
    public void Handle(PacketBase basePacket)
    {
        if (basePacket is S_PlayerListPacket listPacket)
        {
            // 처리
            //S_PlayerListPacket packet = MessagePackSerializer.Deserialize<S_PlayerListPacket>(body);

            foreach (PlayerInfo p in listPacket.Players)
            {
                string id = p.Id;
                float x = p.X;
                float y = p.Y;
                float z = p.Z;

                Vector3 pos = new Vector3(x, y, z);

                DebugManager.Instance.Debug($"플레이어 {id} 위치: ({x}, {y}, {z})");

                PlayerSpawnManager.Instance.MakePlayerPrefab(id, pos);
            }
        }
    }
}
