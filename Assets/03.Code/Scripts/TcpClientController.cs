using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//public class NetworkMessage
//{
//    public string command;
//    public string id;
//    public string target = "all"; //기본 all
//    public Dictionary<string, object> data = new();
//}


public class TcpClientController : Singleton<TcpClientController>
{
    //서버관련
    [SerializeField] private string serverIP = "127.0.0.1";
    [SerializeField] private int port = 7777;

    //UI
    [SerializeField] private TMP_InputField nickInput;
    [SerializeField] private Button connectBtn;

    private TcpClient client;
    private NetworkStream stream;
    private string myId;
    public string MyId => myId;

    private bool isConnected = false;
    private readonly Queue<string> messageQueue = new();
    private Dictionary<string, IMessageHandler> messageHandlers;

    protected override void Awake()
    {
        base.Awake();
        connectBtn.onClick.AddListener(OnClickConnectBtn);
        InitMessageHandler();
    }

    private void Update()
    {
        if (!isConnected) return;

        lock (messageQueue)
        {
            while (messageQueue.Count > 0)
            {
                string msg = messageQueue.Dequeue();
                HandleServerMessage(msg);
            }
        }
    }

    private void InitMessageHandler()
    {
        messageHandlers = new()
        {
            //["connected"] = new ConnectHandler(),
            ["playerList"] = new PlayerListHandler(),
            ["playerJoined"] = new PlayerJoinedHandler(),
            ["disconnected"] = new DisconnectHandler(),
            //["syncPosition"] = new SyncPositionHandler(),
            //["fire"] = new FireHandler(),
        };
    }

    private void ConnectToServer()
    {
        try
        {
            client = new TcpClient(serverIP, port);
            isConnected = true;
            stream = client.GetStream();

            SendConnectMessage();

            //메시지 입력 받기 시작
            _ = Task.Run(ReceiveLoop);
        }
        catch (Exception e)
        {
            DebugManager.Instance.Debug($"[연결 오류] : {e.Message}");
        }
    }

    #region 서버에게 보내는 메시지
    private void SendConnectMessage()
    {
        string msg = $"connected;{myId};";
        SendMessageToServer(msg);
    }

    private void SendDisconnectMessage(string id)
    {
        string msg = $"disconnected;{id};";
        SendMessageToServer(msg);
    }
    #endregion

    private void OnApplicationQuit()
    {
        SendDisconnectMessage(myId);
    }

    //public void SendMoveInput(Vector3 dir, bool isMoving)
    //{
    //    NetworkMessage msg = new NetworkMessage
    //    {
    //        command = "moveInput",
    //        id = myId,
    //        data = new Dictionary<string, object>
    //        {
    //        { "dirX", dir.x },
    //        { "dirY", dir.y },
    //        { "dirZ", dir.z },
    //        { "isMoving", isMoving }
    //        }
    //    };

    //    SendMessageToServer(msg);
    //}

    #region 서버 통신 및 수신
    private async void SendMessageToServer(string msg)
    {
        if (stream == null) return;

        byte[] body = System.Text.Encoding.UTF8.GetBytes(msg);
        int length = body.Length;
        byte[] header = BitConverter.GetBytes(length);
        byte[] packet = new byte[4 + length];

        Buffer.BlockCopy(header, 0, packet, 0, 4);
        Buffer.BlockCopy(body, 0, packet, 4, length);

        await stream.WriteAsync(packet, 0, packet.Length);
    }

    private void HandleServerMessage(string msg)
    {
        if (string.IsNullOrEmpty(msg))
        {
            DebugManager.Instance.Debug("[메시지 없음]");
            return;
        }

        //빈 문자열은 제거
        string[] parts = msg.Split(';');

        if (messageHandlers.TryGetValue(parts[0], out IMessageHandler handler))
        {
            DebugManager.Instance.Debug($"[서버에서 넘어온 메시지] : {msg}");
            handler.Handle(msg);
        }
        else
        {
            DebugManager.Instance.Debug($"[알 수 없는 명령] : {msg}");
        }
    }

    //서버 메시지 받는 함수
    private async void ReceiveLoop()
    {
        byte[] buffer = new byte[4096];
        try
        {
            while (true)
            {
                int headerRead = 0;
                while (headerRead < 4)
                {
                    int read = await stream.ReadAsync(buffer, headerRead, 4 - headerRead);
                    if (read == 0) return;
                    headerRead += read;
                }

                int bodyLength = BitConverter.ToInt32(buffer, 0);
                if (bodyLength <= 0 || bodyLength > buffer.Length)
                {
                    DebugManager.Instance.Debug("[패킷 길이 오류]");
                    return;
                }

                int bodyRead = 0;
                byte[] body = new byte[bodyLength];
                while (bodyRead < body.Length)
                {
                    int read = await stream.ReadAsync(body, bodyRead, bodyLength - bodyRead);
                    if (read == 0) return;
                    bodyRead += read;
                }

                string msg = System.Text.Encoding.UTF8.GetString(body);

                lock (messageQueue)
                {
                    messageQueue.Enqueue(msg);
                }
            }
        }
        catch (Exception e)
        {
            DebugManager.Instance.Debug($"[Receive 예외] {e.Message}");
        }
    }
#endregion

    private void OnClickConnectBtn()
    {
        myId = nickInput.text;
        ConnectToServer();
    }
}
