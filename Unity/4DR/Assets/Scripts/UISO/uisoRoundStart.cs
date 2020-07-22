using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uisoRoundStart : MonoBehaviour
{

    public UILabel[] labelRoundInfo;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SET_INFO(int _value) {

        for(int i = 0; i<labelRoundInfo.Length; i++) {
            labelRoundInfo[i].text = string.Format("ROUND {0}", _value);
        }
    }

}
