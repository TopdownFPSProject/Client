using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectHandler : IMessageHandler
{
    public void Handle(string data)
    {
        string[] parts = data.Split(';', System.StringSplitOptions.RemoveEmptyEntries);
        string id = parts[1];

        PlayerSpawnManager.Instance.DestroyPlayerObj(id);
    }
}
