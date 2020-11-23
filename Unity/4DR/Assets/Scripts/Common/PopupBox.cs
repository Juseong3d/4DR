using UnityEngine;
using System.Collections;


public enum POPUPBOX_ACTION_TYPE {

	OK,
	YESNO,	
}


public enum POPUPBOX_RETURN_TYPE {

	OK,
	YES,
	NO,	
}

public class PopupBox : MonoBehaviour {

	public UILabel titleLabel;
    public UILabel contentLabel;
    //public UISprite titleSprite;
    public UIButton yesButton;
    public UIButton noButton;
    public UIButton okButton;

	public UIButton exceptionButton;

	public UILabel label_ok;

	public UILabel label_no;
	public UILabel label_yes;	
	

    public UILabel label_Change;
    public UILabel labelOkChange;
    public UILabel labelCancleChange;

	public GameObject customButton;
	public UILabel labelCustomRight;
	public UILabel labelCustomLeft;

    public GameObject eventReciver;
    public string funcName;
    private bool isActive;

	public int _ID;

    public TweenScale tweenPopupBox;

	public GameObject[] gameObjectButtonType;

	public POPUPBOX_RETURN_TYPE returnType;
    //int type;

	//Appmain appmain;
    Appsound appsound;

    void Awake() {

		//appmain = (Appmain)FindObjectOfType<Appmain>();
        appsound = Appmain.appsound;

    }


	private void LateUpdate() {
		
		if(Input.GetKeyDown(KeyCode.Return)) {
			OnClickOk();
		}

		//키 처리
	}


	static public PopupBox Create(string content, string title = "", POPUPBOX_ACTION_TYPE actionType = POPUPBOX_ACTION_TYPE.OK, GameObject eventReciver = null, string funcName = "", bool alwaysOnTop = false, bool allowOverlap = true, SOUND_EFFECT_TYPE soundType = SOUND_EFFECT_TYPE.Button_Click,string key = "", bool localize = false) {

		string path = UIDEFINE.PATH_POPUP_BOX;//string.Empty;

		//if(Appmain.appdoc.nowInGameView == INGAME_VIEW_TYPE.VER){
		//	path = UIDEFINE.PATH_POPUP_BOX;
		//}else{
		//	path = UIDEFINE.PATH_POPUP_BOX_HOR;
		//}

		GameObject _prefab = (GameObject)Resources.Load(path);
		GameObject _instan = (GameObject)Instantiate(_prefab);
		PopupBox dialogBox = _instan.GetComponent<PopupBox>();

		_instan.transform.SetParent(Appmain.appui.mainCamera2D.transform);
		_instan.transform.localScale = new Vector3(1, 1, 1);
		UIPanel panel = _instan.GetComponent<UIPanel>();
		panel.depth += Appmain.havePopupCnt;

		if(_prefab == null)
			Debug.Log("error popup box null");
	
        //{
        //    UISprite[] sprite;

        //    sprite = dialogBox.gameObject.GetComponentsInChildren<UISprite>(true);

        //    for(int i = 0; i < sprite.Length; i++) {

        //        sprite[i].atlas = UIDEFINE.getProjectAtlas(Appmain.appmain.projectType, sprite[i].atlas.name);

        //    }
        //}

        //{
        //    UILabel[] label;

        //    label = dialogBox.gameObject.GetComponentsInChildren<UILabel>(true);

        //    for(int i = 0; i < label.Length; i++) {

        //        label[i].trueTypeFont = UIDEFINE.setProjectFont(Appmain.appmain.projectType);

        //    }

        //}

        if (dialogBox) {
            dialogBox.Set(title, content, actionType, eventReciver, funcName, soundType, localize);
            return dialogBox;
		}
		
		return null;
    }


	static public void SMALL_LOADING_ON(bool isBack = true) {

		//if(Appmain.gameObjectSmallLoading == null) {
		//	string path = UIDEFINE.PATH_SMALL_LOADING;
		//	GameObject _prefab = (GameObject)Resources.Load(path);
		//	GameObject _instan = (GameObject)Instantiate(_prefab);

		//	_instan.transform.SetParent(Appui.mainCamera.transform);

		//	_instan.transform.localScale = new Vector3(1, 1, 1);				
		//	Appmain.gameObjectSmallLoading = _instan;

		//	if(isBack == true) {
		//		Appmain.SET_POPUP_BACK();
		//	}
		//}

	}


	static public bool IS_SMALL_LOADING() {

		//return (Appmain.gameObjectSmallLoading != null);
		return false;

	}


	static public void SMALL_LOADING_OFF(bool isAway = false) {		

		//if(Appmain.gameObjectSmallLoading == null) return;

		//NGUITools.Destroy(Appmain.gameObjectSmallLoading);
		//Appmain.UN_SET_POPUP_BACK(isAway);

	}


	public void Set(string title, string content, POPUPBOX_ACTION_TYPE actionType = POPUPBOX_ACTION_TYPE.OK, GameObject eventReciver = null, string funcName = "", SOUND_EFFECT_TYPE soundType = SOUND_EFFECT_TYPE.Button_Click, bool localize = false) {

		bool idParsingResult = int.TryParse(title, out this._ID);

		if(idParsingResult == false) {
			this._ID = -1;
		}

        switch(Appmain.appmain.projectType) {

            default :
		        if(content.Contains("[-]") == true) {
		        	this.contentLabel.color = Color.white;
		        }
            break;
        }

		this.titleLabel.text = title;
		this.contentLabel.text = content;

        this.eventReciver = eventReciver;
        this.funcName = funcName;
		
		//action type과 같은 gameObject(button group)을 true 나머진 false
		for(int i = 0; i<gameObjectButtonType.Length; i++) {
			bool how = ((int)actionType == i);

			NGUITools.SetActive(gameObjectButtonType[i], how);
		}

		switch(actionType) {
			case POPUPBOX_ACTION_TYPE.OK :
                if(localize == false) {
				    label_ok.text = DEFINE.POPUP_OK;;//Localization.Get(LOCALIZEDEFINE._165);
                }else {
				    label_ok.text = DEFINE.POPUP_OK;
                }
				//NGUITools.SetActive(this.okButton.gameObject, true);
				break;
			case POPUPBOX_ACTION_TYPE.YESNO :
                label_yes.text = DEFINE.POPUP_YES;//Localization.Get(LOCALIZEDEFINE._165);
                label_no.text = DEFINE.POPUP_NO;//Localization.Get(LOCALIZEDEFINE._168);

				//NGUITools.SetActive(this.yesButton.gameObject, true);
				//NGUITools.SetActive(this.noButton.gameObject, true);
				break;			

		}

		//if(appsound != null) appsound.playEffect(soundType);		
        this.isActive = true;
		Appmain.havePopupCnt ++;
		Appmain.SET_POPUP_BACK();
    }


	public void OnClickOk() {

        tweenPopupBox.PlayReverse();
        returnType = POPUPBOX_RETURN_TYPE.OK;

		//if(appsound != null) 
		//	appsound.playEffectButton();

		//if (Appmain.gameObjectPopupBack != null) {
		//	UIPanel _panel = Appmain.gameObjectPopupBack.GetComponent<UIPanel>();
		//	_panel.depth -= 4000;
		//}
    }


    public void FinishedPopup() {

        if (tweenPopupBox.transform.localScale.x == 0) {
            InvokeReturnFunction(returnType);
        }
       
    }


    public void OnClickYes() {
        
        tweenPopupBox.PlayReverse();
        returnType = POPUPBOX_RETURN_TYPE.YES;		
		//appsound.playEffectButton();

		//if (Appmain.gameObjectPopupBack != null) {
		//	UIPanel _panel = Appmain.gameObjectPopupBack.GetComponent<UIPanel>();
		//	_panel.depth -= 4000;
		//}
    }

    public void OnClickNo() {
        
        tweenPopupBox.PlayReverse();
        returnType = POPUPBOX_RETURN_TYPE.NO;
		//appsound.playEffectButton();
		
		//if (Appmain.gameObjectPopupBack != null) {
		//	UIPanel _panel = Appmain.gameObjectPopupBack.GetComponent<UIPanel>();
		//	_panel.depth -= 4000;
		//}
    }
		


	void InvokeReturnFunction(POPUPBOX_RETURN_TYPE returnType) {

        if (this.isActive == false)
            return;

		Appmain.havePopupCnt --;

        if (this.eventReciver && string.IsNullOrEmpty(this.funcName) == false)  {
            this.eventReciver.SendMessage(this.funcName, returnType);
        }

        Destroy();
    }


	public void Destroy() {
		
		Appmain.UN_SET_POPUP_BACK();
		NGUITools.Destroy(this.gameObject);

    }
	
	public void OnClickException(){

		//Appmain.appevent.OnConformExit(POPUPBOX_RETURN_TYPE.YES);

	}

}
