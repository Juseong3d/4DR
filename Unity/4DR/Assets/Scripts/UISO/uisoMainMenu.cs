using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class uisoMainMenu : MonoBehaviour
{
    public UIGrid _gridMain;
    public UILabel _label;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnClickButton4Reflash() {

        switch(Appmain.appmain.selectVideoType) {
            case VIDEO_TYPE.WEB_SERVER_LIST:
                Appmain.appnet.__WEB_CONNECT_AND_SEND_RECV_4_FAST_JSON(string.Empty);        
                break;
            case VIDEO_TYPE.LOCAL_LIST:
                Appmain.appimg._SET_LOCAL_LIST_GRID();
                break;
        }
    }

    public void SET_GRID_EMPTY(string _value) {

        if(string.IsNullOrEmpty(_value)) {
            _label.text = string.Empty;
        }else {
            _label.text = string.Format("EMPTY\nCopy to [{0}]", _value);
        }

    }


    public void OnClcickButton4Back() {

        Appmain.appdoc.setGameStatus(GAME_STATUS.GS_TITLE);

    }
}
