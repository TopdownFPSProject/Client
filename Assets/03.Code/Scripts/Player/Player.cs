using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : Players
{
    [SerializeField] private TextMeshPro idText;
    private string id;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Init(string id, Vector3 position)
    {
        this.id = id;
        idText.text = id;
        //transform.position = position;
        targetPosition = position;
    }

    private void Update()
    {
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) dir += Vector2.up;
        if (Input.GetKey(KeyCode.S)) dir += Vector2.down;
        if (Input.GetKey(KeyCode.A)) dir += Vector2.left;
        if (Input.GetKey(KeyCode.D)) dir += Vector2.right;

        bool isMoving = dir.sqrMagnitude > 0.01f;

        if (isMoving)
        {
            //DebugManager.Instance.Debug($"[입력] : {dir.normalized}");
            TcpClientController.Instance.SendMoveInput(dir.normalized, isMoving);
        }
    }
}
