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

			//switch(Appmain.gameStatus) {
			//	case GAME_STATUS.GS_TITLE :
			//		if(appnet.lapStatus >= LAP_STATUS.CATEGORY_LIST_WAIT)
			//			appdoc.setGameStatus(GAME_STATUS.GS_MENU);
			//		break;
			//}

			

			
		}

		if(isDrag == true) {

			//float _x = Input.mousePosition.x - (Screen.width / 2);
			//float _y = Input.mousePosition.y - (Screen.height / 2);

			////Debug.Log("_x :: " + _x + "/" + _y);
			////for testing...
			//GameObject prefab = Appimg.LoadResource4Prefab(UIDEFINE.TEST_EFFECT);
			
			//prefab.transform.SetParent(AppUI.mainCamera.transform);
			//prefab.transform.localPosition = new Vector3(_x, _y, -100.0f);
			//prefab.transform.localScale = new Vector3(50, 50, 50);

		}

		if(Input.GetMouseButtonUp(0)) {

			this.isButtonDown = false;
			this.isDrag = false;			

		}

		if(Input.GetKeyDown(KeyCode.Escape)) {

            //if(Appmain.gameStatus < GAME_STATUS.GS_TITLE)   //로컬라이징 전에 종료 안뜨게 
            //    return;            

			if(isMakedExitPopup == false) {
				PopupBox.Create(DEFINE.POPUP_EXIT_QUSTION, DEFINE.POPUP_EXIT_TITLE, POPUPBOX_ACTION_TYPE.YESNO, this.gameObject, "OnConformExit");
				isMakedExitPopup = true;
			}else {

			}
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

		}else {
			isMakedExitPopup = false;
		}

	}

}
