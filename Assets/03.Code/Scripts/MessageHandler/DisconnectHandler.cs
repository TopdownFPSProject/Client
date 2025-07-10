using SharedPacketLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectHandler : IMessageHandler
{
    public void Handle(PacketBase body)
    {
        if (body is C_DisconnectPacket packet)
        {
            string id = packet.Id;
            PlayerSpawnManager.Instance.DestroyPlayerObj(id);
        }
        //string[] parts = data.Split(';', System.StringSplitOptions.RemoveEmptyEntries);
        //string id = parts[1];
    }
}
