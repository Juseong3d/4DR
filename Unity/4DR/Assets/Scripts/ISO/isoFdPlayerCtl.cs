//using NUnit.Framework;
using FFmpeg.AutoGen;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class isoFdPlayerCtl : MonoBehaviour {
        
    public MediaPlayerCtrl scrMedia;

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

    public TweenPosition tweenPosition4RightMenu;
    public bool isRightMenu = true;
    public TweenPosition tweenPosition4LeftMenu;
    public bool isLeftMenu = true;

    public UILabel labelEffectName;

    public int lastCallFrame;

    // Start is called before the first frame update
    void Start() {

        _info = FindObjectOfType<AppandroidCallback4FDPlayer>();

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
        _idx = 1;

        _contrlerStatusTime = DEFINE.CONTRLER_STATUS_TIME;

        isLeftMenu = true;
        isRightMenu = true;

        labelEffectName.text = GET_EFFECT_NAME(_idx, true);

    }

    private void onDragFinished() {

        int _change = (int)((double)_info.duration * slider.value);
                
        scrMedia.SeekTo(_change);
        
    }

    // Update is called once per frame
    void Update() {

        //if(isLeftTime == false && isRightTime == false) 
            {
            if(isLeft == true) {
                OnClickButton4Left(false);
            }else if(isRight == true) {
                OnClickButton4Right(false);
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
        
        if(_info.duration != 0) {
            
            float _value = (float)_info.time / (float)_info.duration;
            slider.value = _value;

            if (isPressRightCamera == false && isPressLeftCamera == false) {

                Appmain.appui.mainCamera3D.transform.parent.localRotation = Quaternion.Euler(new Vector3(0.0f, _cameraY, 0.0f));                

                _cameraY = _info.channel * _cameraRotationSpeed;
            }
        }

        //if(_contrlerStatusTime > 0.0f) {
        //    _contrlerStatusTime -= Time.deltaTime;
        //}else if(_contrlerStatusTime == 0.0f) {

        //}else {
        //    _contrlerStatusTime = 0.0f;
        //    tweenCtlPanel.PlayReverse();
        //}

        if(Input.GetAxis("Horizontal") == 1.0f) {
            OnClickButton4Right(false);
        }else if(Input.GetAxis("Horizontal") == -1.0f) {
            OnClickButton4Left(false);
        }

        if(Input.GetAxis("HorizontalTurn") == 1.0f) {
            OnClickButton4Right(true);
        }else if(Input.GetAxis("HorizontalTurn") == -1.0f) {
            OnClickButton4Left(true);
        }

        if(Input.GetKeyDown("joystick button 0")) {
            OnClickButton4Pause();
        }

        if(Input.GetKeyDown("joystick button 1")) {
            OnClickButton4Load();
        }

#if UNITY_EDITOR
        lastCallFrame = -999;
#endif
    }   

    public bool isPressPlayerBackButton;
    public void OnClickButton4Player(GameObject obj, bool isPress) {

        if(isPress == true) {
            float _x = Input.mousePosition.x - (Screen.width / 2);
			float _y = Input.mousePosition.y - (Screen.height / 2);

			//Debug.Log("_x :: " + _x + "/" + _y);
			//for testing...			
            string _path = GET_EFFECT_NAME(_idx);			
            int scaleSize = GET_EFFECT_SCALE_SIZE(_idx);

			RaycastHit _hit;

			Ray _ray = Appmain.appui.mainCamera3D.ScreenPointToRay(Input.mousePosition);

            //Debug.DrawRay(_ray, Vector3.forward);
			if(Physics.Raycast(_ray, out _hit)) {
				GameObject prefab = Appimg.LoadResource4Prefab(_path);
				prefab.transform.SetParent(Appmain.appui.transform);                
                				
				prefab.transform.position = _hit.point;//Appmain.appui.mainCamera3D.WorldToScreenPoint(_hit.point);
				prefab.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);
			}

			//GameObject prefab = Appimg.LoadResource4Prefab(_effects_path[_idx]);
			
			//prefab.transform.SetParent(Appmain.appui.transform);
			//prefab.transform.localPosition = new Vector3(_x, _y, -100.0f);
			//prefab.transform.localScale = new Vector3(150, 150, 150);

            tweenCtlPanel.PlayForward();
            _contrlerStatusTime = DEFINE.CONTRLER_STATUS_TIME;

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
                    "Common/_Default_effect_3/CFX4 Environment Bubbles Denser"

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
                    200,//"Common/_Default_effect_3/CFX4 Environment Bubbles Denser"

				};

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

        if(_info.channel == 0) return;

        if( scrMedia.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING) {
            if(_info.frame == lastCallFrame) return;
        }

       StartCoroutine(_OnClickButton4Left(_how));

    }


    IEnumerator _OnClickButton4Left(bool _how) {

        yield return new WaitForFixedUpdate();

        scrMedia.Left(_how);

#if UNITY_EDITOR
        _info.channel --;
        _info.channel = Mathf.Clamp(_info.channel, 0, 45);
#endif

        lastCallFrame = _info.frame;

    }


    public void OnClickButton4Right(bool _how) {

        if( scrMedia.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING) {
            if(_info.frame == lastCallFrame) return;
        }

        StartCoroutine(_OnClickButton4Right(_how));
    }


    IEnumerator _OnClickButton4Right(bool _how) {

        yield return new WaitForFixedUpdate();
                
        scrMedia.Right(_how);        

#if UNITY_EDITOR
        _info.channel ++;
        _info.channel = Mathf.Clamp(_info.channel, 0, 45);
#endif  
                
        lastCallFrame = _info.frame;
    }


    void OnClickButton4Left_Camera(GameObject go, bool press) {       
        
        if(press == true) {
            float now = Appmain.appui.mainCamera3D.transform.localRotation.y;
            
            //Appmain.appui.mainCamera3D.transform.localPosition = new Vector3(0, 0, 0);
            Appmain.appui.mainCamera3D.transform.parent.localRotation = Quaternion.Euler(new Vector3(0.0f, _cameraY, 0.0f));
            //AppUI.mainCamera.transform.localEulerAngles += new Vector3(0.0f, _cameraY, 0.0f);

            _cameraY -= Time.fixedDeltaTime * (_cameraRotationSpeed * 4);
        }

        isPressLeftCamera = press;

    }

    void OnClickButton4Right_Camera(GameObject go, bool press) {
        
        if(press == true) {
            float now = Appmain.appui.mainCamera3D.transform.localRotation.y;

            //Appmain.appui.mainCamera3D.transform.localPosition = new Vector3(0, 0, 0);
            Appmain.appui.mainCamera3D.transform.parent.localRotation = Quaternion.Euler(new Vector3(0.0f, _cameraY, 0.0f));
            //AppUI.mainCamera.transform.localEulerAngles += new Vector3(0.0f, _cameraY, 0.0f);

            _cameraY += Time.fixedDeltaTime * (_cameraRotationSpeed * 4);
        }

        isPressRightCamera = press;

    }



    public void OnClickButton4Load() {

        //Debug.Log("OnClickButton4Load()");
        scrMedia.Play();
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

        scrMedia.Pause();
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

        Appmain.appimg.mainUIPrefab.SetActive(true);
        NGUITools.Destroy(this.beforeParent.gameObject);
        NGUITools.Destroy(this.transform.gameObject);

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


    public TweenAlpha tweenCtlPanel;



}
