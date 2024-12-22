using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomePage : MonoBehaviour
{
    #region private var
    private Main mMain;
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        mMain = GetComponentInParent<Main>();
    }
    #endregion

    #region UI Interaction
    public void OnButtonClick_LoadConversationView(int nIndex)
    {
        mMain.OpenConversation(nIndex);
        this.gameObject.SetActive(false);
    }
    #endregion
}
