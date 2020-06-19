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
		version2 = "3";

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
public class LIST_CONTENT_FDLIVE_ITEM {

	const string _4DREPLAY_TYPE_ = ".4ds";

	public int id;
	public string title;
	public string url;
	public string type;
	public int default_channel;
	public int max_channel;

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
}

//MAP

//BLOCK

//Game Stage Info

//User Info