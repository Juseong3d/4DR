﻿using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class uisoITEM_VIDEO_MINI : MonoBehaviour
{
    public UILabel labelInfo;
    public UILabel labelType;

    public LIST_CONTENT_FDLIVE_ITEM __info;

    public UISprite _cursor;

    public UITexture textureMain;

    // Start is called before the first frame update
    void Start()
    {
        _cursor.enabled = false;
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

        if(!string.IsNullOrEmpty(_info.thumbnail.url)) {
            string url = string.Format("{0}{1}", DEFINE.THUMNAIL_IMG_DOWNLOAD_SERVER_URL, _info.thumbnail.url);
            Debug.Log("url :::: " + url);
            GameObject tmpObject = new GameObject("_TMP_DOWNLOAD_");
		    ImageLoader imgLoader = tmpObject.AddComponent<ImageLoader>();
        
		    imgLoader.SET(url, textureMain, false, true);
		    imgLoader.START();
        }
    }


    public void SET_INFO(string _info) {

        string _tmp = string.Format("{0}", _info);

        labelInfo.text = _tmp;
        labelType.text = "LOCAL";

        __info.url = DEFINE.GET_LOCAL_FOLDER_PATH() +  _info;
    }


    public void OnClickButton4FullScreen() {
        
        Appmain.appimg.LOAD_FULL_SCREEN_VIDEO(__info);//.GETURL());

    }


    public void SET_CURSOR(bool _how) {

        _cursor.enabled = _how;

    }

}
