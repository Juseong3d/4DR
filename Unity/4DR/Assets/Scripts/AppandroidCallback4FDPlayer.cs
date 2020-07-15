using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppandroidCallback4FDPlayer : MonoBehaviour {

    public int videoWidth;
    public int videoHeight;
    public long duration;

    public int prevChannel;
    public int channel;
    public int frame;
    public int frame_cycle;
    public long time;
    public string utc;

    public int max_channel;

    public bool isErrorPopup;
    public bool isZoomMoveR;

    public bool isChangeChannel;

    // Start is called before the first frame update
    void Start()
    {
        duration = 999999;
        isErrorPopup = false;
        isZoomMoveR = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IS_CHANGE_POPUP(UIToggle _value) {

        isErrorPopup = _value.value;
    }


    public void IS_CHANGE_ZOOM_MOVE_R(UIToggle _value) {

        isZoomMoveR = _value.value;

    }


    void CallBackFromFDPlayer(string _value) {

        //Debug.Log("CallBackFromFDPlayer ::: " + _value);
        if(isErrorPopup == true) {
            PopupBox.Create(_value);        
        }

        string[] _tmp = _value.Split(","[0]);

        int code = Convert.ToInt32(_tmp[1]);

        switch(code) {
        case 2300:
        case 2301:
            //setStreamOpenStartTS(time);
            break;

        }
    }


    void getVideoStreamInfo(string _value) {
        
        string[] _tmp = _value.Split(","[0]);

        videoWidth = int.Parse(_tmp[1]);
        videoHeight = int.Parse(_tmp[2]);
        duration = long.Parse(_tmp[3]);

    }


    void getCurrentPlayInfo(string _value) {               

        string[] _tmp = _value.Split(","[0]);
#if !UNITY_EDITOR
        isChangeChannel = (prevChannel != channel);
        prevChannel = channel;
        channel = int.Parse(_tmp[1]);
#endif
        frame = int.Parse(_tmp[2]);
        frame_cycle = int.Parse(_tmp[3]);
        time = long.Parse(_tmp[4]);
        utc = _tmp[5];        
    }
}
