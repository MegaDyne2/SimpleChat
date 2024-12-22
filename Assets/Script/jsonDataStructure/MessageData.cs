using System;

[Serializable]
public class MessageDataRoot
{
    public string MainUserID;
    public bool hasMoreMessage;
    public string[] OthersID;
    public MessageData[] Messages;
}

[Serializable]
public class MessageData
{
    public int ID;
    public int UserIndex;
    public string Message;
    public string Time;
}