using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLoadMoreMessages : MonoBehaviour
{
    #region Private Var
    private MessagesView mMessageView;
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        mMessageView = GetComponentInParent<MessagesView>();
    }
    #endregion

    #region UI Interactions
    public void OnClick_LoadMoreMessages()
    {
        mMessageView.LoadMoreMessages();
    }
    #endregion
}
