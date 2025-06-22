using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Players : MonoBehaviour
{
    [SerializeField] protected float lerpSpeed = 0.1f;

    protected Rigidbody rb;
    protected Vector3 targetPosition;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        targetPosition = transform.position;
    }

    public virtual void SetTargetPosition(Vector3 pos)
    {
        DebugManager.Instance.Debug("움직임");
        targetPosition = pos;
    }

    protected virtual void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            Vector3 newPos = Vector3.Lerp(transform.position, targetPosition, lerpSpeed);
            rb.MovePosition(newPos);
        }
    }
}
