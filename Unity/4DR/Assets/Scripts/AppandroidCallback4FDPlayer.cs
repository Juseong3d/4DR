using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppandroidCallback4FDPlayer : MonoBehaviour {

    public int videoWidth;
    public int videoHeight;
    public long duration;

    public int channel;
    public int frame;
    public int frame_cycle;
    public long time;
    public string utc;

    // Start is called before the first frame update
    void Start()
    {
        duration = 999999;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void CallBackFromFDPlayer(string _value) {

        //Debug.Log("CallBackFromFDPlayer ::: " + _value);
        //PopupBox.Create(_value);        
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
        channel = int.Parse(_tmp[1]);
#endif
        frame = int.Parse(_tmp[2]);
        frame_cycle = int.Parse(_tmp[3]);
        time = long.Parse(_tmp[4]);
        utc = _tmp[5];        
    }
}
