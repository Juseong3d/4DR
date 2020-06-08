//using NUnit.Framework;
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
    
    bool isRight;
    bool isLeft;

    bool isLeftTime;
    bool isRightTime;

    float _cameraY;
    float _cameraRotationSpeed;

    
	public int _idx;

    public Transform beforeParent;

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

    }

    private void onDragFinished() {

        int _change = (int)((double)_info.duration * slider.value);
                
        scrMedia.SeekTo(_change);
        
    }

    // Update is called once per frame
    void FixedUpdate() {

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
        
        if(_info.duration != 0) {
            
            float _value = (float)_info.time / (float)_info.duration;
            slider.value = _value;

            if(isPressRightCamera == false && isPressLeftCamera == false) {
                _cameraY = _info.channel * _cameraRotationSpeed;
                Appmain.appui.mainCamera3D.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, _cameraY, 0.0f));
            }
        }
    }   


    public void OnClickButton4Player(GameObject obj, bool isPress) {

        if(isPress == true) {
            float _x = Input.mousePosition.x - (Screen.width / 2);
			float _y = Input.mousePosition.y - (Screen.height / 2);

			//Debug.Log("_x :: " + _x + "/" + _y);
			//for testing...
			string[] _effects_path = {
					"Common/_Default_Effect/Magic fire 0",
					"Common/_Default_Effect/Magic fire 1",
					"Common/_Default_Effect/Magic fire 2",
					"Common/_Default_Effect/Magic fire 3",
					"Common/_Default_Effect/Magic fire pro blue",	//4

					"Common/_Default_Effect/Magic fire pro green",
					"Common/_Default_Effect/Magic fire pro orange",
					"Common/_Default_Effect/Magic fire pro red",
					"Common/_Default_Effect/Magic fire pro yellow",
					"Common/_Default_Effect/pfb_Effect_Touch",	//9

					"Common/_Default_Effect/RotatorPS1",
					"Common/_Default_Effect/RotatorPS2",
					"Common/_Default_Effect_2/star2",
				};

			_idx = 9;

			RaycastHit _hit;

			Ray _ray = Appmain.appui.mainCamera3D.ScreenPointToRay(Input.mousePosition);

			if(Physics.Raycast(_ray, out _hit)) {
				GameObject prefab = Appimg.LoadResource4Prefab(_effects_path[_idx]);
				prefab.transform.SetParent(Appmain.appui.transform);
				
				prefab.transform.localPosition = new Vector3(_x, _y, -100.0f);
				prefab.transform.localScale = new Vector3(150, 150, 150);
			}

			//GameObject prefab = Appimg.LoadResource4Prefab(_effects_path[_idx]);
			
			//prefab.transform.SetParent(Appmain.appui.transform);
			//prefab.transform.localPosition = new Vector3(_x, _y, -100.0f);
			//prefab.transform.localScale = new Vector3(150, 150, 150);

        }

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

        //Debug.Log("OnClickButton4Left() :: " + _how);
        scrMedia.Left(_how);       
        //AppUI.mainCamera.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, (float)_info.channel * _cameraRotationSpeed, 0.0f));
    }


    public void OnClickButton4Right(bool _how) {

        //Debug.Log("OnClickButton4Right() :: " + _how);
        scrMedia.Right(_how);
        //AppUI.mainCamera.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, (float)_info.channel * _cameraRotationSpeed, 0.0f));
    }

    void OnClickButton4Left_Camera(GameObject go, bool press) {

        isPressLeftCamera = press;
        
        if(press == true) {
            float now = Appmain.appui.mainCamera3D.transform.localRotation.y;
            
            Appmain.appui.mainCamera3D.transform.localPosition = new Vector3(0, 0, 0);
            Appmain.appui.mainCamera3D.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, _cameraY, 0.0f));
            //AppUI.mainCamera.transform.localEulerAngles += new Vector3(0.0f, _cameraY, 0.0f);

            _cameraY -= Time.deltaTime * (_cameraRotationSpeed * 4);
        }

    }

    void OnClickButton4Right_Camera(GameObject go, bool press) {

        isPressRightCamera = press;

        if(press == true) {
            float now = Appmain.appui.mainCamera3D.transform.localRotation.y;

            Appmain.appui.mainCamera3D.transform.localPosition = new Vector3(0, 0, 0);
            Appmain.appui.mainCamera3D.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, _cameraY, 0.0f));
            //AppUI.mainCamera.transform.localEulerAngles += new Vector3(0.0f, _cameraY, 0.0f);

            _cameraY += Time.deltaTime * (_cameraRotationSpeed * 4);
        }
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

}
