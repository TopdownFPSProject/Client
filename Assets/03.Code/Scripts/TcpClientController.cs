using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SharedPacketLib;

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
    private readonly Queue<byte[]> messageQueue = new();
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
                byte[] msg = messageQueue.Dequeue();
                HandleServerMessage(msg);
            }
        }
    }

    private void InitMessageHandler()
    {
        Type[] allType = Assembly.GetExecutingAssembly().GetTypes();
        foreach (Type handlerType in allType)
        {
            if (!typeof(IMessageHandler).IsAssignableFrom(handlerType)) continue;
            CommandAttribute ca = (CommandAttribute)Attribute.GetCustomAttribute(handlerType, typeof(CommandAttribute));
            if (ca == null) continue;

            IMessageHandler handlerInstance = (IMessageHandler)Activator.CreateInstance(handlerType);
            messageHandlers[ca.Command] = handlerInstance;
        }
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
        if (stream == null) return;
        C_ConnectPacket cs = new C_ConnectPacket
        {
            Command = "connected",
            Id = myId
        };
        SendMessageToServer(packet);
    }

    private void SendDisconnectMessage(string id)
    {
        if (stream == null) return;
        var packet = new DisconnectPacket
        {
            Command = "disconnected",
            Id = myId
        };
        SendMessageToServer(packet);
    }

    public void SendMyInputMessage(Vector3 dir)
    {
        if (stream == null) return;
        var packet = new PositionPacket
        {
            Command = "input",
            Id = myId,
            X = dir.x,
            Y = dir.y,
            Z = dir.z
        };
        SendMessageToServer(packet);
    }

    public void SendFireMessage(string time, Vector3 position, Vector3 dir)
    {
        if (stream == null) return;
        string msg = $"fire;{myId};{position.x};{position.y};{position.z};{dir.x};{dir.y};{dir.z};{time}";

        SendMessageToServer(msg);
    }
    #endregion

    private void OnApplicationQuit()
    {
        SendDisconnectMessage(myId);
    }

    #region 서버 통신 및 수신
    public async void SendMessageToServer<T>(T packet)
    {
        if (stream == null) return;

        byte[] body = MessagePack.MessagePackSerializer.Serialize(packet);
        int length = body.Length;
        byte[] header = BitConverter.GetBytes(length);
        byte[] sendPacket = new byte[4 + length];

        Buffer.BlockCopy(header, 0, sendPacket, 0, 4);
        Buffer.BlockCopy(body, 0, sendPacket, 4, length);

        await stream.WriteAsync(sendPacket, 0, sendPacket.Length);
    }

    private void HandleServerMessage(byte[] packet)
    {
        //if (string.IsNullOrEmpty(msg))
        //{
        //    DebugManager.Instance.Debug("[메시지 없음]");
        //    return;
        //}

        //빈 문자열은 제거
        MessagePackBase basePacket = MessagePack.MessagePackSerializer.Deserialize<MessagePackBase>(body);
        string command = basePacket.Command;

        if (messageHandlers.TryGetValue(command, out IMessageHandler handler))
        {
            handler.Handle(packet); // 핸들러에서 body를 실제 타입으로 역직렬화
        }
        else
        {
            Debug.LogWarning($"[알 수 없는 명령] : {command}");
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

                lock (messageQueue)
                {
                    messageQueue.Enqueue(body);
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
