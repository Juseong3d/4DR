using UnityEngine;
using System.Collections;

public class Appevent : MonoBehaviour {

	Appmain appmain;
	Appdoc appdoc;
	Appnet appnet;
	Appimg appimg;

	public bool isButtonDown;
	public bool isDrag;

	public Vector3 startPoint;
	public Vector3 endPoint;

	public Vector3 dragDist;

	public bool isMakedExitPopup;

	// Use this for initialization
	void Start () {

		appmain = (Appmain)FindObjectOfType(typeof(Appmain));
		appdoc = (Appdoc)FindObjectOfType(typeof(Appdoc));
		appnet = (Appnet)FindObjectOfType(typeof(Appnet));
		appimg = (Appimg)FindObjectOfType<Appimg>();

		isMakedExitPopup = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void LateUpdate () {
		eventMain();
	}

	private void eventMain() {

		if(Input.GetMouseButtonDown(0)) {

			isButtonDown = true;
			isDrag = true;

		}

		if(isDrag == true) {

		}

		if(Input.GetMouseButtonUp(0)) {

			this.isButtonDown = false;
			this.isDrag = false;			

		}

		if(Input.GetKeyDown(KeyCode.Escape)) {
            
			switch(Appmain.gameStatus) {
				case GAME_STATUS.GS_TITLE:
				case GAME_STATUS.GS_MENU:
					if(Appmain.appmain.isPlayVideo == false) {
						if(isMakedExitPopup == false) {
							PopupBox.Create(DEFINE.POPUP_EXIT_QUSTION, DEFINE.POPUP_EXIT_TITLE, POPUPBOX_ACTION_TYPE.YESNO, this.gameObject, "OnConformExit");
							isMakedExitPopup = true;
						}else {

						}
					}else {
						Appmain.appimg._nowFullCtl.OnClickButtonExit();
					}
					break;
			}
		}

		if(Appmain.havePopupCnt > 0) {
			return;
		}

		switch(Appmain.gameStatus) {
		case GAME_STATUS.GS_TITLE:
#if _TAE_
			{
				int i = (int)XOBX_ONE_BUTTON.BUTTON_A;
				string tmp = string.Format("joystick button {0}", i);
				
				if(Input.GetKeyDown(tmp) == true) {
					appdoc.setGameStatus(GAME_STATUS.GS_MENU);
				}
			}
#endif
		break;
		case GAME_STATUS.GS_MENU:
			if(Appmain.appmain.isPlayVideo == false) {
				if(appimg.imgMainMenu != null) {
					appimg.imgMainMenu._LateUpdate();
				}
			}
			break;

		}
	}


	public void OnConformExit(POPUPBOX_RETURN_TYPE rtn) {

		if(rtn == POPUPBOX_RETURN_TYPE.YES) {

#if UNITY_EDITOR
			Debug.LogWarning("에디터 플레이를 중단합니다.");
			UnityEditor.EditorApplication.ExecuteMenuItem("Edit/Play");			
#else
			Application.Quit();
#endif
			Appmain.appsound.playEffect(SOUND_EFFECT_TYPE.Button_Exit);

		}else {
			isMakedExitPopup = false;

			Appmain.appsound.playEffect(SOUND_EFFECT_TYPE.Button_Click);
		}

	}

}
