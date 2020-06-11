using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uisoITEM_VIDEO_MINI : MonoBehaviour
{
    public UILabel labelInfo;
    public UILabel labelType;

    public LIST_CONTENT_FDLIVE_ITEM __info;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SET_INFO(LIST_CONTENT_FDLIVE_ITEM _info) {

        string _tmp = string.Format("{0}", _info.title);

        __info = _info;

        labelInfo.text = _tmp;
        labelType.text = _info.type.ToUpper();

        if(labelType.text.Equals("LIVE") == true) {
            labelType.color = Color.red;
        }

    }


    public void OnClickButton4FullScreen() {
        
        Appmain.appimg.LOAD_FULL_SCREEN_VIDEO(__info.GETURL());

    }

}
