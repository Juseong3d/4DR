using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uisoObejctRect : MonoBehaviour
{

    public UISprite spriteMain;
    public UISprite spriteTeam;

    public LineRenderer lineRenderer;

    public UILabel labelName;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SET_LABEL(string _value) {

        labelName.text = _value;

    }

    public void SET_RECT(int[] info) {

        spriteMain.transform.localPosition = new Vector3(info[0], -info[1], 0);

        spriteMain.width = info[2];
        spriteMain.height = info[3];

    }


    public void SET_LINE(int index, int[] _tr) {

        int x = _tr[0] + (_tr[2] >> 1);
        int y = _tr[1] + _tr[3];

        lineRenderer.SetPosition(index, new Vector3(x, -y, 0));

    }
}
