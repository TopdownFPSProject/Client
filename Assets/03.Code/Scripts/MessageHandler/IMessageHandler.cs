using SharedPacketLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMessageHandler
{
    void Handle(PacketBase body);
}
