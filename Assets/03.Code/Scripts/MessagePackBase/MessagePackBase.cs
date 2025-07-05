using MessagePack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[MessagePackObject]
public class MessagePackBase
{
    [Key(0)]
    public string Command { get; set; }
}

[MessagePackObject]
public class ConnectPacket : MessagePackBase
{
    [Key(1)] public string Id { get; set; }
}

[MessagePackObject]
public class DisconnectPacket : MessagePackBase
{
    [Key(1)] public string Id { get; set; }
}

[MessagePackObject]
public class PositionPacket : MessagePackBase 
{ 
    [Key(1)]
    public string Id { get; set; }
    [Key(2)]
    public float X { get; set; }
    [Key(3)]
    public float Y { get; set; }
    [Key(4)]
    public float Z { get; set; }
}
