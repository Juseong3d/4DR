using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isoCameraZoom : MonoBehaviour
{
    
    Vector3 touchStart;
    public float zoomOutMin;
    public float zoomOutMax;

    public int doubleTouchCnt;

    public AppandroidCallback4FDPlayer _callback;

    public isoFdPlayerCtl _fdPlayerCTLUI;
    public MediaPlayerCtrl _mediaMain;

    public Vector3 prevStartPoint;
    public Vector3 startPoint;
    public Vector3 distPoint;
    public Vector3 nowPoint;

    public Vector3 _distDouble;

    public int oldDist;

    public Vector3 _target;
    public bool isDoubleTouch;

    public float lastTouchTime;

    isoShakeCamera camerashake;

    private void Start() {

        if(Appmain.appui != null) {
            camerashake = Appmain.appui.mainCamera3D.GetComponent<isoShakeCamera>();
        }
        _callback = FindObjectOfType<AppandroidCallback4FDPlayer>();
        isDoubleTouch = false;
        lastTouchTime = 0.0f;
    }

    public void INIT_CAMERA() {
        zoom(-zoomOutMax);
    }

    // Update is called once per frame
    void FixedUpdate () {
        
        if(Appmain.gameStatus < GAME_STATUS.GS_MENU) 
            return;
        
        if(Appmain.appimg._nowFullCtl == null) return;

        if(Appmain.appimg._nowFullCtl.isPressLeftCamera == true || Appmain.appimg._nowFullCtl.isPressRightCamera == true) {
            doubleTouchCnt ++;
            return;
        }

        if(Appmain.appimg._nowFullCtl.isRight == true || Appmain.appimg._nowFullCtl.isLeft == true) {
            doubleTouchCnt ++;
            return;
        }

        if(Appmain.appimg._nowFullCtl.isLeftTime == true || Appmain.appimg._nowFullCtl.isRightTime == true) {
            doubleTouchCnt ++;
            return;
        }

        //transform.LookAt(Appmain.appimg._nowFullVideo.transform);

        if(Input.GetMouseButtonDown(0)){
            touchStart = Appmain.appui.mainCamera3D.ScreenToWorldPoint(Input.mousePosition);
            prevStartPoint = startPoint;
            startPoint = getInputMouse(Input.mousePosition);

            _target = Appmain.appui.mainCamera3D.ScreenToWorldPoint(Input.mousePosition);

            _distDouble = prevStartPoint - startPoint;

            if(Mathf.Abs(_distDouble.x) < 20.0f && Mathf.Abs(_distDouble.x) < 20.0f) {
                isDoubleTouch = true;
            }

            lastTouchTime = DEFINE.DOUBLE_TOUCH_TIME;
        }
        if(Input.touchCount == 2){
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            //Vector3 direction = touchStart - Appmain.appui.mainCamera3D.ScreenToWorldPoint(Input.mousePosition);
            //Appmain.appui.mainCamera3D.transform.position += direction * (Appmain.appui.mainCamera3D.orthographicSize + 1.0f);
            if(touchZero.phase == TouchPhase.Moved || touchOne.phase == TouchPhase.Moved) {
                zoom(difference * 0.001f);

#if _TAE_
                Vector3 direction = (touchStart - Appmain.appui.mainCamera3D.ScreenToWorldPoint(Input.mousePosition));                

                Appmain.appui.mainCamera3D.transform.position += direction;
#endif

            }

            doubleTouchCnt ++;


        }else if (Input.GetMouseButton(0)) {

#if _TAE_
            Vector3 direction = (touchStart - Appmain.appui.mainCamera3D.ScreenToWorldPoint(Input.mousePosition)); 

            //Debug.Log("direction.x :: " + direction.x);
            if(Mathf.Abs(direction.x) > 0.05f) {

                bool isPlaying = (_mediaMain.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING);

                if(direction.x > 0) {
                    _fdPlayerCTLUI.OnClickButton4Left(!isPlaying);
                }else if(direction.x < 0) {
                    _fdPlayerCTLUI.OnClickButton4Right(!isPlaying);
                }
            }
            
#else
            if (doubleTouchCnt == 0) {
                Vector3 direction = (touchStart - Appmain.appui.mainCamera3D.ScreenToWorldPoint(Input.mousePosition)); 

                //Debug.Log("one touch :: " + direction);

                Appmain.appui.mainCamera3D.transform.position += direction;
            }

#endif

        }else if(Input.GetMouseButtonUp(0)) {
#if _TAE_
            //RaycastHit _hit;
            RaycastHit[] hithit;

			Ray _ray = Appmain.appui.mainCamera3D.ScreenPointToRay(Input.mousePosition);

            //Debug.Log("Input.mousePosition :: " + Input.mousePosition);

            hithit = Physics.RaycastAll(_ray);

            //for(int i = 0; i<hithit.Length; i++) {

            //    Debug.Log(i + " #### " + hithit[i].collider.gameObject.name);

            //}

            //Debug.DrawRay(_ray, Vector3.forward);
			//if(Physics.Raycast(_ray, out _hit)) 
            if(hithit.Length <= 1) {

                Vector3 direction = (touchStart - Appmain.appui.mainCamera3D.ScreenToWorldPoint(Input.mousePosition)); 

                if(Mathf.Abs(direction.x) < 0.05f && Mathf.Abs(direction.y) < 0.05f) {
                    if(_mediaMain.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING) {
                        _mediaMain.Pause();
                    }else {
                        _mediaMain.Play();
                    }                
                }
            }
#endif
        }
              
        float htValue = Input.GetAxis("HorizontalTurn");

        if(Input.GetAxis("axis14") != 0) {
            htValue = Input.GetAxis("axis14");
        }

        if(htValue > 0.5f) {
            
            float _r = 1f;

            if(_callback.isZoomMoveR != true)
                _r = -1f;

            float _value = (htValue) * Time.deltaTime * _r;

            Appmain.appui.mainCamera3D.transform.position += new Vector3(_value, 0, 0);
        }else if(htValue < -0.5f) {
            float _r = 1f;

            if(_callback.isZoomMoveR != true)
                _r = -1f;

            float _value = (htValue) * Time.deltaTime * _r;

            Appmain.appui.mainCamera3D.transform.position += new Vector3(_value, 0, 0);
        }

        float vtValue = Input.GetAxis("VerticalTurn");
        
        if(Input.GetAxis("axis15") != 0) {
            vtValue = Input.GetAxis("axis15");
        }

        if(vtValue > 0.5f) {
            float _value = vtValue * Time.deltaTime;

            Appmain.appui.mainCamera3D.transform.position += new Vector3(0, _value, 0);
        }else if(vtValue < -0.5f) {
            float _value = vtValue * Time.deltaTime;

            Appmain.appui.mainCamera3D.transform.position += new Vector3(0, _value, 0);
        }

//#if _DOUBLE_TOUCH_
//        if (Mathf.Abs(distPoint.x) > 10.0f || Mathf.Abs(distPoint.y) > 10.0f || Mathf.Abs(distPoint.z) > 10.0f) {
//            _target = new Vector3();
//        }

//        if (isDoubleTouch == true) {
//            if ((_target.x != 0f) || (_target.y != 0f) || Appmain.appui.mainCamera3D.orthographicSize > 0.5f) {
//                Vector3 _tmpTarget = new Vector3(
//                        Mathf.Lerp(Appmain.appui.mainCamera3D.transform.position.x, _target.x, Time.deltaTime * 5f),
//                        Mathf.Lerp(Appmain.appui.mainCamera3D.transform.position.y, _target.y, Time.deltaTime * 5f),
//                        Mathf.Lerp(Appmain.appui.mainCamera3D.transform.position.z, _target.z, Time.deltaTime * 5f)
//                    );

//                Appmain.appui.mainCamera3D.transform.position = _tmpTarget;
//                zoom(0.01f);

//                Vector3 ___ddddd = Appmain.appui.mainCamera3D.transform.position - _tmpTarget;

//                if (Mathf.Abs(___ddddd.x) < 0.01f && Mathf.Abs(___ddddd.y) < 0.01f && Appmain.appui.mainCamera3D.orthographicSize <= 0.5f) {
//                    _target = new Vector3();
//                    isDoubleTouch = false;
//                }
//            }
//        }
//#endif

        zoom(Input.GetAxis("Mouse ScrollWheel"));

        //if(Input.GetAxis("Horizontal") < 0.5f && Input.GetAxis("Horizontal") > -0.5f) 
            {
            if(Input.GetAxis("Vertical") > 0.5f) {
                zoom((-Input.GetAxis("Vertical")) * 0.01f);
            }else  if(Input.GetAxis("Vertical") < -0.5f) {
                zoom((-Input.GetAxis("Vertical")) * 0.01f);
            }
        }

        if(Input.GetKeyDown("joystick button 8")) {
            zoom(-zoomOutMax);
        }

        if(Input.touchCount == 0) {
            doubleTouchCnt = 0;            
        }

        if(lastTouchTime > 0f) {
            lastTouchTime -= Time.deltaTime;
        }else {
            lastTouchTime = 0f;
            _distDouble = new Vector3();            
            nowPoint = new Vector3();
            startPoint = new Vector3();
            prevStartPoint = new Vector3(999999, 999999);
        }

	}


    void LateUpdate() {
        {
			if(Input.GetMouseButton(0))
            {
				nowPoint = getInputMouse(Input.mousePosition);
				
				distPoint = nowPoint - startPoint;

				//appmain.startPoint += (appmain.distPoint / 6);
			}else if(Input.GetMouseButtonUp(0)) {
                distPoint = new Vector3();
            }
		}
    }


    void zoom(float increment){

        //if(increment != 0) 
        {
            Appmain.appui.mainCamera3D.orthographicSize = Mathf.Clamp(Appmain.appui.mainCamera3D.orthographicSize - increment, zoomOutMin, zoomOutMax);

            //float height = 2f * Appmain.appui.mainCamera3D.orthographicSize;
            //float width = (height * Appmain.appui.mainCamera3D.aspect) / 2;

            //float minX = -width;
            //float maxX = width;

            //float minY = -(1.0f - Appmain.appui.mainCamera3D.orthographicSize);
            //float maxY = (1.0f - Appmain.appui.mainCamera3D.orthographicSize);

            //float _x = Mathf.Clamp(Appmain.appui.mainCamera3D.transform.position.x, minX, maxX);
            //float _y = Mathf.Clamp(Appmain.appui.mainCamera3D.transform.position.y, minY, maxY);

            //Appmain.appui.mainCamera3D.transform.position = new Vector3(_x, _y, 0);
            if(camerashake != null) { 
                if(camerashake.leftright == true || camerashake.updown == true) return;
            }
                    
            {
                float minX = -((1.0f - (Appmain.appui.mainCamera3D.orthographicSize * 1.0f)) * (1.6f / 0.9f));
                float maxX = ((1.0f - (Appmain.appui.mainCamera3D.orthographicSize * 1.0f)) * (1.6f / 0.9f));

                float minY = -(1.0f - Appmain.appui.mainCamera3D.orthographicSize);
                float maxY = (1.0f - Appmain.appui.mainCamera3D.orthographicSize);

                //Debug.Log("minX : " + minX + "/maxX : " + maxX + "/" + Appmain.appui.mainCamera3D.transform.position.x);

                float _x = Mathf.Clamp(Appmain.appui.mainCamera3D.transform.position.x, minX, maxX);
                float _y = Mathf.Clamp(Appmain.appui.mainCamera3D.transform.position.y, minY, maxY);

                Appmain.appui.mainCamera3D.transform.position = new Vector3(_x, _y, 0);
            }
        }
    }


    internal Vector3 getInputMouse(Vector3 inputMouse) {
        		
		float perX = (DEFINE.BASE_SCREEN_WIDTH * inputMouse.x) / Screen.width;
		float perY = (DEFINE.BASE_SCREEN_HEIGHT * inputMouse.y) / Screen.height;
		Vector3 rtn = new Vector3(perX, perY, 0.0f);

		return rtn;
	}


    internal void SET_DEFAULT() {

        zoom(-zoomOutMax);

    }

}
