//using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class isoFdPlayerCtl : MonoBehaviour {
        
    public MediaPlayerCtrl scrMedia;

    public AppandroidCallback4FDPlayer _info;

    public UIButton buttonTL;
    public UIButton buttonTR;

    public UISlider slider;
    
    bool isRight;
    bool isLeft;

    bool isLeftTime;
    bool isRightTime;

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
        
        if(Appmain.appevent.isButtonDown == false && _info.duration != 0) {
            
            float _value = (float)_info.time / (float)_info.duration;
            slider.value = _value;

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

    }


    public void OnClickButton4Right(bool _how) {

        //Debug.Log("OnClickButton4Right() :: " + _how);
        scrMedia.Right(_how);

    }


    public void OnClickButton4Load() {

        //Debug.Log("OnClickButton4Load()");
        scrMedia.Play();
        buttonTL.isEnabled = false;
        buttonTR.isEnabled = false;

    }


    public void OnClickButton4Pause() {

        scrMedia.Pause();
        buttonTL.isEnabled = true;
        buttonTR.isEnabled = true;

    }

}
