using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private string ownerId;
    private long spawnedTime;
    private Vector3 spawnedPos;
    private float angle;
    private bool isSpawned = false;
    private bool _isDestroyed = false;
    private Coroutine _destroyCoroutine;

    private IObjectPool<Bullet> _managedPool;

    //private void Start()
    //{
    //    Destroy(gameObject, 3f);
    //}

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
        if (_isDestroyed) return;

        // 히트 판정 (총알 주인만 처리)
        if (ownerId == TcpClientController.Instance.MyId)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("OtherPlayer") ||
                other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (other.TryGetComponent<Players>(out Players player))
                {
                    if (player.id == ownerId) return;  // 자신의 총알이 자신에게 맞지 않도록
                    TcpClientController.Instance.SendHitMessage(ownerId, player.id, spawnedTime);
                }
            }
        }

        // 총알 파괴 처리 (모든 클라이언트에서 처리)
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall") ||
            ((other.gameObject.layer == LayerMask.NameToLayer("OtherPlayer") ||
              other.gameObject.layer == LayerMask.NameToLayer("Player")) &&
             other.TryGetComponent<Players>(out Players hitPlayer) &&
             hitPlayer.id != ownerId))  // 총알 주인이 아닌 플레이어와 충돌할 때만 파괴
        {
            DestroyBullet();
        }
    }

    public void SetManegedPool(IObjectPool<Bullet> pool)
    {
        // 풀에서 가져올 때 초기화
        _isDestroyed = false;
        _managedPool = pool;

        // 이전 코루틴이 있다면 정지
        if (_destroyCoroutine != null)
        {
            StopCoroutine(_destroyCoroutine);
        }
        // 새로운 코루틴 시작
        _destroyCoroutine = StartCoroutine(DestroyAfterDelay());
    }
    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        DestroyBullet();
    }
    public void DestroyBullet()
    {
        // 이미 파괴된 상태면 무시
        if (_isDestroyed) return;  

        _isDestroyed = true;

        // 실행 중인 코루틴이 있다면 정지
        if (_destroyCoroutine != null)
        {
            StopCoroutine(_destroyCoroutine);
            _destroyCoroutine = null;
        }

        if (_managedPool != null)
        {
            _managedPool.Release(this);
        }
    }
    private void OnDisable()
    {
        // 오브젝트가 비활성화될 때 코루틴 정지
        if (_destroyCoroutine != null)
        {
            StopCoroutine(_destroyCoroutine);
            _destroyCoroutine = null;
        }
    }
}
