using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppCommandCtlCamera : MonoBehaviour
{
    public const string PATH_EFFECT_TEXT = "Common/_Default_Effect/TAE/pfb_effect_text";
    public Camera _mainCamera;
    public isoCameraZoom _cameraCtl;
    public isoShakeCamera _cameraShake;
    public MediaPlayerCtrl _mediaMain;
    public AppandroidCallback4FDPlayer _videoInfo;

    public isoFdPlayerCtl _fdPlayerCTLUI;

    public List<Q_COMMAND_CTL_CAMERA> _commandes;

    public float _baseSpeed;

    //for testing..
    public float _frameTime;

    public int _lastCallChangeChannelFrame;
    public int _lastCallChangeTimeMove;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Appmain.appui.mainCamera3D;
        _cameraCtl = _mainCamera.GetComponent<isoCameraZoom>();
        _cameraShake = _mainCamera.GetComponent<isoShakeCamera>();
        _mediaMain = this.gameObject.GetComponentInChildren<MediaPlayerCtrl>();
        _fdPlayerCTLUI = this.gameObject.transform.parent.GetComponentInChildren<isoFdPlayerCtl>();
        
        _videoInfo = FindObjectOfType<AppandroidCallback4FDPlayer>();        

        _commandes.Clear();
        _commandes = new List<Q_COMMAND_CTL_CAMERA>();

        //_commandes.Add(new Q_COMMAND_CTL_CAMERA("test") { _frame = 9999 });

        //Common/_Default_Table/t_unity_cv_ctl_test_MBC_TEST03

        foreach(uisoITEM_CameraScript _script in Appmain.appmain._selectCameraScript) {
            string path = string.Format("{0}/{1}", UIDEFINE.PATH_CAMERA_SCRIPT_ALL, _script._label.text.Trim());

            Debug.Log("path ::: " + path);
            LOAD_COMMANDS_4_TABLE(path);
        }
        //LOAD_COMMANDS_4_TABLE("Common/_Default_Table/t_unity_cv_ctl_test_MBC_TEST03");

        _frameTime = 0f;
        _baseSpeed = 2f;
        _lastCallChangeChannelFrame = 0;
        _lastCallChangeTimeMove = 0;

        _mediaMain.OnEnd += OnEndSoInitCommandStatus;
    }

    private void OnEndSoInitCommandStatus() {

        _commandes.Clear();

        foreach(uisoITEM_CameraScript _script in Appmain.appmain._selectCameraScript) {
            string path = string.Format("{0}/{1}", UIDEFINE.PATH_CAMERA_SCRIPT_ALL, _script._label.text.Trim());

            Debug.Log("path ::: " + path);
            LOAD_COMMANDS_4_TABLE(path);
        }
        Debug.Log("OnEndSoInitCommandStatus()");
    }


    public void LOAD_COMMANDS_4_TABLE(string path) {

		int totalCnt = 0;
		int i = 0;
		string[] allData = null;

		allData = CSVReader.ReadFile(path, false);

		if (allData == null) {
			Debug.Log(path + " :: allData is null");
			return;
		}

		string[] tmp = allData[0].Split(","[0]);
		totalCnt = Convert.ToInt32(tmp[0]);		

		for (i = 0; i < totalCnt; i++) {
            _commandes.Add(new Q_COMMAND_CTL_CAMERA(allData[i + 1]));
		}

        int a = 0;
    }


    public string GET_COMMANDS_STATUS() {
        
        string _tmp = string.Empty;

        foreach(Q_COMMAND_CTL_CAMERA _cmd in _commandes) {
            if(_cmd.status == COMMAND_STATUS.ING) {
                _tmp += string.Format("[ffff64]{0}/{1}/{2}[-]\n", _cmd.cmd, _cmd.status, _cmd._ori_command);
            }else if(_cmd.status == COMMAND_STATUS.DONE) {
                _tmp += string.Format("[646464]{0}/{1}/{2}[-]\n", _cmd.cmd, _cmd.status, _cmd._ori_command);
            }else {
                _tmp += string.Format("{0}/{1}/{2}\n", _cmd.cmd, _cmd.status, _cmd._ori_command);
            }
        }

        return _tmp;

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        processingCommandCTLCamera();
    }


    public void processingCommandCTLCamera() {

        if(_fdPlayerCTLUI != null) {
            _fdPlayerCTLUI.labelCommandList.text = GET_COMMANDS_STATUS();
        }

        if(_mediaMain == null) return;
        if(_mainCamera == null) return;
        if(_cameraCtl == null) return;
        if(_videoInfo == null) return;        

        foreach(Q_COMMAND_CTL_CAMERA _cmd in _commandes) {
            
        #if UNITY_EDITOR
            
            //Debug.Log(_cmd._frame + " : ////// : " + _frameTime * 1000f);

            if(_cmd._frame > _frameTime * 1000f) continue;
        #else
            if(_cmd._frame > _videoInfo.time) {
                _cmd.status = COMMAND_STATUS.WAIT;
                continue;
            }
        #endif
            
            if(_cmd.status == COMMAND_STATUS.DONE || _cmd.status == COMMAND_STATUS.NONE) {
                continue;
            }

            _cmd.ING();

            switch(_cmd.cmd) {
            case COMMAND_CTL_CAMERA.DEFAULT :
                {
                //카메라의 orthsize leap하기
                //_mainCamera.orthographicSize = Mathf.Lerp(_mainCamera.orthographicSize, 1f, Time.deltaTime * _baseSpeed);

                    if (_mediaMain.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED) {
                        _mediaMain.Play();
                    }

                //if(Mathf.Abs(_mainCamera.orthographicSize - 1f) < 0.001f) {

                //    _mainCamera.orthographicSize = 1f;
                //    _cmd.Clear();

                //}
                    _mainCamera.orthographicSize = 1f;
                    _cmd.Clear();

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
                    {
                        _cmd._pause_time -= Time.deltaTime;                       

                        if(_cmd._pause_time <= 0f) {
                            _mediaMain.Play();
                            _cmd._pause_time = 0f;
                            _cmd.Clear();
                        }else {
                            if(_mediaMain.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED) {
                                _mediaMain.Pause();
                            }
                        }
                    }
                }
                break;
            case COMMAND_CTL_CAMERA.ZOOM :
                {
                    _mainCamera.orthographicSize = Mathf.Lerp(_mainCamera.orthographicSize, _cmd._zoom, Time.deltaTime * _baseSpeed);

                    if(_mainCamera.orthographicSize - _cmd._zoom < 0.001f) {
                        _cmd.Clear();
                    }
                }
                break;
            case COMMAND_CTL_CAMERA.LOOKAT :
                {

                    Vector3 _position = _mainCamera.ScreenToWorldPoint(new Vector3(_cmd._x, _cmd._y));
                    Vector3 _tmpTarget = new Vector3(
                            Mathf.Lerp(_mainCamera.transform.localPosition.x, _position.x, Time.deltaTime),
                            Mathf.Lerp(_mainCamera.transform.localPosition.y, _position.y, Time.deltaTime),
                            Mathf.Lerp(_mainCamera.transform.localPosition.z, 0f, Time.deltaTime)
                        );
                    Vector3 ___ddddd = _mainCamera.transform.localPosition - _position;
                    Debug.Log("___ddddd :: " + ___ddddd.x + "/" + ___ddddd.y + "//" + _mainCamera.transform.localPosition + "//" + _tmpTarget);
                    
                    if (Mathf.Abs(___ddddd.x) <= 0.2f && Mathf.Abs(___ddddd.y) <= 0.2f /*&& _mainCamera.orthographicSize <= _cmd._zoom*/) {                            
                            _cmd.Clear();
                    }else {
                    
                        _mainCamera.transform.localPosition = _tmpTarget;
                        //_mainCamera.orthographicSize = Mathf.Lerp(_mainCamera.orthographicSize, _cmd._zoom, Time.deltaTime * _baseSpeed);

                        //if(Mathf.Abs(_mainCamera.orthographicSize - _cmd._zoom) <= 0.001f) {
                        //    _mainCamera.orthographicSize = _cmd._zoom;
                        //}

                    }
                    
                }
                break;
            case COMMAND_CTL_CAMERA.CHANNEL :

                 {
                    bool isCheckType = true;

                    if(Appmain.appmain.selectVideoType == VIDEO_TYPE.WEB_SERVER_LIST) {
                        isCheckType = (_lastCallChangeChannelFrame != _videoInfo.frame);
                    }else if(Appmain.appmain.selectVideoType == VIDEO_TYPE.LOCAL_LIST) {
                        if(_cmd.statusCnt % 3 != 0) break;
                    }
                        
                    if(isCheckType) {
                        //time은 pause만 되는거라함 그래서 
                        bool isPlaying = (_mediaMain.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING);

                        //for(int i = 0; i<Mathf.Abs(_cmd._channel_index); i++) 
                        //{
                        //    if (_cmd._channel_index < 0) {
                        //        _mediaMain.Left(!isPlaying);                            
                        //    }else if (_cmd._channel_index > 0) {
                        //        _mediaMain.Right(!isPlaying);                         
                        //    }
                        //}

                        //_cmd.Clear();

                        if(_cmd._channel_index == 0) {
                            _cmd.Clear();
                        }else if(_cmd._channel_index < 0) {
                            _mediaMain.Left(!isPlaying);
                            _lastCallChangeChannelFrame = _videoInfo.frame;
                            _cmd._channel_index ++;
                        }else if(_cmd._channel_index > 0) {
                            _mediaMain.Right(!isPlaying);
                            _lastCallChangeChannelFrame = _videoInfo.frame;
                            _cmd._channel_index --;
                        }
                    }
                }
                break;
            case COMMAND_CTL_CAMERA.EFFECT :
                {
                    GameObject _prefab = Appimg.LoadResource4Prefab(_cmd._effect_info.GET_PATH());
                    Vector3 _position = _mainCamera.ScreenToWorldPoint(new Vector3(_cmd._x, _cmd._y));

                    _prefab.transform.localPosition = _position;
                    _prefab.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

                    _prefab.transform.SetParent(Appmain.appui._EFFECT_MAIN.transform);

                    _cmd.Clear();
                }
                break;
            case COMMAND_CTL_CAMERA.TEXT :
                {
                    GameObject _prefab = Appimg.LoadResource4Prefab4UI(PATH_EFFECT_TEXT);
                    //Vector3 _position = _mainCamera.ScreenToWorldPoint(new Vector3(_cmd._x, _cmd._y));
                    Vector3 _position = new Vector3(_cmd._x, _cmd._y);

                    _prefab.transform.localPosition = _position;
                    _prefab.transform.SetParent(Appmain.appui._EFFECT_MAIN.transform);
                    _prefab.transform.localScale = new Vector3(1, 1, 1);

                    uisoEffectText _text = _prefab.GetComponent<uisoEffectText>();
                    isoDestoryTime _desttime = _prefab.GetComponent<isoDestoryTime>();

                    _text.SET_LABEL(_cmd._str);
                    _text.SET_SPRITE_COLOR(_cmd._backcolor);
                    _desttime.SET_DESTROY_TIMER(_cmd._life_time);

                    _cmd.Clear();

                }
                break;
            case COMMAND_CTL_CAMERA.CHANNEL_TO :
                if(_lastCallChangeChannelFrame != _videoInfo.frame) {
                    bool isPlaying = (_mediaMain.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING);
                    int whereto = _cmd._channel_index - _videoInfo.channel;

                    Debug.Log("whereto : " + whereto + "/" + _cmd._channel_index + "/vc: + " + _videoInfo.channel);

                    if(whereto == 0) {
                        _cmd.Clear();
                    }else if(whereto > 0) {                        
                        _mediaMain.Right(!isPlaying);
                        _lastCallChangeChannelFrame = _videoInfo.frame;
                    }else if(whereto < 0) {
                        _mediaMain.Left(!isPlaying);
                        _lastCallChangeChannelFrame = _videoInfo.frame;
                    }
                }
                break;
            case COMMAND_CTL_CAMERA.CAMERA_SHAKE:
                {                    
                    _cameraShake.StartUpDownShake(_cmd._cameraShake_ud);
                    _cameraShake.StartLeftRightShake(_cmd._cameraShake_lr);
                    _cmd.Clear();
                }
                break;
            case COMMAND_CTL_CAMERA.TIME_REWIND:
                if(_lastCallChangeTimeMove != _videoInfo.frame) {

                    if(_mediaMain.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING) {
                        _mediaMain.Pause();
                    }else {                    

                        if(_cmd._time_rewind == 0) {
                            _cmd.Clear();
                        }else if(_cmd._time_rewind > 0) {
                            _mediaMain.Left(true);
                            _lastCallChangeTimeMove = _videoInfo.frame;
                            _cmd._time_rewind --;
                        }
                    }
                }  
                break;
            case COMMAND_CTL_CAMERA.TIME_FORWARD:
                if(_lastCallChangeTimeMove != _videoInfo.frame) {

                    if(_mediaMain.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING) {
                        _mediaMain.Pause();
                    }else {                    

                        if(_cmd._time_forward == 0) {
                            _cmd.Clear();
                        }else if(_cmd._time_forward > 0) {
                            _mediaMain.Right(true);
                            _lastCallChangeTimeMove = _videoInfo.frame;
                            _cmd._time_forward --;
                        }
                    }
                }  
                break;
            case COMMAND_CTL_CAMERA.SPEED:
                {
                    _mediaMain.Speed(_cmd._speed);
                    _cmd.Clear();
                }
                break;
            
            case COMMAND_CTL_CAMERA.NONE :
                //Debug.Log("remove index == " + _cmd.index);
                //_commandes.RemoveAt(_cmd.index);
                break;

            }

            _cmd.statusCnt ++;
        }

        _frameTime += Time.deltaTime;        

    }
}


//vod url이나 local file name 등을 기준으로
//resources 폴더에 파일 존재를 체크 후
//파싱해서 해당 video 플레이시 아래 자동 전개 진행

[Serializable]
public class Q_COMMAND_CTL_CAMERA {

    public string _ori_command;

    public int index;

    public COMMAND_STATUS status;
    public COMMAND_CTL_CAMERA cmd;

    public double _frame { get; set; }

    public float _x;
    public float _y;

    public float _pause_time;             //if.... pause sec have

    public float _zoom;

    public int _prevChannel;
    public int _channel_index;

    public int _effect_index;       //effect list index
    public DEFAULT_EFFECT_LIST _effect_info;
    public string _str;             //BBCODE 
    public string _backcolor;       //RGBA //ff ff ff ff
    public float _life_time;

    //camera shake
    public float _cameraShake_ud;
    public float _cameraShake_lr;
    public float _camera_amp;
    public float _camera_freq;

    public int _time_rewind;
    public int _time_forward;

    public float _speed;

    public int statusCnt;

    //table parsing용
    public Q_COMMAND_CTL_CAMERA(string _ori) {

        string[] _tmp = _ori.Split(","[0]);
        int i = 0;

        _ori_command = _ori;

        index = Convert.ToInt32(_tmp[i++]);
        _frame = Convert.ToDouble(_tmp[i++]);

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
            _pause_time = Convert.ToSingle(_tmp[i]) / 1000f;
            break;
        case COMMAND_CTL_CAMERA.ZOOM :
            _zoom = Convert.ToSingle(_tmp[i]);
            break;
        case COMMAND_CTL_CAMERA.LOOKAT :
            _x = Convert.ToInt32(_tmp[i++]);
            _y = Convert.ToInt32(_tmp[i++]);
            _zoom = Convert.ToSingle(_tmp[i++]);
            break;
        case COMMAND_CTL_CAMERA.CHANNEL :
            _channel_index = Convert.ToInt32(_tmp[i++]);
            break;
        case COMMAND_CTL_CAMERA.EFFECT :
            _x = Convert.ToInt32(_tmp[i++]);
            _y = Convert.ToInt32(_tmp[i++]);
            _effect_index = Convert.ToInt32(_tmp[i++]);
            _effect_info = Appmain.appmain.GET_DEFAULT_EFFECT(_effect_index);
            break;
        case COMMAND_CTL_CAMERA.TEXT :
            _x = Convert.ToInt32(_tmp[i++]);
            _y = Convert.ToInt32(_tmp[i++]);
            _str = _tmp[i++];
            _backcolor = _tmp[i++];
            _life_time = Convert.ToSingle(_tmp[i++]) / 1000f;
            break;
        case COMMAND_CTL_CAMERA.CHANNEL_TO:
            _channel_index = Convert.ToInt32(_tmp[i++]);
            break;
        case COMMAND_CTL_CAMERA.CAMERA_SHAKE:
            _cameraShake_ud = Convert.ToSingle(_tmp[i++]);
            _cameraShake_lr = Convert.ToSingle(_tmp[i++]);
            _camera_amp = Convert.ToSingle(_tmp[i++]);
            _camera_freq = Convert.ToSingle(_tmp[i++]);
            break;
        case COMMAND_CTL_CAMERA.TIME_REWIND:
            _time_rewind = Convert.ToInt32(_tmp[i++]);
            break;
        case COMMAND_CTL_CAMERA.TIME_FORWARD:
            _time_forward = Convert.ToInt32(_tmp[i++]);
            break;
        case COMMAND_CTL_CAMERA.SPEED:
            _speed = Convert.ToSingle(_tmp[i++]);
            break;
        }

        status = COMMAND_STATUS.WAIT;
    }


    public void ING() {

        status = COMMAND_STATUS.ING;

    }

    public void Clear() {

        status = COMMAND_STATUS.DONE;
        //cmd = COMMAND_CTL_CAMERA.NONE;

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
    CHANNEL_TO = 8,
    CAMERA_SHAKE,    
    TIME_REWIND,
    TIME_FORWARD,
    SPEED
}


public enum COMMAND_STATUS {

    NONE = -1,
    WAIT,
    ING,
    DONE,

}