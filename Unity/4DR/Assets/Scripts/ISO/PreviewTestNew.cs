using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using UnityEngine.Assertions;

public class PreviewTestNew : MonoBehaviour {

    AndroidJavaClass androidNativeCam;
    AndroidJavaObject androidNativeCamActivity;    

    public Texture2D camTexture;

    private int texWidth;
    private int texHeight;


    Appandroid appandroid;

    private AndroidJavaObject javaClass;

    public Renderer _renderer;

    public static bool isUpdate;

    // Use this for initialization
    public void _Start() {  
#if UNITY_ANDROID

        //appandroid = (Appandroid)FindObjectOfType<Appandroid>();

        ////
        //appandroid.call_open1();

        //javaClass = new AndroidJavaObject("com.fdp.fdplibrary4unity.FdpMain");

        Debug.Log("PreviewTestNew :::::::::: start");
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
            using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity")) {
                 //find the plugin
                //AndroidJNI.AttachCurrentThread();
                //androidNativeCam = new AndroidJavaObject("com.fdp.fdplibrary4unity.FdpMain");
                //androidNativeCam = new AndroidJavaClass("com.fdp.fdplibrary4unity.FdpMain.UnityPlayerNativeActivity");
                //Assert.IsNotNull(androidNativeCam);
                //androidNativeCamActivity = javaClass.GetStatic<AndroidJavaObject>("mContext");

                //start cam (this will generate a texture on the java side)
                Debug.Log("1111111111");
                int nativeTextureID = jo.Call<int>("startCamera");
                Debug.Log("22222222");
                texWidth = 256;//androidNativeCamActivity.Call<int>("getPreviewSizeWidth");
                texHeight = 256;//androidNativeCamActivity.Call<int>("getPreviewSizeHeight");
        
                Debug.Log("3333333");
                Assert.IsTrue(nativeTextureID > 0, "nativeTextureID=" + nativeTextureID);
                Assert.IsTrue(nativeTextureID > 0, "width=" + texWidth);
                Assert.IsTrue(nativeTextureID > 0, "height=" + texHeight);
        
                Debug.Log("44444444 : nativeTextureID == " + nativeTextureID);
                camTexture = Texture2D.CreateExternalTexture(texWidth, texHeight, TextureFormat.YUY2, false, true, new IntPtr(nativeTextureID));
                _renderer.material.mainTexture = camTexture; // TODO this line causes the error
                Debug.Log("555555555");
            }
        }

        Debug.Log("PreviewTestNew :::::::::: end");

        isUpdate = true;
       
#endif
    }

    // Update is called once per frame
    void Update() {

        if(isUpdate == false) return;
        //if (texWidth != camTexture.width || texHeight != camTexture.height)
        //{
        //    Debug.LogWarning("texWidth != camTexture.width || texHeight != camTexture.height");
        //    camTexture.Resize(texWidth, texHeight, TextureFormat.YUY2, false);
        //    camTexture.Apply();
        //}
        try {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
            using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity")) {
                    jo.Call("updateTexture");
                }
            }
        }catch(Exception e) {

        }
        //transform.Rotate(Time.deltaTime * 10, Time.deltaTime * 30, 0);
    }

    //void OnGUI() {
    //    GUI.Label(new Rect(10, 10, Screen.width - 20, Screen.height - 20), msg);
    //}
}