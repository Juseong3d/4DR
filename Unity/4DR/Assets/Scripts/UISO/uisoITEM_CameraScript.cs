using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uisoITEM_CameraScript : MonoBehaviour
{

    public UIToggle _toggle;
    public UILabel _label;

    public LIST_SCRIPT_LIST_ITEM _info;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SET_LABEL(string _value) {

        Debug.Log("_value :: " + _value);
        _label.text = _value;

    }
}
