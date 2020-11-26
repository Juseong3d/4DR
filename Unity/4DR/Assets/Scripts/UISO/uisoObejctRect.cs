using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uisoObejctRect : MonoBehaviour
{

    public UISprite spriteMain;
    public UISprite spriteTeam;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SET_RECT(int[] info) {

        spriteMain.transform.localPosition = new Vector3(info[0], -info[1], 0);

        spriteMain.width = info[2];
        spriteMain.height = info[3];

    }
}
