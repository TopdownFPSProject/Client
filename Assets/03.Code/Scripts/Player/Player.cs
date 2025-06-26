using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class Player : Players
{
    [SerializeField] private TextMeshPro idText;

    //private Rigidbody rb;
    //private string id;
    //private float minSpeed = 0.02f;

    //메시지 전송
    [SerializeField] private float sendInterval = 0.05f;
    private float sendTimer = 0f;

    //위치
    //private Vector3 lastSentPosition;
    //private float positionThreshold = 0.02f;
    private WaitForSeconds sendTime = new WaitForSeconds(0.25f);

    public void Init(string id, Vector3 position)
    {
        this.id = id;
        idText.text = id;
        targetPosition = position;
    }

    protected override void Update()
    {
        base.Update();

        Vector3 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) dir += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) dir += Vector3.back;
        if (Input.GetKey(KeyCode.A)) dir += Vector3.left;
        if (Input.GetKey(KeyCode.D)) dir += Vector3.right;
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Fire();
        //}

        //Move(dir);

        if (dir != Vector3.zero)
        {
            Vector3 direction = dir.normalized;
            string msg = $"input;{id};{dir.x};{dir.y};{dir.z}";
            TcpClientController.Instance.SendMyInputMessage(msg);
        }
    }

    //private void Move(Vector3 inputDir)
    //{
    //    if (inputDir != Vector3.zero)
    //    {
    //        Vector3 velocity = new Vector3(inputDir.x * moveSpeed, rb.velocity.y, inputDir.z * moveSpeed);
    //        //rb.velocity = velocity;
    //    }
    //    else
    //    {
    //        //rb.velocity = new Vector3(0, 0, 0);
    //    }
    //}

    private void Fire()
    {
        string time = DateTime.Now.ToString();
        Vector3 position = transform.position;
        Vector3 forward = transform.forward;
        //DebugManager.Instance.Debug($"transform.forward : {transform.forward}");
        print($"fire 호출");

        TcpClientController.Instance.SendFireMessage(time, position, forward);
    }

    //private IEnumerator SendPositionCoroutine()
    //{
    //    while (true)
    //    {
    //        if (Vector3.Distance(transform.position, lastSentPosition) > positionThreshold)
    //        {
    //            TcpClientController.Instance.SendMyPosition(transform.position);
    //            lastSentPosition = transform.position;
    //        }
    //        //yield return new WaitForSeconds(0.05f);
    //        yield return sendTime;
    //    }
    //}

    float testTimer = 1f;
    float elapsedTime = 0f;
    private IEnumerator BulletTestCoroutine()
    {
        while (true)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= testTimer)
            {
                elapsedTime = 0f;
                Fire();
            }
            yield return null;
        }
    }
}
