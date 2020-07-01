using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uisoMainMenu : MonoBehaviour
{
    public UIGrid _gridMain;

    public int maxCnt;

    public int prevCursor;
    public int nowCursor;

    public bool isPressedDPADV;
    public bool isPressedDPADH;

    // Start is called before the first frame update
    void Start()
    {
        prevCursor = 0;
        nowCursor = -1;
        
        _gridMain.onReposition += SET_MAX_CNT;

        isPressedDPADV = false;
        isPressedDPADH = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void LateUpdate() {               
                       
        if((Input.GetAxis("DPAD_v") < 0.0f) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            if(isPressedDPADV == false) {
                
                prevCursor = nowCursor;
                nowCursor = Mathf.Clamp(nowCursor - 2, 0, maxCnt);
                SET_CURSOR();
                isPressedDPADV = true;

            }
        }else if((Input.GetAxis("DPAD_v") > 0.0f) || Input.GetKeyDown(KeyCode.RightArrow)) {
            if(isPressedDPADV == false) {
                
                prevCursor = nowCursor;
                nowCursor = Mathf.Clamp(nowCursor + 2, 0, maxCnt);            
                SET_CURSOR();
                isPressedDPADV = true;

            }
        }else if((Input.GetAxis("DPAD_h") < 0.0f) || Input.GetKeyDown(KeyCode.UpArrow)) {
            if(isPressedDPADH == false) {
                
                prevCursor = nowCursor;
                nowCursor = Mathf.Clamp(nowCursor - 1, 0, maxCnt);            
                SET_CURSOR();
                isPressedDPADH = true;

            }
        }else if((Input.GetAxis("DPAD_h") > 0.0f) || Input.GetKeyDown(KeyCode.DownArrow)) {
            if(isPressedDPADH == false) {

                prevCursor = nowCursor;
                nowCursor = Mathf.Clamp(nowCursor + 1, 0, maxCnt);     
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
            UICenterOnChild _center = _gridMain.GetComponentInChildren<UICenterOnChild>();

            _center.CenterOn(_now);
            _mini.SET_CURSOR(true);
        }

    }


    public void OnClickButton4Reflash() {

        switch(Appmain.appmain.selectVideoType) {
            case VIDEO_TYPE.WEB_SERVER_LIST:
                Appmain.appnet.__WEB_CONNECT_AND_SEND_RECV_4_FAST_JSON(string.Empty);        
                break;
            case VIDEO_TYPE.LOCAL_LIST:
                Appmain.appimg._SET_LOCAL_LIST_GRID();
                break;
        }

        maxCnt = _gridMain.GetChildList().size - 1;
    }


    public void OnClcickButton4Back() {

        Appmain.appdoc.setGameStatus(GAME_STATUS.GS_TITLE);

    }

    
    private void SET_MAX_CNT() {

        maxCnt = _gridMain.GetChildList().size - 1;

    }
}
