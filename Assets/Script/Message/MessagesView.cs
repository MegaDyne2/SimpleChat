using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MessagesView : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    private RectTransform mContent;
    [SerializeField]
    private GameObject mPrefab_MessagesUI;
    [SerializeField]
    private GameObject mPrefab_MoreMessages;
    [SerializeField]
    private GameObject mPrefab_LabelLog;
    #endregion
    
    #region Private Var
    private string mConversationID;
    private MessageDataRoot mData;
    private string mMainUserID;
    private string[] mOthersID;
    private bool mHasMoreMessage = false;
    private int mEarliestID = 0;
    private GameObject mBtnMoreMessage;
    private GameObject mLblLog;
    private TMP_Text mTextLog;
    #endregion
    
    #region Unity Function
    // Start is called before the first frame update
    void Awake()
    {
        mDictionary_UserID_MessageUIScript = new Dictionary<string, List<SingleMessage>>();
        mDictionary_UserID_UserData = new Dictionary<string, UserData>();
        mListAllMessages = new List<SingleMessage>();
    }

    #endregion

    #region UI Interactions
    public void OnButtonClose()
    {
        this.gameObject.SetActive(false);
    }
    #endregion

    #region public functions
    /// <summary>
    /// for loading up 
    /// </summary>
    /// <param name="inConversationID"></param>
    public void Setup(string inConversationID)
    {
        mConversationID = inConversationID;
        ClearAllUserDictionary();
        ClearMessages();
        CreateAllMessagesUI(-1);
        SetTopMessage();
        SetEarliestID();

    }

    public void LoadMoreMessages()
    {
        //get the next number
        CreateAllMessagesUI(mEarliestID - 1);
        SetTopMessage();
        SetEarliestID();
    }

    public void ResetScrollbar()
    {
        mContent.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }

    #endregion

    #region private functions
    private void SetEarliestID()
    {
        //save he earlist number
        if (mData != null && mData.Messages != null && mData.Messages.Length > 0)
            mEarliestID = mData.Messages[0].ID;
    }


    private void SetTopMessage()
    {

        if(mHasMoreMessage)
        {
            if (mBtnMoreMessage == null)
            {
                mBtnMoreMessage = GameObject.Instantiate(mPrefab_MoreMessages, mContent);
            }
            mBtnMoreMessage.transform.SetAsFirstSibling();


            if(mPrefab_LabelLog != null)
            {
                Destroy(mLblLog);
            }
        }
        else
        {
            if (mBtnMoreMessage != null)
            {
                Destroy(mBtnMoreMessage);
            }

            if(mLblLog == null)
            {
                mLblLog = GameObject.Instantiate(mPrefab_LabelLog, mContent);
                mTextLog = mLblLog.GetComponentInChildren<TMP_Text>();
            }
            mLblLog.transform.SetAsFirstSibling();

            switch(mErrorMessage)
            {
                case ErrorMessage.NoError:
                case ErrorMessage.NoMoreMessage:
                    mTextLog.SetText("No More Message");
                    break;
                case ErrorMessage.FailedToLoadJson:
                    mTextLog.SetText("Error: Cannot load Json");
                    break;
            }
            
        }
    }

    private void ClearMessages()
    {
        for(int i = 0; i < mContent.childCount; i++)
        {
            Transform currTransform = mContent.GetChild(i);
            Destroy(currTransform.gameObject);
        }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="lastMessageID"> -1 is recent messages</param>
    private void CreateAllMessagesUI(int lastMessageID = -1)
    {
        //mDictionary_UserID_MessageUIScript.Clear();
        ClearAllUserDictionary();

        mHasMoreMessage = false;


        bool bReverse = false;
        string szResourceName = "Messages/" + mConversationID +"/";
        if(lastMessageID == -1)
        {
            szResourceName += "Recent";
        }
        else
        {
            szResourceName += lastMessageID;
            bReverse = true;
        }

        var szJson = Resources.Load<TextAsset>(szResourceName);

        if (szJson == null)
        {
            Debug.LogError("Failed to get json");
            mErrorMessage = ErrorMessage.FailedToLoadJson;
            return;
        }
        mData = JsonUtility.FromJson<MessageDataRoot>(szJson.text);

        //save the mainuserid and othersid
        if(mData != null)
        {
            mMainUserID = mData.MainUserID;
            mOthersID = mData.OthersID;
            mHasMoreMessage = mData.hasMoreMessage;

            if (mData.Messages != null)
            {
                if (bReverse == false)
                {
                    //make the items.
                    for (int i = 0; i < mData.Messages.Length; i++)
                    {
                        MessageData currMessageData = mData.Messages[i];
                        CreateMessageUI(currMessageData);
                    }
                }
                else
                {
                    for (int i = mData.Messages.Length -1; i >= 0; i--)
                    {
                        MessageData currMessageData = mData.Messages[i];
                        GameObject newGameObject = CreateMessageUI(currMessageData);
                        newGameObject.transform.SetAsFirstSibling();

                    }
                }

                LayoutRebuilder.ForceRebuildLayoutImmediate(mContent);

                StartCoroutine(DownloadAndLoadImages());
            }
            else
            {
                Debug.LogError("No data found, Messages");
            }
        }
        else
        {
            Debug.LogError("No data found");
        }

        mErrorMessage = ErrorMessage.NoError;

    }

    private GameObject CreateMessageUI(MessageData currMessageData)
    {
        if(currMessageData == null)
        {
            Debug.LogError("NoMessageData");
            return null;

        }

        string userID;
        if(currMessageData.UserIndex == -1)
        {
            //use main.
            userID = mMainUserID;
        }
        else
        {
            //use index
            userID = mOthersID[currMessageData.UserIndex];

     
        }


        if (mDictionary_UserID_MessageUIScript.ContainsKey(userID) == false)
        {
            mDictionary_UserID_MessageUIScript.Add(userID, new List<SingleMessage>());
        }

        if(mDictionary_UserID_UserData.ContainsKey(userID) == false)
        {
            string szResourceName = "UserData/" + userID;
            var szJson = Resources.Load<TextAsset>(szResourceName);
            UserData myUserData = JsonUtility.FromJson<UserData>(szJson.text);
            mDictionary_UserID_UserData.Add(userID, myUserData);
        }


        GameObject currObject = (GameObject)Instantiate(mPrefab_MessagesUI, mContent);
        SingleMessage currMessageUI = currObject.GetComponent<SingleMessage>();
        mListAllMessages.Add(currMessageUI);
        List<SingleMessage> currMessageUIScript = mDictionary_UserID_MessageUIScript[userID];
        currMessageUIScript.Add(currMessageUI);


        currMessageUI.Setup(null, currMessageData.Message, currMessageData.UserIndex != -1);

        return currObject;

    }

    private void ClearAllUserDictionary()
    {
        mDictionary_UserID_MessageUIScript.Clear();
        mDictionary_UserID_UserData.Clear();
    }


    IEnumerator DownloadAndLoadImages()
    {
        foreach (KeyValuePair<string, UserData> entry in mDictionary_UserID_UserData)
        {
            string proxiedUrl = "https://api.allorigins.win/raw?url=" + entry.Value.IconURL;

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

                    if (mDictionary_UserID_MessageUIScript.ContainsKey(entry.Key))
                    {
                        List<SingleMessage> listOfConversation = mDictionary_UserID_MessageUIScript[entry.Key];
                        for (int i = 0; i < listOfConversation.Count; i++)
                        {
                            listOfConversation[i].SetIcon(newSprite);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// keeping track of which UI element is being used by a certain user to load image to.
    /// </summary>
    private Dictionary<string, List<SingleMessage>> mDictionary_UserID_MessageUIScript;

    /// <summary>
    /// use to save images.
    /// </summary>
    private Dictionary<string, UserData> mDictionary_UserID_UserData;

    private List<SingleMessage> mListAllMessages;

    private ErrorMessage mErrorMessage = ErrorMessage.NoError;
    #endregion
    
    #region enum
    public enum ErrorMessage
    {
        NoError,
        NoMoreMessage,
        FailedToLoadJson,
        Max

    }
    #endregion
}
