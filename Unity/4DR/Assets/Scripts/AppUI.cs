﻿using UnityEngine;
using System.Collections;

public class AppUI : MonoBehaviour {

	public static UIRoot uiRoot;
	public static Camera mainCamera;
	public UICamera uiCamera;
	public Appmain appmain;
	public Appandroid appandroid;

	public UILabel labelTopStatus;

	void Awake() {

		uiRoot = (UIRoot)FindObjectOfType<UIRoot>();
		mainCamera = uiRoot.GetComponentInChildren<Camera>();
		uiCamera = GetComponentInParent<UICamera>();

		//꽉차지 않은 경우 ios에서 반려때문에 늘려줄때 사용
		//uiCamera.GetComponent<Camera>().aspect = 1280.0f / 720.0f;

		//Screen.SetResolution( Screen.width, (Screen.width * 16) / 9 , true);
		Rect _rect = mainCamera.rect;
		float scaleheight = ((float)Screen.width / Screen.height) / ((float)16 / 9);
		float scalewidth = 1f / scaleheight;

		if(scaleheight < 1) {
			_rect.height = scaleheight;
			_rect.y = (1f - scaleheight) / 2f;
		}else {
			_rect.width = scalewidth;
			_rect.x = (1f - scalewidth) / 2f;
		}

		mainCamera.rect = _rect;
	}


	// Use this for initialization
	void Start () {
		
		appmain = (Appmain)FindObjectOfType<Appmain>();
		appandroid = (Appandroid)FindObjectOfType<Appandroid>();

	}
	
	// Update is called once per frame
	void Update () {

		string tmp = string.Format("{0:F3}", Appmain.gameStatusTime);

		labelTopStatus.text = Appmain.gameStatus + "/" + Appmain.gameStatusCnt + "/" + tmp;

	}

}
