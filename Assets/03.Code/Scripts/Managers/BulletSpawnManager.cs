using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnManager : Singleton<BulletSpawnManager>
{
    [SerializeField] private GameObject bulletPrefab;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Fire(string id, Vector3 spawnPos, Vector3 dir, string time) 
    {
        GameObject obj = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        Bullet bullet = obj.GetComponent<Bullet>();
        bullet.Init(id, spawnPos, dir, time);
    }
}
