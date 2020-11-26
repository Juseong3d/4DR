using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.Text;
using System.Security.Cryptography;

public class Appdoc : MonoBehaviour {
	
	Appimg appimg;

	// Use this for initialization
	void Start () {

		appimg = (Appimg)GetComponent<Appimg>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void FixedUpdate() {

		processMain();

	}
	

	//게임 시작시 한번 초기활 내용들
	public void initGameData() {

		Appmain.appmain.isPlayVideo = false;

	}


	public void setGameStatus(GAME_STATUS status) {

		Debug.Log("call setGameStatus : " + status);
		if(status == Appmain.gameStatus) {
			Debug.Log(status + " : gameStatus : " + Appmain.gameStatus);
			return;
		}

		if(Appmain.nextGameStatus == GAME_STATUS.GS_NONE) {
			Appmain.nextGameStatus = status;

			Appmain.gameLoadingStatusCnt = (int)GAME_STATUS_NEXT_STATUS.NEXT_START;
			Appmain.gameStatusTime = 0.0f;
			Appmain.gameStatusCnt = 0;
			//appimg.isLoading = true;
			
			//Application.LoadLevel(((int)status));
			SceneManager.LoadScene(((int)status));
		}

	}


	void processMain() {

		if(Appmain.nextGameStatus != GAME_STATUS.GS_NONE) {
			if(Application.isLoadingLevel == true)
				return;

			GAME_STATUS_NEXT_STATUS changeNextStatus = (GAME_STATUS_NEXT_STATUS)Appmain.gameLoadingStatusCnt;

			switch(changeNextStatus) {
				case GAME_STATUS_NEXT_STATUS.NEXT_START :					

					Appmain.prevGameStatus = Appmain.gameStatus;
					Appmain.gameStatus = Appmain.nextGameStatus;

					break;
				case GAME_STATUS_NEXT_STATUS.FREE_STATUS :
					appimg.freeImage4Status();
					break;
				case GAME_STATUS_NEXT_STATUS.LOAD_STATUS :
					appimg.loadImage4Status();
					break;
				case GAME_STATUS_NEXT_STATUS.INIT_STATUS :
					initGameStatus();					
					break;
				case GAME_STATUS_NEXT_STATUS.START_STATUS :
					Appmain.nextGameStatus = GAME_STATUS.GS_NONE;
					//appimg.isLoading = false;
					break;
			}

			Appmain.gameLoadingStatusCnt ++;		

			return;
		}

		if(Appmain.gameLoadingStatusCnt < 0) return;

		switch(Appmain.gameStatus) {
			case GAME_STATUS.GS_START :
				setGameStatus(GAME_STATUS.GS_INIT);				
				break;
			case GAME_STATUS.GS_INIT :
				setGameStatus(GAME_STATUS.GS_INTRO);
				
				break;
			case GAME_STATUS.GS_INTRO :
				if(Appmain.gameStatusCnt > 100) {
					setGameStatus(GAME_STATUS.GS_TITLE);
				}
				break;
			case GAME_STATUS.GS_MENU:
				
				break;
		}


		Appmain.gameStatusCnt ++;
		Appmain.gameStatusTime += Time.deltaTime;

	}

	private void initGameStatus() {

		switch(Appmain.gameStatus) {
			case GAME_STATUS.GS_START :

				break;
			case GAME_STATUS.GS_INIT :
				Appmain.appnet.__WEB_CONNECT_AND_SEND_RECV_4_FAST_JSON(NET_WEB_API_CMD.video);
				Appmain.appnet.__WEB_CONNECT_AND_SEND_RECV_4_FAST_JSON(NET_WEB_API_CMD.script);
				initGameData();
				break;
			case GAME_STATUS.GS_INTRO :

				break;
			case GAME_STATUS.GS_TITLE:
				Appmain.appsound.playBGM(SOUND_BGM_TYPE.Intro_Bgm);
				break;
			case GAME_STATUS.GS_MENU:
				{
					Appmain.appclass._list_video_extra_info.result.Clear();
					
				}
				break;
		}		
	}


	public static COUNTRY_CODE GET_COUNTRY_CODE(string _value) {

		COUNTRY_CODE rtn = null;

		for(int i = 0; i<Appmain.appmain.defaultCountryCode.Length; i++) {

			if(Appmain.appmain.defaultCountryCode[i].alpha2Code.Equals(_value) == true || Appmain.appmain.defaultCountryCode[i].alpha3Code.Equals(_value) == true) {
				rtn = Appmain.appmain.defaultCountryCode[i];
				break;
			}
		}

		return rtn;

	}


	public static string getNumberToDateTime4Ori(long seconds, string format, bool isShot = false) {

		int sec = (int)(seconds % 60);
		int min = (int)(seconds / 60 % 60);
		int hour = (int)(seconds / 60 / 60 % 24);
		int day = (int)(seconds / 60 / 60 / 24);
		
		string rtnString = string.Empty;
		
		if(day == 0) {
            rtnString = string.Format("[FFFFFF]{1:00}:{2:00}:{3:00}[-]", day, hour, min, sec);
		}else {
			return rtnString;
		}

		if(!string.IsNullOrEmpty(format)) {
			rtnString = string.Format(format, day, hour, min, sec);
		}

		if(isShot == true) {
			if(hour == 0) {
                rtnString = string.Format("[FFFFFF]{2:00}:{3:00}[-]", day, hour, min, sec);
			}else {
				return rtnString;
			}

			if(min == 0) {
                rtnString = string.Format("[FFFFFF]{3:00}[-]", day, hour, min, sec);
			}else {
				return rtnString;
			}
		}

		return rtnString;
	}


	public static long GET_TIME_NOW() {

        var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));

        return (long)timeSpan.TotalSeconds;

    }


	public static string GET_MD5_HASH(MD5 md5Hash, string input) { 

		// Convert the input string to a byte array and compute the hash. 
		byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input)); 

		// Create a new Stringbuilder to collect the bytes 
		// and create a string. 
		StringBuilder sBuilder = new StringBuilder(); 

		// Loop through each byte of the hashed data 
		// and format each one as a hexadecimal string. 
		for (int i = 0; i < data.Length; i++) { 
			sBuilder.Append(data[i].ToString("x2")); 
		} // Return the hexadecimal string. 

		return sBuilder.ToString(); 
	}


	public static string GET_STRING_DEL_BOM(string _ori) {

		// utf-8 인코딩
		byte [] bytesForEncoding = Encoding.UTF8.GetBytes (_ori);
		string encodedString = Convert.ToBase64String(bytesForEncoding);

		// utf-8 디코딩
		byte[] decodedBytes = Convert.FromBase64String(encodedString);
		string decodedString = Encoding.UTF8.GetString(decodedBytes);

		return decodedString;

	}


}
