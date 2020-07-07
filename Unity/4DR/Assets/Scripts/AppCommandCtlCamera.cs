using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppCommandCtlCamera : MonoBehaviour
{
    public Camera _mainCamera;
    public isoCameraZoom _cameraCtl;
    public MediaPlayerCtrl _mediaMain;
    public AppandroidCallback4FDPlayer _videoInfo;
    public List<Q_COMMAND_CTL_CAMERA> _commandes;

    // Start is called before the first frame update
    void Start()
    {
        //_commandes.Add(new Q_COMMAND_CTL_CAMERA("test") { _frame = 9999 });
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        processingCommandCTLCamera();
    }


    public void processingCommandCTLCamera() {

        if(_mediaMain == null) return;
        if(_mainCamera == null) return;
        if(_cameraCtl == null) return;
        if(_videoInfo == null) return;

        foreach(Q_COMMAND_CTL_CAMERA _cmd in _commandes) {
            
            if(_cmd._frame < _videoInfo.frame) continue;

            switch(_cmd.cmd) {
            case COMMAND_CTL_CAMERA.DEFAULT :
                {
                    //카메라의 orthsize leap하기
                    _mainCamera.orthographicSize = Mathf.Lerp(_mainCamera.orthographicSize, 1f, Time.deltaTime * 5f);

                    if(_mediaMain.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED) {
                        _mediaMain.Play();
                    }

                    if(_mainCamera.orthographicSize - 1f < 0.001f) {
                        _cmd.Clear();
                    }
                }
                break;
            case COMMAND_CTL_CAMERA.PLAY :
                {
                    _mediaMain.Play();
                    _cmd.Clear();
                }
                break;
            case COMMAND_CTL_CAMERA.PAUSE :
                if(_cmd._pause_time == -1) {
                    _mediaMain.Pause();
                    _cmd.Clear();
                }else {
                    if(_mediaMain.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED) {
                        _mediaMain.Pause();
                    }else {
                        _cmd._pause_time -= Time.deltaTime;

                        if(_cmd._pause_time <= 0f) {
                            _mediaMain.Play();
                            _cmd._pause_time = 0f;
                            _cmd.Clear();
                        }
                    }
                }
                break;
            case COMMAND_CTL_CAMERA.ZOOM :
                {
                    _mainCamera.orthographicSize = Mathf.Lerp(_mainCamera.orthographicSize, _cmd._zoom, Time.deltaTime * 5f);

                    if(_mainCamera.orthographicSize - _cmd._zoom < 0.001f) {
                        _cmd.Clear();
                    }
                }
                break;
            case COMMAND_CTL_CAMERA.LOOKAT :
                {
                    if ((_cmd._x != 0f) || (_cmd._y != 0f) || Appmain.appui.mainCamera3D.orthographicSize > 0.5f) {
                        Vector3 _tmpTarget = new Vector3(
                                Mathf.Lerp(Appmain.appui.mainCamera3D.transform.position.x, _cmd._x, Time.deltaTime * 5f),
                                Mathf.Lerp(Appmain.appui.mainCamera3D.transform.position.y, _cmd._y, Time.deltaTime * 5f),
                                Mathf.Lerp(Appmain.appui.mainCamera3D.transform.position.z, 0f, Time.deltaTime * 5f)
                            );

                        _mainCamera.transform.position = _tmpTarget;
                        _mainCamera.orthographicSize = Mathf.Lerp(_mainCamera.orthographicSize, 0.5f, Time.deltaTime * 5f);

                        Vector3 ___ddddd = _mainCamera.transform.position - _tmpTarget;

                        if (Mathf.Abs(___ddddd.x) < 0.01f && Mathf.Abs(___ddddd.y) < 0.01f && _mainCamera.orthographicSize <= 0.5f) {                            
                            _cmd.Clear();
                        }
                    }
                }
                break;
            case COMMAND_CTL_CAMERA.CHANNEL :
                {
                    //time은 pause만 되는거라함 그래서 
                    bool isPlaying = (_mediaMain.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING);

                    if(_cmd._channel_index < 0) {
                    
                        _mediaMain.Left(!isPlaying);
                    }else if(_cmd._channel_index > 0) {
                        _mediaMain.Right(!isPlaying);
                    }else {
                        _cmd.Clear();
                    }
                }                
                break;
            case COMMAND_CTL_CAMERA.EFFECT :

                break;
            case COMMAND_CTL_CAMERA.TEXT :

                break;
            case COMMAND_CTL_CAMERA.CHANNEL_TO :

                break;
            case COMMAND_CTL_CAMERA.NONE :
                Debug.Log("remove index == " + _cmd.index);
                _commandes.Remove(_cmd);
                break;

            }
        }

    }
}


//vod url이나 local file name 등을 기준으로
//resources 폴더에 파일 존재를 체크 후
//파싱해서 해당 video 플레이시 아래 자동 전개 진행

public class Q_COMMAND_CTL_CAMERA {

    public string _ori_command;

    public int index;

    public COMMAND_CTL_CAMERA cmd;

    public long _frame { get; set; }

    public float _x;
    public float _y;

    public float _pause_time;             //if.... pause sec have

    public float _zoom;

    public int _channel_index;

    public int _effect_index;       //effect list index
    public string _str;             //BBCODE 
    public string _backcolor;       //RGBA //ff ff ff ff

    //table parsing용
    public Q_COMMAND_CTL_CAMERA(string _ori) {

        string[] _tmp = _ori.Split(","[0]);
        int i = 0;

        _ori_command = _ori;

        index = Convert.ToInt32(_tmp[i++]);
        _frame = Convert.ToInt64(_tmp[i++]);

        //COMMAND_CTL_CAMERA out _tmpCmd;
        bool _result = Enum.TryParse<COMMAND_CTL_CAMERA>(_tmp[i++], out cmd);

        if(_result == false) {
            Debug.Log("### Q_COMMAND_CTL_CAMERA Check command");
            return;
        }

        switch(cmd) {
         case COMMAND_CTL_CAMERA.DEFAULT :

            break;
        case COMMAND_CTL_CAMERA.PLAY :

            break;
        case COMMAND_CTL_CAMERA.PAUSE :
            _pause_time = Convert.ToInt32(_tmp[i]);
            break;
        case COMMAND_CTL_CAMERA.ZOOM :
            _zoom = Convert.ToInt32(_tmp[i]);
            break;
        case COMMAND_CTL_CAMERA.LOOKAT :
            _x = Convert.ToInt32(_tmp[i++]);
            _y = Convert.ToInt32(_tmp[i++]);
            break;
        case COMMAND_CTL_CAMERA.CHANNEL :
            _channel_index = Convert.ToInt32(_tmp[i++]);
            break;
        case COMMAND_CTL_CAMERA.EFFECT :
            _x = Convert.ToInt32(_tmp[i++]);
            _y = Convert.ToInt32(_tmp[i++]);
            _effect_index= Convert.ToInt32(_tmp[i++]);
            break;
        case COMMAND_CTL_CAMERA.TEXT :
            _x = Convert.ToInt32(_tmp[i++]);
            _y = Convert.ToInt32(_tmp[i++]);
            _str = _tmp[i++];
            _backcolor = _tmp[i++];
            break;
        }
    }

    public void Clear() {

        cmd = COMMAND_CTL_CAMERA.NONE;

    }
}


public enum COMMAND_CTL_CAMERA {

    NONE = -1,
    DEFAULT = 0,
    PLAY = 1,
    PAUSE = 2,
    ZOOM = 3,
    LOOKAT = 4,
    CHANNEL = 5,    
    EFFECT = 6,
    TEXT = 7,
    CHANNEL_TO = 8
}