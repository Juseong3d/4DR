using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isoNGUIJoystick : MonoBehaviour
{
    public UISlider _sider;

    // Start is called before the first frame update
    void Awake()
    {
        _sider = GetComponent<UISlider>();
        _sider.onDragFinished += OnReleaseButton;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPress() {

        Debug.Log("OnPress()");

    }


    public void OnReleaseButton() {

        Debug.Log("OnReleaseButton()");
        _sider.value = 0.5f;

    }
}
