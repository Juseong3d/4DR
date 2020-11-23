using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uisoTITLE : MonoBehaviour
{

    public VIDEO_TYPE nowSelectVideoType;
    public UILabel labelVersion;

    public GameObject[] gameObjectMenu;
    public GameObject gameObjectMenuCursor;
    public TweenPosition tweenPositionR;
    public TweenPosition tweenPosotionL;

    public bool isPressedDPadH;
    // Start is called before the first frame update
    void Start()
    {
        
        nowSelectVideoType = VIDEO_TYPE.NONE;
        gameObjectMenuCursor.SetActive(false);

        labelVersion.text = Appmain.appmain.appInfo.appVersion;

        isPressedDPadH = false;

    }

    // Update is called once per frame
    void Update()
    {
        


        if(Input.GetAxisRaw("DPAD_h") == 1.0f || Input.GetKeyDown(KeyCode.DownArrow)) {
            if(isPressedDPadH == false) {
                if(nowSelectVideoType < VIDEO_TYPE.LOCAL_LIST) {
                    nowSelectVideoType ++;

                    gameObjectMenuCursor.SetActive(true);

                    tweenPosotionL.from.y = gameObjectMenu[(int)nowSelectVideoType].transform.localPosition.y;
                    tweenPosotionL.to.y = gameObjectMenu[(int)nowSelectVideoType].transform.localPosition.y;

                    tweenPositionR.from.y = gameObjectMenu[(int)nowSelectVideoType].transform.localPosition.y;
                    tweenPositionR.to.y = gameObjectMenu[(int)nowSelectVideoType].transform.localPosition.y;
                }
            }

            isPressedDPadH = true;
        }

        if(Input.GetAxisRaw("DPAD_h") == -1.0f || Input.GetKeyDown(KeyCode.UpArrow)) {
            if(isPressedDPadH == false) {
                if(nowSelectVideoType > VIDEO_TYPE.WEB_SERVER_LIST) {
                    nowSelectVideoType --;

                    gameObjectMenuCursor.SetActive(true);

                    tweenPosotionL.from.y = gameObjectMenu[(int)nowSelectVideoType].transform.localPosition.y;
                    tweenPosotionL.to.y = gameObjectMenu[(int)nowSelectVideoType].transform.localPosition.y;

                    tweenPositionR.from.y = gameObjectMenu[(int)nowSelectVideoType].transform.localPosition.y;
                    tweenPositionR.to.y = gameObjectMenu[(int)nowSelectVideoType].transform.localPosition.y;
                }
            }

            isPressedDPadH = true;
        }

        if(Input.GetAxisRaw("DPAD_h") == 0.0f) {
            //Debug.Log("DPAD_H : " + Input.GetAxis("DPAD_h"));
            isPressedDPadH = false;
        }

        
        if(Input.GetAxisRaw("DPAD_v") != 0.0f) {
            //Debug.Log("DPAD_V : " + Input.GetAxis("DPAD_v"));            
        }else if(Input.GetAxis("DPAD_v") == 0.0f) {
            
        }

        for(int i = 0; i<1; i++) {
            string tmp = string.Format("joystick button {0}", i);            

            if(Input.GetKeyDown(tmp) || Input.GetKeyDown(KeyCode.Return)) {

                //Debug.Log(tmp);

                switch((XOBX_ONE_BUTTON)i) {
                    case XOBX_ONE_BUTTON.BUTTON_A:

                        if(nowSelectVideoType != VIDEO_TYPE.NONE) {
                            OnClcikButtonSelectVideoType(gameObjectMenu[(int)nowSelectVideoType]);
                        }
                        break;

                }
            }
        }
    }


    public void OnClickButton4Title() {

        Appmain.appsound.stopBGM();
        Appmain.appsound.playEffect(SOUND_EFFECT_TYPE.Button_Click);
        Appmain.appdoc.setGameStatus(GAME_STATUS.GS_MENU);

    }


    public void OnClcikButtonSelectVideoType(GameObject _obj) {

        Appmain.appmain.selectVideoType = (VIDEO_TYPE)int.Parse(_obj.name);
        OnClickButton4Title();

    }
}
