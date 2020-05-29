using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Assertions;

public class Appandroid : MonoBehaviour {
		
	// Use this for initialization
	void Start () {		
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void call_getContactList() {
#if UNITY_ANDROID
		try {
			using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
				using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity")) {
					Debug.Log("CallAndroid : getContactList");
					jo.Call("getContactList");
				}
			}
		}
		catch (Exception e)
		{
			Debug.Log(e.StackTrace);
		}
#endif

	}

//	public void call_SetVideoSize() {
//		Debug.Log("CallAndroid : call_TEST");
//#if UNITY_ANDROID
//		try {
//			using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
//				using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity")) {					
//					Debug.Log("CallAndroid : setVideoSize");
//					//jo.Call("TEST");

//					jo.Call("setVideoSize", new object[] { 1280, 720 });
//				}
//			}
//		}
//		catch (Exception e)
//		{
//			Debug.Log(e.StackTrace);
//		}		
//#endif

//	}


//	public void call_SET_SURFACEVIEW_Z() {
//		Debug.Log("CallAndroid : SET_SURFACEVIEW_Z");
//#if UNITY_ANDROID
//		try {
//			using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
//				using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity")) {					
//					Debug.Log("CallAndroid : SET_SURFACEVIEW_Z");
//					//jo.Call("TEST");

//					jo.Call("SET_SURFACEVIEW_Z", new object[] { VIEW_Z });

//					VIEW_Z = !VIEW_Z;
//				}
//			}
//		}
//		catch (Exception e)
//		{
//			Debug.Log(e.StackTrace);
//		}		
//#endif

//	}

//	public void call_open1() {
//		Debug.Log("CallAndroid : call_open1");
//#if UNITY_ANDROID
//		try {
//			using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
//				using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity")) {
//					Debug.Log("CallAndroid : call_open1");
//					//public void open1(String _url, boolean _isTCP, boolean _isHWDec, int _port) {
//					string _url = "rtsp://app.4dlive.kr/vod_test01.4ds?type=vod&quality=fhd&target=15";
//					bool _isTCP = true;
//					bool _isHWDec = false;
//					int _port = 7070;

//					jo.Call("open1", new object[4] { _url, _isTCP, _isHWDec, _port });

//					PreviewTestNew.isUpdate = true;
//				}
//			}
//		}
//		catch (Exception e)
//		{
//			Debug.Log(e.StackTrace);
//		}
//#endif
//	}


//	public void _SET_TEST() {

//#if UNITY_ANDROID
//		try {
//			Debug.Log("PreviewTestNew :::::::::: start");
//			using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
//				using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity")) {
//					 //find the plugin
//					//AndroidJNI.AttachCurrentThread();
//					//androidNativeCam = new AndroidJavaObject("com.fdp.fdplibrary4unity.FdpMain");
//					//androidNativeCam = new AndroidJavaClass("com.fdp.fdplibrary4unity.FdpMain.UnityPlayerNativeActivity");
//					//Assert.IsNotNull(androidNativeCam);
//					//androidNativeCamActivity = javaClass.GetStatic<AndroidJavaObject>("mContext");

//					//start cam (this will generate a texture on the java side)
//					Debug.Log("1111111111");
//					int nativeTextureID = jo.Call<int>("PutBitmapInGraphicsCard");
//					Debug.Log("22222222");
//					texWidth = 256;//androidNativeCamActivity.Call<int>("getPreviewSizeWidth");
//					texHeight = 256;//androidNativeCamActivity.Call<int>("getPreviewSizeHeight");
        
//					Debug.Log("3333333");
//					Assert.IsTrue(nativeTextureID > 0, "nativeTextureID=" + nativeTextureID);
//					Assert.IsTrue(nativeTextureID > 0, "width=" + texWidth);
//					Assert.IsTrue(nativeTextureID > 0, "height=" + texHeight);
        
//					Debug.Log("44444444 : nativeTextureID == " + nativeTextureID);
//					camTexture = Texture2D.CreateExternalTexture(texWidth, texHeight, TextureFormat.YUY2, false, true, new IntPtr(nativeTextureID));
//					_renderer.material.mainTexture = camTexture; // TODO this line causes the error
//					Debug.Log("555555555");
//				}
//			}
//		}catch (Exception e) {
//			Debug.Log(e.StackTrace);
//		}
//#endif

//	}


	public void fromAndroidTest(string _value) {

		//Int32 texPtr = Int32.Parse(_value);

		//Debug.Log("_value :::: " + _value);
		////_renderer.material.mainTexture = _tex;

		//Texture2D nativeTexture = Texture2D.CreateExternalTexture(256, 256, TextureFormat.ARGB32, false, false, (IntPtr)texPtr);
		//// Update it with the new PTR
		//nativeTexture.UpdateExternalTexture(nativeTexture.GetNativeTexturePtr());

		//_renderer.material.mainTexture = nativeTexture;

		//byte[] _byteArray = Encoding.UTF8.GetBytes(_value);

		//Debug.Log("fromAndroidTest :: " + _value);
				
		//if(camTexture != null) {
		//	camTexture.LoadRawTextureData(_byteArray);
		//	camTexture.Apply();
		//}


		//Debug.Log("fromAndroidTest :: done");

	}


//	public void call_GETVIEWBITMAP() {

//#if UNITY_ANDROID
//		try {
//			using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
//				using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity")) {

//					byte[] _tex = jo.Call<byte[]>("getViewBitmap");

//					SET_TESTURE___(_tex);
//				}
//			}
//		}catch (Exception e) {
//			Debug.Log(e.StackTrace);
//		}
//#endif


//	}


//	public void SET_TESTURE___(byte[] _value) {

//		//Int32 texPtr = Int32.Parse(_value);

//		//Debug.Log("_value :::: " + _value);
//		////_renderer.material.mainTexture = _tex;

//		//Texture2D nativeTexture = Texture2D.CreateExternalTexture(256, 256, TextureFormat.ARGB32, false, false, (IntPtr)texPtr);
//		//// Update it with the new PTR
//		//nativeTexture.UpdateExternalTexture(nativeTexture.GetNativeTexturePtr());

//		//_renderer.material.mainTexture = nativeTexture;

//		//byte[] _byteArray = Encoding.UTF8.GetBytes(_value);

//		Debug.Log("fromAndroidTest :: " + _value);
				
//		if(camTexture != null) {
//			camTexture.LoadRawTextureData(_value);
//			camTexture.Apply();
//		}


//		Debug.Log("fromAndroidTest :: done");

//	}


}
