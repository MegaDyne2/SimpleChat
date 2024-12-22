using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    #region SerialzeField
    [SerializeField]
    private RectTransform panelListOfUsers;
    [SerializeField]
    private ProfileView mProfileView;
    [SerializeField]
    private MessagesView mMessageView;
    [SerializeField]
    private ConversationView mConversationView;

    #endregion

    #region Public Functions
    public void ShowProfileView(string inUserID)
    {
        mProfileView.gameObject.SetActive(true);
        mProfileView.Setup(inUserID);
    }

    public void ShowMessageView(string szID)
    {
        mMessageView.gameObject.SetActive(true);
        mMessageView.Setup(szID);
        mMessageView.ResetScrollbar();
    }

    public void OpenConversation(int conversationIndex)
    {
        mConversationView.CreateAllConversations(conversationIndex);
    }
    #endregion
}
