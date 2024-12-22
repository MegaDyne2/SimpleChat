using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System;
using System.Globalization;

public class ConversationDataItem : MonoBehaviour
{

    #region Serialzed Fields
    [SerializeField]
    private Image mImageIcon;
    [SerializeField]
    public TMP_Text mTextName;
    [SerializeField]
    public TMP_Text mTextDate;
    #endregion
    
    #region Private Vars
    private Main mMain;
    private string mConversationID = "";
    private string mUserID = "";
    private UserData mUserData;
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        mMain = GetComponentInParent<Main>();
    }
    #endregion

    #region Functions setters.

    public void SetIcon(Sprite inSprite)
    {
        mImageIcon.sprite = inSprite;
    }
    public void SetItems(UserData userData, string szTime, string conversationID, string userID)
    {
        mUserData = userData;

        DateTime d2;// = DateTime.Parse(szTime, null, System.Globalization.DateTimeStyles.RoundtripKind);

        if (DateTime.TryParse(szTime, out d2))
        {
            //https://docs.microsoft.com/en-us/dotnet/api/system.datetime.tostring?view=net-5.0
            mTextDate.SetText(d2.ToString("f", CultureInfo.CreateSpecificCulture("en-US")));

        }
        else
        {
            Debug.LogErrorFormat("Cannot Parse string for Last time: {0}", szTime);
            mTextDate.SetText("Error");
        }

        mConversationID = conversationID;
        mUserID = userID;
    }

    public void SetName(string inName)
    {
        mTextName.SetText(inName);
    }
    #endregion

    #region UI interactions
    public void OnIconButtonPress()
    {
        if (mUserData == null)
            return;

        mMain.ShowProfileView(mUserID);
    }

    public void OnNameClicked()
    {
        mMain.ShowMessageView(mConversationID);
    }
    #endregion


}
