using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
		}


		Appmain.gameStatusCnt ++;
		Appmain.gameStatusTime += Time.deltaTime;

	}

	private void initGameStatus() {

		switch(Appmain.gameStatus) {
			case GAME_STATUS.GS_START :

				break;
			case GAME_STATUS.GS_INIT :

				break;
			case GAME_STATUS.GS_INTRO :

				break;
		}		
	}

}
