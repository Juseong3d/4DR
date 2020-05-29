package com.fdp.fdpplayer4unityb;

import android.Manifest;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.NativeActivity;
import android.content.Context;
import android.content.pm.PackageManager;
import android.content.res.Configuration;
import android.graphics.Bitmap;
import android.graphics.Camera;
import android.graphics.Canvas;
import android.graphics.PixelFormat;
import android.graphics.Rect;
import android.graphics.SurfaceTexture;
import android.opengl.EGL14;
import android.opengl.EGLConfig;
import android.opengl.EGLContext;
import android.opengl.EGLDisplay;
import android.opengl.EGLSurface;
import android.opengl.GLES11Ext;
import android.opengl.GLES20;
import android.opengl.GLUtils;
import android.os.Build;
import android.os.Bundle;
import android.os.Debug;
import android.support.annotation.RequiresApi;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.util.Base64;
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.Surface;
import android.view.SurfaceHolder;
import android.view.SurfaceView;
import android.view.TextureView;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.view.WindowManager;
import android.widget.FrameLayout;
import android.widget.LinearLayout;
import android.widget.Toast;

import com.FDReplay.FDLivePlayerLib.FDLivePlayer;
import com.FDReplay.FDLivePlayerLib.FDLivePlayerCallback;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.lang.reflect.Field;
import java.nio.IntBuffer;

public class UnityPlayerNativeActivity extends Activity implements SurfaceTexture.OnFrameAvailableListener { //SurfaceHolder.Callback {

    static final String _UNITY_ANDROID_ = "_UNITY_ANDROID_";
    static final String TAG_UNITY = "Unity";
    static final String TAG = "Unity";
    static final String LOG_TAG = "Unity";

    private static final int EXTERNAL_STORAGE_PERMISSION_CODE = 2;

    protected UnityPlayer mUnityPlayer;        // don't change the name of this variable; referenced from native code

    private FDLivePlayer m4DLivePlayer = null;

    //private UnityPlayer mSurfaceView;
    private Surface mSurface;

    private SurfaceView mSurfaceView;
    SurfaceHolder mHolder;

    ////////////////////////////////// camera preview start
    public static Context mContext;

    private Camera mCamera;
    private SurfaceTexture mTexture;

    // unity texture
    private int nativeTexturePointer = -1;

    private int prevHeight;
    private int prevWidth;
    ////////////////////////////////// camera preview end

    public native void RenderScene(float[] paramArrayOfFloat, int paramInt1, int paramInt2);

    @RequiresApi(api = Build.VERSION_CODES.LOLLIPOP)
    @Override
    protected void onCreate(Bundle savedInstanceState) {

        ////////////////////////////////// camera preview start
        mContext = this;
        ////////////////////////////////// camera preview end

        Log.d(TAG, "com.fdp.fdpplayer4unityb onCreate Start :::::");
        super.onCreate(savedInstanceState);
        requestWindowFeature(Window.FEATURE_NO_TITLE);
        getWindow().takeSurface(null);
        setTheme(android.R.style.Theme_NoTitleBar_Fullscreen);
        getWindow().setFormat(PixelFormat.RGBX_8888); // <--- This makes xperia play happy

        Log.d(TAG, "com.fdp.fdpplayer4unityb onCreate Start ::::: new surfaceview");


        //mSurfaceView.setZOrderOnTop(false);

        Log.d(TAG, "com.fdp.fdpplayer4unityb onCreate Start ::::: new mUnityPlayer");
        mUnityPlayer = new UnityPlayer(this);

        if (mUnityPlayer.getSettings().getBoolean("hide_status_bar", true)) {
            getWindow ().setFlags (WindowManager.LayoutParams.FLAG_FULLSCREEN,
                    WindowManager.LayoutParams.FLAG_FULLSCREEN);
        }

        //mSurfaceView = new SurfaceView(this);

        //mUnityPlayer.addView(mSurfaceView);
        //mUnityPlayer.addViewToPlayer(mSurfaceView, false);

        //mHolder = mSurfaceView.getHolder();
        //mHolder.addCallback(this);
        //mSurface = mHolder.getSurface();

        //setVideoSize();
        setContentView(mUnityPlayer);
        mUnityPlayer.requestFocus();

        Log.d(TAG, "com.fdp.fdpplayer4unityb onCreate Start ::::: new m4DLivePlayer");
        //m4DLivePlayer = new FDLivePlayer(this, m4DPlayerCallback);

        //Log.d(TAG, "com.fdp.fdpplayer4unityb onCreate Start ::::: call CHECK_PERMISION");
        //CHECK_PERMISSION();

        Log.d(TAG, "com.fdp.fdpplayer4unityb onCreate Start ::::: end onCreate");

        //SurfaceView _unitySFV = findSurfaceView(mUnityPlayer.getView());

        //mSurfaceView.setZOrderMediaOverlay(true);
        //_unitySFV.setZOrderMediaOverlay(false);
    }

    // Quit Unity
    @Override
    protected void onDestroy() {
        mUnityPlayer.quit();
        super.onDestroy();
    }

    // Pause Unity
    @Override
    protected void onPause() {
        super.onPause();
        //AppEventsLogger.deactivateApp(this);
        mUnityPlayer.pause();
    }

    // Resume Unity
    @Override
    protected void onResume() {
        super.onResume();
        mUnityPlayer.resume();
    }

    // This ensures the layout will be correct.
    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);
        mUnityPlayer.configurationChanged(newConfig);
    }

    // Notify Unity of the focus change.
    @Override
    public void onWindowFocusChanged(boolean hasFocus) {
        super.onWindowFocusChanged(hasFocus);
        mUnityPlayer.windowFocusChanged(hasFocus);
    }

    // For some reason the multiple keyevent type is not supported by the ndk.
    // Force event injection by overriding dispatchKeyEvent().
    @Override
    public boolean dispatchKeyEvent(KeyEvent event) {
        if (event.getAction() == KeyEvent.ACTION_MULTIPLE)
            return mUnityPlayer.injectEvent(event);
        return super.dispatchKeyEvent(event);
    }

    // Pass any events not handled by (unfocused) views straight to UnityPlayer
    @Override
    public boolean onKeyUp(int keyCode, KeyEvent event) {
        return mUnityPlayer.injectEvent(event);
    }

    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event) {
        return mUnityPlayer.injectEvent(event);
    }

    @Override
    public boolean onTouchEvent(MotionEvent event) {
        return mUnityPlayer.injectEvent(event);
    }

    /*API12*/
    public boolean onGenericMotionEvent(MotionEvent event) {
        return mUnityPlayer.injectEvent(event);
    }


    private SurfaceView findSurfaceView(View v) {
        if (v == null) return null;
        else if (v instanceof SurfaceView) return (SurfaceView) v;
        else if (v instanceof ViewGroup) {
            int childCount = ((ViewGroup) v).getChildCount();
            for (int i = 0; i < childCount; i++) {
                SurfaceView ret = findSurfaceView(((ViewGroup) v).getChildAt(i));
                if (ret != null) return ret;
            }
        }
        return null;
    }


    public void SET_SURFACEVIEW_Z(boolean _how) {

//        if(mSurfaceView != null)
//            mSurfaceView.setZOrderMediaOverlay(_how);

        SurfaceView _unitySFV = findSurfaceView(mUnityPlayer.getView());

        mSurfaceView.setZOrderMediaOverlay(_how);
        _unitySFV.setZOrderMediaOverlay(!_how);

        Log.d(TAG, "_how ::: " + _how);

    }


    public static Bitmap viewToBitmap(View view) {

        Log.d(TAG, "viewToBitmap :: " + view.getWidth() + "/" + view.getHeight());

        Bitmap bitmap = Bitmap.createBitmap(view.getWidth(), view.getHeight(), Bitmap.Config.ARGB_8888);
        Canvas canvas = new Canvas(bitmap);
        if (view instanceof SurfaceView) {
            SurfaceView surfaceView = (SurfaceView) view;
            //surfaceView.setZOrderOnTop(true);
            surfaceView.draw(canvas);
            //surfaceView.setZOrderOnTop(false);
            return bitmap;
        } else {
            //For ViewGroup & View
            view.draw(canvas);
            return bitmap;
        }
    }


    public void TEST() {

        Log.d(TAG_UNITY, "com.fdp.fdpplayer4unityb ::: get SurfaceView TEST()");

        try {
//            UnityPlayer view = (UnityPlayer) mUnityPlayer.getView();
//            Field f = view.getClass().getDeclaredField("i");
//            f.setAccessible(true);
//            mSurfaceView = (SurfaceView) f.get(view);

            //SurfaceHolder sfhTrackHolder = mSurfaceView.getHolder();
            //mSurface = sfhTrackHolder.getSurface();
            //sfhTrackHolder.addCallback(myHolderCallback);
        }catch(Exception e) {

        }

        UnityPlayer.UnitySendMessage(_UNITY_ANDROID_, "fromAndroidTest", "&&&&&&&&&&&&&&&&&&&");

    }


    private int PutBitmapInGraphicsCard() {

        final int[] textureHandle = new int[1];

        GLES20.glGenTextures(1, textureHandle, 0);
        final Bitmap bitmap = viewToBitmap(mUnityPlayer.getRootView());//GenerateTestImage();
        GLES20.glActiveTexture(GLES20.GL_TEXTURE0);
        GLES20.glBindTexture(GLES20.GL_TEXTURE_2D, textureHandle[0]);
        GLUtils.texImage2D(GLES20.GL_TEXTURE_2D,0, bitmap, 0);
        GLES20.glTexParameteri(GLES20.GL_TEXTURE_2D, GLES20.GL_TEXTURE_MIN_FILTER, GLES20.GL_LINEAR);
        GLES20.glTexParameteri(GLES20.GL_TEXTURE_2D, GLES20.GL_TEXTURE_MAG_FILTER, GLES20.GL_LINEAR);
        GLES20.glTexParameteri(GLES20.GL_TEXTURE_2D, GLES20.GL_TEXTURE_WRAP_S, GLES20.GL_CLAMP_TO_EDGE);
        GLES20.glTexParameteri(GLES20.GL_TEXTURE_2D, GLES20.GL_TEXTURE_WRAP_T, GLES20.GL_CLAMP_TO_EDGE);
        GLES20.glBindTexture(GLES20.GL_TEXTURE_2D, 0);

        return textureHandle[0];

    }

    public byte[] getViewBitmap() {

        Bitmap _tmp = viewToBitmap(mUnityPlayer.getRootView());
        String rtn = null;
        ByteArrayOutputStream stream = new ByteArrayOutputStream() ;

        _tmp.compress(Bitmap.CompressFormat.PNG, 100, stream);

        byte[] byteArray = stream.toByteArray();

        return byteArray;

    }


    // This function is a (almost) direct copy from the question linked above
    public void CreateContext(){
        // gets hold of the display
        EGLDisplay dpy = EGL14.eglGetDisplay(EGL14.EGL_DEFAULT_DISPLAY);
        int[] vers = new int[2];
        EGL14.eglInitialize(dpy, vers, 0, vers, 1);
        // get some basic configs going
        int[] configAttr = {
                EGL14.EGL_COLOR_BUFFER_TYPE, EGL14.EGL_RGB_BUFFER,
                EGL14.EGL_LEVEL, 0,
                EGL14.EGL_RENDERABLE_TYPE, EGL14.EGL_OPENGL_ES2_BIT,
                EGL14.EGL_SURFACE_TYPE, EGL14.EGL_PBUFFER_BIT,
                EGL14.EGL_NONE
        };
        EGLConfig[] configs = new EGLConfig[1];
        int[] numConfig = new int[1];
        EGL14.eglChooseConfig(dpy, configAttr, 0,
                configs, 0, 1, numConfig, 0);
        if (numConfig[0] == 0) {
            Log.e("GLEDB-Error", "Could not find/create config!!");
        }
        EGLConfig config = configs[0];
        // creating an offscreen (PBuffer) surface to render stuff
        int[] surfAttr = {
                EGL14.EGL_WIDTH, 100,
                EGL14.EGL_HEIGHT, 100,
                EGL14.EGL_NONE
        };
        EGLSurface surf = EGL14.eglCreatePbufferSurface(dpy, config, surfAttr, 0);
        // create the context
        int[] ctxAttrib = {
                EGL14.EGL_CONTEXT_CLIENT_VERSION, 2,
                EGL14.EGL_NONE
        };
        EGLContext ctx = EGL14.eglCreateContext(dpy, config, EGL14.EGL_NO_CONTEXT, ctxAttrib, 0);
        // connect all these things together
        EGL14.eglMakeCurrent(dpy, surf, surf, ctx);
    }


    private FDLivePlayerCallback m4DPlayerCallback = new FDLivePlayerCallback()
    {
        @Override
        public void getError(int code, String msg, String ls_ip)
        {
            Log.e(TAG, "getError " + code + " , " + msg + " , " + ls_ip);
            final String str = "getError " + code + " , " + msg + " , " + ls_ip;
            final int err_code = code;
        }

        @Override
        public void getVideoStreamInfo(int width, int height, int duration)
        {
            final String log = "getVideoStreamInfo " + width + " , " + height + " , " + duration;
            Log.d(TAG, log);
        }

        @Override
        public void getCurrentPlayInfo(int channel, int frame, int frame_cycle, long time, String utc)
        {
            Log.d(TAG, "getCurrentPlayInfo " + channel + " / " + frame_cycle + " / " + frame + " / " + time +  " / " + utc);

            long cur_msec = time;
            long cur_frame = frame;

            final String info = "Play ch:" + channel + " /frame_cycle:" + frame_cycle + " /frame:" + frame + " /time:" + time;
            final String sUTC = "utc: " + utc;
            //mTextPlay.setText(info);
//            Bitmap _tmp = viewToBitmap(mSurfaceView);
//            String rtn = null;
//            ByteArrayOutputStream stream = new ByteArrayOutputStream() ;
//
//            _tmp.compress(Bitmap.CompressFormat.PNG, 100, stream);
//
//            byte[] byteArray = stream.toByteArray();
//
//            rtn = Base64.encodeToString(byteArray, Base64.DEFAULT);
//
//            UnityPlayer.UnitySendMessage(_UNITY_ANDROID_, "fromAndroidTest",  rtn);


//                final int[] textureHandle = new int[1];
//                final Bitmap bitmap = viewToBitmap(mSurfaceView);;//GenerateTestImage(); // this creates a simple bitmap, blank with a blue circle in the center, works fine in android.
//
//                GLES20.glGenTextures(1, textureHandle, 0);
//                GLES20.glBindTexture(GLES20.GL_TEXTURE_2D, textureHandle[0]);
//                GLES20.glTexParameteri(GLES20.GL_TEXTURE_2D, GLES20.GL_TEXTURE_MIN_FILTER, GLES20.GL_NEAREST);
//                GLES20.glTexParameteri(GLES20.GL_TEXTURE_2D, GLES20.GL_TEXTURE_MAG_FILTER, GLES20.GL_NEAREST);
//                GLUtils.texImage2D(GLES20.GL_TEXTURE_2D, 0, bitmap, 0);
//                bitmap.recycle();
//
//
//                String rtn = Integer.toString(textureHandle[0]);
//
//                UnityPlayer.UnitySendMessage(_UNITY_ANDROID_, "fromAndroidTest",  rtn);



        }

        @Override
        public void getStart(int code)
        {
            if (code == -1)
                Log.e(TAG, "getStart " + code);
            else if (code == 0)
                Log.d(TAG, "getStart " + code);

            final String log = "getStart " + code;
        }

        @Override
        public void getStop(int code)
        {
            Log.d(TAG, "getStop " + code);

            final String log = "getStop " + code;
        }

        @Override
        public void getPlay()
        {
            Log.d(TAG, "getPlay ");

            final String log = "getPlay ";
        }

        @Override
        public void getPause()
        {
            Log.d(TAG, "getPause ");

            final String log = "getPause ";
        }

        @Override
        public void getPlayDone()
        {
            Log.d(TAG, "getPlayDone ");

            final String log = "getPlayDone ";

            close();
        }

        @Override
        public void getSlowReceiveFrame(int isSlow)
        {
            Log.d(TAG, "getSlowReceiveFrame : " + isSlow);

            final String log = "getSlowReceiveFrame : " + isSlow;
        }
    };


    public void open1(String _url, boolean _isTCP, boolean _isHWDec, int _port) {

        Log.d(TAG, "############ open1 Start");
        boolean isSub = false;

        if (m4DLivePlayer.isStreamOpened()) {
            m4DLivePlayer.streamClose();
            Log.d(TAG,"streamClose");
            return;
        }

        final String url = _url;//mEditURL.getText().toString();
        final boolean isTCP = _isTCP;//mCheckTCP.isChecked();
        final boolean isHWDec = _isHWDec;//mCheckHW.isChecked();
        Surface surfaceMain = mSurface;

        final Surface surfaceMainEx = surfaceMain;

        boolean _isOpened = m4DLivePlayer.isStreamOpened();

        Log.d(TAG, "check _isOpened 000 :::  " + _isOpened);

        int ret = 1;
        if (!m4DLivePlayer.isStreamOpened()) {
            ret = m4DLivePlayer.streamOpen(url, surfaceMainEx, isTCP, isHWDec);
        }

        Log.d(TAG, "streamOpen End ::: ret = " + ret);

        _isOpened = m4DLivePlayer.isStreamOpened();

        Log.d(TAG, "check _isOpened 111 :::  " + _isOpened);

        final int finalRet = ret;

        if (m4DLivePlayer.isStreamOpened()) {
            int port = _port;//Integer.parseInt(mEditRestFulPort.getText().toString());
            //String ip = mEditRestFulIP.getText().toString();

            String sub1 = url.substring(url.lastIndexOf("//")+2);
            String sub2 = sub1.substring(sub1.lastIndexOf("/")+1);
            String sub3 = sub2.substring(sub2.lastIndexOf("?")+1);

            Log.d(TAG, "sub1 : " + sub1);
            Log.d(TAG, "sub2 : " + sub2);
            Log.d(TAG, "sub3 : " + sub3);

            String ss_ip = sub1.substring(0, sub1.indexOf("/"));
            if (ss_ip.indexOf(":") > 0)
                ss_ip = ss_ip.substring(0, ss_ip.indexOf(":"));

            Log.d(TAG, "restful : " + ss_ip + " / " + port);

            m4DLivePlayer.RESTFulOpen(ss_ip, port);
        }


        //m4DLivePlayer.play();
        Log.d(TAG, "open1 end");
    }


    public void close() {
        m4DLivePlayer.pause();
        m4DLivePlayer.streamClose();
        Log.d(TAG, "call streamClose ");
    }


    public void setVideoSize(int videoWidth, int videoHeight) {
        //int videoWidth = 1280 /2;
        //int videoHeight = 720 / 2;
        float videoProportion = (float) videoWidth / (float) videoHeight;

        int screenWidth = getWindowManager().getDefaultDisplay().getWidth();
        int screenHeight = getWindowManager().getDefaultDisplay().getHeight();
        float screenProportion = (float) screenWidth / (float) screenHeight;

        //android.view.ViewGroup.LayoutParams lp = m_SurfaceView.getLayoutParams();
        FrameLayout.LayoutParams lp = new FrameLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.MATCH_PARENT);
        if (videoProportion > screenProportion) {
            lp.width = screenWidth;
            lp.height = (int) ((float) screenWidth / videoProportion);
        }
        else {
            lp.width = (int) (videoProportion * (float) screenHeight);
            lp.height = screenHeight;
        }

        WindowManager wm = getWindowManager();
        if (wm.getDefaultDisplay().getRotation() == Surface.ROTATION_90 || wm.getDefaultDisplay().getRotation() == Surface.ROTATION_270) {
            if (videoProportion > screenProportion) {
                int h = (screenHeight - lp.height) / 2;
                lp.setMargins(0, h, 0, 0);
                Log.d(TAG,"1 setMargins : " + h);
            } else {
                int w = (screenWidth - lp.width) / 2;
                lp.setMargins(w, 0, 0, 0);
                Log.d(TAG,"2 setMargins : " + w);
            }
        }

        Log.d(TAG, "SET Video Size " + videoWidth + "/" + videoHeight);
        lp.setMargins(screenWidth - videoWidth, 0, 0, 0);
        lp.width = videoWidth;
        lp.height = videoHeight;

        mSurfaceView.setLayoutParams(lp);
        Log.d(TAG, "set Video Size done");
        //m_lp = lp;
    }


    void CHECK_PERMISSION() {

        if (Build.VERSION.SDK_INT >= 23) {
            if (ContextCompat.checkSelfPermission(this, Manifest.permission.READ_EXTERNAL_STORAGE) != PackageManager.PERMISSION_GRANTED) {
                Log.v(TAG, "Permission is revoked");
                ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.READ_EXTERNAL_STORAGE}, 1);
            } else {
                Log.v(TAG, "Permission is granted");
            }
        } else {
            Toast.makeText(this, "External Storage Permission is Grant", Toast.LENGTH_SHORT).show();
            Log.d(TAG, "External Storage Permission is Grant ");
        }

        // Check if we have write permission
        int permission = ContextCompat.checkSelfPermission(this, Manifest.permission.WRITE_EXTERNAL_STORAGE);
        if (permission != PackageManager.PERMISSION_GRANTED) {
            Log.w(TAG, "Write permissions is not granted");
            // Request permissions
            ActivityCompat.requestPermissions(this,
                    new String[] {Manifest.permission.WRITE_EXTERNAL_STORAGE},
                    EXTERNAL_STORAGE_PERMISSION_CODE);
        }

    }


    /*
     * JAVA texture creation
     */
    public int startCamera() {

        Log.d(LOG_TAG, "startCamera() ::: call");


        // create the texture
        nativeTexturePointer = createExternalTexture();

        //mSurfaceView = new SurfaceView(this);

        //mTexture = new SurfaceTexture(nativeTexturePointer);
        //mTexture.setOnFrameAvailableListener(this);

        //mSurface = new Surface(mTexture);

        //mHolder = mSurfaceView.getHolder();
        //mHolder.getSurface();

        //mUnityPlayer.addView(mSurfaceView);
        //mUnityPlayer.displayChanged(1, mSurface);
//
//        // open the camera
//        mCamera = Camera.open();
//        setupCamera();
//
//        Log.d(LOG_TAG, "camera opened: " + (mCamera != null));
//
//        try {
//            mCamera.setPreviewTexture(texture);
//            mCamera.startPreview();
//
//        } catch (IOException ioe) {
//            Log.w("MainActivity", "CAM LAUNCH FAILED");
//        }

        Log.d(LOG_TAG, "nativeTexturePointer="+nativeTexturePointer);
        return nativeTexturePointer;
    }

    @SuppressLint("NewApi")
    private void setupCamera() {
//        Camera.Parameters parms = mCamera.getParameters();
//
//        // Give the camera a hint that we're recording video. This can have a
//        // big impact on frame rate.
//        parms.setRecordingHint(true);
//        parms.setPreviewFormat(20);
//
//        // leave the frame rate set to default
//        mCamera.setParameters(parms);
//
//        Camera.Size mCameraPreviewSize = parms.getPreviewSize();
//        prevWidth = parms.getPreviewSize().width;
//        prevHeight = parms.getPreviewSize().height;
//
////		mPixelBuf = ByteBuffer.allocateDirect(prevWidth * prevHeight * 4);
////		mPixelBuf.order(ByteOrder.LITTLE_ENDIAN);
//
//        // only for debugging output
//        int[] fpsRange = new int[2];
//        parms.getPreviewFpsRange(fpsRange);
//        String previewFacts = mCameraPreviewSize.width + "x"
//                + mCameraPreviewSize.height;
//        if (fpsRange[0] == fpsRange[1]) {
//            previewFacts += " @" + (fpsRange[0] / 1000.0) + "fps";
//        } else {
//            previewFacts += " @[" + (fpsRange[0] / 1000.0) + " - "
//                    + (fpsRange[1] / 1000.0) + "] fps";
//        }

//		previewFacts += ", supported Preview Formats: ";
//		List<Integer> formats = parms.getSupportedPreviewFormats();
//		for (int i = 0; i < formats.size(); i++) {
//			previewFacts += formats.get(i).toString() + " ";
//		}
//		Integer format = parms.getPreviewFormat();
//		previewFacts += ", Preview Format: ";
//		previewFacts += format.toString();

        //Log.i(LOG_TAG, "previewFacts=" + previewFacts);

        checkGlError("endSetupCamera");
    }

    public void updateTexture() {

        // check for errors at the beginning
        //checkGlError("begin_updateTexture()");

        //Log.d(LOG_TAG, "GLES20.glActiveTexture..");
        //GLES20.glActiveTexture(GLES20.GL_TEXTURE0);
        //checkGlError("glActiveTexture");
        //Log.d(LOG_TAG, "GLES20.glBindTexture..");
        //GLES20.glBindTexture(GLES11Ext.GL_TEXTURE_EXTERNAL_OES, nativeTexturePointer);
        //checkGlError("glBindTexture");

        //Log.d(LOG_TAG,"ThreadID="+Thread.currentThread().getId());
        //Log.d(LOG_TAG, "texture.updateTexImage..");
        //mTexture.updateTexImage();
        //checkGlError("updateTexImage");

        Bitmap _tmp = viewToBitmap(mSurfaceView);
        String rtn = null;
        ByteArrayOutputStream stream = new ByteArrayOutputStream() ;
        //_tmp.compress(Bitmap.CompressFormat.PNG, 100, stream) ;
        byte[] byteArray = stream.toByteArray() ;

        rtn = Base64.encodeToString(byteArray, Base64.DEFAULT);

        UnityPlayer.UnitySendMessage(_UNITY_ANDROID_, "fromAndroidTest",  rtn);

//		mPixelBuf.rewind();
//		Log.d(LOG_TAG, "GLES20.glReadPixels..");
//		GLES20.glReadPixels(0, 0, prevWidth, prevHeight, GLES20.GL_RGBA,
//				GLES20.GL_UNSIGNED_SHORT_4_4_4_4, mPixelBuf);
//		checkGlError("glReadPixels");

//		Log.d(LOG_TAG, "mPixelBuf.get(0)=" + mPixelBuf.get(0));

    }

    public int getPreviewSizeWidth() {
        return prevWidth;
    }

    public int getPreviewSizeHeight() {

        return prevHeight;
    }

    @Override
    public void onFrameAvailable(SurfaceTexture arg0) {

        Log.d(LOG_TAG, "onFrameAvailable");
    }

    // create texture here instead by Unity
    private int createExternalTexture() {
        int[] textureIdContainer = new int[1];
        GLES20.glGenTextures(1, textureIdContainer, 0);
        GLES20.glBindTexture(GLES11Ext.GL_TEXTURE_EXTERNAL_OES,
                textureIdContainer[0]);

        GLES20.glTexParameterf(GLES11Ext.GL_TEXTURE_EXTERNAL_OES,
                GLES20.GL_TEXTURE_MIN_FILTER, GLES20.GL_NEAREST);
        GLES20.glTexParameterf(GLES11Ext.GL_TEXTURE_EXTERNAL_OES,
                GLES20.GL_TEXTURE_MAG_FILTER, GLES20.GL_LINEAR);
        GLES20.glTexParameteri(GLES11Ext.GL_TEXTURE_EXTERNAL_OES,
                GLES20.GL_TEXTURE_WRAP_S, GLES20.GL_CLAMP_TO_EDGE);
        GLES20.glTexParameteri(GLES11Ext.GL_TEXTURE_EXTERNAL_OES,
                GLES20.GL_TEXTURE_WRAP_T, GLES20.GL_CLAMP_TO_EDGE);

        return textureIdContainer[0];
    }

    // check for OpenGL errors
    private void checkGlError(String op) {
        int error;
        while ((error = GLES20.glGetError()) != GLES20.GL_NO_ERROR) {
            Log.e(LOG_TAG, op + ": glError 0x" + Integer.toHexString(error));
        }
    }
}
