using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isoCameraZoom : MonoBehaviour
{
    
    Vector3 touchStart;
    public float zoomOutMin = 0.1f;
    public float zoomOutMax = 8.0f;

    public int doubleTouchCnt = 0;

    private void Start() {

        //zoomOutMin = 0.1f;
        //zoomOutMax = 8.0f;
        
    }

    // Update is called once per frame
    void LateUpdate () {
        
        if(Appmain.gameStatus < GAME_STATUS.GS_MENU) 
            return;
        
        if(Appmain.appimg._nowFullCtl == null) return;

        if(Appmain.appimg._nowFullCtl.isPressLeftCamera == true || Appmain.appimg._nowFullCtl.isPressRightCamera == true) {
            return;
        }

        if(Input.GetMouseButtonDown(0)){
            touchStart = Appmain.appui.mainCamera3D.ScreenToWorldPoint(Input.mousePosition);
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
        }else if(Input.GetMouseButton(0)){

            if(doubleTouchCnt == 0) {
                Vector3 direction = (touchStart - Appmain.appui.mainCamera3D.ScreenToWorldPoint(Input.mousePosition)) * ((1.0f - Appmain.appui.mainCamera3D.orthographicSize) + 1.0f);
                Appmain.appui.mainCamera3D.transform.position += direction;

                float minX = -((1.0f - (Appmain.appui.mainCamera3D.orthographicSize * 1.0f)) * (1.6f / 0.9f));
                float maxX = ((1.0f - (Appmain.appui.mainCamera3D.orthographicSize * 1.0f)) * (1.6f / 0.9f));

                float minY = -(1.0f - Appmain.appui.mainCamera3D.orthographicSize);
                float maxY = (1.0f - Appmain.appui.mainCamera3D.orthographicSize);

                float _x = Mathf.Clamp(Appmain.appui.mainCamera3D.transform.position.x, minX, maxX);
                float _y = Mathf.Clamp(Appmain.appui.mainCamera3D.transform.position.y, minY, maxY);

                Appmain.appui.mainCamera3D.transform.position = new Vector3(_x, _y, 0);
            }
        }
        zoom(Input.GetAxis("Mouse ScrollWheel"));

        if(Input.touchCount == 0) {
            doubleTouchCnt = 0;
        }
	}

    void zoom(float increment){

        if(increment != 0) {
            Appmain.appui.mainCamera3D.orthographicSize = Mathf.Clamp(Appmain.appui.mainCamera3D.orthographicSize - increment, zoomOutMin, zoomOutMax);

            float minX = -((1.0f - (Appmain.appui.mainCamera3D.orthographicSize * 1.0f)) * (1.6f / 0.9f));
            float maxX = ((1.0f - (Appmain.appui.mainCamera3D.orthographicSize * 1.0f)) * (1.6f / 0.9f));

            float minY = -(1.0f - Appmain.appui.mainCamera3D.orthographicSize);
            float maxY = (1.0f - Appmain.appui.mainCamera3D.orthographicSize);

            float _x = Mathf.Clamp(Appmain.appui.mainCamera3D.transform.position.x, minX, maxX);
            float _y = Mathf.Clamp(Appmain.appui.mainCamera3D.transform.position.y, minY, maxY);
            
            Appmain.appui.mainCamera3D.transform.position = new Vector3(_x, _y, 0);

        }
    }

}
