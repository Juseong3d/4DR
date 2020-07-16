using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class Appclass : MonoBehaviour {

	public LIST_CONTENT_FDLIVE _list_conent_fdlist;
	public LIST_SCRIPT_LIST _list_script_list;
}


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
		version1 = "3";
		version2 = "6";

		appVersion = "V." + version0 + "." + version1 + "." + version2;
		appVersion += ".PROTO";
#if _DIRECT_URL_
		appVersion += ".D";
#endif

	}
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
	public int default_channel;
	public int max_channel;

	public sub_category category;
	public sub_thumbnail thumbnail;

	public MediaPlayerCtrl controler;

	public LIST_CONTENT_FDLIVE_ITEM() {

		//_prefab = new GameObject();
	}

	internal string GETURL() {

		if(url.Contains(_4DREPLAY_TYPE_) == true) {
			return string.Format("{0}?type={1}&quality=fhd&target={2}", url, type, default_channel);
		}else {
			return url;
		}

		throw new NotImplementedException();
	}

	[Serializable]
	public class sub_category {
		public int id;
		public string name;
	}

	[Serializable]
	public class sub_thumbnail {
		public string url;
	}
}


[Serializable]
public class LIST_SCRIPT_LIST_ITEM_SUB {

	public int id;
	public string name;
	public string content;
	public string filename;
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

//MAP

//BLOCK

//Game Stage Info

//User Info


[Serializable]
public class TEMP_TEXTURE {

	public string url;
	public Texture texture;

	public TEMP_TEXTURE() {
		url = string.Empty;		
	}

}
