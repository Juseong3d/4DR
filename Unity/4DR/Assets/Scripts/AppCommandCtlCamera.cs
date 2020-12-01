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
    public MediaPlayerCtrl _mpc;
    public AppandroidCallback4FDPlayer _videoInfo;

    public isoFdPlayerCtl _fdPlayerCTLUI;

    public List<Q_COMMAND_CTL_CAMERA> _commandes;

    public float _baseSpeed;

    //for testing..
    public float _frameTime;

    public int _lastCallChangeChannelFrame;
    public int _lastCallChangeTimeMove;

    public bool isChangeChannel;


    [Header("* TAE PROJECT --------------")]
    public GameObject gameObjectPlayerInfoVs;
    public GameObject gameObjectTAEScore;

    public float commanderReflashTime;


    private float fixedDeltaTime;

    public Q_COMMAND_CTL_CAMERA _lastLookAt;
    public Q_COMMAND_CTL_CAMERA _lastSetLookAt;

    public float slowSpeed;

    private void Awake() {
        
		this.fixedDeltaTime = Time.fixedDeltaTime;

    }


    // Start is called before the first frame update
    void Start()
    {
        if(Appmain.appui != null)
            _mainCamera = Appmain.appui.mainCamera3D;
        _cameraCtl = _mainCamera.GetComponent<isoCameraZoom>();
        _cameraShake = _mainCamera.GetComponent<isoShakeCamera>();
        _mpc = this.gameObject.GetComponentInChildren<MediaPlayerCtrl>();
        _fdPlayerCTLUI = this.gameObject.transform.parent.GetComponentInChildren<isoFdPlayerCtl>();


        _cameraCtl._mediaMain = this._mpc;
        _cameraCtl._fdPlayerCTLUI = this._fdPlayerCTLUI;
        
        _videoInfo = FindObjectOfType<AppandroidCallback4FDPlayer>();        

        initEndSoInitCommandStatus();

        _frameTime = 0f;
        _baseSpeed = 2f;
        _lastCallChangeChannelFrame = 0;
        _lastCallChangeTimeMove = 0;

        _videoInfo.time = 0;

        slowSpeed = 1f;

        commanderReflashTime = DEFINE.COMMAND_REFLASH_TIME;

        _mpc.OnEnd += initEndSoInitCommandStatus;
    }

    ~AppCommandCtlCamera() {
        NGUITools.Destroy(gameObjectTAEScore);
        NGUITools.Destroy(gameObjectPlayerInfoVs);
        _commandes.Clear();
    }

    private void initEndSoInitCommandStatus() {

        _commandes.Clear();
        _commandes = new List<Q_COMMAND_CTL_CAMERA>();

        foreach(uisoITEM_CameraScript _script in Appmain.appmain._selectCameraScript) {
            //string path = string.Format("{0}/{1}", UIDEFINE.PATH_CAMERA_SCRIPT_ALL, _script._label.text.Trim());

            //Debug.Log("path ::: " + path);
            //LOAD_COMMANDS_4_TABLE(path);

            LOAD_COMMANDS_4_TABLE(_script._info);
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

    }


    public void LOAD_COMMANDS_4_TABLE(LIST_SCRIPT_LIST_ITEM _value) {

		int totalCnt = 0;
		int i = 0;
		string[] allData = null;

		allData = CSVReader.ReadFileFromString(_value.cs_commands_data, false);

		if (allData == null) {
			Debug.Log(_value + " :: allData is null");
			return;
		}

		string[] tmp = allData[0].Split(","[0]);
		totalCnt = Convert.ToInt32(tmp[0]);		

		for (i = 0; i < totalCnt; i++) {
            _commandes.Add(new Q_COMMAND_CTL_CAMERA(allData[i + 1]));
		}
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

#if _COMMANDER_
        float tmpDeltaTime = ((1.0f - Time.timeScale) + 1.0f) * Time.deltaTime;
        commanderReflashTime -= tmpDeltaTime;

        if(commanderReflashTime <= 0f) {

            Appmain.appnet.__WEB_CONNECT_AND_SEND_RECV_4_FAST_JSON(NET_WEB_API_CMD.commander);
            commanderReflashTime = DEFINE.COMMAND_REFLASH_TIME;

        }
#endif
    }


    public void processingCommandCTLCamera() {

        if(_commandes == null) return;

        if(_fdPlayerCTLUI != null) {
            _fdPlayerCTLUI.labelCommandList.text = GET_COMMANDS_STATUS();
        }

        if(_mpc == null) return;
        if(_mainCamera == null) return;
        if(_cameraCtl == null) return;
        if(_videoInfo == null) return;        
        

        foreach(Q_COMMAND_CTL_CAMERA _cmd in _commandes) {
            

            if(_cmd._frame != 0) {
#if UNITY_EDITOR
            
                //Debug.Log(_cmd._frame + " : ////// : " + _frameTime * 1000f);
            
                if(_cmd._frame > _frameTime * 1000f) continue;
#else
                if(_cmd._frame > _videoInfo.time) {
                    _cmd.status = COMMAND_STATUS.WAIT;
                    continue;
                }
#endif
            }

            if(_cmd.status == COMMAND_STATUS.DONE || _cmd.status == COMMAND_STATUS.NONE) {
                continue;
            }

            _cmd.ING();

            switch(_cmd.cmd) {
            case COMMAND_CTL_CAMERA.DEFAULT :
                {
                //카메라의 orthsize leap하기
                //_mainCamera.orthographicSize = Mathf.Lerp(_mainCamera.orthographicSize, 1f, Time.deltaTime * _baseSpeed);

                    if (_mpc.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED) {
                        _mpc.Play();
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
                    _mpc.Play();
                    _cmd.Clear();
                }
                break;
            case COMMAND_CTL_CAMERA.PAUSE :
                if(_cmd._pause_time == 0) {
                    _mpc.Pause();
                    _cmd.Clear();
                }else {                    
                    {
                        _cmd._pause_time -= Time.deltaTime;                       

                        if(_cmd._pause_time <= 0f) {
                            _mpc.Play();
                            _cmd._pause_time = 0f;
                            _cmd.Clear();
                        }else {
                            if(_mpc.GetCurrentState() != MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED) {
                                _mpc.Pause();
                            }
                        }
                    }
                }
                break;
            case COMMAND_CTL_CAMERA.ZOOM :
                {
                    _mainCamera.orthographicSize = Mathf.Lerp(_mainCamera.orthographicSize, _cmd._zoom, Time.deltaTime * _baseSpeed);                                        

                    if(Mathf.Abs(_mainCamera.orthographicSize - _cmd._zoom) < 0.01f) {
                        _mainCamera.orthographicSize = _cmd._zoom;
                        _cmd.Clear();
                    }
                }
                break;
            case COMMAND_CTL_CAMERA.SET_LOOKAT:
                {
                    Vector3 _position = new Vector3();
                    _mainCamera.orthographicSize = Mathf.Lerp(_mainCamera.orthographicSize, 0.7f, Time.deltaTime * _baseSpeed);                                        
                    

                    if(_cmd.setScoreWho == WHAT_TEAM_COLOR.BLUE) {
                        int[] _tr = Appmain.appimg.imgObjects[(int)EXTRA_OBJECT_TYPE.blue].GET_RECT();
                        
                        if(_tr[0] < 0) {
                            _tr = Appmain.appimg.imgObjects[(int)EXTRA_OBJECT_TYPE.hug].GET_RECT();
                        }
                        
                        int x1 = _tr[0] + (_tr[2] >> 1);
                        int y1 = (int)DEFINE.BASE_SCREEN_HEIGHT - (_tr[1] + (_tr[3] >> 1));

                        _position = Appmain.appui.mainCamera3D.ScreenToWorldPoint(new Vector3(x1, y1));

                    }else if(_cmd.setScoreWho == WHAT_TEAM_COLOR.RED) {
                        
                        int[] _tr = Appmain.appimg.imgObjects[(int)EXTRA_OBJECT_TYPE.red].GET_RECT();

                        if(_tr[0] < 0) {
                            _tr = Appmain.appimg.imgObjects[(int)EXTRA_OBJECT_TYPE.hug].GET_RECT();
                        }
                        
                        int x1 = _tr[0] + (_tr[2] >> 1);
                        int y1 = (int)DEFINE.BASE_SCREEN_HEIGHT - (_tr[1] + (_tr[3] >> 1));

                        _position = Appmain.appui.mainCamera3D.ScreenToWorldPoint(new Vector3(x1, y1));

                    }else if(_cmd.setScoreWho == WHAT_TEAM_COLOR.REFREEE) {
                        int[] _tr = Appmain.appimg.imgObjects[(int)EXTRA_OBJECT_TYPE.refree].GET_RECT();

                        int x1 = _tr[0] + (_tr[2] >> 1);
                        int y1 = (int)DEFINE.BASE_SCREEN_HEIGHT - (_tr[1] + (_tr[3] >> 1));

                        _position = Appmain.appui.mainCamera3D.ScreenToWorldPoint(new Vector3(x1, y1));
                    }

                    _position -= new Vector3(0.2f, 0.1f, 0f);
                    _position = _position * (1.0f - Appmain.appui.mainCamera3D.orthographicSize);                

                    float _speed = Time.deltaTime * 3;
                    Vector3 _tmpTarget = new Vector3(
                            Mathf.Lerp(Appmain.appui.mainCamera3D.transform.localPosition.x, _position.x, _speed),
                            Mathf.Lerp(Appmain.appui.mainCamera3D.transform.localPosition.y, _position.y, _speed),
                            Mathf.Lerp(Appmain.appui.mainCamera3D.transform.localPosition.z, 0f, _speed)
                        );
                    Vector3 ___ddddd = Appmain.appui.mainCamera3D.transform.localPosition - _position;                

                    Appmain.appui.mainCamera3D.transform.localPosition = _tmpTarget;

                    //if (Mathf.Abs(___ddddd.x) <= 0.09f && Mathf.Abs(___ddddd.y) <= 0.09f /*&& _mainCamera.orthographicSize <= _cmd._zoom*/) {
                    //    //Appmain.appui.mainCamera3D.transform.localPosition = _position;
                    //    //_cmd.Clear();
                    //}else {
                        
                    //}

                }
                break;
            case COMMAND_CTL_CAMERA.LOOKAT :
                {
                    Vector3 _position = Appmain.appui.mainCamera3D.ScreenToWorldPoint(new Vector3(_cmd._x, _cmd._y));
                    _position -= new Vector3(0.2f, 0.1f, 0f);
                    _position = _position * (1.0f - Appmain.appui.mainCamera3D.orthographicSize);                

                    Vector3 _tmpTarget = new Vector3(
                            Mathf.Lerp(Appmain.appui.mainCamera3D.transform.localPosition.x, _position.x, Time.deltaTime),
                            Mathf.Lerp(Appmain.appui.mainCamera3D.transform.localPosition.y, _position.y, Time.deltaTime),
                            Mathf.Lerp(Appmain.appui.mainCamera3D.transform.localPosition.z, 0f, Time.deltaTime)
                        );
                    Vector3 ___ddddd = Appmain.appui.mainCamera3D.transform.localPosition - _position;                

                    if (Mathf.Abs(___ddddd.x) <= 0.09f && Mathf.Abs(___ddddd.y) <= 0.09f /*&& _mainCamera.orthographicSize <= _cmd._zoom*/) {
                        Appmain.appui.mainCamera3D.transform.localPosition = _position;
                        _cmd.Clear();
                    }else {
                        Appmain.appui.mainCamera3D.transform.localPosition = _tmpTarget;
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
                        
                    if(_cmd.statusCnt == 0) {
                        isChangeChannel = true;
                        isCheckType = true;
                    }

                    //if(_videoInfo.isChangeChannel && isCheckType) 
                    {
                        //time은 pause만 되는거라함 그래서 
                        bool isPlaying = (_mpc.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING);

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
                            //_mediaMain.Left(!isPlaying);
                            //_lastCallChangeChannelFrame = _videoInfo.frame;
                            _fdPlayerCTLUI.OnClickButton4Left(!isPlaying);
                            _cmd._channel_index ++;
                        }else if(_cmd._channel_index > 0) {
                            //_mediaMain.Right(!isPlaying);
                            //_lastCallChangeChannelFrame = _videoInfo.frame;
                            _fdPlayerCTLUI.OnClickButton4Right(!isPlaying);
                            _cmd._channel_index --;
                        }
                    }
                }
                break;
            case COMMAND_CTL_CAMERA.EFFECT :
                {
                    Transform[] _nowHave = Appmain.appui._EFFECT_MAIN.transform.GetComponentsInChildren<Transform>();
                    bool isHave = false;

                    for(int i = 0; i<_nowHave.Length; i++) {
                        if(_nowHave[i].transform.name == _cmd._effect_index.ToString()) {
                            isHave = true;
                            break;
                        }
                    }

                    if(isHave == false) {
                        GameObject _prefab = Appimg.LoadResource4Prefab(_cmd._effect_info.GET_PATH());
                        //Common/_Default_Effect/TAE/​pfb_effect_test​
                        //GameObject _prefab = Appimg.LoadResource4Prefab("Common/_Default_Effect/TAE/​pfb_effect_test​");
                        //GameObject _prefab = Appimg.LoadResource4Prefab("Common/_Default_Effect/TAE/ROUND_FX");

                        //D:\Project\2018\4DR_TAE\Unity\4DR\Assets\Resources\Common\_Default_Effect\TAE
                        //GameObject _prefab = Appimg.LoadResource4Prefab("Common/_Default_Effect/TAE/BLUE_HIT__MIDDLE");

                        if(_prefab != null) {

                            _prefab.name = _cmd._effect_index.ToString();

                            _prefab.transform.SetParent(Appmain.appui._EFFECT_MAIN.transform);                        

                            //Vector3 _position = _mainCamera.ScreenToWorldPoint(new Vector3(_cmd._x, DEFINE.BASE_SCREEN_HEIGHT - _cmd._y));

                            _prefab.transform.localPosition = new Vector3(_cmd._x, -_cmd._y, 0);
                            _prefab.transform.localScale = new Vector3(100.0f, 100.0f, 100.0f);                        

                            Appmain.appsound.playEffect(SOUND_EFFECT_TYPE.Effect_Sound);
                        
                            isoDestoryTime dtt = _prefab.AddComponent<isoDestoryTime>();
                        
                            dtt.SET_DESTROY_TIMER(4f);
                        }                    

                        _cmd.Clear();
                    }else {
                        _cmd.Clear();
                    }
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
                {
                    
                    bool isCheckType = (_lastCallChangeChannelFrame != _videoInfo.frame);

                    if(_cmd.statusCnt == 0) {
                        isChangeChannel = true;
                        isCheckType = true;
                    }

                    //if(_videoInfo.isChangeChannel && isCheckType) 
                    {
                        bool isPlaying = (_mpc.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING);
                        int whereto = _cmd._channel_index - _videoInfo.channel;

                        //Debug.Log("whereto : " + whereto + "/" + _cmd._channel_index + "/vc: + " + _videoInfo.channel);

                        if(whereto == 0) {
                            _cmd.Clear();
                        }else if(whereto > 0) {                        
                            //_mediaMain.Right(!isPlaying);
                            //_lastCallChangeChannelFrame = _videoInfo.frame;
                            _fdPlayerCTLUI.OnClickButton4Right(!isPlaying);
                        }else if(whereto < 0) {
                            //_mediaMain.Left(!isPlaying);
                            //_lastCallChangeChannelFrame = _videoInfo.frame;
                            _fdPlayerCTLUI.OnClickButton4Left(!isPlaying);
                        }
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
                //if(_lastCallChangeTimeMove != _videoInfo.frame) 
                {

                    if(_mpc.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING) {
                        _mpc.Pause();
                    }
                    {
                        //if(_cmd._time_rewind == 0) {
                        //    _cmd.Clear();
                        //}else if(_cmd._time_rewind > 0) {
                        //    _mediaMain.Left(true);
                        //    _lastCallChangeTimeMove = _videoInfo.frame;
                        //    _cmd._time_rewind --;
                        //}
                        _fdPlayerCTLUI.OnClickButton4Left(true);
                        _cmd._time_rewind -= Time.deltaTime;
                           
                        //Debug.Log("_cmd._time_rewind : " + _cmd._time_rewind);
                        if(_cmd._time_rewind <= 0f) {
                            _cmd.Clear();

                            if(_cmd._isReplay) _mpc.Play();
                        }
                    }
                }  
                break;
            case COMMAND_CTL_CAMERA.TIME_FORWARD:
                //if(_lastCallChangeTimeMove != _videoInfo.frame) 
                {
                    if(_mpc.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING) {
                        _mpc.Pause();
                    }
                    {                    

                        //if(_cmd._time_forward == 0) {
                        //    _cmd.Clear();
                        //}else if(_cmd._time_forward > 0) {
                        //    _mediaMain.Right(true);
                        //    _lastCallChangeTimeMove = _videoInfo.frame;
                        //    _cmd._time_forward --;
                        //}
                        _fdPlayerCTLUI.OnClickButton4Right(true);
                        _cmd._time_forward -= Time.deltaTime;

                        //Debug.Log("_cmd._time_forward : " + _cmd._time_forward);
                        if(_cmd._time_forward <= 0f) {
                            
                            _cmd.Clear();
                            if(_cmd._isReplay) _mpc.Play();
                        }
                    }
                }  
                break;
            case COMMAND_CTL_CAMERA.SPEED:
                if(_cmd._speed > 0) {
                    //_mediaMain.Speed(_cmd._speed);
                    Time.timeScale = _cmd._speed;
                    slowSpeed = _cmd._speed;

                    Debug.Log("Time.timeScale : " + Time.timeScale);
                    
		            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;

                    if(_cmd._speed == 1) {
                        if( _mpc.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED) {
                            _fdPlayerCTLUI.OnClickButton4Load();
                        }
                    }

                    _cmd.Clear();
                }
                break;
            case COMMAND_CTL_CAMERA.CHANNEL_LEFT:
                {
                    _fdPlayerCTLUI.OnClickButton4Left(false);
                    _cmd._channel_time -= Time.deltaTime;
                    
                    //Debug.Log("_cmd._channel_time : " + _cmd._channel_time);
                    if(_cmd._channel_time <= 0.0f) {
                        _cmd.Clear();
                    }
                }
                break;
            case COMMAND_CTL_CAMERA.CHANNEL_RIGHT:
                {
                    _fdPlayerCTLUI.OnClickButton4Right(false);
                    _cmd._channel_time -= Time.deltaTime;
                    
                    //Debug.Log("_cmd._channel_time : " + _cmd._channel_time);
                    if(_cmd._channel_time <= 0.0f) {
                        _cmd.Clear();
                    }
                }
                break;
#if _TAE_
                case COMMAND_CTL_CAMERA.PLAYER_INFO_ON:
                if(gameObjectPlayerInfoVs == null) {
                    gameObjectPlayerInfoVs = Appimg.LoadResource4Prefab4UI(UIDEFINE.PATH_TAE_PLAYER_INFO);                    
                    _cmd.Clear();


                    uisoPlayersVS _info = gameObjectPlayerInfoVs.GetComponent<uisoPlayersVS>();
                    
                    _info._blue.SET_INFO(Appmain.appmain.defaultPlayList[_cmd.blud_playerindex]);
                    _info._red.SET_INFO(Appmain.appmain.defaultPlayList[_cmd.red_playerindex]);

                    Appmain.appsound.playEffect(SOUND_EFFECT_TYPE.Player_Info);

                }else {
                    Debug.Log("gameObjectPlayerInfoVs not nulllllllll");
                }
                break;
            case COMMAND_CTL_CAMERA.PLAYER_INFO_OFF:
                {                    
                    TweenPosition[] _tw =  gameObjectPlayerInfoVs.GetComponentsInChildren<TweenPosition>();
                    TweenScale[] _tws = gameObjectPlayerInfoVs.GetComponentsInChildren<TweenScale>();

                    for(int i = 0; i<_tw.Length; i++) {
                        _tw[i].PlayReverse();
                    }

                    for(int i = 0; i<_tws.Length; i++) {
                        _tws[i].PlayReverse();
                    }

                    isoDestoryTime idt = gameObjectPlayerInfoVs.AddComponent<isoDestoryTime>();
                    idt.SET_DESTROY_TIMER(0f);
                    _cmd.Clear();
                }
                break;
            case COMMAND_CTL_CAMERA.ROUND_START:
                {
                    GameObject _prefab = Appimg.LoadResource4Prefab4UI(UIDEFINE.PATH_TAE_ROUND_START);                    
                    _cmd.Clear();

                    uisoRoundStart _roundStart = _prefab.GetComponent<uisoRoundStart>();

                    _roundStart.SET_INFO(_cmd.round_index);
                    
                    isoDestoryTime idt = _prefab.AddComponent<isoDestoryTime>();
                    idt.SET_DESTROY_TIMER(2f);

                    if(gameObjectTAEScore != null) {
                        uisoGameInfo _uigameInfo = gameObjectTAEScore.GetComponent<uisoGameInfo>();
                        _uigameInfo.SET_INFO_ONLY_ROUND(_cmd.round_index);
                        _uigameInfo._info.isPlaying = true;                        
                    }

                    Appmain.appsound.playEffect(SOUND_EFFECT_TYPE.Start_Round);

                }
                break;
            case COMMAND_CTL_CAMERA.ROUND_PAUSE:
                if(gameObjectTAEScore != null) {
                    uisoGameInfo _uigameInfo = gameObjectTAEScore.GetComponent<uisoGameInfo>();
                    
                    _uigameInfo._info.isPlaying = false;
                    _cmd.Clear();

                }
                break;
            case COMMAND_CTL_CAMERA.ROUND_RESTART:
                if(gameObjectTAEScore != null) {
                    uisoGameInfo _uigameInfo = gameObjectTAEScore.GetComponent<uisoGameInfo>();
                    
                    _uigameInfo._info.isPlaying = true;
                    _cmd.Clear();
                }
                break;
            case COMMAND_CTL_CAMERA.SET_GAME_INFO:
                {
                    GAME_INFO_TAE gameInfo = Appmain.appmain.defaultGameInfo[_cmd.gameInfoIndex];

                    //몇강 인지 설정
                    gameInfo.nowStageCnt = _cmd.nowStageCnt;
                    //현재 몇라운드 인지 설정
                    gameInfo.nowRoundCnt = _cmd.round_index;

                    //최초 이므로 초기화
                    gameInfo.isPlaying = false;
                    gameInfo.nowRoundTime = gameInfo.roundTime;
                
                    NGUITools.Destroy(gameObjectTAEScore);

                    //UI SET
                    if(gameInfo.gameType == GAME_TYPE_TAE.MINUS) {                        

                        gameObjectTAEScore = Appimg.LoadResource4Prefab4UI(UIDEFINE.PATH_TAE_SCORE_MINUS);
                        uisoGameInfo _uigameInfo = gameObjectTAEScore.GetComponent<uisoGameInfo>();
                        
                        //0이면 결승
                        if(gameInfo.nowStageCnt == 0) {
                            gameInfo.roundInfo = new ROUND_INFO_TAE[gameInfo.maxRoudnCnt_final];
                        }else {
                            gameInfo.roundInfo = new ROUND_INFO_TAE[gameInfo.maxRoundCnt_normal];
                        }

                        //선수 정보 셋팅
                        for(int i = 0; i<gameInfo.roundInfo.Length; i++) {
                            gameInfo.roundInfo[i] = new ROUND_INFO_TAE(Appmain.appmain.defaultPlayList[_cmd.blud_playerindex], Appmain.appmain.defaultPlayList[_cmd.red_playerindex]);
                        }

                        gameInfo.roundInfo[gameInfo.nowRoundCnt].blueWinCnt = _cmd.blueWinCnt;
                        gameInfo.roundInfo[gameInfo.nowRoundCnt].redWinCnt = _cmd.redWinCnt;

                        gameInfo.roundInfo[gameInfo.nowRoundCnt].blueScore = DEFINE.MAX_MINUS_GAME_SCORE;
                        gameInfo.roundInfo[gameInfo.nowRoundCnt].redScore = DEFINE.MAX_MINUS_GAME_SCORE;

                        gameInfo.roundInfo[gameInfo.nowRoundCnt].prevBlueScore = DEFINE.MAX_MINUS_GAME_SCORE;
                        gameInfo.roundInfo[gameInfo.nowRoundCnt].prevRedScore = DEFINE.MAX_MINUS_GAME_SCORE;

                        _uigameInfo.SET_INFO(gameInfo);
                    }else if(gameInfo.gameType == GAME_TYPE_TAE.PLUS) {
                        gameObjectTAEScore = Appimg.LoadResource4Prefab4UI(UIDEFINE.PATH_TAE_SCORE_PLUS);
                        uisoGameInfo _uigameInfo = gameObjectTAEScore.GetComponent<uisoGameInfo>();

                        gameInfo.roundInfo = new ROUND_INFO_TAE[gameInfo.maxRoundCnt_normal];

                        //선수 정보 셋팅
                        for(int i = 0; i<gameInfo.roundInfo.Length; i++) {
                            gameInfo.roundInfo[i] = new ROUND_INFO_TAE(Appmain.appmain.defaultPlayList[_cmd.blud_playerindex], Appmain.appmain.defaultPlayList[_cmd.red_playerindex]);
                        }

                        gameInfo.roundInfo[gameInfo.nowRoundCnt].blueWinCnt = _cmd.blueWinCnt;
                        gameInfo.roundInfo[gameInfo.nowRoundCnt].redWinCnt = _cmd.redWinCnt;

                        gameInfo.roundInfo[gameInfo.nowRoundCnt].blueScore = 0;
                        gameInfo.roundInfo[gameInfo.nowRoundCnt].redScore = 0;

                        _uigameInfo.SET_INFO(gameInfo);

                    }

                    _cmd.Clear();

                }
                break;
            case COMMAND_CTL_CAMERA.SET_GAME_INFO_OFF:
                {
                    NGUITools.Destroy(gameObjectTAEScore);
                    _cmd.Clear();
                }
                break;
            case COMMAND_CTL_CAMERA.SET_SCORE:
                if(gameObjectTAEScore != null) {
                    uisoGameInfo _uigameInfo = gameObjectTAEScore.GetComponent<uisoGameInfo>();
                    
                    if(_cmd.setScoreWho == WHAT_TEAM_COLOR.BLUE) {
                        _uigameInfo._info.roundInfo[_uigameInfo._info.nowRoundCnt].prevBlueScore = _uigameInfo._info.roundInfo[_uigameInfo._info.nowRoundCnt].blueScore;
                        _uigameInfo._info.roundInfo[_uigameInfo._info.nowRoundCnt].blueScore += _cmd.setScore;
                    }else {
                        _uigameInfo._info.roundInfo[_uigameInfo._info.nowRoundCnt].prevRedScore = _uigameInfo._info.roundInfo[_uigameInfo._info.nowRoundCnt].redScore;
                        _uigameInfo._info.roundInfo[_uigameInfo._info.nowRoundCnt].redScore += _cmd.setScore;
                    }

                    _uigameInfo.UPDATE_SCORE(_cmd);

                    if(_cmd.setScore > 0) {
                        Appmain.appsound.playEffect(SOUND_EFFECT_TYPE.Point_Up);
                    }else {
                        Appmain.appsound.playEffect(SOUND_EFFECT_TYPE.Energy_Gauge);
                    }

                    _cmd.Clear();
                    
                }
                break;
            case COMMAND_CTL_CAMERA.GAME_RESULT:
            case COMMAND_CTL_CAMERA.ROUND_RESULT:
                {                    
                    string path = ((_cmd.setScoreWho == WHAT_TEAM_COLOR.BLUE) ? UIDEFINE.PATH_EFFECT_WIN_BLUE:UIDEFINE.PATH_EFFECT_WIN_RED);

                    GameObject _prefab = Appimg.LoadResource4Prefab4UI(path);

                    //_prefab.transform.SetParent(Appmain.appui._EFFECT_MAIN.transform);
                    isoDestoryTime _life = _prefab.AddComponent<isoDestoryTime>();
                    _life.SET_DESTROY_TIMER(_cmd._life_time);

                    _cmd.Clear();

                }
                break;
            case COMMAND_CTL_CAMERA.PENALTY_START:
                if(gameObjectTAEScore != null) {
                    uisoGameInfo _uigameInfo = gameObjectTAEScore.GetComponent<uisoGameInfo>();

                    _uigameInfo.SET_PERNALTY(_cmd.setScoreWho);

                    _cmd.Clear();

                }
                break;
#endif
            case COMMAND_CTL_CAMERA.NONE :
                //Debug.Log("remove index == " + _cmd.index);
                //_commandes.RemoveAt(_cmd.index);
                break;
            case COMMAND_CTL_CAMERA.SET_VIBRATE:
                {
                    Appsound.VIBRATE();
                    _cmd.Clear();
                }
                break;
            case COMMAND_CTL_CAMERA.SET_VIBRATE_PATTEN:
                {
                    _mpc.VIBRATOR(_cmd.vibratior_pattern);
                    _cmd.Clear();
                }
                break;
            case COMMAND_CTL_CAMERA.SET_YELLOW_CARD:
                if(gameObjectTAEScore != null) {
                    uisoGameInfo _uigameInfo = gameObjectTAEScore.GetComponent<uisoGameInfo>();

                    if(_cmd.setScoreWho == WHAT_TEAM_COLOR.BLUE) {
                        _uigameInfo._info.roundInfo[_uigameInfo._info.nowRoundCnt].blue.yellowCardCnt = _cmd.setScore;
                    }else if(_cmd.setScoreWho == WHAT_TEAM_COLOR.RED) {
                        _uigameInfo._info.roundInfo[_uigameInfo._info.nowRoundCnt].red.yellowCardCnt = _cmd.setScore;
                    }

                    _uigameInfo.SET_YELLOW_CARD(_cmd.setScoreWho, _cmd.setScore);
                    _cmd.Clear();
                }
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

    public float _ori_zoom;
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

    public float _time_rewind;
    public float _time_forward;
    public bool _isReplay;
    public float _channel_time;

    public float _speed;

    public int statusCnt;


    //////////
    public int blud_playerindex;
    public int red_playerindex;

    ////
    ///
    public int round_index;
    public int blueWinCnt;
    public int redWinCnt;

    public int nowStageCnt;

    public int gameInfoIndex;

    ////
    ///
    public WHAT_TEAM_COLOR setScoreWho;
    public int setScore;

    public long[] vibratior_pattern;

    //table parsing용
    public Q_COMMAND_CTL_CAMERA(string _ori) {

        string[] _tmp = _ori.Split(","[0]);

        for(int j = 0; j<_tmp.Length; j++) {
            if(string.IsNullOrEmpty(_tmp[j])) {
                _tmp[j] = "0";
            }
        }

        int i = 0;

        _ori_command = _ori;

        index = Convert.ToInt32(_tmp[i++]);
        
        string __tmp = _tmp[i++];

        if(!string.IsNullOrEmpty(__tmp))
            _frame = Convert.ToDouble(__tmp);
        else
            _frame = 0;

        //COMMAND_CTL_CAMERA out _tmpCmd;
        bool _result = Enum.TryParse<COMMAND_CTL_CAMERA>(_tmp[i++], out cmd);

        if(_result == false) {
            Debug.Log("### Q_COMMAND_CTL_CAMERA Check command");
            return;
        }

        status = COMMAND_STATUS.WAIT;
        Debug.Log("Add Command Done : " + cmd);

        switch(cmd) {
         case COMMAND_CTL_CAMERA.DEFAULT :

            break;
        case COMMAND_CTL_CAMERA.PLAY :

            break;
        case COMMAND_CTL_CAMERA.PAUSE :            
            _pause_time = Convert.ToSingle(_tmp[i]);// / 1000f;
            Debug.Log("__pause_time :: " + _pause_time);
            if(_pause_time > 0f) {
                _pause_time = _pause_time / 1000f;
            }
            break;
        case COMMAND_CTL_CAMERA.ZOOM :
            _zoom = Convert.ToSingle(_tmp[i]);
            _ori_zoom = _zoom;

            if(Appmain.appimg._nowFullCtl.toggleOption[(int)SETTING.AUTO_ZOOM].value == false) {
                Clear();
            }
            break;
        case COMMAND_CTL_CAMERA.LOOKAT :

            if(Appmain.appimg._nowVideoCommander._lastLookAt != null) {
                Appmain.appimg._nowVideoCommander._lastLookAt.Clear();
            }

            _x = Convert.ToInt32(_tmp[i++]);
            _y = Convert.ToInt32(_tmp[i++]);
            //_zoom = Convert.ToSingle(_tmp[i++]);
            if(Appmain.appimg._nowFullCtl.toggleOption[(int)SETTING.AUTO_ZOOM].value == false) {
                Clear();
            }

            Appmain.appimg._nowVideoCommander._lastLookAt = this;
            break;
        case COMMAND_CTL_CAMERA.CHANNEL :
            _channel_index = Convert.ToInt32(_tmp[i++]);
            if(Appmain.appimg._nowFullCtl.toggleOption[(int)SETTING.CAMERA_ROTATION].value == false) {
                Clear();
            }
            break;
        case COMMAND_CTL_CAMERA.EFFECT :
            _x = Convert.ToInt32(_tmp[i++]);
            _y = Convert.ToInt32(_tmp[i++]);
            _effect_index = Convert.ToInt32(_tmp[i++]);
            _effect_info = Appmain.appmain.GET_DEFAULT_EFFECT(_effect_index);

            if(Appmain.appimg._nowFullCtl.toggleOption[(int)SETTING.EFFECT].value == false) {
                Clear();
            }
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
            if(Appmain.appimg._nowFullCtl.toggleOption[(int)SETTING.CAMERA_ROTATION].value == false) {
                Clear();
            }
            break;
        case COMMAND_CTL_CAMERA.CAMERA_SHAKE:
            _cameraShake_ud = Convert.ToSingle(_tmp[i++]);
            _cameraShake_lr = Convert.ToSingle(_tmp[i++]);
            _camera_amp = Convert.ToSingle(_tmp[i++]);
            _camera_freq = Convert.ToSingle(_tmp[i++]);
            break;
        case COMMAND_CTL_CAMERA.TIME_REWIND:
            _time_rewind = Convert.ToSingle(_tmp[i++]) / 1000f;
            _isReplay = (_tmp[i].Equals("1") == true);
            if(Appmain.appimg._nowFullCtl.toggleOption[(int)SETTING.TIME_MACHINE].value == false) {
                Clear();
            }
            break;
        case COMMAND_CTL_CAMERA.TIME_FORWARD:
            _time_forward = Convert.ToSingle(_tmp[i++]) / 1000f;
            _isReplay = (_tmp[i].Equals("1") == true);
            break;
        case COMMAND_CTL_CAMERA.SPEED:
            _speed = Convert.ToSingle(_tmp[i++]);
            break;
        case COMMAND_CTL_CAMERA.CHANNEL_LEFT:
        case COMMAND_CTL_CAMERA.CHANNEL_RIGHT:
            _channel_time = Convert.ToSingle(_tmp[i++]) / 1000f;
            if(Appmain.appimg._nowFullCtl.toggleOption[(int)SETTING.TIME_MACHINE].value == false) {
                Clear();
            }
            break;
#if _TAE_
        case COMMAND_CTL_CAMERA.PLAYER_INFO_ON:
            blud_playerindex = Convert.ToInt32(_tmp[i++]);
            red_playerindex = Convert.ToInt32(_tmp[i++]);
            break;        
        case COMMAND_CTL_CAMERA.PLAYER_INFO_OFF:
            break;

        case COMMAND_CTL_CAMERA.ROUND_START:
            round_index = Convert.ToInt32(_tmp[i++]);
            break;

        case COMMAND_CTL_CAMERA.SET_GAME_INFO:
            gameInfoIndex = Convert.ToInt32(_tmp[i++]);
            
            nowStageCnt = Convert.ToInt32(_tmp[i++]);

            {
                string[] _tmptmp = _tmp[i++].Split("|"[0]);

                if(_tmptmp.Length > 1) {
                    round_index = Convert.ToInt32(_tmptmp[0]);
                    blueWinCnt = Convert.ToInt32(_tmptmp[1]);
                    redWinCnt = Convert.ToInt32(_tmptmp[2]);
                }
            }
            blud_playerindex = Convert.ToInt32(_tmp[i++]);
            red_playerindex = Convert.ToInt32(_tmp[i++]);
            break;
        case COMMAND_CTL_CAMERA.SET_YELLOW_CARD:
        case COMMAND_CTL_CAMERA.SET_SCORE:
            setScoreWho = (WHAT_TEAM_COLOR)Convert.ToInt32(_tmp[i++]);
            setScore = Convert.ToInt32(_tmp[i++]);
            break;
        case COMMAND_CTL_CAMERA.ROUND_PAUSE:
        case COMMAND_CTL_CAMERA.ROUND_RESTART:
            break;
        case COMMAND_CTL_CAMERA.SET_LOOKAT:
            if(Appmain.appimg._nowVideoCommander._lastSetLookAt != null) {
                Appmain.appimg._nowVideoCommander._lastSetLookAt.Clear();
            }
            setScoreWho = (WHAT_TEAM_COLOR)Convert.ToInt32(_tmp[i++]);
            Appmain.appimg._nowVideoCommander._lastSetLookAt = this;
            break;
        case COMMAND_CTL_CAMERA.PENALTY_START:
            setScoreWho = (WHAT_TEAM_COLOR)Convert.ToInt32(_tmp[i++]);
            break;
        case COMMAND_CTL_CAMERA.GAME_RESULT:
        case COMMAND_CTL_CAMERA.ROUND_RESULT:
            setScoreWho = (WHAT_TEAM_COLOR)Convert.ToInt32(_tmp[i++]);
            _life_time = Convert.ToSingle(_tmp[i++]) / 1000f;
            break;

#endif
        case COMMAND_CTL_CAMERA.SET_VIBRATE_PATTEN:
            {
                string[] _tmptmp = _tmp[i++].Split("|"[0]);

                Debug.Log("_tmptmp.Length : " + _tmptmp.Length);

                vibratior_pattern = new long[_tmptmp.Length];

                for(int j = 0; j<_tmptmp.Length; j++) {
                    vibratior_pattern[j] = Convert.ToInt64(_tmptmp[j]);
                }
            }
            break;
        case COMMAND_CTL_CAMERA.UNSET_LOOKAT:
            if(Appmain.appimg._nowVideoCommander._lastSetLookAt != null) {
                Appmain.appimg._nowVideoCommander._lastSetLookAt.Clear();
            }
            break;
        }        
    }


    public void ING() {

        status = COMMAND_STATUS.ING;

    }

    public void Clear() {

        status = COMMAND_STATUS.DONE;
        //cmd = COMMAND_CTL_CAMERA.NONE;

    }
}


[Serializable]
public class VIDEO_EXTRA_INFOMATION {

    public int frame;
    public int channel;
    public _object[] objects;

    public int[] hitting_coord;

    public bool slow_motion_trigger;

    [Serializable]
    public class _object {
        public int id;
        public int[] rect;
        public float probability;

        //public _object() {
        //    changey();
        //}

        //public void changey() {
        //    Debug.Log("b rect[1] = " + rect[1]);
        //    rect[1] = (int)DEFINE.BASE_SCREEN_HEIGHT - rect[1];
        //    Debug.Log("a rect[1] = " + rect[1]);
        //}
    }


    public _object GET_OBJECT_FROM_ID(int id) {

        if(objects == null) return null;

        for(int i = 0; i<objects.Length; i++) {

            if(objects[i].id == id) {
                return objects[i];
            }
        }

        return null;

    }

}


public enum COMMAND_CTL_CAMERA {

    NONE = -1,
    DEFAULT = 0,
    PLAY,
    PAUSE,
    ZOOM,
    LOOKAT,
    CHANNEL,    
    EFFECT,
    TEXT,
    CHANNEL_TO,
    CAMERA_SHAKE,    
    TIME_REWIND,
    TIME_FORWARD,
    SPEED,
    CHANNEL_LEFT,
    CHANNEL_RIGHT,

#if _TAE_
    
    //video의 종류에 따라(카테고리) 분기되어야함.
    SET_GAME_INFO,
    SET_GAME_INFO_OFF,

    PLAYER_INFO_ON,
    PLAYER_INFO_OFF,

    ROUND_START,

    SET_SCORE,

    ROUND_PAUSE,
    ROUND_RESTART,

    PENALTY_START,        
    ROUND_RESULT,
    GAME_RESULT,

    SET_YELLOW_CARD,

    SET_LOOKAT,
    UNSET_LOOKAT,

    SET_SLOW,
    UNSET_SLOW,

#endif

    SET_VIBRATE,

    SET_VIBRATE_PATTEN,

}
 

public enum COMMAND_STATUS {

    NONE = -1,
    WAIT,
    ING,
    DONE,

}


public enum WHAT_TEAM_COLOR {

    NONE = -1,
    BLUE = 0,
    RED = 1,
    REFREEE = 2

}


public enum PARTS {

	head,
	body,
	kick,
	punch

}


public enum EXTRA_OBJECT_TYPE {
    
    red = 0,
    blue = 1,
    refree = 2,
    hug = 3,

    LENGTH
}