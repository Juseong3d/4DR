using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

public class Appimg : MonoBehaviour {

	Appmain appmain;
	Appandroid appandroid;

	public GameObject mainUIPrefab;

	public uisoMainMenu imgMainMenu;

	public isoFdPlayerCtl _nowFullCtl;
	public AppCommandCtlCamera _nowVideoCommander;
	public MediaPlayerCtrl _nowFullVideo;

	public uisoStartScale[] _startTween;

	public List<string> FilePathList;
    public List<string> CoverPathList;

	public TEMP_TEXTURE[] tempTexture;

	// Use this for initialization
	void Start () {

		appmain = (Appmain)GetComponent<Appmain>();
		appandroid = (Appandroid)FindObjectOfType<Appandroid>();

		loadImage4Common();

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	internal void loadImage4Common() {
		
		loadTable4DefaultCountryCode();

		loadTable4EffectList();	
		loadTable4EffectTable();
		loadTable4PlayerList();
		loadTable4GameList();

		tempTexture = null;//new TEMP_TEXTURE[1];

		if(tempTexture != null) {
			for(int i = 0; i<tempTexture.Length; i++) {
				tempTexture[i] = new TEMP_TEXTURE();
			}
		}

	}



	internal void freeImage4Status() {

		switch(Appmain.prevGameStatus) {
			case GAME_STATUS.GS_INTRO :
			case GAME_STATUS.GS_TITLE :
			case GAME_STATUS.GS_MENU :
				NGUITools.Destroy(mainUIPrefab);
				break;
		}
	}


	internal void loadImage4Status() {

		switch(Appmain.gameStatus) {
			case GAME_STATUS.GS_INTRO :
				mainUIPrefab = LoadResource4Prefab4UI(UIDEFINE.PATH_INTRO, true);				
				break;
			case GAME_STATUS.GS_TITLE :
				//{
				//	GameObject _videoManager = LoadResource4Prefab(UIDEFINE.PATH_VIDEO_MANAGER, true);
				//	MediaPlayerCtrl _ctl = _videoManager.GetComponent<MediaPlayerCtrl>();

				//	_videoManager.transform.SetParent(appmain.appui.transform);

				//	_videoManager.transform.localScale = new Vector3(DEFINE.BASE_SCREEN_WIDTH, DEFINE.BASE_SCREEN_HEIGHT, 1);
				//	_videoManager.transform.localPosition = new Vector3(0, 0, _videoManager.transform.localPosition.z);					

				//	GameObject playerCtl = LoadResource4Prefab4UI(UIDEFINE.PATH_VIDEO_CTL, true);
				//	isoFdPlayerCtl _playerCtl = playerCtl.GetComponent<isoFdPlayerCtl>();

				//	_playerCtl.scrMedia = _ctl;
				//}
				{
					mainUIPrefab = LoadResource4Prefab4UI(UIDEFINE.PATH_TITLE, true);
				}
				break;
			case GAME_STATUS.GS_MENU :
				{
					mainUIPrefab = LoadResource4Prefab4UI(UIDEFINE.PATH_MAIN_MENU, true);
					imgMainMenu = mainUIPrefab.GetComponent<uisoMainMenu>();
#if _DIRECT_URL_
				LOAD_FULL_SCREEN_VIDEO(DEFINE.DIRECT_TEST_URL);
#else

				switch(appmain.selectVideoType) {
					case VIDEO_TYPE.WEB_SERVER_LIST:
						_SET_MINI_VIDEO_GRID();						
						break;
					case VIDEO_TYPE.DIRECT_URL:
						LOAD_FULL_SCREEN_VIDEO(DEFINE.DIRECT_TEST_URL);
						break;
					case VIDEO_TYPE.LOCAL_LIST:
						_SET_LOCAL_LIST_GRID();
						break;
				}
#endif
					{
						isoCameraZoom cameractl = Appmain.appui.mainCamera3D.GetComponent<isoCameraZoom>();
						cameractl.INIT_CAMERA();
					}
				}
				
				_SET_CAMERA_SCRIPT();
				break;
		}
	}


	public void _SET_CAMERA_SCRIPT() {

		{
			BetterList<Transform> _gridrightcs = imgMainMenu._gridRightMain.GetChildList();

			for(int i = 0; i<_gridrightcs.size; i++) {
				NGUITools.Destroy(_gridrightcs[i].gameObject);
			}


			//string[] _tmpcslist = {
			//	"t_unity_cv_ctl_test_MBC_TEST03",
			//	"t_cs_channel_change"
			//};

			GameObject _prefab = Appimg.LoadResources4PrefabOnly(UIDEFINE.PATH_CAMERA_SCRIPT_ITEM);

			for(int i = 0; i<Appmain.appclass._list_script_list.result.Count; i++) {
								
				if(Appmain.appclass._list_script_list.result[i].type == RECV_TYPE.CAMERA_SCRIPT) {
					GameObject _instan = Appimg._INSTANTIATE4UI(_prefab);
					uisoITEM_CameraScript _info = _instan.GetComponent<uisoITEM_CameraScript>();

					_info._info = Appmain.appclass._list_script_list.result[i];
					_info.SET_LABEL(Appmain.appclass._list_script_list.result[i].name);

					_instan.transform.SetParent(imgMainMenu._gridRightMain.transform);

					_instan.transform.localScale = new Vector3(1, 1, 1);
				}								
			}

			imgMainMenu._gridRightMain.repositionNow = true;
		}

	}


	public void _SET_LOCAL_LIST_GRID() {


		uisoMainMenu mainMenu = mainUIPrefab.GetComponent<uisoMainMenu>();	
		BetterList<Transform> _gridC = mainMenu._gridMain.GetChildList();

		for(int i = 0; i<_gridC.size; i++) {			

			NGUITools.Destroy(_gridC[i].gameObject);
			//NGUITools.SetActive(_gridC[i].gameObject, false);
		}

		_gridC.Clear();
		//////////////

		string filepath;
		string _path = DEFINE.GET_LOCAL_FOLDER_PATH();

		var folder = Directory.CreateDirectory(_path); // returns a DirectoryInfo object

		Debug.Log("_SET_LOCAL_LIST_GRID() " + _path);
		DirectoryInfo directoryInfo = new DirectoryInfo(_path);
		//FileInfo[] files = directoryInfo.GetFiles();
		Debug.Log("_SET_LOCAL_LIST_GRID() ");

		//각 비디오의 패스(URL) 리스트 만들기
        foreach (var file in directoryInfo.GetFiles()) {
            if (file.Extension != ".meta" && file.Extension != ".DS_Store") { //비디오 이외의 확장자를 가진 파일 제외시키기
                filepath = _path + "/" + file.Name;

				Debug.Log("ppppp : " + filepath);
                if (!filepath.Contains ("._")) { //파일명 에러 수정해주기
                    // filepath = filepath.Replace ("._", "");
                    if (filepath.Contains (".mp4")) { //비디오 파일 add 리스트
						Debug.Log("filepath : " + filepath);
                        FilePathList.Add (file.Name); 
						StartCoroutine(_LOAD_MINI_VIDEO_FOR_LOCAL(file.Name));
					}
                }
            }
		}
	}


	public void _SET_MINI_VIDEO_GRID() {

		uisoMainMenu mainMenu = mainUIPrefab.GetComponent<uisoMainMenu>();	
		BetterList<Transform> _gridC = mainMenu._gridMain.GetChildList();

		for(int i = 0; i<_gridC.size; i++) {

			MediaPlayerCtrl _ctl = _gridC[i].GetComponentInChildren<MediaPlayerCtrl>();

			_ctl.UnLoad();

			//NGUITools.Destroy(_gridC[i].gameObject);
			NGUITools.SetActive(_gridC[i].gameObject, false);
		}

		_gridC.Clear();

		float _delay = 0.01f;

		for(int i = 0; i<Appmain.appclass._list_conent_fdlist.result.Count; i++) {
			if(!Appmain.appclass._list_conent_fdlist.result[i].type.Equals(DEFINE.VOD_TYPE_NOT_USE)) {

			#if _TAE_
				if((Appmain.appclass._list_conent_fdlist.result[i].category != null) && (!string.IsNullOrEmpty(Appmain.appclass._list_conent_fdlist.result[i].category.name))) {

					//Debug.Log("Appmain.appclass._list_conent_fdlist.result[i].category.name : " + Appmain.appclass._list_conent_fdlist.result[i].category.name);
					//Debug.Log("Appmain.appclass._list_conent_fdlist.result[i].category.key : " + Appmain.appclass._list_conent_fdlist.result[i].category.key);
					//Debug.Log("Appmain.appclass._list_conent_fdlist.result[i].category.key : " + Appmain.appclass._list_conent_fdlist.result[i].category._key);

					if(Appmain.appclass._list_conent_fdlist.result[i].category._key == CATEGORY_KEY.TAE) {
						StartCoroutine(_LOAD_MINI_VIDEO(i, _delay));
					}
				}
			#else 
				StartCoroutine(_LOAD_MINI_VIDEO(i, _delay));
			#endif

				_delay += 0.01f;
			}
		}
	}
	

	IEnumerator _LOAD_MINI_VIDEO(int i, float _delay = 0.5f) {

		uisoMainMenu mainMenu = mainUIPrefab.GetComponent<uisoMainMenu>();					

		GameObject _instan = LoadResource4Prefab(UIDEFINE.PATH_VIDEO_ITEM_MINI, true);		

		_instan.transform.SetParent(mainMenu._gridMain.transform);
		_instan.transform.localScale = new Vector3(1, 1, 1);		

		mainMenu._gridMain.repositionNow = true;

		yield return new WaitForSeconds(_delay);		
		
		uisoITEM_VIDEO_MINI _info = _instan.GetComponentInChildren<uisoITEM_VIDEO_MINI>();
		MediaPlayerCtrl _ctl = _instan.GetComponentInChildren<MediaPlayerCtrl>();
		
		Appmain.appclass._list_conent_fdlist.result[i].controler = _ctl;
		_info.SET_INFO(Appmain.appclass._list_conent_fdlist.result[i]);		

		_ctl.m_strFileName = Appmain.appclass._list_conent_fdlist.result[i].GETURL();		

	}


	IEnumerator _LOAD_MINI_VIDEO_FOR_LOCAL(string _path, float _delay = 0.5f) {

		Debug.Log("_path ::: " + _path);
		uisoMainMenu mainMenu = mainUIPrefab.GetComponent<uisoMainMenu>();					

		GameObject _instan = LoadResource4Prefab(UIDEFINE.PATH_VIDEO_ITEM_MINI, true);
		

		_instan.transform.SetParent(mainMenu._gridMain.transform);
		_instan.transform.localScale = new Vector3(1, 1, 1);		

		mainMenu._gridMain.repositionNow = true;

		yield return new WaitForSeconds(_delay);		
		
		uisoITEM_VIDEO_MINI _info = _instan.GetComponentInChildren<uisoITEM_VIDEO_MINI>();
		MediaPlayerCtrl _ctl = _instan.GetComponentInChildren<MediaPlayerCtrl>();
		
		//Appmain.appclass._list_conent_fdlist.result[i].controler = _ctl;
		//_info.SET_INFO(Appmain.appclass._list_conent_fdlist.result[i]);		

		_info.SET_INFO(_path);

		//_ctl.m_strFileName = _path;//Appmain.appclass._list_conent_fdlist.result[i].GETURL();		
		_ctl.enabled = false;

	}

	public void LOAD_FULL_SCREEN_VIDEO(string _url) {

		LIST_CONTENT_FDLIVE_ITEM _vaule = new LIST_CONTENT_FDLIVE_ITEM();

		_vaule.url = _url;

		LOAD_FULL_SCREEN_VIDEO(_vaule);

	}


	public void LOAD_FULL_SCREEN_VIDEO(LIST_CONTENT_FDLIVE_ITEM __info) {

		string _url = __info.GETURL();

		Debug.Log("LOAD_FULL_SCREEN_VIDEO :: " + _url);;

		for(int i = 0; i<Appmain.appclass._list_conent_fdlist.result.Count; i++) {

			if(Appmain.appclass._list_conent_fdlist.result[i].controler != null) 
				Appmain.appclass._list_conent_fdlist.result[i].controler.Pause();			
					
		}

		{
            GameObject _videoManager = LoadResource4Prefab(UIDEFINE.PATH_VIDEO_MANAGER, true);
			
			//_startTween = _videoManager.GetComponentsInChildren<uisoStartScale>();

            MediaPlayerCtrl _ctl = _videoManager.GetComponentInChildren<MediaPlayerCtrl>();

			_nowFullVideo = _ctl;			
			_nowFullCtl = _videoManager.GetComponentInChildren<isoFdPlayerCtl>();			

			_nowFullCtl.max_channel = __info.max_channel;
			_ctl.m_strFileName = _url;
            _videoManager.transform.SetParent(Appmain.appui.transform);
			_videoManager.transform.localScale = new Vector3(1, 1, 1);

            _ctl.transform.localScale = new Vector3(DEFINE.BASE_SCREEN_WIDTH, DEFINE.BASE_SCREEN_HEIGHT, 1);
            _ctl.transform.localPosition = new Vector3(0, 0, -10);

            //GameObject playerCtl = LoadResource4Prefab4UI(UIDEFINE.PATH_VIDEO_CTL, true);
            //isoFdPlayerCtl _playerCtl = playerCtl.GetComponent<isoFdPlayerCtl>();

            //_playerCtl.scrMedia = _ctl;

			_nowFullCtl.beforeParent = _videoManager.gameObject.transform;
			_nowFullCtl.transform.SetParent(Appmain.appui.transform);
			
			_nowVideoCommander = _nowFullCtl.beforeParent.GetComponent<AppCommandCtlCamera>();

			_nowFullCtl._videoInfo = __info;


			{
				appmain._selectCameraScript.Clear();
				appmain._selectCameraScript = new List<uisoITEM_CameraScript>();

				BetterList<Transform> ccc = imgMainMenu._gridRightMain.GetChildList();
				

				foreach(Transform obj in ccc) {
					uisoITEM_CameraScript _tmp = obj.GetComponent<uisoITEM_CameraScript>();

					if(_tmp._toggle.value == true) {
						appmain._selectCameraScript.Add(_tmp);
					}
				}

			}

			Appmain.appmain.isPlayVideo = true;
			mainUIPrefab.SetActive(false);
        }
	}


	public static GameObject LoadResource4Prefab(string path, bool isScaleOne = false) {

#if _ASSET_BUNDLE_

		string[] tmpString = path.Split('/');

		path = tmpString[tmpString.Length-1];

		GameObject prefab = Appmain.appimg.bundleMgr.bundle[DEFINE.ASSET_BUNDLE_IDX].Load(path, typeof(GameObject)) as GameObject;
		GameObject instan = (GameObject)Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity);
		
#else
		GameObject prefab = (GameObject)Resources.Load(path);
		GameObject instan = (GameObject)Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
#endif
		if(isScaleOne == true) {
			instan.transform.localScale = new Vector3(1, 1, 1);
		}

		return instan;
	}


	public static GameObject LoadResource4Prefab4UI(string path, bool noinit = false) {

#if _ASSET_BUNDLE_

		string[] tmpString = path.Split('/');

		path = tmpString[tmpString.Length-1];

		GameObject instan = null;

		if(noinit == true) {
			instan = (GameObject)Instantiate(Appmain.appimg.bundleMgr.bundle[DEFINE.ASSET_BUNDLE_IDX].Load(path));
		}else {
			instan = (GameObject)Instantiate(Appmain.appimg.bundleMgr.bundle[DEFINE.ASSET_BUNDLE_IDX].Load(path), new Vector3(0, 0, 0), Quaternion.identity);
		}

#else
		GameObject prefab = (GameObject)Resources.Load(path);
		GameObject instan = null;

		if(noinit == true) {
			instan = (GameObject)Instantiate(prefab);
		}else {
			instan = (GameObject)Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
		}
#endif

		instan.transform.SetParent(Appmain.appui.mainCamera2D.transform);
		instan.transform.localScale = new Vector3(1, 1, 1);

		return instan;
	}


	public static GameObject LoadResources4PrefabOnly(string path) {

		GameObject prefab = (GameObject)Resources.Load(path);

		return prefab;

	}


	public static GameObject _INSTANTIATE4UI(GameObject prefab, bool ischeck = false) {

		GameObject instan = (GameObject)Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);

		//instan.transform.SetParent(Appmain.appui.mainCamera2D.transform);
		instan.transform.localScale = new Vector3(1, 1, 1);

        if( ischeck == true)
        {
            UISprite[] sprite;

            sprite = instan.GetComponentsInChildren<UISprite>(true);
            
            for(int i = 0; i < sprite.Length; i++) {

                sprite[i].atlas = UIDEFINE.getProjectAtlas(Appmain.appmain.projectType, sprite[i].atlas.name);

            }

            {
                UILabel[] label;

                label = instan.GetComponentsInChildren<UILabel>(true);

                for(int i = 0; i < label.Length; i++) {

                    label[i].trueTypeFont = UIDEFINE.setProjectFont(Appmain.appmain.projectType);

                }

            }
        }

		return instan;
	}


	internal void loadTable4EffectList(string _allData = null) {

		string path = "Common/_Default_Table/tb_effect_list";

		int totalCnt = 0;
		int i = 0;
		string[] allData = null;

		if(_allData != null) {
			allData = CSVReader.ReadFileFromString(_allData);
		} else {
			allData = CSVReader.ReadFile(path, false);
		}

		if (allData == null) {
			Debug.Log(path + " :: allData is null");
			return;
		}

		string[] tmp = allData[0].Split(","[0]);
		totalCnt = Convert.ToInt32(tmp[0]);

		appmain.defaultEffectList = new DEFAULT_EFFECT_LIST[totalCnt];

		for (i = 0; i < totalCnt; i++) {

			string[] tableData = allData[i + 1].Split(","[0]);

			appmain.defaultEffectList[i] = new DEFAULT_EFFECT_LIST(tableData);
		}
	}


	internal void loadTable4EffectTable(string _allData = null) {

		string path = "Common/_Default_Table/tb_effect_table";

		int totalCnt = 0;
		int i = 0;
		string[] allData = null;

		if(_allData != null) {
			allData = CSVReader.ReadFileFromString(_allData);
		} else {
			allData = CSVReader.ReadFile(path, false);
		}

		if (allData == null) {
			Debug.Log(path + " :: allData is null");
			return;
		}

		string[] tmp = allData[0].Split(","[0]);
		totalCnt = Convert.ToInt32(tmp[0]);

		appmain.defaultEffectTable = new DEFAULT_EFFECT_TABLE[totalCnt];

		for (i = 0; i < totalCnt; i++) {

			string[] tableData = allData[i + 1].Split(","[0]);

			appmain.defaultEffectTable[i] = new DEFAULT_EFFECT_TABLE(tableData);
		}
	}


	internal void loadTable4PlayerList(string _allData = null) {

		string path = "Common/_Default_Table/tb_player_list";

		int totalCnt = 0;
		int i = 0;
		string[] allData = null;

		if(_allData != null) {
			allData = CSVReader.ReadFileFromString(_allData);
		} else {
			allData = CSVReader.ReadFile(path, false);
		}

		if (allData == null) {
			Debug.Log(path + " :: allData is null");
			return;
		}

		string[] tmp = allData[0].Split(","[0]);
		totalCnt = Convert.ToInt32(tmp[0]);

		appmain.defaultPlayList = new DEFAULT_PLAYER_LIST[totalCnt];

		for (i = 0; i < totalCnt; i++) {

			string[] tableData = allData[i + 1].Split(","[0]);

			appmain.defaultPlayList[i] = new DEFAULT_PLAYER_LIST(tableData);
		}
	}


	internal void loadTable4GameList(string _allData = null) {

		string path = "Common/_Default_Table/tb_game_list";

		int totalCnt = 0;
		int i = 0;
		string[] allData = null;

		if(_allData != null) {
			allData = CSVReader.ReadFileFromString(_allData);
		} else {
			allData = CSVReader.ReadFile(path, false);
		}

		if (allData == null) {
			Debug.Log(path + " :: allData is null");
			return;
		}

		string[] tmp = allData[0].Split(","[0]);
		totalCnt = Convert.ToInt32(tmp[0]);

		appmain.defaultGameInfo = new GAME_INFO_TAE[totalCnt];

		for (i = 0; i < totalCnt; i++) {

			string[] tableData = allData[i + 1].Split(","[0]);

			appmain.defaultGameInfo[i] = new GAME_INFO_TAE(tableData);
		}
	}


	internal void loadTable4DefaultCountryCode(string _allData = null) {

		string path = "Common/_Default_Table/tb_country_code";

		int totalCnt = 0;
		int i = 0;
		string[] allData = null;

		if(_allData != null) {
			allData = CSVReader.ReadFileFromString(_allData);
		} else {
			allData = CSVReader.ReadFile(path, false);
		}

		if (allData == null) {
			Debug.Log(path + " :: allData is null");
			return;
		}

		string[] tmp = allData[0].Split(","[0]);
		totalCnt = Convert.ToInt32(tmp[0]);

		appmain.defaultCountryCode = new COUNTRY_CODE[totalCnt];

		for (i = 0; i < totalCnt; i++) {

			string[] tableData = allData[i + 1].Split(","[0]);

			appmain.defaultCountryCode[i] = new COUNTRY_CODE(tableData);
		}
	}


	internal void overwriteTable(LIST_SCRIPT_LIST_ITEM_SUB recvValue) {

		TABLE_LIST whatTable;
		bool isResult = Enum.TryParse<TABLE_LIST>(recvValue.filename, out whatTable);

		if(isResult == false) {
			Debug.Log("none table list check :: " + recvValue.filename);
			return;
		}

		switch (whatTable) {
		case TABLE_LIST.tb_country_code:
			loadTable4DefaultCountryCode(recvValue.content);
			break;
		case TABLE_LIST.tb_effect_list:
			loadTable4EffectList(recvValue.content);
			break;
		case TABLE_LIST.NONE:
			break;
		case TABLE_LIST.tb_effect_table:
			loadTable4EffectTable(recvValue.content);
			break;
		case TABLE_LIST.tb_Game_list:
			loadTable4GameList(recvValue.content);
			break;
		case TABLE_LIST.tb_player_list:
			loadTable4PlayerList(recvValue.content);
			break;
		default:

			break;
		}

	}

}
