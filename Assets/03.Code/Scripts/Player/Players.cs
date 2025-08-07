using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Players : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float lerpSpeed = 0.1f;
    // 오차가 snapThreshold을 넘으면 순간이동으로 동기화 됨
    [SerializeField] protected float snapThreshold = 5.0f;
    [SerializeField] private Image hpBar;

    public string id;
    protected float maxHp;
    protected float currentHp;
    protected Vector3 targetPosition;

    protected virtual void FixedUpdate()
    {
        float dist = Vector3.Distance(transform.position, targetPosition);
        if (dist > snapThreshold)
        {
            Debug.Log("순간이동");
            transform.position = targetPosition; // 순간이동
        }
        else
        {
            //Debug.Log("부드러운 이동");
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed);
        }
    }

    public virtual void Init(string id, Vector3 position)
    {
        maxHp = 100;
        currentHp = 100;
    }

    // 실시간으로 위치와 시야 동기화
    public void SetServerPosition(Vector3 newPosition, float angle)
    {
        targetPosition = newPosition;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        float ratio = Mathf.Clamp01(currentHp / maxHp);
        hpBar.fillAmount = ratio;
    }
}
