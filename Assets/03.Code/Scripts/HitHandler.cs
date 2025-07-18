using SharedPacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
class HitHandler : IMessageHandler
{
    public void Handle(PacketBase basePacket)
    {
        if (basePacket is S_HitInfoPacket pac)
        {
            if (PlayerSpawnManager.Instance.Players.TryGetValue(pac.target, out Players targetPlayer))
            {
                PlayerSpawnManager.Instance.DestroyPlayerObj(pac.target);
                DebugManager.Instance.Debug($"{pac.target}가 죽음");
                Debug.Log($"{pac.target}가 죽음");
                Debug.Log($"{pac.shooter}가 쏨");
            }
        }
    }
}

