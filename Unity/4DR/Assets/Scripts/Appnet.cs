using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine.Networking;

public enum NET_STATUS {
	
	NONE = 9999,
	SEND_WAIT = 10000,
	SEND_WAIT_BACK = 10100,		//무시하고 보낸다.
	RECV_COMP = 11000,
	RECV_COMP_ERROR = 11010,
	RECV_COMP_BACK = 11100
}


[Serializable] 
public class XML_DATA {

	public string[] paramName;
	public string[] value;
	int paramCnt;
	//HtmlDecoder htmlDecoder;
	ArrayList listParam;
	int listCnt;

	public XML_DATA(string src) {

		//htmlDecoder = new HtmlDecoder();
		//Debug.Log("xml data parsring src = " + src);
		listParam = new ArrayList();
		_GO(src);
	}

	void _GO(string src) {

		string[] tmpCut = src.Trim().Split("\n"[0]);
		
		//string[] tmpCut;
		//int beforeIndex = 0;
		//int lastIndex = 0;
		//int maxcnt = 0;
		int i = 0;

		//src = src.Trim();

		//for(i = 0; i<src.Length; i+=lastIndex) {
		//    lastIndex = src.LastIndexOf(">\n", lastIndex);
		//    maxcnt ++;
		//}

		//tmpCut = new string[maxcnt];
		
		//lastIndex = 0;

		//for(i = 0; i<maxcnt; i++) {
		//    beforeIndex = lastIndex;
		//    lastIndex = src.LastIndexOf(">\n", lastIndex);
		//    tmpCut[i] = src.Substring(beforeIndex, lastIndex - beforeIndex);
		//    Debug.Log(tmpCut[i]);
		//}


		if(tmpCut.Length > 2) {
			//setParamCnt(tmpCut.Length - 3);		
			setParamCnt(tmpCut.Length - 2);		//cmd 까지 가지고 있도록 수정 20120827

			for(i = 0; i<paramCnt; i++) {
				//setParam(i, tmpCut[i + 2]);
				setParam(i, tmpCut[i + 1]);		//cmd 까지 가지고 있도록 수정 20120827
			}
		}
	}


	void setParamCnt(int cnt) {

		paramCnt = cnt;
		
		if(paramCnt > 0) {
			paramName = new string[paramCnt];
			value = new string[paramCnt];
		}

	}


	void setParam(int idx, string src) {

		string[] tmpData = src.Split(">"[0]);
		
		//for testing...s
		//for(int i = 0; i<tmpData.Length; i++) {
		//	Debug.Log("tmpData[i] = " + tmpData[i]);
		//}
		//for testing...e
			
		paramName[idx] = tmpData[0].Trim().Substring(1);

		int valueLength = tmpData[1].Length - (paramName[idx].Length + 2);

		if(valueLength < 0) {
			value[idx] = "-1";
			//Debug.Log("valueLength		:: " + valueLength);
		}else {
			value[idx] = tmpData[1].Substring(0, valueLength);
		}
	}


	internal string getValue(string name, int idx = 0) {

		bool find = false;
		int findCnt = 0;
		int i = 0;

		//Debug.Log("getValue = " + getValue);

		for(i = 0; i<paramCnt; i++) {
			if(name.Equals(paramName[i])) {
				
				findCnt ++;

				if(findCnt == idx + 1) {
					find = true;
					break;
				}
			}
		}

		if(find == true) {
			//Debug.Log("b value[i] = " + value[i]);
			//string decodeString = htmlDecoder.HtmlDecode(value[i]);
			string decodeString = value[i];//WWW.UnEscapeURL(value[i]);

			//Debug.Log("n value[i] = " + decodeString);
			return decodeString;
		}

		return name + " [CHECK_NO_VALUE]";
	}


	internal int getValueCnt(string name) {

		int i = 0;
		int findCnt = 0;

		for(i = 0; i<paramCnt; i++) {
			if(name.Equals(paramName[i])) {				
				findCnt ++;
			}
		}

		return findCnt;

	}


	internal int getValueToInt32(string name, int idx = 0) {

		bool find = false;
		int findCnt = 0;
		int i = 0;

		//Debug.Log("getValue = " + getValue);

		for(i = 0; i<paramCnt; i++) {
			if(name.Equals(paramName[i])) {
				
				findCnt ++;

				if(findCnt == idx + 1) {
					find = true;
					break;
				}
			}
		}

		if(find == true) {
			//Debug.Log("b value[i] = " + value[i]);
			//string decodeString = htmlDecoder.HtmlDecode(value[i]);

			int decodeString = -99999;			
			bool result = Int32.TryParse(value[i], out decodeString);//Convert.ToInt32(value[i]);//WWW.UnEscapeURL(value[i]);

			//Debug.Log("n value[i] = " + decodeString);
			return decodeString;
		}

		return -999999;
	}


	internal long getValueToInt64(string name, int idx = 0) {

		bool find = false;
		int findCnt = 0;
		int i = 0;

		//Debug.Log("getValue = " + getValue);

		for(i = 0; i<paramCnt; i++) {
			if(name.Equals(paramName[i])) {
				
				findCnt ++;

				if(findCnt == idx + 1) {
					find = true;
					break;
				}
			}
		}

		if(find == true) {
			//Debug.Log("b value[i] = " + value[i]);
			//string decodeString = htmlDecoder.HtmlDecode(value[i]);
			long decodeString = -99999;			
			bool result = Int64.TryParse(value[i], out decodeString);			

			//Debug.Log("n value[i] = " + decodeString);
			return decodeString;
		}

		return -999999;
	}
	
}


[Serializable] 
public class NETWORK_DATA {

	public string functionName;
	public string sendDataStr;
	public XML_DATA sendData;
	public string tmpRecvData;
	public XML_DATA[] recvData;	

	public float recvTime;

	public GameObject eventReceiver;
	public string eventFuncName;

	public NETWORK_DATA() {
		//init();
		functionName = "";
		sendDataStr = "";
		sendData = null;
		tmpRecvData = "";
		recvData = null;
	}

	public NETWORK_DATA(NETWORK_DATA nData)
	{
		this.sendDataStr = nData.sendDataStr;
		this.sendData = nData.sendData;
		this.functionName = nData.functionName;
		this.eventReceiver = nData.eventReceiver;
		this.eventFuncName = nData.eventFuncName;
		this.recvData = null;
		this.tmpRecvData = nData.tmpRecvData;
	}

	public void init() {
		eventReceiver = Appmain.MAIN_GAMEOBJECT;
		eventFuncName = "onRecvNetErrorFunc";
	}
}


public class Appnet : MonoBehaviour {

	enum WEB_TYPE {

		APPLICATION_XML,
		MULTIPART_FORMED_DATA,
		TEXT_XML,
		OCTET_STREAM,
		ACCEPT_JSON

	}	

	public string _IP;
	public string _URL;

	WWWForm _form;
#if UNITY_5_3_OR_NEWER
	 Dictionary<string, string> _headers;// = new Dictionary<string,string>();
#else
	Hashtable _headers;
#endif

	public string AUTH_URL_CHECK;
	public string AUTH_URL_CHECK_RETURN;
	public string AUTH_URL_RESULT;
	public string AUTH_URL_RETRY;

	public Queue<NETWORK_DATA> sendNetworkQueue;

	public int netStatusCnt; 
	public NET_STATUS netStatus;

	public string _COOKIE_ID;
	public string _USER_SEQ;
	public string _USER_ID;
	public string _USER_SOCKET_ID;

	public string _AUTH_REQ_INFO;
	public string _AUTH_STR_SEQUENCE;
	public int _AUTH_SEND_TRY_CNT;


	public string _IAB_ORDER_ID;
	public string _IAB_PACKAGE_NAME;
	public string _IAB_TOKEN;

	public NETWORK_DATA tmpNetWorkData;

	Appmain appmain;
	Appdoc appdoc;
	Appimg appimg;
	Appevent appevent;
    Appsound appsound;

	// Use this for initialization
	void Start () {

		appmain = GetComponent<Appmain>();
		appdoc = GetComponent<Appdoc>();
		appimg = GetComponent<Appimg>();
		appsound = (Appsound)GetComponent<Appsound>();
		appevent = (Appevent)GetComponent<Appevent>();

		_IP = "110.45.132.199:7888/web_api.php?xml=";
		
		_URL = "http://" + _IP;

		initNetwork();

		_URL = "https://work.muteklab.com:50443/admin/content/unity/api";
		__WEB_CONNECT_AND_SEND_RECV_4_FAST_JSON(string.Empty);		

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void initNetwork() {

		if(this.sendNetworkQueue != null) sendNetworkQueue = null;

		this.sendNetworkQueue = new Queue<NETWORK_DATA>();

		netStatus = NET_STATUS.NONE;
		netStatusCnt = 0;
		_USER_SEQ = string.Empty;

	}


	void __WEB_CONNECT_SET_HEADER_EX(WEB_TYPE type) {
	
		//Debug.Log("call __WEB_CONNECT_SET_HEADER : " + type);

		_form = null;

		_form = new WWWForm();
		_headers = _form.headers;

#if UNITY_5_3_OR_NEWER

		if(type == WEB_TYPE.MULTIPART_FORMED_DATA) {
			_headers.Add("Content-Type", "multipart/formed-data");
		}else if(type == WEB_TYPE.APPLICATION_XML) {
			_headers.Add("Content-Type", "application/xml");
		}else if(type == WEB_TYPE.TEXT_XML) {
			_headers.Add("Content-Type", "text/xml");
		}else if (type == WEB_TYPE.OCTET_STREAM) {
			_headers.Add("Content-Type", "application/octet-stream");
		}else if (type == WEB_TYPE.ACCEPT_JSON) {
			_headers.Add("Accept", "application/json");
		}
#else
		_headers = _form.headers;

		if(type == WEB_TYPE.MULTIPART_FORMED_DATA) {
			_headers["Content-Type"] = "multipart/formed-data";			
		}else if(type == WEB_TYPE.APPLICATION_XML) {
			_headers["Content-Type"] = "application/xml";					
		}else if(type == WEB_TYPE.TEXT_XML) {
			_headers["Content-Type"] = "text/xml";			
		}else if (type == WEB_TYPE.OCTET_STREAM)		{
			_headers["Content-Type"] = "application/octet-stream";			
		}
#endif
	}


	public WWW GET(string url) {

		//string url = WWW.EscapeURL(_url);		
		WWW www = new WWW (url);		

#if UNITY_EDITOR
		Debug.Log("SEND GET :\n" + url);
#endif
		StartCoroutine (WaitForRequest (www));

		return www; 
    }


	 private IEnumerator WaitForRequest(WWW www) {
        yield return www;

        // check for errors
        if (www.error == null) {
			NETWORK_DATA networkData = new NETWORK_DATA();

			//if (www.responseHeaders.ContainsKey("SET-COOKIE")){
			//	_COOKIE_ID = www.responseHeaders["SET-COOKIE"];
			//}

            //Debug.Log("WWW Ok!: " + www.text);
			networkData.tmpRecvData = www.text;

#if UNITY_EDITOR
			Debug.Log("RECV TEXT :\n" + www.text);
#endif
			//{
			//	int cnt = __GET_XML_COUNT(networkData.tmpRecvData);
			//	int i = 0;

			//	networkData.recvData = new XML_DATA[cnt];
			//	for (i = 0; i < cnt; i++) {
			//		string tmpcutString = __CUT_XML(networkData.tmpRecvData, i + 1);

			//		//Debug.Log("-----------" + i);
			//		//Debug.Log(tmpcutString);
			//		networkData.recvData[i] = new XML_DATA(tmpcutString);
			//	}
			//}	

			//recvCompleate4Web(networkData);		
			tmpNetWorkData = networkData;
			this.netStatus = NET_STATUS.NONE;

        } else {
            Debug.Log("WWW Error: "+ www.error);
        }    
    }


	IEnumerator __WEB_CONNECT_AND_SEND_RECV(NETWORK_DATA netData = null, string anotherURL = "") {

		NETWORK_DATA networkData = null;
		bool noDequeue = false;

		if(netData != null)
			networkData = new NETWORK_DATA(netData);
		else
			networkData = this.sendNetworkQueue.Peek();
		
		WWW www;

		if(string.IsNullOrEmpty(anotherURL) == true) {
			__WEB_CONNECT_SET_HEADER_EX(WEB_TYPE.TEXT_XML);
			_form.AddField("xml", networkData.sendDataStr);
			_headers.Add("Content-Length", networkData.sendDataStr.Length.ToString());
			_headers.Add("Host", _URL);
			www = new WWW(_URL, _form);

		}else {
			__WEB_CONNECT_SET_HEADER_EX(WEB_TYPE.TEXT_XML);			
			_form.AddField("xml", networkData.sendDataStr);
			_headers.Add("Content-Length", networkData.sendDataStr.Length.ToString());
			_headers.Add("Host", anotherURL);
			www = new WWW(anotherURL, _form);

			noDequeue = true;
		}

		this.netStatus = NET_STATUS.SEND_WAIT;
		yield return www;
		this.netStatus = NET_STATUS.RECV_COMP;	
		
		if(!www.isDone || www.error != null) {
			string trimError = www.error.Trim();
		
			Debug.Log("www.error : " + trimError);
			if (!String.IsNullOrEmpty(trimError)) {
				
				this.netStatus = NET_STATUS.RECV_COMP_ERROR;
				//print("www.error :\n" + www.error);
				//PopupBox.Create(DEFINE.LZ_SOCKET_SERVER_DISCONNECT_ERROR, "", POPUPBOX_ACTION_TYPE.OK, null, "", false, true, SOUND_EFFECT_TYPE.POPUP_WARNING,LOCALIZINGDEFINE.DISCCONECT_ERROR);
			}
		}else if(www.isDone && www.error == null) {
			networkData.tmpRecvData = www.text;

//#if UNITY_EDITOR
			Debug.Log("RECV TEXT :\n" + www.text);
//#endif
			{
				int cnt = __GET_XML_COUNT(networkData.tmpRecvData);
				int i = 0;

				networkData.recvData = new XML_DATA[cnt];
				for (i = 0; i < cnt; i++) {
					string tmpcutString = __CUT_XML(networkData.tmpRecvData, i + 1);

					//Debug.Log("-----------" + i);
					//Debug.Log(tmpcutString);
					networkData.recvData[i] = new XML_DATA(tmpcutString);
				}
			}	

			recvCompleate4Web(networkData);		
			tmpNetWorkData = networkData;
			this.netStatus = NET_STATUS.NONE;

			if(noDequeue == false) this.sendNetworkQueue.Dequeue(); 
		}
	}


	internal void __WEB_CONNECT_AND_SEND_RECV_4_FAST(string sendData) {

		NETWORK_DATA networkData = new NETWORK_DATA();
		networkData.init();

		networkData.sendDataStr = sendData;
		networkData.sendData = new XML_DATA(sendData);
		networkData.functionName = networkData.sendData.getValue("cmd");
		
		WWW www;

		{
			__WEB_CONNECT_SET_HEADER_EX(WEB_TYPE.TEXT_XML);
			_form.AddField("xml", networkData.sendDataStr);
			_headers.Add("Content-Length", networkData.sendDataStr.Length.ToString());
			_headers.Add("Host", _URL);
			www = new WWW(_URL, _form);

		}		
	}


	internal void __WEB_CONNECT_AND_SEND_RECV_4_FAST_JSON(string sendData) {

		NETWORK_DATA networkData = new NETWORK_DATA();
		networkData.init();

		networkData.sendDataStr = sendData;
		//networkData.sendData = new XML_DATA(sendData);
		//networkData.functionName = networkData.sendData.getValue("cmd");
		
		//WWW www;

		{
			//__WEB_CONNECT_SET_HEADER_EX(WEB_TYPE.ACCEPT_JSON);
			//_form.AddField("json", networkData.sendDataStr);			
			//_form.headers.Add("Content-Length", networkData.sendDataStr.Length.ToString());
			//_form.headers.Add("Host", _URL);

			//Debug.Log("_URL : " + _URL);
			//www = new WWW(_URL, _form);

			//StartCoroutine (WaitForRequest (www));

			Dictionary<string, string> headers = new Dictionary<string,string>();
			headers.Add("Accept", "application/json");

			WWW www = new WWW(_URL, null, headers);

			StartCoroutine (WaitForRequest (www));			
		}		
	}


	private void recvCompleate4Web(NETWORK_DATA networkData) {

		int i = 0;
		bool isError = false;

#if _TEST_SERVER_
		networkData.eventReceiver.SendMessage(networkData.eventFuncName, networkData, SendMessageOptions.DontRequireReceiver);
		return;
#endif

		//for (i = 0; i < networkData.recvData.Length; i++) 
		{

			string whatCmd = networkData.recvData[i].getValue("cmd");			
			int errcode = networkData.recvData[i].getValueToInt32("errcode");
			string out_errormessage = networkData.recvData[i].getValue("out_errormessage");

			Debug.Log("### : " + whatCmd);
			if(!string.IsNullOrEmpty(out_errormessage)) {

			}			
		}
		
		if (networkData.eventReceiver != null && !networkData.eventFuncName.Equals("")) {
			if(isError == false) {
				networkData.eventReceiver.SendMessage(networkData.eventFuncName, networkData, SendMessageOptions.DontRequireReceiver);
			}
		}
	}


	void FixedUpdate () {
		
		netStatusCnt ++;
		processNetworkData();
	
	}	


	//internal void setNetSendData(string sendData) {
	//	setNetSendData(sendData, null, "");
	//}


	internal void setNetSendData(string sendData, GameObject eventReceiver = null, string eventFuncName = "") {
		NETWORK_DATA networkData = new NETWORK_DATA();
		networkData.init();
		networkData.sendDataStr = sendData;
		networkData.sendData = new XML_DATA(sendData);
		networkData.functionName = networkData.sendData.getValue("cmd");
		networkData.eventReceiver = eventReceiver;
		networkData.eventFuncName = eventFuncName;

		this.sendNetworkQueue.Enqueue(networkData);
	}


	//큐 잡고 순차적으로 보내도록 해야하고..
	internal void processNetworkData() {

		if (this.netStatus == NET_STATUS.NONE) {
			if(this.sendNetworkQueue != null) {
				if (this.sendNetworkQueue.Count > 0)
					StartCoroutine(__WEB_CONNECT_AND_SEND_RECV());
			}

		}
	}


	//-------------------------------------------------------------------------------
	internal int __GET_XML_COUNT(string str) {

		string[] tmpString = str.Trim().Split("\n"[0]);
		
		int i = 0;
		int cnt = 0;

		for(i = 0; i<tmpString.Length; i++) {

			if(tmpString[i].Equals(__WEB_XML(1)) == true) {
				cnt ++;
			}
		}

		return cnt;

	}


	internal string __CUT_XML(string str, int what) {

		string tmpString = "";
		int cnt = 0;
		int i = 0;
		int beforewhere = 0;
		int where = 0;
		string lastString = __WEB_XML(1).Trim();

		//Debug.Log("[" + lastString + "]");
		//Debug.Log("str.length = " + str.Length);

		for(i = 0; i<what; i++) {
			
			beforewhere = where;
			where = str.IndexOf(lastString, beforewhere) + lastString.Length;

			//Debug.Log("beforewhere = " + beforewhere);
			//Debug.Log("where = " + where);
		}

		return str.Substring(beforewhere, where - beforewhere);

	}

	//-------------
	internal string __WEB_SEND_GET_HEADER(string xmlVersion, string encoding) {

		string sad = "<?xml version=\"" + xmlVersion + "\" encoding=\"" + encoding + "\"?>\n";

		return sad;

	}


	internal string __WEB_API_VALUE(int s){

		string rtnString = "<api_value>\n";
		string rtnStringe = "</api_value>\n";

		if(s == 0)
			return rtnString;		

		return rtnStringe;

	}


	internal string __WEB_XML(int s){

		string rtnString = "<xml>\n";
		string rtnStringe = "</xml>";

		if(s == 0)
			return rtnString;		

		return rtnStringe;

	}


	internal int GET_CRC(string sendData, int seed) {

		int sum = 0;
		string tmpString = sendData;
		byte[] sendData4Byte = System.Text.Encoding.UTF8.GetBytes(tmpString);
		int len = sendData4Byte.Length;
		int i = 0;

		for(i = 0; i<len; i++) {
			sum += (sendData4Byte[i] + i + seed);
		}

		return sum % 10000;

	}


	internal string __WEB_XML(int s, string sendData){

		string rtnString = "<xml>\n";
		string rtnStringe = "</xml>";

		//////
		//CRC 처리에서는 마지막줄 </xml> 에서 \n을 빼야 함.
		/////

		if(s == 0) {
			return rtnString;		
		}else {


			//for testing...
			string testdbors = "";//__WEB_XML_VALUE("dbros", UnityEngine.Random.RandomRange(1000000, 10000000).ToString());

			sendData = testdbors + sendData;

			string tmpSendData = sendData + __WEB_XML_VALUE("c8c", "0") + rtnStringe;

			int len = GET_CRC(tmpSendData, 0);
			string tmpString = __WEB_XML_VALUE("c8c", len + "");
			
			return testdbors + tmpString + rtnStringe;
		}
	}


	internal string __WEB_API_VALUE(int s, string cmd) {

		string rtnString = "<" + cmd + ">\n";
		string rtnStringe = "</" + cmd + ">\n";

		if(s == 0)
			return rtnString;		
			
		return rtnStringe;

	}


	internal string __WEB_API_VALUE(string cmd, string value) {

		string rtnString = "<" + cmd + ">" + value + "</" + cmd + ">\n";	

		return rtnString;

	}


	internal string __WEB_XML_VALUE(string cmd, string value) {

		string rtnString = "<" + cmd + ">" + value + "</" + cmd + ">\n";	

		return rtnString;

	}


	internal string __WEB_XML_VALUE(string cmd, int value) {

		string rtnString = "<" + cmd + ">" + value + "</" + cmd + ">\n";	

		return rtnString;

	}


	internal string __WEB_API_FUNCTION(string cmd) {

		string rtnString = "<function>" + cmd + "</function>\n";	

		return rtnString;

	}


	internal string __WEB_XML_CMD(string cmd) {

		string rtnString = "<cmd>" + cmd + "</cmd>\n";	
		// 모든 패킷에 클라이언트 타임스템프를 보낸다.
		//rtnString += __WEB_XML_VALUE("cts", string.Format("{0:0}",GET_CLIENT_TS()));
		
		return rtnString;

	}


	internal string GET_UID() {

		return SystemInfo.deviceUniqueIdentifier;

	}


//	internal void test() {

//		string tmpString = "";

//		tmpString += __WEB_XML(0);

//		tmpString += __WEB_XML_CMD("login");

//		tmpString += __WEB_XML_VALUE("user_id", "test2");
//		tmpString += __WEB_XML_VALUE("user_passwd", "test2");
//		tmpString += __WEB_XML_VALUE("user_channel", "1");
//		tmpString += __WEB_XML_VALUE("input_device_no", "1");
//		tmpString += __WEB_XML_VALUE("input_mac_address", "1");
//		tmpString += __WEB_XML_VALUE("input_r_id", "1");

//		tmpString += __WEB_XML(1, tmpString);

//#if UNITY_EDITOR
//		Debug.Log("send [test] = \n" + tmpString);
//#endif

//		setNetSendData(tmpString);
//	}


//	internal void test2() {

//		string tmpString = "";

//		tmpString += __WEB_XML(0);

//		tmpString += __WEB_XML_CMD("login");

//		tmpString += __WEB_XML_VALUE("user_id", "keeee@naver.com");
//		tmpString += __WEB_XML_VALUE("user_passwd", "sdsdsd");
//		tmpString += __WEB_XML_VALUE("user_channel", "1");
//		tmpString += __WEB_XML_VALUE("input_device_no", "1");
//		tmpString += __WEB_XML_VALUE("input_mac_address", "1");
//		tmpString += __WEB_XML_VALUE("input_r_id", "1");

//		tmpString += __WEB_XML(1, tmpString);

//#if UNITY_EDITOR
//		Debug.Log("send [test] = \n" + tmpString);
//#endif

//		setNetSendData(tmpString);
//	}

//	internal void test3() {

//		string tmpString = "";

//		tmpString += __WEB_XML(0);

//		tmpString += __WEB_XML_CMD("login");

//		tmpString += __WEB_XML_VALUE("user_id", "sdinfotest");
//		tmpString += __WEB_XML_VALUE("user_passwd", "sdinfotest");
//		tmpString += __WEB_XML_VALUE("user_channel", "1");
//		tmpString += __WEB_XML_VALUE("input_device_no", "1");
//		tmpString += __WEB_XML_VALUE("input_mac_address", "1");
//		tmpString += __WEB_XML_VALUE("input_r_id", "1");

//		tmpString += __WEB_XML(1, tmpString);

//#if UNITY_EDITOR
//		Debug.Log("send [test] = \n" + tmpString);
//#endif

//		setNetSendData(tmpString);
//	}


//	internal void auth(string uid, string ver)
//	{

//		string tmpString = "";//appnet.__WEB_SEND_GET_HEADER("1.0", "UTF-8");

//		tmpString += __WEB_XML(0);

//		tmpString += __WEB_XML_CMD("auth");

//		tmpString += __WEB_XML_VALUE("uid", uid);
//		tmpString += __WEB_XML_VALUE("ver", ver);

//		//for testing...
//		//tmpString += __WEB_XML_VALUE("debug_reset", "1");

//		tmpString += __WEB_XML(1, tmpString);

//#if UNITY_EDITOR
//		Debug.Log("send [auth] = " + tmpString);
//#endif

//		setNetSendData(tmpString);
//	}
	
}

