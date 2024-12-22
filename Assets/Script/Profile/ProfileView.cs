using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class ProfileView : MonoBehaviour
{
    #region SerialzeField
    [SerializeField]
    private Image mImageIcon;
    [SerializeField]
    private TMP_Text mTextName;

    #endregion

    #region private var
    private UserData mUserData;
    #endregion

    #region public Functions
    public void SetImageAndName(Sprite userSprite, string szName)
    {
        mImageIcon.sprite = userSprite;
        mTextName.SetText(szName);
    }

    public void Setup(string szUserID)
    {
        //clear views
        mImageIcon.sprite = null;
        mTextName.SetText("");


        //load from json
        string szResourceName = "UserData/" + szUserID;

        var szJson = Resources.Load<TextAsset>(szResourceName);

        mUserData = JsonUtility.FromJson<UserData>(szJson.text);


        mTextName.SetText(mUserData.FullName);

        StartCoroutine(DownloadAndSetImageToIcon());
    }
    #endregion

    #region UI Interactions
    public void OnButtonClose()
    {
        this.gameObject.SetActive(false);
    }
    #endregion

    #region private functions

    IEnumerator DownloadAndSetImageToIcon()
    {
        string proxiedUrl = "https://api.allorigins.win/raw?url=" + mUserData.IconURL;

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(proxiedUrl))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);

                mImageIcon.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            }
        }
    }
    #endregion
}
