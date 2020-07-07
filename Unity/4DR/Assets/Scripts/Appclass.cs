using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class Appclass : MonoBehaviour {

	public LIST_CONTENT_FDLIVE _list_conent_fdlist;

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
		version2 = "5";

		appVersion = "V." + version0 + "." + version1 + "." + version2;
		appVersion += "2.PROTO";
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
public class LIST_CONTENT_FDLIVE_ITEM {

	const string _4DREPLAY_TYPE_ = ".4ds";

	public int id;
	public string title;
	public string url;
	public string type;
	public int default_channel;
	public int max_channel;

	public sub_category category;

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