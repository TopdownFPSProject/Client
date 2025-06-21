using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : Singleton<PlayerSpawnManager>
{
    [SerializeField] private GameObject playerObj;
    [SerializeField] private GameObject otherPlayerObj;
    [SerializeField] private Vector3 playerSpawnPos;

    private Dictionary<string, Players> players = new();
    public Dictionary<string, Players> Players { get { return players; } }

    public void MakePlayerPrefab(string id, Vector3 position)
    {
        if (players.ContainsKey(id)) return;

        if (id == TcpClientController.Instance.MyId)
        {
            Player player = Instantiate(playerObj).GetComponent<Player>();
            player.transform.position = playerSpawnPos;
            player.Init(id, position);
            players[id] = player as Players;
            DebugManager.Instance.Debug($"[내 플레이어 생성]");
        }
        else
        {
            OtherPlayer otherPlayer = Instantiate(otherPlayerObj).GetComponent<OtherPlayer>();
            otherPlayer.transform.position = position;
            otherPlayer.Init(id, position);
            players[id] = otherPlayer as Players;
            DebugManager.Instance.Debug($"[플레이어{id} 생성]");
        }
    }

    public void DestroyPlayerObj(string id)
    {
        try
        {
            if (players.ContainsKey(id))
            {
                Destroy(players[id].gameObject);
                players.Remove(id);
            }
        }
        catch (Exception e)
        {
            DebugManager.Instance.Debug($"[플레이어 제거 중 오류발생] : {e.Message}");
        }
    }
}
