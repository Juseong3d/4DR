using UnityEngine;
using System.Collections;

public class DEFINE : MonoBehaviour {

	public const int CAMERA_RATIO_W = 16;
	public const int CAMERA_RATIO_H = 9;

	public const float BASE_SCREEN_WIDTH = 1920;
	public const float BASE_SCREEN_HEIGHT = 1080;

	public const float DOUBLE_TOUCH_TIME = 1.0f;

	public const string KEY_OPTION_BGM = "KEY_OPTION_BGM";
	public const string KEY_OPTION_EFFECT_SOUND = "KEY_OPTION_EFFECT_SOUND";


    public const string POPUP_OK = "OK";
	public const string POPUP_YES = "YES";
	public const string POPUP_NO = "NO";

	public const string POPUP_EXIT_TITLE = "Close";
	public const string POPUP_EXIT_QUSTION = "Are you sure you want to exit 4D Taekwondo?";//"Want Close?";

	public const int FDPLAYER_PORT = 7070;
	public const float CONTRLER_STATUS_TIME = 5.0f;

	//public const string DIRECT_TEST_URL = "rtsp://app.4dlive.com/mbc_test01.4ds?type=vod";
	public const string DIRECT_TEST_URL = "rtsp://192.168.18.152:8554/main";

	public const string LOCAL_FOLDER_PATH = "/4DLocal";

	public const string VOD_TYPE_NOT_USE = "not_used";

	public const string CATAGORI_TAE = "태권도";
	
	public const string THUMNAIL_IMG_DOWNLOAD_SERVER_URL = "https://unity.4dreplay.io:50443";

	public const float MAX_MINUS_GAME_SCORE = 100;
	public const float MAX_PENALTY_TIME = 10;

	public const float COMMAND_REFLASH_TIME = 0.5f;
	public const int COMMANDER_CUT_TIME = 15;

	public const float PLAYER_CONTROLLER_TIME = 3f;

	public const float LIVE_BACK_SEEK = 10f;

	static public string GET_LOCAL_FOLDER_PATH() {

		return Application.persistentDataPath + "" + "/4DLocal/";

	}
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


public enum XOBX_ONE_BUTTON {

	BUTTON_A = 0,
	BUTTON_B = 1,
	BUTTON_X = 2,
	BUTTON_Y = 3,
	LB = 4,
	RB = 5,
	RS_B = 9

}


public enum NVIDIA_REMOTE_CONTROL {

	POWER = 26,
	MENU = 82,
	UP = 19,
	LEFT = 21,
	OK = 23,
	RIGHT = 22,
	DOWN = 20,
	BACK = 4,
	HONE = 3,
	FOWARD = 90,
	MIC = 219,
	PLAY_PAUSE = 85,
	VOLLUM_UP = 24,
	REWARD = 89,
	VOLLUM_DOWN = 25,
	NETFLIX = 199

}


public enum VIDEO_TYPE {

	NONE = -1,
	WEB_SERVER_LIST,
	DIRECT_URL,
	LOCAL_LIST

}


public enum RECV_TYPE {

	NONE = -1,
	TABLE,
	LIST,
	CAMERA_SCRIPT

}


public enum TABLE_LIST {

	NONE = -1,
	tb_country_code,
	tb_effect_list,
	tb_effect_table,
	tb_Game_list,
	tb_player_list

}


public enum CATEGORY_KEY {

	none = 0,
	TAE

}


public enum VIDEO_CONTENT_TYPE {

	none = 0,
	not_used,
	vod,
	live,
	local

}


public enum SETTING {

	EFFECT,
	AUTO_ZOOM,
	CAMERA_ROTATION,
	TIME_MACHINE

}




