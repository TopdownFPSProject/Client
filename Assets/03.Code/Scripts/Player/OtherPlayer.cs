using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OtherPlayer : Players
{
    [SerializeField] private TextMeshPro idText;
    [SerializeField] private float lerpSpeed;

    private Vector3 targetPosition;
    //private string id;

    protected void Awake()
    {
        targetPosition = transform.position;
    }

    public void SetTargetPosition(Vector3 pos)
    {
        DebugManager.Instance.Debug("움직임");
        targetPosition = pos;
    }

    protected void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            Vector3 newPos = Vector3.Lerp(transform.position, targetPosition, lerpSpeed);
            transform.position = newPos;
            //rb.MovePosition(newPos);
        }
    }
    public void Init(string id, Vector3 position)
    {
        this.id = id;
        idText.text = id;
        //transform.position = position;
        transform.position = position;
    }
}
