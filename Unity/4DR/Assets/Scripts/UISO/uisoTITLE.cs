using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uisoTITLE : MonoBehaviour
{


    public UILabel labelVersion;
    // Start is called before the first frame update
    void Start()
    {
        
        labelVersion.text = Appmain.appmain.appInfo.appVersion;

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnClickButton4Title() {

        Appmain.appdoc.setGameStatus(GAME_STATUS.GS_MENU);

    }


    public void OnClcikButtonSelectVideoType(GameObject _obj) {

        Appmain.appmain.selectVideoType = (VIDEO_TYPE)int.Parse(_obj.name);
        OnClickButton4Title();

    }
}
