using System;

[Serializable]
public class ConversationDataRoot
{
    public ConversationData[] ConversationDatas;
}


[Serializable]
public class ConversationData
{
    public string ID;
    public string UserID;
    public string TimeLastMessage;
}

