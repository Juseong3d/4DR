using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uisoTITLE : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnClickButton4Title() {

        Appmain.appdoc.setGameStatus(GAME_STATUS.GS_MENU);

    }
}
