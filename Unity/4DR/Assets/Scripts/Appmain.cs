using UnityEngine;
using System.Collections;

public class Appmain : MonoBehaviour {

	public static Appmain appmain;
	public static AppUI appui;
	public static Appdoc appdoc;
	public static Appsound appsound;
	public static Appevent appevent;
	public static Appclass appclass;

	public static Appimg appimg;

	public static Appnet appnet;

	public static GameObject MAIN_GAMEOBJECT;

	public static GAME_STATUS nextGameStatus;
	public static GAME_STATUS prevGameStatus;
	public static GAME_STATUS gameStatus;
	internal static int havePopupCnt;

	public APP_INFO appInfo;

	[Header(" * COMMON ------------")]
	public static bool isLoading;
	public static GameObject gameObjectPopupBack;
	public static GameObject gameObjectPopupBack9000;
    public static bool isShortLoading;

	public double serverTime;
	public float tmpStartToLogin;
	internal PROJECT_TYPE projectType;

	public static int gameLoadingStatusCnt;
	public static int gameStatusCnt;
	public static float gameStatusTime;

	[Header(" * Default Table & List ------------")]
	public DEFAULT_EFFECT_LIST[] defaultEffectList;
	public DEFAULT_EFFECT_TABLE[] defaultEffectTable;


	[Header(" * CUSTOM ------------")]
	public bool isErrorPopup;	
	public VIDEO_TYPE selectVideoType;

	void Start() {

		appInfo = new APP_INFO();		
		MAIN_GAMEOBJECT = this.gameObject;

		appmain = this; 

		appui = FindObjectOfType<AppUI>();
		appdoc = (Appdoc)GetComponent<Appdoc>();
		appsound = GetComponent<Appsound>();
		appevent = GetComponent<Appevent>();
		appclass = GetComponent<Appclass>();
		appimg = GetComponent<Appimg>();
		appnet = GetComponent<Appnet>();

		initGameApp();

	}


	void initGameApp() {

		gameLoadingStatusCnt = 0;//(int)GAME_STATUS_NEXT_STATUS.NEXT_START;
		gameStatus = GAME_STATUS.GS_START;
		nextGameStatus = GAME_STATUS.GS_NONE;

		//appdoc.setGameStatus(GAME_STATUS.GS_START);
		
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		Application.targetFrameRate = 32;
		Application.runInBackground = true;

		Time.captureFramerate = 0;

		Debug.Log("Screen Size :: " + Screen.width + "/" + Screen.height);		

	}



	/////
	///
	public double getPresentServerTs() {		
		
        return (this.serverTime + Time.realtimeSinceStartup - appmain.tmpStartToLogin);

    }


	public static void SET_POPUP_BACK() {
		
		Debug.Log("SET_POPUP_BACK() : " + havePopupCnt);

		if(gameObjectPopupBack == null) {	//gameObjectPopupBack == null
			string path = UIDEFINE.PATH_POPUP_BOX_BACK;
			//GameObject _prefab = (GameObject)Resources.Load(path);
			//GameObject _instan = (GameObject)Instantiate(_prefab);
			GameObject _instan = Appimg.LoadResource4Prefab4UI(path);

			//isoDestoryTime die = _instan.AddComponent<isoDestoryTime>();

			//die.SET_DESTROY_TIMER(10);

			gameObjectPopupBack = _instan;
		}
	}


	public static void SET_POPUP_BACK_9000() {
		
		Debug.Log("SET_POPUP_BACK() 9000 : " + havePopupCnt);

		if(gameObjectPopupBack9000 == null) {	//gameObjectPopupBack == null
			string path = UIDEFINE.PATH_POPUP_BOX_BACK_9000;
			//GameObject _prefab = (GameObject)Resources.Load(path);
			//GameObject _instan = (GameObject)Instantiate(_prefab);
			GameObject _instan = Appimg.LoadResource4Prefab4UI(path);

			//isoDestoryTime die = _instan.AddComponent<isoDestoryTime>();

			//die.SET_DESTROY_TIMER(10);

			gameObjectPopupBack9000 = _instan;
		}
	}


	public void UN_SET_POPUP_BACK_9000(POPUPBOX_RETURN_TYPE rtn) {

		Debug.Log("UN_SET_POPUP_BACK_9000");
		NGUITools.Destroy(gameObjectPopupBack9000);
		gameObjectPopupBack9000 = null;

	}


	public static void UN_SET_POPUP_BACK(bool isAway = false) {

		Debug.Log("UN_SET_POPUP_BACK() : " + havePopupCnt);

		if(havePopupCnt <= 0) {
			NGUITools.Destroy(gameObjectPopupBack);
			gameObjectPopupBack = null;
			havePopupCnt = 0;
		}

		if(isAway == true) {
			NGUITools.Destroy(gameObjectPopupBack);
			gameObjectPopupBack = null;
			havePopupCnt = 0;
		}

	}


	public DEFAULT_EFFECT_LIST GET_DEFAULT_EFFECT(int _index) {

		for(int i = 0; i<defaultEffectList.Length; i++) {
			if(defaultEffectList[i].index == _index) {
				return defaultEffectList[i];
			}
		}

		return null;

	}
}
