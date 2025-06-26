using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private string ownerId;
    private string spawnedTime;
    private Vector3 spawnedPos;
    private Vector3 dir;
    private bool isSpawned = false;

    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    public void Init(string id, Vector3 spawnedPos, Vector3 dir, string time)
    {
        ownerId = id;
        spawnedTime = time;
        this.spawnedPos = spawnedPos;
        this.dir = dir;
        isSpawned = true;
    }

    private void Update()
    {
        if (!isSpawned) return;

        transform.Translate(dir * moveSpeed * Time.deltaTime);
        //DebugManager.Instance.Debug($"dir : {dir}, moveSpeed : {moveSpeed}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hitable"))
        {
            if (other.TryGetComponent<Players>(out Players player))
            {
                if (player.id == ownerId) return;
                PlayerSpawnManager.Instance.DestroyPlayerObj(player.id);
            }
        }
    }
}
