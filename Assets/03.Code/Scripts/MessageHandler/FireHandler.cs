using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHandler : IMessageHandler
{
    public void Handle(string data)
    {
        string[] parts = data.Split(';', System.StringSplitOptions.RemoveEmptyEntries);

        string id = parts[1];
        float posX = float.Parse(parts[2]);
        float posY = float.Parse(parts[3]);
        float posZ = float.Parse(parts[4]);
        float idX = float.Parse(parts[5]);
        float idY = float.Parse(parts[6]);
        float idZ = float.Parse(parts[7]);
        string time = parts[8];
        Vector3 spawnedPos = new Vector3(posX, posY, posZ);
        Vector3 dir = new Vector3(idX, idY, idZ);

        BulletSpawnManager.Instance.Fire(id, spawnedPos, dir, time);
    }
}
