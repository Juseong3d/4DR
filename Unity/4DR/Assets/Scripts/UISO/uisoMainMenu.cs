using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uisoMainMenu : MonoBehaviour
{
    public UIGrid _gridMain;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnClickButton4Reflash() {

        Appmain.appnet.__WEB_CONNECT_AND_SEND_RECV_4_FAST_JSON(string.Empty);        

    }
}
