using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : Players
{
    [SerializeField] private TextMeshPro idText;
    private string id;

    public void Init(string id, Vector3 position)
    {
        this.id = id;
        idText.text = id;
        transform.position = position;
    }

    private void Update()
    {
        Vector3 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) dir += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) dir += Vector3.back;
        if (Input.GetKey(KeyCode.A)) dir += Vector3.left;
        if (Input.GetKey(KeyCode.D)) dir += Vector3.right;

        bool isMoving = dir.sqrMagnitude > 0.01f;

        if (isMoving)
        {
            //TcpClientController.Instance.SendMoveInput(dir.normalized, isMoving);
        }
    }
}
