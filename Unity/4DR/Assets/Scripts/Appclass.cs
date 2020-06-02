using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class Appclass : MonoBehaviour {

	public LIST_CONTENT_FDLIVE _list_conent_fdlist;

}

[Serializable]
public class  LIST_CONTENT_FDLIVE {

	public List<LIST_CONTENT_FDLIVE_ITEM> result;
}

[Serializable]
public class LIST_CONTENT_FDLIVE_ITEM {

	public int id;
	public string title;
	public string url;
	public string type;

	public LIST_CONTENT_FDLIVE_ITEM() {

	}

	internal string GETURL() {

		return string.Format("{0}?type={1}", url, type);
		throw new NotImplementedException();
	}
}

//MAP

//BLOCK

//Game Stage Info

//User Info