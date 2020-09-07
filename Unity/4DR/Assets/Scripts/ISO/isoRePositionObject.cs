using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isoRePositionObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        tweenPosiiton.from = start.localPosition;

        int rand = Random.Range(0, 2);

        Debug.Log("rand : " + rand);

        if(rand == 0) {
            tweenPosiiton.to = end.localPosition;
        }else {
            tweenPosiiton.to = end2.localPosition;
        }

        tweenPosiiton.PlayForward();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //GameObject
    //transform
    //transform
    //Speed

    //도착 후에 RigidBody
    public Transform start;
    public Transform end;
    public Transform end2;
    public TweenPosition tweenPosiiton;

    public BoxCollider2D boxcollider;

    public void OnFinished() {

        
        boxcollider.gameObject.SetActive(true);
        boxcollider.enabled = true;

    }


}
