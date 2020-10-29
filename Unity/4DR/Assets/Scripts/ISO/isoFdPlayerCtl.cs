using FFmpeg.AutoGen;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class isoFdPlayerCtl : MonoBehaviour {

    public Appfdcontroller fdcontroller;
    
    public MediaPlayerCtrl _mpc;

    public AppandroidCallback4FDPlayer _info;
    
    public UIButton _player;
    public UIButton buttonTL;
    public UIButton buttonTR;

    public UIButton buttonLeftCamera;
    public UIButton buttonRightCamera;
    public bool isPressLeftCamera;
    public bool isPressRightCamera;

    public UISlider slider;
    
    public bool isRight;
    public bool isLeft;

    public bool isLeftTime;
    public bool isRightTime;

    float _cameraY;
    float _cameraRotationSpeed;

    
	public int _idx;

    public Transform beforeParent;

    public float _contrlerStatusTime;


    public TweenAlpha tweenCtlPanel;


    public TweenPosition tweenPosition4RightMenu;
    public bool isRightMenu = true;
    public TweenPosition tweenPosition4LeftMenu;
    public bool isLeftMenu = true;
    public TweenPosition tweenPosition4BottomMenu;
    public bool isBottomMenu = true;

    public UILabel labelEffectName;

    public int lastCallFrame;

    public bool isPressed;

    public bool isPressed_AX7;
    public bool isPressed_AX8;

    public int max_channel;

    isoShakeCamera camerashake;


    public UILabel labelFrameInfo;
    public UILabel labelCommandList;


    public isoNGUIJoystick joyStickChannel;
    public isoNGUIJoystick joyStickTime;

    public bool isSlowChange;

    public UIToggle togglePlayButton;

    public UILabel labelTime;

    // Start is called before the first frame update
    void Start() {

        _info = FindObjectOfType<AppandroidCallback4FDPlayer>();
        _info._mpc = this._mpc;

        if(Appmain.appui != null) 
            camerashake = Appmain.appui.mainCamera3D.GetComponent<isoShakeCamera>();

        isRight = false;
        isLeft = false;
        isLeftTime = false;
        isRightTime = false;

        buttonTL.isEnabled = false;
        buttonTR.isEnabled = false;

        slider.onDragFinished += onDragFinished;

        buttonLeftCamera.gameObject.AddComponent<UIEventListener>().onPress = OnClickButton4Left_Camera;
        buttonRightCamera.gameObject.AddComponent<UIEventListener>().onPress = OnClickButton4Right_Camera;
        _player.gameObject.AddComponent<UIEventListener>().onPress = OnClickButton4Player;
        _cameraY = 0.0f;
        _cameraRotationSpeed = 4.5f;

        //beforeParent = this.gameObject.transform.parent;
        //this.transform.SetParent(this.gameObject.transform.parent.parent);
        _idx = 0;

        _contrlerStatusTime = DEFINE.CONTRLER_STATUS_TIME;

        isLeftMenu = true;
        isRightMenu = true;

        labelEffectName.text = GET_EFFECT_NAME(_idx, true);

        isPressed = false;
        isPressed_AX7 = false;
        isPressed_AX8 = false;

        isBottomMenu = true;

        //joyStickChannel = GetComponentInChildren<isoNGUIJoystick>();
        joyStickChannel._sider.onDragFinished += OnReleaseJoyStick;
        joyStickTime._sider.onDragFinished += OnReleaseJoyStick;
    }

    private void onDragFinished() {

        int _change = (int)((double)_info.duration * slider.value);
                
        _mpc.SeekTo(_change);
        
    }


    public void OnValueChangeJoyStick() {

        if(joyStickChannel._sider.value < 0.5f) {
            isLeft = true;
            isRight = false;
            if(joyStickChannel._sider.value > 0f) {
                isSlowChange = true;
            }else {
                isSlowChange = false;
            }
        }
        
        if(joyStickChannel._sider.value > 0.5f) {
            isLeft = false;
            isRight = true;
            if(joyStickChannel._sider.value < 1f) {
                isSlowChange = true;
            }else {
                isSlowChange = false;
            }
        }
        
        if(joyStickChannel._sider.value == 0.5f) {
            isLeft = false;
            isRight = false;            
            isSlowChange = false;            
        }
    }


    public void OnValueChangeJoyStick4Time() {

        if(joyStickTime._sider.value < 0.5f) {
            isRightTime = false;
            isLeftTime = true;
        }
        
        if(joyStickTime._sider.value > 0.5f) {
            isLeftTime = false;
            isRightTime = true;
        }
        
        if(joyStickTime._sider.value == 0.5f) {
            isLeftTime = false;
            isRightTime = false;
        }
    }


    public void OnReleaseJoyStick() {

        isLeft = false;
        isRight = false;
        isLeftTime = false;
        isRightTime = false;

    }

    // Update is called once per frame
    void LateUpdate() {

        //if(isLeftTime == false && isRightTime == false) 
            {
            if(isLeft == true || joyStickChannel._sider.value == 0f) {

                if(isSlowChange == true) {
                    if(Appmain.gameStatusCnt % 5 == 0) {
                        OnClickButton4Left(false);
                    }
                }else {
                    OnClickButton4Left(false);
                }
            }else if(isRight == true || joyStickChannel._sider.value == 1f) {
                if(isSlowChange == true) {
                    if(Appmain.gameStatusCnt % 5 == 0) {
                        OnClickButton4Right(false);
                    }
                }else {
                    OnClickButton4Right(false);
                }                
            }
        }

        //if(isLeft == false && isRight == false) 
            {
            if(isLeftTime == true) {
                OnClickButton4Left(true);
            }else if(isRightTime == true) {
                OnClickButton4Right(true);
            }
        }
        
        if(isPressLeftCamera == true) {
            OnClickButton4Left_Camera(this.gameObject, true);
        }

        if(isPressRightCamera == true) {
            OnClickButton4Right_Camera(this.gameObject, true);
        }

        //if(isPressPlayerBackButton == true) {
        //    OnClickButton4Player(this.gameObject, true);
        //}
        if(_info != null) {
            if(_info.duration != 0) {            

                float _value = (float)_info.time / (float)_info.duration;
                labelFrameInfo.text = string.Format("Time {0}/{1}\nFrame {2}\nChannel {3}\nwidth {4}\nheight {5}", _info.time, _info.duration, _info.frame, _info.channel, _info.videoWidth, _info.videoHeight);
                slider.value = _value;

                if (isPressRightCamera == false && isPressLeftCamera == false) {

                    Appmain.appui._EFFECT_MAIN.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, _cameraY, 0.0f));                

                    _cameraY = _info.channel * _cameraRotationSpeed;
                }
                
                labelTime.text = Appdoc.getNumberToDateTime4Ori(_info.time / 1000, string.Empty, false);
                
            }else {
                labelTime.text = "UNKNOW";
            }
        }

        //if(_contrlerStatusTime > 0.0f) {
        //    _contrlerStatusTime -= Time.deltaTime;
        //}else if(_contrlerStatusTime == 0.0f) {

        //}else {
        //    _contrlerStatusTime = 0.0f;
        //    tweenCtlPanel.PlayReverse();
        //}

        if(Input.GetAxis("Vertical") < 0.5f && Input.GetAxis("Vertical") > -0.5f) 
            {
            if(Input.GetAxis("Horizontal") > 0.5f) {
                OnClickButton4Right(false);
            }else if(Input.GetAxis("Horizontal") < -0.5f) {
                OnClickButton4Left(false);
            }
        }

        if(Input.GetAxis("DPAD_h") != 0.0f) {

            //Debug.Log("DPAD_H : " + Input.GetAxis("DPAD_h"));

        }

        if(Input.GetAxis("DPAD_v") != 0.0f) {
            //Debug.Log("DPAD_V : " + Input.GetAxis("DPAD_v"));
            if(isPressed == false) {
                if(Input.GetAxis("DPAD_v") > 0.0f) {
                    OnClickButton4Right(false);
                }else {
                    OnClickButton4Left(false);
                }
            }
            isPressed = true;
        }else if(Input.GetAxis("DPAD_v") == 0.0f) {
            isPressed = false;
        }

        if((Input.GetAxis("axis7") != 0.0f) || (Input.GetAxis("axis13") != 0.0f)) {
            if(isPressed_AX7 == false) {
                //OnClickButton4Load();
                OnClickButton4Left(false);
            }
            isPressed_AX7 = true;
        }else if((Input.GetAxis("axis7") == 0.0f) || (Input.GetAxis("axis13") == 0.0f)) {
            isPressed_AX7 = false;
        }

        if((Input.GetAxis("axis8") != 0.0f) || (Input.GetAxis("axis12") != 0.0f)) {
            if(isPressed_AX8 == false) {
                //scrMedia.PlayToNow();
                OnClickButton4Right(false);
            }
            isPressed_AX8 = true;
        }else if((Input.GetAxis("axis8") == 0.0f) || (Input.GetAxis("axis12") == 0.0f)) {
            isPressed_AX8 = false;
        }

        //for(int i = 5; i<16; i++) {
            
        //    string tmp = string.Format("axis{0}", i);            
        //    float _value = Input.GetAxis(tmp);

        //    if(_value != 0.0f) {
        //        Debug.Log(tmp + "::: " + _value);
        //    }
        //}

        for(int i = 0; i<10; i++) {
            string tmp = string.Format("joystick button {0}", i);            

            if(Input.GetKeyDown(tmp)) {

                Debug.Log(tmp);

                switch((XOBX_ONE_BUTTON)i) {
                    case XOBX_ONE_BUTTON.BUTTON_Y:
                        {
                            AppCommandCtlCamera _ccc = this.beforeParent.GetComponent<AppCommandCtlCamera>();
                            
                            _ccc._cameraShake.StartUpDownShake(3f);
                            _ccc._cameraShake.StartLeftRightShake(2f);
                        }
                        break;
                    case XOBX_ONE_BUTTON.BUTTON_A:
                        if( _mpc.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED) {
                            OnClickButton4Load();
                        }
                        break;
                    case XOBX_ONE_BUTTON.BUTTON_B:
                        _mpc.PlayToNow();
                        break;
                    case XOBX_ONE_BUTTON.BUTTON_X:
                        OnClickButtonExit();
                        break;
                    case XOBX_ONE_BUTTON.LB:
                        if( _mpc.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING) {
                            OnClickButton4Pause();
                        }
                        break;
                    case XOBX_ONE_BUTTON.RB:
                        if( _mpc.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING) {
                            OnClickButton4Pause();
                        }
                        break;
                    case XOBX_ONE_BUTTON.RS_B:
                        if( _mpc.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING) {
                            OnClickButton4Pause();
                        }else if( _mpc.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED) {
                            OnClickButton4Load();
                        }
                        break;
                }
            }else if(Input.GetKey(tmp)) {
                switch((XOBX_ONE_BUTTON)i) {
                    case XOBX_ONE_BUTTON.LB:                        
                        OnClickButton4Left(true);
                        break;
                    case XOBX_ONE_BUTTON.RB:
                        OnClickButton4Right(true);
                        break;
                }
            }
        }

#if UNITY_EDITOR
        lastCallFrame = -999;
#endif
    }   

    public bool isPressPlayerBackButton;
    public void OnClickButton4Player(GameObject obj, bool isPress) {

#if _TAE_
        return;
#endif

        if(isPress == true) {
            float _x = Input.mousePosition.x - (Screen.width / 2);
			float _y = Input.mousePosition.y - (Screen.height / 2);

			//Debug.Log("_x :: " + _x + "/" + _y);
			//for testing...			
            string _path = GET_EFFECT_NAME(_idx);			
            int scaleSize = GET_EFFECT_SCALE_SIZE(_idx);

            Debug.Log("_path :: " + _path);

            if(_path.Contains("CameraShakeLeftRight") == true) {
                //isoShakeCamera camerashake = Appmain.appui.mainCamera3D.GetComponent<isoShakeCamera>();

                camerashake.StartLeftRightShake(1.0f);
                //camerashake.StartUpDownShake(0.5f);

            }else if(_path.Contains("CameraShakeUpDown") == true) {

                //isoShakeCamera camerashake = Appmain.appui.mainCamera3D.GetComponent<isoShakeCamera>();

                //camerashake.StartLeftRightShake(0.5f);
                camerashake.StartUpDownShake(1.0f);

            }else if(_path.Contains("CamearShakeLRUP") == true) {

                //isoShakeCamera camerashake = Appmain.appui.mainCamera3D.GetComponent<isoShakeCamera>();

                camerashake.StartLeftRightShake(1.0f);
                camerashake.StartUpDownShake(1.0f);

            }else {

			    RaycastHit _hit;

			    Ray _ray = Appmain.appui.mainCamera3D.ScreenPointToRay(Input.mousePosition);                

                //Debug.DrawRay(_ray, Vector3.forward);
			    if(Physics.Raycast(_ray, out _hit)) {
				    GameObject prefab = Appimg.LoadResource4Prefab(_path);
				    prefab.transform.SetParent(Appmain.appui._EFFECT_MAIN.transform);                
                				
				    prefab.transform.position = _hit.point;//Appmain.appui.mainCamera3D.WorldToScreenPoint(_hit.point);
				    prefab.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);

                    Debug.Log("hit point == " + _hit.point);
			    }

			    //GameObject prefab = Appimg.LoadResource4Prefab(_effects_path[_idx]);
			
			    //prefab.transform.SetParent(Appmain.appui.transform);
			    //prefab.transform.localPosition = new Vector3(_x, _y, -100.0f);
			    //prefab.transform.localScale = new Vector3(150, 150, 150);

                tweenCtlPanel.PlayForward();
                _contrlerStatusTime = DEFINE.CONTRLER_STATUS_TIME;           
            }
        }

        isPressPlayerBackButton = isPress;
    }


    string GET_EFFECT_NAME(int _index, bool isShort = false) {

        string[] _effects_path = {
					//"Common/_Default_Effect/Magic fire 0",
					//"Common/_Default_Effect/Magic fire 1",
					//"Common/_Default_Effect/Magic fire 2",
					//"Common/_Default_Effect/Magic fire 3",
					//"Common/_Default_Effect/Magic fire pro blue",	//4

					//"Common/_Default_Effect/Magic fire pro green",
					//"Common/_Default_Effect/Magic fire pro orange",
					//"Common/_Default_Effect/Magic fire pro red",
					//"Common/_Default_Effect/Magic fire pro yellow",
                    "Common/_Default_Effect/Hit_A_01_Blue",
                    "Common/_Default_Effect/Hit_A_02_Blue",
                    "Common/_Default_Effect/Hit_A_03_Blue",
                    "Common/_Default_Effect/Hit_A_04_Blue",
                    "Common/_Default_Effect/Hit_A_05_Blue",

                    "Common/_Default_Effect/Hit_A_01_Red",
                    "Common/_Default_Effect/Hit_A_02_Red",
                    "Common/_Default_Effect/Hit_A_03_Red",
                    "Common/_Default_Effect/Hit_A_04_Red",
                    "Common/_Default_Effect/Hit_A_05_Red",

					"Common/_Default_Effect/pfb_Effect_Touch",	//9

					//"Common/_Default_Effect/RotatorPS1",
					//"Common/_Default_Effect/RotatorPS2",
					//"Common/_Default_Effect_2/star2",

                    "Common/_Default_effect_3/CFX_MagicPoof",
                    "Common/_Default_effect_3/CFX_Hit_A Red+RandomText",
                    "Common/_Default_effect_3/CFX2_BrokenHeart",
                    "Common/_Default_effect_3/CFX3_Fire_Explosion",
                    "Common/_Default_effect_3/CFX3_Hit_Light_C_Air",
                    "Common/_Default_effect_3/CFX3_Snow_Dense",
                    "Common/_Default_effect_3/CFX4 Environment Bubbles Denser",
                    "c/_d/CameraShakeLeftRight",
                    "c/_d/CameraShakeUpDown",
                    "c/_d/CamearShakeLRUP"

				};
        
        int rtn = Mathf.Clamp(_index, 0, _effects_path.Length - 1);
        string _rtnString = _effects_path[rtn];
        if(isShort == true) {
            string[] _tmp = _rtnString.Split("/"[0]);

            _rtnString = _tmp[2];
        }

        return _rtnString;

    }

    int GET_EFFECT_SCALE_SIZE(int _index) {

        int[] _effects_scale_size = {
					//"Common/_Default_Effect/Magic fire 0",
					//"Common/_Default_Effect/Magic fire 1",
					//"Common/_Default_Effect/Magic fire 2",
					//"Common/_Default_Effect/Magic fire 3",
					//"Common/_Default_Effect/Magic fire pro blue",	//4

					//"Common/_Default_Effect/Magic fire pro green",
					//"Common/_Default_Effect/Magic fire pro orange",
					//"Common/_Default_Effect/Magic fire pro red",
					//"Common/_Default_Effect/Magic fire pro yellow",
                    50,
                    50,
                    50,
                    50,
                    50,//5

                    50,
                    50,
                    50,
                    50,
                    50,


					50,//"Common/_Default_Effect/pfb_Effect_Touch",	//9

					//"Common/_Default_Effect/RotatorPS1",
					//"Common/_Default_Effect/RotatorPS2",
					//"Common/_Default_Effect_2/star2",

                    50,//"Common/_Default_effect_3/CFX_MagicPoof",
                    50,//"Common/_Default_effect_3/CFX_Hit_A Red+RandomText",
                    50,//"Common/_Default_effect_3/CFX2_BrokenHeart",
                    50,//"Common/_Default_effect_3/CFX3_Fire_Explosion",
                    50,//"Common/_Default_effect_3/CFX3_Hit_Light_C_Air",
                    100,//"Common/_Default_effect_3/CFX3_Snow_Dense",
                    200,//"Common/_Default_effect_3/CFX4 Environment Bubbles Denser",
                    1,
                    1,
                    1

				};

        if(_index >= _effects_scale_size.Length) {
            return 1;
        }

        return _effects_scale_size[_index];
    }


    public void OnClickButton4EffectIndex() {

        _idx ++;
        labelEffectName.text = GET_EFFECT_NAME(_idx, true);

    }


    public void OnClickButton4EffectIndexM() {

        _idx --;
        labelEffectName.text = GET_EFFECT_NAME(_idx, true);

    }

    public void OnClickButton4PressLeft() {
        
        isLeft = true;
    }


    public void OnClickButton4ReleaseLeft() {
        isLeft = false;
    }


    public void OnClickButton4PressRight() {
        isRight = true;
    }


    public void OnClickButton4ReleaseRight() {
        isRight = false;
    }


    public void OnClickButton4PressLeftTime() {
        isLeftTime = true;
    }


    public void OnClickButton4ReleaseLeftTime() {
        isLeftTime = false;
    }


    public void OnClickButton4PressRightTime() {
        isRightTime = true;
    }


    public void OnClickButton4ReleaseRightTime() {
        isRightTime = false;
    }


    public void OnClickButton4Left(bool _how) {

        
        if(Appmain.appmain.selectVideoType != VIDEO_TYPE.LOCAL_LIST) {
#if !UNITY_EDITOR
            if(_info.channel == 0) return;

            if( _mpc.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING) {
                if(_info.frame == lastCallFrame) return;
            }
#endif
        }

        
       StartCoroutine(_OnClickButton4Left(_how));

    }


    IEnumerator _OnClickButton4Left(bool _how) {

        yield return new WaitForFixedUpdate();

        _mpc.Left(_how);

#if UNITY_EDITOR
        //_info.channel --;
        //_info.channel = Mathf.Clamp(_info.channel, 0, 45);
         SEND_FDLIVE_SWIPE _send = new SEND_FDLIVE_SWIPE();

		_send.sessionId = _mpc.GET_RTSP_SESSION_ID();
		_send.actionType = "normal";
		_send.direction = "left";
		_send.speed = 1;
		_send.moveFrame = 1;

		string message = JsonUtility.ToJson(_send);
		Debug.Log("message = " + message);
		fdcontroller._SEND(string.Empty, message);
#endif

        lastCallFrame = _info.frame;

    }


    public void OnClickButton4Right(bool _how) {
                
        if(Appmain.appmain.selectVideoType != VIDEO_TYPE.LOCAL_LIST) {
            if(max_channel != 0) {
                if(_info.channel == max_channel) return;
            }

            if( _mpc.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING) {
                if(_info.frame == lastCallFrame) return;
            }
        }               

        StartCoroutine(_OnClickButton4Right(_how));
    }


    IEnumerator _OnClickButton4Right(bool _how) {

        yield return new WaitForFixedUpdate();
                
        _mpc.Right(_how);        

#if UNITY_EDITOR
        //_info.channel ++;
        //_info.channel = Mathf.Clamp(_info.channel, 0, 45);
        SEND_FDLIVE_SWIPE _send = new SEND_FDLIVE_SWIPE();

        //setSwipe : 21 normal right 1 1

		_send.sessionId = _mpc.GET_RTSP_SESSION_ID();
		_send.actionType = "normal";
		_send.direction = "right";
		_send.speed = 1;
		_send.moveFrame = 1;

		string message = JsonUtility.ToJson(_send);
		Debug.Log("message = " + message);
		//StartCoroutine(fdcontroller._SEND_(string.Empty, message.Trim()));
        fdcontroller._SEND(string.Empty, message);
#endif
                
        lastCallFrame = _info.frame;
    }


    void OnClickButton4Left_Camera(GameObject go, bool press) {       
        
        if(press == true) {
            float now = Appmain.appui._EFFECT_MAIN.transform.localRotation.y;
            
            //Appmain.appui.mainCamera3D.transform.localPosition = new Vector3(0, 0, 0);
            Appmain.appui._EFFECT_MAIN.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, _cameraY, 0.0f));
            //AppUI.mainCamera.transform.localEulerAngles += new Vector3(0.0f, _cameraY, 0.0f);

            _cameraY -= Time.fixedDeltaTime * (_cameraRotationSpeed * 4);
        }

        isPressLeftCamera = press;

    }

    void OnClickButton4Right_Camera(GameObject go, bool press) {
        
        if(press == true) {
            float now = Appmain.appui._EFFECT_MAIN.transform.localRotation.y;

            //Appmain.appui.mainCamera3D.transform.localPosition = new Vector3(0, 0, 0);
            Appmain.appui._EFFECT_MAIN.transform.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, _cameraY, 0.0f));
            //AppUI.mainCamera.transform.localEulerAngles += new Vector3(0.0f, _cameraY, 0.0f);

            _cameraY += Time.fixedDeltaTime * (_cameraRotationSpeed * 4);
        }

        isPressRightCamera = press;

    }



    public void OnClickButton4Load() {

        //Debug.Log("OnClickButton4Load()");
        _mpc.Play();
        buttonTL.isEnabled = false;
        buttonTR.isEnabled = false;       

    }


    public void OnClickButton4PlayParticle() {

        {
            ParticleSystem[] _ppp = Appmain.appui.GetComponentsInChildren<ParticleSystem>();

            for(int i = 0; i<_ppp.Length; i++) {
                _ppp[i].Play();
            }
        }

    }


    public void OnClickButton4Pause() {

        _mpc.Pause();
        buttonTL.isEnabled = true;
        buttonTR.isEnabled = true;       

    }


    public void OnClickButton4PauseParticle() {

        {
            ParticleSystem[] _ppp = Appmain.appui.GetComponentsInChildren<ParticleSystem>();

            for(int i = 0; i<_ppp.Length; i++) {
                _ppp[i].Pause();
            }
        }

    }


    public void OnClickButton4ClearParticle() {

        {
            ParticleSystem[] _ppp = Appmain.appui.GetComponentsInChildren<ParticleSystem>();

            for(int i = 0; i<_ppp.Length; i++) {
                NGUITools.Destroy(_ppp[i].gameObject);
            }
        }

    }


    public void OnClickButtonExit() {


#if _DIRECT_URL_
        NGUITools.Destroy(this.beforeParent.gameObject);
        NGUITools.Destroy(this.transform.gameObject);

        Appmain.appdoc.setGameStatus(GAME_STATUS.GS_TITLE);
#else
        Appmain.appimg.mainUIPrefab.SetActive(true);

        AppCommandCtlCamera _ccc = this.beforeParent.GetComponent<AppCommandCtlCamera>();

        NGUITools.Destroy(_ccc.gameObjectPlayerInfoVs);
        NGUITools.Destroy(_ccc.gameObjectTAEScore);

        NGUITools.Destroy(this.beforeParent.gameObject);
        NGUITools.Destroy(this.transform.gameObject);
#endif

        OnClickButton4ClearParticle();
        Appmain.appui.mainCamera3D.orthographicSize = 1f;

        {
            uisoEffectText[] _tmp = Appmain.appui._EFFECT_MAIN.GetComponentsInChildren<uisoEffectText>();

            for(int i = 0; i<_tmp.Length; i++) {

                NGUITools.Destroy(_tmp[i].gameObject);

            }
        }

    }
        

    public void OnClickButton4RightMenu() {

        if(isRightMenu == true) {
            tweenPosition4RightMenu.PlayForward();
        }else {
            tweenPosition4RightMenu.PlayReverse();
        }

        isRightMenu = !isRightMenu;

    }
    

    public void OnClickButton4LeftMenu() {

        if(isLeftMenu == true) {
            tweenPosition4LeftMenu.PlayForward();
        }else {
            tweenPosition4LeftMenu.PlayReverse();
        }

        isLeftMenu = !isLeftMenu;

    }


    public void OnClickButton4BottomMenu() {

        if(isBottomMenu == true) {
            tweenPosition4BottomMenu.PlayForward();
        }else {
            tweenPosition4BottomMenu.PlayReverse();
        }

        isBottomMenu = !isBottomMenu;

    }


    public void OnClickButtton4Play_Pause(UIToggle toggle) {
        
        if(toggle.value == true) {
            OnClickButton4Load();
        }else {
            OnClickButton4Pause();
        }
    }
}
