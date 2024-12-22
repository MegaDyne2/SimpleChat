using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleMessage : MonoBehaviour
{
    #region SerialzedField
    [SerializeField]
    public Image mImageUserIcon;
    [SerializeField]
    public TMP_Text mTextMessage;
    [SerializeField]
    public Sprite mDebugSprite1;
    [SerializeField]
    public Sprite mDebugSprite2;
    #endregion

    #region Private var
    HorizontalLayoutGroup mHorizontalLayoutGroup;
    private RectTransform myRectTransform;
    #endregion

    #region Unity Functions
    private void Awake()
    {
        mHorizontalLayoutGroup = this.GetComponent<HorizontalLayoutGroup>();
        myRectTransform = this.GetComponent<RectTransform>();
    }
    #endregion

    #region public functions

    public void Setup(Sprite userSprite, string szMessage, bool isUserIconOnRight = false)
    {
        mImageUserIcon.sprite = userSprite;
        mTextMessage.SetText(szMessage);

        if(isUserIconOnRight == false)
        {
            //Icon on left
            mImageUserIcon.rectTransform.SetAsFirstSibling();
        }
        else
        {
            //Icon on right
            mImageUserIcon.rectTransform.SetAsLastSibling();
        }


        //refresh the ContentSizeFitter ASAP.
        LayoutRebuilder.ForceRebuildLayoutImmediate(myRectTransform);
    }

    public void SetIcon(Sprite inSprite)
    {
        mImageUserIcon.sprite = inSprite;
    }
    #endregion

}
