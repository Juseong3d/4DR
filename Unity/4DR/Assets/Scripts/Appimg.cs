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
				{
					GameObject _videoManager = LoadResource4Prefab4UI(UIDEFINE.PATH_VIDEO_MANAGER, true);
					MediaPlayerCtrl _ctl = _videoManager.GetComponent<MediaPlayerCtrl>();

					_videoManager.transform.localScale = new Vector3(1280, 720, 1);
					_videoManager.transform.localPosition = new Vector3(0, 0, _videoManager.transform.localPosition.z);

					GameObject playerCtl = LoadResource4Prefab4UI(UIDEFINE.PATH_VIDEO_CTL, true);
					isoFdPlayerCtl _playerCtl = playerCtl.GetComponent<isoFdPlayerCtl>();

					_playerCtl.scrMedia = _ctl;
				}
				break;
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
