using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isoCameraZoom : MonoBehaviour
{
    
    Vector3 touchStart;
    public float zoomOutMin = 0.1f;
    public float zoomOutMax = 8.0f;

    public int doubleTouchCnt = 0;

    public AppandroidCallback4FDPlayer _callback;

    public Vector3 startPoint;
    public Vector3 distPoint;
    public Vector3 nowPoint;

    public int oldDist;

    private void Start() {

        //zoomOutMin = 0.1f;
        //zoomOutMax = 8.0f;
        _callback = FindObjectOfType<AppandroidCallback4FDPlayer>();
        
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
            startPoint = getInputMouse(Input.mousePosition);
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
            }

            doubleTouchCnt ++;
        }else if (Input.GetMouseButton(0)) {

            if (doubleTouchCnt == 0) {
                Vector3 direction = (touchStart - Appmain.appui.mainCamera3D.ScreenToWorldPoint(Input.mousePosition)); 

                Appmain.appui.mainCamera3D.transform.position += direction;
            }

        }
              

        if(Input.GetAxis("HorizontalTurn") > 0.5f) {
            float _value = Input.GetAxis("HorizontalTurn") * Time.deltaTime;

            Appmain.appui.mainCamera3D.transform.position += new Vector3(_value, 0, 0);
        }else if(Input.GetAxis("HorizontalTurn") < -0.5f) {
            float _value = Input.GetAxis("HorizontalTurn") * Time.deltaTime;

            Appmain.appui.mainCamera3D.transform.position += new Vector3(_value, 0, 0);
        }

        if(Input.GetAxis("VerticalTurn") > 0.5f) {
            float _value = Input.GetAxis("VerticalTurn") * Time.deltaTime;

            Appmain.appui.mainCamera3D.transform.position += new Vector3(0, _value, 0);
        }else if(Input.GetAxis("VerticalTurn") < -0.5f) {
            float _value = Input.GetAxis("VerticalTurn") * Time.deltaTime;

            Appmain.appui.mainCamera3D.transform.position += new Vector3(0, _value, 0);
        } 

        zoom(Input.GetAxis("Mouse ScrollWheel"));

        if(Input.GetAxis("Vertical") > 0.5f) {
            zoom((-Input.GetAxis("Vertical")) * 0.01f);
        }else  if(Input.GetAxis("Vertical") < -0.5f) {
            zoom((-Input.GetAxis("Vertical")) * 0.01f);
        }

        if(Input.GetKeyDown("joystick button 8")) {
            zoom(-zoomOutMax);
        }

        if(Input.touchCount == 0) {
            doubleTouchCnt = 0;
        }
	}


    void LateUpdate() {
        {
			//if(Input.touchCount <= 1) 
            {
				nowPoint = getInputMouse(Input.mousePosition);
				
				distPoint = nowPoint - startPoint;

				//appmain.startPoint += (appmain.distPoint / 6);
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

            float minX = -((1.0f - (Appmain.appui.mainCamera3D.orthographicSize * 1.0f)) * (1.6f / 0.9f));
            float maxX = ((1.0f - (Appmain.appui.mainCamera3D.orthographicSize * 1.0f)) * (1.6f / 0.9f));

            float minY = -(1.0f - Appmain.appui.mainCamera3D.orthographicSize);
            float maxY = (1.0f - Appmain.appui.mainCamera3D.orthographicSize);

            //Debug.Log("minX : " + minX + "/maxX : " + maxX + "/" + Appmain.appui.mainCamera3D.transform.position.x);

            float _x = Mathf.Clamp(Appmain.appui.mainCamera3D.transform.position.x - Appmain.appui.mainCamera3D.transform.position.z, minX, maxX);
            float _y = Mathf.Clamp(Appmain.appui.mainCamera3D.transform.position.y, minY, maxY);

            Appmain.appui.mainCamera3D.transform.position = new Vector3(_x, _y, 0);

        }
    }


    internal Vector3 getInputMouse(Vector3 inputMouse) {

		
		float perX = (DEFINE.BASE_SCREEN_WIDTH * inputMouse.x) / Screen.width;
		float perY = (DEFINE.BASE_SCREEN_HEIGHT * inputMouse.y) / Screen.height;
		Vector3 rtn = new Vector3(perX, perY, 0.0f);

		return rtn;
	}

}
