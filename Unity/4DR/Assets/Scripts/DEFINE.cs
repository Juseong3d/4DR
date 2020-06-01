using UnityEngine;
using System.Collections;

public class DEFINE : MonoBehaviour {

	public const int CAMERA_RATIO_W = 16;
	public const int CAMERA_RATIO_H = 9;

	public const float BASE_SCREEN_WIDTH = 1920;
	public const float BASE_SCREEN_HEIGHT = 1080;

	public const string KEY_OPTION_BGM = "KEY_OPTION_BGM";
	public const string KEY_OPTION_EFFECT_SOUND = "KEY_OPTION_EFFECT_SOUND";

	
    public const string POPUP_OK = "OK";
	public const string POPUP_YES = "YES";
	public const string POPUP_NO = "NO";

	public const string POPUP_EXIT_TITLE = "Close";
	public const string POPUP_EXIT_QUSTION = "Want Close?";

	public const int FDPLAYER_PORT = 7070;	
   
}

public enum PROJECT_TYPE {

	NONE = 0,
	DEFAULT = 1,

}

public enum STATUS {

	//기다리는 상태
	//이동상태
	//매치 체크 상태
	//제거 상태
	//채우기 상태		

}


 public enum GAME_STATUS {
        
	GS_NONE = -1,
    GS_START,
    GS_INIT,
    GS_INTRO,
    GS_TITLE,
    GS_MENU    

}


public enum GAME_STATUS_NEXT_STATUS {

	NEXT_START = -5,
	FREE_STATUS,
	LOAD_STATUS,
	INIT_STATUS,
	START_STATUS

}
