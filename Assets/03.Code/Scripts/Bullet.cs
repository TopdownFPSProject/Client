using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private string ownerId;
    private long spawnedTime;
    private Vector3 spawnedPos;
    private float angle;
    private bool isSpawned = false;

    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    public void Init(string id, Vector3 spawnedPos, float angle, long time)
    {
        ownerId = id;
        spawnedTime = time;
        this.spawnedPos = spawnedPos;
        this.angle = angle;
        isSpawned = true;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    private void Update()
    {
        if (!isSpawned) return;

        //transform.rotation = Quaternion.Euler(0, angle, 0);
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        //DebugManager.Instance.Debug($"dir : {dir}, moveSpeed : {moveSpeed}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hitable"))
        {
            if (other.TryGetComponent<Players>(out Players player))
            {
                if (player.id == ownerId) return;
                string myId = TcpClientController.Instance.MyId;
                TcpClientController.Instance.SendHitMessage(myId, player.id, spawnedTime);
                //DebugManager.Instance.Debug("총알 맞음");
                //PlayerSpawnManager.Instance.DestroyPlayerObj(player.id);
            }
        }
    }

    public void Test()
    {

    }
}
