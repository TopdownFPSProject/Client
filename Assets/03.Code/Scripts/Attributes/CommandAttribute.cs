using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class)]
public class CommandAttribute : Attribute
{
    public string Command { get; }
    public CommandAttribute(string command) => Command = command;
}
