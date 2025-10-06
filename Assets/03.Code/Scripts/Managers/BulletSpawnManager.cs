using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletSpawnManager : Singleton<BulletSpawnManager>
{
    [SerializeField] private GameObject bulletPrefab;

    private IObjectPool<Bullet> _pool;

    protected override void Awake()
    {
        base.Awake();
        _pool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize : 100); 
    }

    public void Fire(string id, Vector3 spawnPos, float dir, long time) 
    {
        Bullet bullet = _pool.Get();
        bullet.transform.position = spawnPos;
        bullet.Init(id, spawnPos, dir, time);
        bullet.SetManegedPool(_pool);
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab).GetComponent<Bullet>();
        return bullet;
    }

    private void OnGetBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
}
