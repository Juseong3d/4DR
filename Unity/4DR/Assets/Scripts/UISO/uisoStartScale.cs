using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uisoStartScale : MonoBehaviour
{

    public TweenScale tsStart;
    public TweenAlpha taStart;
    public TweenPosition tpStart;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TWEEN_START() {

        if(tsStart != null) tsStart.PlayForward();
        if(taStart != null) taStart.PlayForward();
        if(tpStart != null) tpStart.PlayForward();

    }


    public void TWEEN_REVERSE() {

        if(tsStart != null) tsStart.PlayReverse();
        if(taStart != null) taStart.PlayReverse();
        if(tpStart != null) tpStart.PlayReverse();

    }

}
