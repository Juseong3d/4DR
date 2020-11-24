using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using AnimationOrTween;

[Serializable]
public class Appclass : MonoBehaviour {

	public LIST_CONTENT_FDLIVE _list_conent_fdlist;
	public LIST_SCRIPT_LIST _list_script_list;
	public LIST_COMMANDER _list_commander;
}

/////////////////////////////////////
/// Default class
/////////////////////////////////////
///
[Serializable] 
public class APP_INFO {

	public string version0;
	public string version1;
	public string version2;

	public string appVersion;

	public string phoneNumber;

	public string market_url;
	public string market_url_short;
	public string facebook_url;
	public string facebook_link_icon;
	public string bundle_id;

	public APP_INFO() {
		
		phoneNumber = "UNKNOW";

		version0 = "0";
		version1 = "4";
		version2 = "2";

		appVersion = "V." + version0 + "." + version1 + "." + version2;
		appVersion += ".2.ALPHA";
#if _DIRECT_URL_
		appVersion += ".D";
#endif

	}
}


[Serializable]
public class TEMP_TEXTURE {

	public string url;
	public Texture texture;

	public TEMP_TEXTURE() {
		url = string.Empty;		
	}

}


/////////////////////////////////////
/// Custom class
/////////////////////////////////////

[Serializable]
public class LIST_COMMANDER {

	public List<LIST_COMMANDER_ITEM> result;

}


[Serializable]
public class LIST_COMMANDER_ITEM {

	public int id;
	public int video_id;
	public string data;
	public long timestamp;

}


[Serializable]
public class  LIST_CONTENT_FDLIVE {

	public List<LIST_CONTENT_FDLIVE_ITEM> result;

	~LIST_CONTENT_FDLIVE() {

	}
}


[Serializable]
public class LIST_SCRIPT_LIST {

	public List<LIST_SCRIPT_LIST_ITEM> result;

}


[Serializable]
public class LIST_CONTENT_FDLIVE_ITEM {

	const string _4DREPLAY_TYPE_ = ".4ds";

	public int id;
	public string title;
	public string url;
	public string type;
	public VIDEO_CONTENT_TYPE _type;
	public int default_channel;
	public int max_channel;

	public sub_category category;
	public sub_thumbnail thumbnail;

	public MediaPlayerCtrl controler;

	public LIST_CONTENT_FDLIVE_ITEM() {

		//_prefab = new GameObject();
		bool isResult = Enum.TryParse<VIDEO_CONTENT_TYPE>(type, out _type);
	}

	internal string GETURL() {

		if(url.Contains(_4DREPLAY_TYPE_) == true) {
			return string.Format("{0}?type={1}&quality=fhd&target={2}", url, type, default_channel);
		}else {
			return url;
		}

		throw new NotImplementedException();
	}

	public void SET_CATEGOTY_KET() {

		bool result = Enum.TryParse<CATEGORY_KEY>(category.key, out category._key);

	}

	[Serializable]
	public class sub_category {
		public int id;
		public string name;
		public string key;		

		public CATEGORY_KEY _key;
	}	

	[Serializable]
	public class sub_thumbnail {
		public string url;
	}
}


[Serializable]
public class LIST_SCRIPT_LIST_ITEM_SUB {

	public RECV_TYPE type;

	public int id;
	public string name;
	public string content;
	public string filename;

	public void SET_TYPE() {
		if(!string.IsNullOrEmpty(filename)) {
			string[] _tmp = filename.Split("_"[0]);

			if(_tmp[0].Equals("cs")) {
				type = RECV_TYPE.CAMERA_SCRIPT;
			}else if(_tmp[0].Equals("tb")) {
				type = RECV_TYPE.TABLE;
			}else if(_tmp[0].Equals("l")) {
				type = RECV_TYPE.LIST;
			}
		}
	}
}

[Serializable]
public class LIST_SCRIPT_LIST_ITEM {

	public RECV_TYPE type;
	public int id;
	public string name;
	public string filename;

	public string cs_commands_data;

	public LIST_SCRIPT_LIST_ITEM() {
		
	}


	public void SET_TYPE() {
		if(!string.IsNullOrEmpty(filename)) {
			string[] _tmp = filename.Split("_"[0]);

			if(_tmp[0].Equals("cs")) {
				type = RECV_TYPE.CAMERA_SCRIPT;
			}else if(_tmp[0].Equals("tb")) {
				type = RECV_TYPE.TABLE;
			}else if(_tmp[0].Equals("l")) {
				type = RECV_TYPE.LIST;
			}
		}
	}

}


[Serializable]
public class DEFAULT_EFFECT_LIST {

	public int index;
	public string resources_path;
	public string pfb_name;
	public string etc;

	public DEFAULT_EFFECT_LIST(string[] tableData) {

		int j = 0;

		this.index = Convert.ToInt32(tableData[j ++]);
		this.resources_path = tableData[j ++];
		this.pfb_name = tableData[j ++];
		this.etc = tableData[j ++];

		resources_path = resources_path.TrimStart();
		resources_path = resources_path.TrimEnd();
		resources_path = resources_path.Trim();
		
		pfb_name = pfb_name.TrimStart();
		pfb_name = pfb_name.TrimEnd();
		pfb_name = pfb_name.Trim();

	}


	public string GET_PATH() {
		return string.Format("{0}{1}", resources_path, pfb_name);
	}
}


[Serializable]
public class DEFAULT_EFFECT_TABLE {

	public int index;
	public int power_min;
	public int power_max;
	public int effect_index;
	public DEFAULT_EFFECT_LIST _effect_index;
	public float scaleX;
	public float scaleY;
	public float scaleZ;
	public string etc;


	public DEFAULT_EFFECT_TABLE(string[] tableData) {
		
		int j = 0;

		this.index = Convert.ToInt32(tableData[j ++]);
		this.power_min = Convert.ToInt32(tableData[j ++]);
		this.power_max = Convert.ToInt32(tableData[j ++]);
		this.effect_index = Convert.ToInt32(tableData[j ++]);

		this.scaleX = Convert.ToSingle(tableData[j ++]);
		this.scaleY = Convert.ToSingle(tableData[j ++]);
		this.scaleZ = Convert.ToSingle(tableData[j ++]);

		this.etc = tableData[j ++];

	}

}


[Serializable]
public class DEFAULT_PLAYER_LIST {
	
	public int index;
	public string country;
	public string teamName;
	public string playerName;
	public int age;
	public int tall;
	public int weight;
	public string s_skill;
	public string spcial_info;
	public string thumnail_url;

	public bool isPenalty;
	public float nowPenaltyTime;

	public int yellowCardCnt;

	public DEFAULT_PLAYER_LIST(string[] tableData) {

		int j = 0;

		this.index = Convert.ToInt32(tableData[j ++]);
		this.country = tableData[j ++];
		this.teamName = tableData[j ++];
		this.playerName = tableData[j ++];

		this.age = Convert.ToInt32(tableData[j ++]);
		this.tall = Convert.ToInt32(tableData[j ++]);
		this.weight = Convert.ToInt32(tableData[j ++]);
		this.s_skill = tableData[j ++];
		this.s_skill = this.s_skill.Replace("|", "\n");
		this.spcial_info = tableData[j ++];
		this.spcial_info = this.spcial_info.Replace("|", "\n");
		this.thumnail_url = tableData[j ++];

	}
}


[Serializable]
public class GAME_INFO_TAE {

	public GAME_TYPE_TAE gameType;	//득점제 실점제

	///////////////////////////
	
	public int index;

	public string gameName;
	public string stadiumName;
	public long gameStartTime;	//기간 : 경기 시작
	public long gameEndTime;	//기간 : 경기 종료

	public string gameWeight;
	///////////////////////////

	public int maxStageCnt; //총 경기수
	public int nowStageCnt;	//경기차수	
	public int maxRoundCnt_normal;	//총 라운드수 3판 2선승이면 3
	public int maxRoudnCnt_final;

	
	public int roundTime;
	
	public bool isPlaying;
	public int nowRoundCnt;
	public float nowRoundTime;

	public ROUND_INFO_TAE[] roundInfo;	//어디서 생성하지...?


	public GAME_INFO_TAE(string[] tableData) {

		int j = 0;

		this.index = Convert.ToInt32(tableData[j ++]);
		bool result = Enum.TryParse<GAME_TYPE_TAE>(tableData[j ++], out this.gameType);
		if(result == false) {
			Debug.Log("need check GAME_TYPE_TAE");
		}
		this.gameName = tableData[j ++];
		this.stadiumName = tableData[j ++];

		this.gameStartTime = Convert.ToInt64(tableData[j ++]);
		this.gameEndTime = Convert.ToInt64(tableData[j ++]);

		this.gameWeight = tableData[j ++];

		this.maxStageCnt = Convert.ToInt32(tableData[j ++]);

		this.maxRoundCnt_normal = Convert.ToInt32(tableData[j ++]);
		this.maxRoudnCnt_final = Convert.ToInt32(tableData[j ++]);

		this.roundTime = Convert.ToInt32(tableData[j ++]);
	}

}


[Serializable]
public class ROUND_INFO_TAE {
	
	public int nowRoundCnt;	//진행중인 라운드 수

	public float prevBlueScore;
	public float prevRedScore;
		
	public float blueScore;
	public float redScore;

	public int blueWinCnt;
	public int redWinCnt;

	public DEFAULT_PLAYER_LIST blue;
	public DEFAULT_PLAYER_LIST red;

	public ROUND_INFO_TAE() {

	}

	public ROUND_INFO_TAE(DEFAULT_PLAYER_LIST blue, DEFAULT_PLAYER_LIST red) {

		this.blue = blue;
		this.red = red;

		this.blue.isPenalty = false;
		this.red.isPenalty = false;

		this.blue.yellowCardCnt = 0;
		this.red.yellowCardCnt = 0;

	}

}


[Serializable]
public class COUNTRY_CODE {

	public int index;
	public string name;
	public string alpha2Code;
	public string alpha3Code;
	public int numbericCode;
	public string isoCode;

	public COUNTRY_CODE(string[] tableData) {
		int i = 0;

		this.index = Convert.ToInt32(tableData[i++]);
		this.name = tableData[i++];
		this.alpha2Code = tableData[i++];
		this.alpha3Code = tableData[i++];
		this.numbericCode = Convert.ToInt32(tableData[i++]);
		this.isoCode = tableData[i++];

	}
}


public enum GAME_TYPE_TAE {

	NONE = -1,
	PLUS,	//득점제
	MINUS	//실점제

}


public class SEND_FDLIVE_SWIPE {

	public string sessionId;
	public string actionType;
	public string direction;
	public int speed;
	public int moveFrame;

}
