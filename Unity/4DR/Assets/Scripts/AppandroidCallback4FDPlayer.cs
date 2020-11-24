using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppandroidCallback4FDPlayer : MonoBehaviour {

    public MediaPlayerCtrl _mpc;

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

    public enum FDLIVE_ERROR {

	    STREAM_RECIVING_FAILURE_DICONNECTION = 2300,
	    STREAM_RECIVING_FAILURE_WAITING_TIME_OUT = 2301

    }


    // Start is called before the first frame update
    void Start() {
        duration = 999999;
        isErrorPopup = false;
        isZoomMoveR = true;
    }


    /*
     * Scene GameObject Default Name : _UNITY_ANDROID_
     */
    public void call_SetUnityGameObjectName4NativeMessage(string _reName) {
#if UNITY_ANDROID
		try {
			using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
				using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity")) {
					Debug.Log("CallAndroid : getContactList");
					jo.Call("SetUnityGameObjectName4NativeMessage", new object[] { _reName });
				}
			}
		}
		catch (Exception e)
		{
			Debug.Log(e.StackTrace);
		}
#endif

	}

	
	void CallBackFromFDPlayer(string _value) {

        getError(_value);

    }


	void getError(string _value) {
        
        //final String str = "getError," + code + "," + msg + "," + ls_ip;
        string[] _tmp = _value.Split(","[0]);
        int code = Convert.ToInt32(_tmp[1]);

        switch((FDLIVE_ERROR)code) {
        case FDLIVE_ERROR.STREAM_RECIVING_FAILURE_DICONNECTION:
        case FDLIVE_ERROR.STREAM_RECIVING_FAILURE_WAITING_TIME_OUT:
            //_mpc.setStreamOpenStartTS((int)time);
            //_mpc.Load(_mpc.m_strFileName);
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

        Debug.Log("### :: " + _value);
    }
}
