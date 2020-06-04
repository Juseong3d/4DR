using UnityEngine;
using System.Collections;

public class Appimg : MonoBehaviour {

	Appmain appmain;
	Appandroid appandroid;

	public GameObject mainUIPrefab;

	// Use this for initialization
	void Start () {

		appmain = (Appmain)GetComponent<Appmain>();
		appandroid = (Appandroid)FindObjectOfType<Appandroid>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	internal void loadImage4Common() {


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
					uisoMainMenu mainMenu = mainUIPrefab.GetComponent<uisoMainMenu>();					
					

					for(int i = 0; i<Appmain.appclass._list_conent_fdlist.result.Count; i++) {
						//GameObject _instan = LoadResource4Prefab(UIDEFINE.PATH_VIDEO_ITEM_MINI, true);
						//uisoITEM_VIDEO_MINI _info = _instan.GetComponentInChildren<uisoITEM_VIDEO_MINI>();
						//MediaPlayerCtrl _ctl = _instan.GetComponentInChildren<MediaPlayerCtrl>();

						//_info.SET_INFO(Appmain.appclass._list_conent_fdlist.result[i]);
						//_ctl.m_strFileName = Appmain.appclass._list_conent_fdlist.result[i].GETURL();
						//_instan.transform.SetParent(mainMenu._gridMain.transform);
						//_instan.transform.localScale = new Vector3(1, 1, 1);

						if(!Appmain.appclass._list_conent_fdlist.result[i].type.Equals("not_used")) {
							StartCoroutine(_LOAD_MINI_VIDEO(i));
						}
					}

					//mainMenu._gridMain.repositionNow = true;

				}
				break;
		}
	}


	IEnumerator _LOAD_MINI_VIDEO(int i) {

		yield return new WaitForEndOfFrame();

		uisoMainMenu mainMenu = mainUIPrefab.GetComponent<uisoMainMenu>();					

		GameObject _instan = LoadResource4Prefab(UIDEFINE.PATH_VIDEO_ITEM_MINI, true);
		uisoITEM_VIDEO_MINI _info = _instan.GetComponentInChildren<uisoITEM_VIDEO_MINI>();
		MediaPlayerCtrl _ctl = _instan.GetComponentInChildren<MediaPlayerCtrl>();

		_info.SET_INFO(Appmain.appclass._list_conent_fdlist.result[i]);
		_ctl.m_strFileName = Appmain.appclass._list_conent_fdlist.result[i].GETURL();
		_instan.transform.SetParent(mainMenu._gridMain.transform);
		_instan.transform.localScale = new Vector3(1, 1, 1);

		mainMenu._gridMain.repositionNow = true;

	}

	public void LOAD_FULL_SCREEN_VIDEO(string _url) {

		{
            GameObject _videoManager = LoadResource4Prefab(UIDEFINE.PATH_VIDEO_MANAGER, true);
            MediaPlayerCtrl _ctl = _videoManager.GetComponentInChildren<MediaPlayerCtrl>();

			_ctl.m_strFileName = _url;
            _videoManager.transform.SetParent(Appmain.appui.transform);
			_videoManager.transform.localScale = new Vector3(1, 1, 1);

            _ctl.transform.localScale = new Vector3(DEFINE.BASE_SCREEN_WIDTH, DEFINE.BASE_SCREEN_HEIGHT, 1);
            _ctl.transform.localPosition = new Vector3(0, 0, -10);

            //GameObject playerCtl = LoadResource4Prefab4UI(UIDEFINE.PATH_VIDEO_CTL, true);
            //isoFdPlayerCtl _playerCtl = playerCtl.GetComponent<isoFdPlayerCtl>();

            //_playerCtl.scrMedia = _ctl;

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

		instan.transform.SetParent(AppUI.mainCamera.transform);
		instan.transform.localScale = new Vector3(1, 1, 1);

		return instan;
	}

}
