using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkMessage
{
    public string command;
    public string id;
    public string target = "all"; //기본 all
    public Dictionary<string, object> data = new();
}


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

    private void Awake()
    {
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
                string json = messageQueue.Dequeue();
                HandleServerMessage(json);
            }
        }
    }

    private void InitMessageHandler()
    {
        messageHandlers = new()
        {
            ["connected"] = new ConnectHandler(),
            ["playerList"] = new ConnectHandler(),
            ["playerJoined"] = new ConnectHandler(),
            ["disconnected"] = new DisconnectHandler(),
            ["position"] = new PositionHandler(),
            ["fire"] = new FireHandler(),
        };
    }

    private void ConnectToServer()
    {
        try
        {
            client = new TcpClient(serverIP, port);
            isConnected = true;
            stream = client.GetStream();
            myId = nickInput.text;

            SendConnectMessage();

            //메시지 입력 받기 시작
            _ = Task.Run(ReceiveLoop);
        }
        catch (Exception e)
        {
            DebugManager.Instance.Debug($"[연결 오류] : {e.Message}");
        }
    }

    private void SendConnectMessage()
    {
        var msg = new NetworkMessage()
        {
            command = "connected",
            id = myId,
        };
        SendMessageToServer(msg);
    }

#region 서버 통신 및 수신
    private async void SendMessageToServer(NetworkMessage msg)
    {
        if (stream == null) return;
        string json = JsonUtility.ToJson(msg);
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        int length = body.Length;
        byte[] header = BitConverter.GetBytes(length);
        byte[] packet = new byte[4 + length];

        Buffer.BlockCopy(header, 0, packet, 0, 4);
        Buffer.BlockCopy(body, 0, packet, 4, length);

        await stream.WriteAsync(packet, 0, packet.Length);
    }

    private void HandleServerMessage(string json)
    {
        NetworkMessage msg = JsonUtility.FromJson<NetworkMessage>(json);
        if (msg == null || string.IsNullOrEmpty(msg.command))
        {
            DebugManager.Instance.Debug("[메시지 파싱 오류]");
            return;
        }

        if (messageHandlers.TryGetValue(msg.command, out IMessageHandler handler))
        {
            handler.Handle(msg);
        }
        else
        {
            DebugManager.Instance.Debug($"[알 수 없는 명령] : {msg.command}");
        }
    }
#endregion

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

                string json = System.Text.Encoding.UTF8.GetString(body);

                lock (messageQueue)
                {
                    messageQueue.Enqueue(json);
                }
            }
        }
        catch (Exception e)
        {
            DebugManager.Instance.Debug($"[Receive 예외] {e.Message}");
        }
    }

    private void OnClickConnectBtn()
    {
        ConnectToServer();
    }
}
