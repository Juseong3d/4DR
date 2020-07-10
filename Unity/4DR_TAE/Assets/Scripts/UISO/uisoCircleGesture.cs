using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uisoCircleGesture : MonoBehaviour {

    public Transform target;
    public Vector3 targetStart;
    public float previous;
    public float current;

    public UISprite sprite;
    public UILabel label;

    private void Update() {

        //if(target == null) return;

        CircleGestureRotate();
    }


    private void CircleGestureRotate() {

        if (Input.GetMouseButtonDown(0)) {
            previous = 0;
            current = 0;

            targetStart = Appmain.appui.mainCamera3D.ScreenToWorldPoint(Input.mousePosition);
        }
 
        if (Input.GetMouseButton(0)) {
            previous = current;
            
            if(target != null) {
                current = GetAngle(target.position, Appmain.appui.mainCamera3D.ScreenToWorldPoint(Input.mousePosition));
            }
            
            {
                current = GetAngle(targetStart, Appmain.appui.mainCamera3D.ScreenToWorldPoint(Input.mousePosition));
            }

            if (previous != 0.0f) {

                float diffrence = current - previous;

                if(target != null) {
                    target.Rotate(0, 0, diffrence);                
                }

                if(sprite != null) {
                    
                    float _fill = Mathf.Clamp(current / 360.0f, 0.1f, 1.0f);
                    sprite.fillAmount = _fill;
                    label.text = string.Format("Speed\n{0:0.0}", _fill);

                }
            }
        }
    }
 
    public float GetAngle(Vector3 vStart, Vector3 vEnd) {

        Vector3 v = vEnd - vStart;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        return angle;
    }

}
