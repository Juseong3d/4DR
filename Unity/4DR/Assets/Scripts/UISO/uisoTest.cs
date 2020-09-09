using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uisoTest : MonoBehaviour
{

    public TweenPosition tweenPosiiton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnFfff() {

        tweenPosiiton.ResetToBeginning();
        tweenPosiiton.PlayForward();
    }
}
