using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class uisoMainMenu : MonoBehaviour
{
    public UIGrid _gridMain;
    public UICenterOnChild centerOnChildMain;
    public int maxCnt;

    public int prevCursor;
    public int nowCursor;

    public bool isPressedDPADV;
    public bool isPressedDPADH;

    
    [Header("* Right Select Camera Script ----")]
    public UIGrid _gridRightMain;
    public TweenPosition _tweenPosition;
    public bool isRightmenu;

    public GameObject reflashButtonCursor;

    // Start is called before the first frame update
    void Start()
    {
        prevCursor = 0;
        nowCursor = -2;
        NGUITools.SetActive(reflashButtonCursor, false);

        _gridMain.onReposition += SET_MAX_CNT;

        isPressedDPADV = false;
        isPressedDPADH = false;

        isRightmenu = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void _LateUpdate() {               
                       
        if((Input.GetAxis("DPAD_v") < 0.0f) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            if(isPressedDPADV == false) {
                
                prevCursor = nowCursor;
                nowCursor = Mathf.Clamp(nowCursor - 1, 0, maxCnt);
                SET_CURSOR();
                isPressedDPADV = true;

            }
        }else if((Input.GetAxis("DPAD_v") > 0.0f) || Input.GetKeyDown(KeyCode.RightArrow)) {
            if(isPressedDPADV == false) {
                
                prevCursor = nowCursor;
                nowCursor = Mathf.Clamp(nowCursor + 1, 0, maxCnt);            
                SET_CURSOR();
                isPressedDPADV = true;

            }
        }else if((Input.GetAxis("DPAD_h") < 0.0f) || Input.GetKeyDown(KeyCode.UpArrow)) {
            if(isPressedDPADH == false) {
                
                prevCursor = nowCursor;
                nowCursor = Mathf.Clamp(nowCursor - 3, -1, maxCnt);            
                if(nowCursor == -1) {
                    centerOnChildMain.enabled = false;
                }
                SET_CURSOR();
                isPressedDPADH = true;

            }
        }else if((Input.GetAxis("DPAD_h") > 0.0f) || Input.GetKeyDown(KeyCode.DownArrow)) {
            if(isPressedDPADH == false) {

                prevCursor = nowCursor;
                if(nowCursor < 0) {
                    nowCursor = 0;
                    centerOnChildMain.enabled = true;
                }else {
                    nowCursor = Mathf.Clamp(nowCursor + 3, 0, maxCnt);     
                }
                SET_CURSOR();
                isPressedDPADH = true;

            }
        }

        if(Input.GetAxis("DPAD_v") == 0f) {
            isPressedDPADV = false;
        }

        if(Input.GetAxis("DPAD_h") == 0f) {
            isPressedDPADH = false;
        }

        
        for(int i = 0; i<10; i++) {
            string tmp = string.Format("joystick button {0}", i);            

            if(Input.GetKeyDown(tmp) || Input.GetKeyDown(KeyCode.Return)) {

                //Debug.Log(tmp);

                switch((XOBX_ONE_BUTTON)i) {
                case XOBX_ONE_BUTTON.BUTTON_A:
                    if(nowCursor >= 0) {
                        Transform _now = _gridMain.GetChild(nowCursor);
                        uisoITEM_VIDEO_MINI _mini = _now.GetComponent<uisoITEM_VIDEO_MINI>();

                        _mini.OnClickButton4FullScreen();
                    }

                    if(nowCursor == -1) {
                        Appmain.appimg.imgMainMenu.OnClickButton4Reflash();   
                    }
                    break;
                case XOBX_ONE_BUTTON.BUTTON_X:
                    {
                        OnClcickButton4Back();
                    }
                    break;
                }
            }
        }
        

    }


    public void SET_CURSOR() {

        if(prevCursor >= 0) {
            Transform _now = _gridMain.GetChild(prevCursor);
            uisoITEM_VIDEO_MINI _mini = _now.GetComponent<uisoITEM_VIDEO_MINI>();
            _mini.SET_CURSOR(false);
        }

        if(nowCursor >= 0) {
            Transform _now = _gridMain.GetChild(nowCursor);
            uisoITEM_VIDEO_MINI _mini = _now.GetComponent<uisoITEM_VIDEO_MINI>();
            //UICenterOnChild _center = _gridMain.GetComponentInChildren<UICenterOnChild>();

            if(centerOnChildMain.enabled == true) {                
                centerOnChildMain.CenterOn(_now);
            }
            _mini.SET_CURSOR(true);
        }

        if(nowCursor == -1) {
            SET_CURSOR_REFLASH(true);
        }else {
            SET_CURSOR_REFLASH(false);
        }
    }


    public void SET_CURSOR_REFLASH(bool ishow) {

        NGUITools.SetActive(reflashButtonCursor, ishow);

    }


    public void OnClickButton4Reflash() {
        
        Appmain.appnet.__WEB_CONNECT_AND_SEND_RECV_4_FAST_JSON(NET_WEB_API_CMD.script);

        switch(Appmain.appmain.selectVideoType) {
            case VIDEO_TYPE.WEB_SERVER_LIST:
                Appmain.appnet.__WEB_CONNECT_AND_SEND_RECV_4_FAST_JSON(NET_WEB_API_CMD.video);
                break;
            case VIDEO_TYPE.LOCAL_LIST:
                Appmain.appimg._SET_LOCAL_LIST_GRID();
                break;
        }

        maxCnt = _gridMain.GetChildList().Count - 1;
    }


    public void OnClcickButton4Back() {

        Appmain.appdoc.setGameStatus(GAME_STATUS.GS_TITLE);

    }

    
    private void SET_MAX_CNT() {

        maxCnt = _gridMain.GetChildList().Count - 1;

    }


    public void OnClickButton4RightMenu() {
        
        if(isRightmenu == false) {
            _tweenPosition.PlayForward();
        }else {
            _tweenPosition.PlayReverse();    
        }

        isRightmenu = !isRightmenu;
    }
}
