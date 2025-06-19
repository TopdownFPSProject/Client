using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectHandler : IMessageHandler
{
    public void Handle(NetworkMessage msg)
    {
        if (PlayerSpawnManager.Instance.Players.ContainsKey(msg.id))
        {
            PlayerSpawnManager.Instance.DestroyPlayerObj(msg.id);
        }
    }
}
