using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ConversationView : MonoBehaviour
{

    #region SerializeFields
    [SerializeField]
    public RectTransform mContent;
    [SerializeField]
    public GameObject mPrefab_Conversation;
    //[SerializeField]
    //public int CONVERSATION_DATA_TO_LOAD = 0;
    #endregion
    
    #region private var
    List<ConversationDataItem> mListAllConversation;
    ConversationDataRoot mData;
    Dictionary<string, UserData> mDictionary_UserID_UserData;
    Dictionary<string, List<ConversationDataItem>> mDictionary_UserID_ConversationDataItem;
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        mListAllConversation = new List<ConversationDataItem>();
        mDictionary_UserID_UserData = new Dictionary<string, UserData>();
        mDictionary_UserID_ConversationDataItem = new Dictionary<string, List<ConversationDataItem>>();
        //CreateAllConversations();
        
    }
    #endregion
    
    #region Creations Functions
    void CreateConversation(string szTime, string conversationID, string userID)
    {
        if(mDictionary_UserID_UserData.ContainsKey(userID) == false)
        {
            //load user id
            string szResourceName = "UserData/" + userID;
            var szJson = Resources.Load<TextAsset>(szResourceName);
            UserData myUserData = JsonUtility.FromJson<UserData>(szJson.text);
            mDictionary_UserID_UserData.Add(userID, myUserData);
        }

        if(mDictionary_UserID_ConversationDataItem.ContainsKey(userID) == false)
        {
            //might as well load this up too.
            mDictionary_UserID_ConversationDataItem.Add(userID, new List<ConversationDataItem>());
        }


        //create the GameObject, set to lists
        GameObject currObject = (GameObject)Instantiate(mPrefab_Conversation, mContent);
        ConversationDataItem currConversation = currObject.GetComponent<ConversationDataItem>();
        mListAllConversation.Add(currConversation);

        List<ConversationDataItem> currListOfDataItem = mDictionary_UserID_ConversationDataItem[userID];
        currListOfDataItem.Add(currConversation);

        currConversation.SetItems(mDictionary_UserID_UserData[userID], szTime, conversationID, userID);
      

    }

    public void CreateAllConversations(int nIndex = 0)
    {
        mListAllConversation.Clear();
        mDictionary_UserID_ConversationDataItem.Clear();

        //get and set the json.
        string szResourceName = "Conversations/conversations_";
        szResourceName += nIndex.ToString();
        var szJson = Resources.Load<TextAsset>(szResourceName);
        mData = JsonUtility.FromJson<ConversationDataRoot>(szJson.text);

        if (mData != null)
        {
            if (mData.ConversationDatas != null)
            {
                for (int i = 0; i < mData.ConversationDatas.Length; i++)
                {
                    ConversationData currData = mData.ConversationDatas[i];
                    CreateConversation(currData.TimeLastMessage, currData.ID, currData.UserID);

                }
                
                LayoutRebuilder.ForceRebuildLayoutImmediate(mContent);
                StartCoroutine(DownloadAndLoadImages());
            }
            else
            {
                Debug.LogError("No data found, ConversationDatas");
            }
        }
        else
        {
            Debug.LogError("No data found");
        }
    }

    IEnumerator DownloadAndLoadImages()
    {
        foreach (KeyValuePair<string, UserData> entry in mDictionary_UserID_UserData)
        {
            
            string proxiedUrl = "https://api.allorigins.win/raw?url=" + entry.Value.IconURL;
            Debug.Log($"DownloadStart = {proxiedUrl}");

            //load in the icon
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(proxiedUrl))
            {
                yield return uwr.SendWebRequest();

                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    //failed
                    Debug.Log(uwr.error);
                }
                else
                {
                    //successful
                    // Get downloaded asset bundle
                    var texture = DownloadHandlerTexture.GetContent(uwr);
                    Sprite newSprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

                    List<ConversationDataItem> listOfConversation = mDictionary_UserID_ConversationDataItem[entry.Key];
                    for(int i = 0; i < listOfConversation.Count; i++)
                    {
                        listOfConversation[i].SetIcon(newSprite);
                        listOfConversation[i].SetName(entry.Value.FullName);
                    }
                }
            }

        }
    }
    #endregion


}
