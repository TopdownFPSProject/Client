using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float lerpSpeed = 0.1f;
    // 오차가 snapThreshold을 넘으면 순간이동으로 동기화 됨
    [SerializeField] protected float snapThreshold = 1.0f;

    public string id;
    protected Vector3 targetPosition;

    protected virtual void Update()
    {
        float dist = Vector3.Distance(transform.position, targetPosition);
        if (dist > snapThreshold)
        {
            transform.position = targetPosition; // 순간이동
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed);
        }
    }

    public void SetServerPosition(Vector3 newPosition)
    {
        targetPosition = newPosition;    
    }
}
