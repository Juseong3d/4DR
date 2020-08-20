package com.EasyMovieTexture;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

import com.android.vending.expansion.zipfile.ZipResourceFile;
import com.android.vending.expansion.zipfile.ZipResourceFile.ZipEntryRO;

import android.app.Activity;
import android.content.Context;
import android.content.res.AssetFileDescriptor;
import android.content.res.AssetManager;
import android.graphics.SurfaceTexture;
import android.graphics.SurfaceTexture.OnFrameAvailableListener;
import android.media.AudioManager;
import android.media.MediaExtractor;
import android.media.MediaFormat;
import android.media.MediaPlayer;
import android.net.ConnectivityManager;
import android.net.Uri;
import android.opengl.GLES20;
import android.os.Debug;
import android.util.Log;
import android.view.Surface;

import com.FDReplay.FDLivePlayerLib.FDLivePlayer;
import com.FDReplay.FDLivePlayerLib.FDLivePlayerCallback;

import com.FDReplay.FDMediaPlayerLib.FDMediaPlayer;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

public class EasyMovieTexture implements MediaPlayer.OnPreparedListener, MediaPlayer.OnBufferingUpdateListener, MediaPlayer.OnCompletionListener, MediaPlayer.OnErrorListener, OnFrameAvailableListener {

    static final String _UNITY_ANDROID_ = "_UNITY_ANDROID_";
    static final String TAG = "Unity";
    private FDLivePlayer m4DLivePlayer = null;
    private boolean m_bTcp = true;
    private boolean m_bHWDec = false;
    private int m_iRESTFul_Port = 7070;

    private int m_iVideoWidth = 1920;
    private int m_iVideoheight = 1080;
    private int m_iDuration = 0;


    private FDMediaPlayer fdLocalPlayer = null;
    private int startVideoTrack;
    private int videoTrack;
    private int audioTrack;

    /////////////////////////////////////////////////
    private Activity        m_UnityActivity = null;
    private MediaPlayer 	m_MediaPlayer = null;

    private int				m_iUnityTextureID = -1;
    private int				m_iSurfaceTextureID = -1;
    private SurfaceTexture m_SurfaceTexture = null;
    private Surface m_Surface = null;
    private int 			m_iCurrentSeekPercent = 0;
    private int				m_iCurrentSeekPosition = 0;
    public int 				m_iNativeMgrID;
    private String 			m_strFileName;
    private int 			m_iErrorCode;
    private int				m_iErrorCodeExtra;
    private boolean			m_bRockchip = true;
    private boolean 		m_bSplitOBB = false;
    private String 			m_strOBBName;
    public boolean 			m_bUpdate= false;

    public static ArrayList<EasyMovieTexture> m_objCtrl = new ArrayList<EasyMovieTexture>();

    public static EasyMovieTexture GetObject(int iID)
    {
        for(int i = 0; i < m_objCtrl.size(); i++)
        {
            if(m_objCtrl.get(i).m_iNativeMgrID == iID)
            {
                return m_objCtrl.get(i);
            }
        }

        return null;

    }

    private static final int GL_TEXTURE_EXTERNAL_OES = 0x8D65;

    public native int InitNDK(Object obj);
    public native void SetAssetManager(AssetManager assetManager);
    public native int InitApplication();
    public native void QuitApplication();
    public native void SetWindowSize(int iWidth,int iHeight,int iUnityTextureID,boolean bRockchip);
    public native void RenderScene(float [] fValue, int iTextureID,int iUnityTextureID);
    public native void SetManagerID(int iID);
    public native int GetManagerID();
    public native int InitExtTexture();
    public native void SetUnityTextureID(int iTextureID);

    static {
        System.loadLibrary("BlueDoveMediaRender");
    }

    MEDIAPLAYER_STATE m_iCurrentState = MEDIAPLAYER_STATE.NOT_READY;

    public void Destroy() {
        if(m_iSurfaceTextureID != -1) {
            int [] textures = new int[1];
            textures[0] = m_iSurfaceTextureID;
            GLES20.glDeleteTextures(1, textures, 0);
            m_iSurfaceTextureID = -1;
        }

        SetManagerID(m_iNativeMgrID);
        QuitApplication();

        m_objCtrl.remove(this);
    }

    public void _LEFT(boolean isTime) {

        int _result = -1;

        if(m4DLivePlayer != null) {
            if(isTime == false) {
                if (!m4DLivePlayer.isPlaying()) {
                    _result = m4DLivePlayer.setChangeFrameCh("pause", "left");
                } else {
                    _result = m4DLivePlayer.setChangeChannel("normal", "left", 1);
                }
            }else {
                if (!m4DLivePlayer.isPlaying()) {
                    _result = m4DLivePlayer.setChangeFrameCh("rewind", "stop");
                }
            }
        }else {
            Log.d(TAG, "player nulllll");
        }

        if(fdLocalPlayer != null) {
            if(isTime == false) {
                onChangeChannelFrameLeft();
            }else {
                onTimeShiftRewind();
            }
        }
    }


    public void _RIGHT(boolean isTime) {

        int _result = -1;

        if(m4DLivePlayer != null) {
            if (isTime == false) {
                if (!m4DLivePlayer.isPlaying()) {
                    _result = m4DLivePlayer.setChangeFrameCh("pause", "right");
                } else {
                    _result = m4DLivePlayer.setChangeChannel("normal", "right", 1);
                }
            }else {
                if (!m4DLivePlayer.isPlaying()) {
                    _result = m4DLivePlayer.setChangeFrameCh("forward", "stop");
                }
            }
        }else {
            Log.d(TAG, "player nulllll");
        }

        if(fdLocalPlayer != null) {
            if(isTime == false) {
                onChangeChannelFrameRight();
            }else {
                onTimeShiftForward();
            }
        }

    }


    public void _PLAY_TO_NOW() {

        if(m4DLivePlayer != null) {
            m4DLivePlayer.playToNow();
        }

        if(fdLocalPlayer != null) {
            //nothing
        }
    }


    public void UnLoad() {

        Log.d(TAG, "###################### UnLoad() ");
        if(m4DLivePlayer != null) {
            Log.d(TAG, "###################### UnLoad() " + m_iCurrentState);
            if (m_iCurrentState != MEDIAPLAYER_STATE.NOT_READY) {
                try {
                    m4DLivePlayer.pause();
                    m4DLivePlayer.streamClose();
                } catch (SecurityException e) {
                    // TODO Auto-generated catch block
                    e.printStackTrace();
                } catch (IllegalStateException e) {
                    // TODO Auto-generated catch block
                    e.printStackTrace();
                }
                Log.d(TAG, "################ 4dPlayer nulllllllllllllllllllllll 1");
                m4DLivePlayer = null;
            } else {
                try {
                    m4DLivePlayer.streamClose();
                } catch (SecurityException e) {
                    // TODO Auto-generated catch block
                    e.printStackTrace();
                } catch (IllegalStateException e) {
                    // TODO Auto-generated catch block
                    e.printStackTrace();
                }
                Log.d(TAG, "################# 4dPlayer nulllllllllllllllllllllll 2");
                m4DLivePlayer = null;
            }
        }

        if(fdLocalPlayer != null) {
            stop();
            fdLocalPlayer = null;
        }

        if(m_MediaPlayer!=null) {
            if (m_iCurrentState != MEDIAPLAYER_STATE.NOT_READY) {
                try {
                    m_MediaPlayer.stop();
                    m_MediaPlayer.release();
                } catch (SecurityException e) {
                    // TODO Auto-generated catch block
                    e.printStackTrace();
                } catch (IllegalStateException e) {
                    // TODO Auto-generated catch block
                    e.printStackTrace();
                }
                m_MediaPlayer = null;
            } else {
                try {
                    m_MediaPlayer.release();
                } catch (SecurityException e) {
                    // TODO Auto-generated catch block
                    e.printStackTrace();
                } catch (IllegalStateException e) {
                    // TODO Auto-generated catch block
                    e.printStackTrace();
                }
                m_MediaPlayer = null;
            }
        }

        if(m_Surface != null) {
            m_Surface.release();
            m_Surface = null;
        }

        if(m_SurfaceTexture != null) {
            m_SurfaceTexture.release();
            m_SurfaceTexture = null;
        }

        if(m_iSurfaceTextureID != -1) {
            int [] textures = new int[1];
            textures[0] = m_iSurfaceTextureID;
            GLES20.glDeleteTextures(1, textures, 0);
            m_iSurfaceTextureID = -1;
        }

    }


    public boolean Load() throws SecurityException, IllegalStateException, IOException {

        Log.d(TAG, "############################# Load Start");

        UnLoad();

        m_iCurrentState = MEDIAPLAYER_STATE.NOT_READY;

        //Log.d(TAG, "999 ?????????????? " + m4DLivePlayer);
        Context _cont = m_UnityActivity.getBaseContext();
        //Context _cont = m_UnityActivity.getApplicationContext();
        m4DLivePlayer = new FDLivePlayer(_cont, m4DPlayerCallback);

        //Log.d(TAG, "888 ?????????????? " + m4DLivePlayer);
        m_MediaPlayer = new MediaPlayer();
        m_MediaPlayer.setAudioStreamType(AudioManager.STREAM_MUSIC);

        m_bUpdate = false;

        if(m_strFileName.contains("file://") == true) {
            File sourceFile = new File(m_strFileName.replace("file://", ""));

            if ( sourceFile.exists() ) {
                FileInputStream fs = new FileInputStream(sourceFile);
                m_MediaPlayer.setDataSource(fs.getFD());
                fs.close();
            }
        }else if(m_strFileName.contains("://") == true) {
            if(m_strFileName.contains(".4ds") == true || m_strFileName.contains("8554/main") == true) {

            }else {
                try {
                    Map<String, String> headers = new HashMap<String, String>();
                    headers.put("rtsp_transport", "tcp");
                    headers.put("max_analyze_duration", "500");

                    m_MediaPlayer.setDataSource(m_UnityActivity, Uri.parse(m_strFileName), headers);
                    //m_MediaPlayer.setDataSource(m_strFileName);
                } catch (IOException e) {
                    // TODO Auto-generated catch block
                    Log.e("Unity", "Error m_MediaPlayer.setDataSource() : " + m_strFileName);
                    e.printStackTrace();

                    m_iCurrentState = MEDIAPLAYER_STATE.ERROR;

                    return false;
                }
            }
        }else {
            if(m_strFileName.contains(".mp4") == true) {
                Log.d(TAG, "########## ok check mp4");
            }else if(m_bSplitOBB) {
                try {
                    ZipResourceFile expansionFile = new ZipResourceFile(m_strOBBName);

                    Log.e(TAG, m_strOBBName + " " + m_strFileName);
                    AssetFileDescriptor afd = expansionFile.getAssetFileDescriptor("assets/" + m_strFileName);

                    ZipEntryRO[] data =expansionFile.getAllEntries();

                    for(int i = 0; i <data.length; i++)
                    {
                        Log.e(TAG, data[i].mFileName);
                    }

                    Log.e(TAG, afd + " " );
                    m_MediaPlayer.setDataSource(afd.getFileDescriptor(),afd.getStartOffset(),afd.getLength());

                } catch (IOException e) {
                    m_iCurrentState = MEDIAPLAYER_STATE.ERROR;
                    e.printStackTrace();
                    return false;
                }
            }else {
                AssetFileDescriptor afd;
                try {
                    afd = m_UnityActivity.getAssets().openFd(m_strFileName);
                    m_MediaPlayer.setDataSource(afd.getFileDescriptor(),afd.getStartOffset(),afd.getLength());
                    afd.close();
                } catch (IOException e) {
                    // TODO Auto-generated catch block
                    Log.e("Unity","Error m_MediaPlayer.setDataSource() : " + m_strFileName);
                    e.printStackTrace();
                    m_iCurrentState = MEDIAPLAYER_STATE.ERROR;
                    return false;
                }
            }
        }

        if(m_iSurfaceTextureID == -1) {
            m_iSurfaceTextureID = InitExtTexture();
        }

        m_SurfaceTexture = new SurfaceTexture(m_iSurfaceTextureID);
        m_SurfaceTexture.setOnFrameAvailableListener(this);
        m_Surface = new Surface( m_SurfaceTexture);

        if(m_strFileName.contains(".4ds") == true || m_strFileName.contains("8554/main") == true) {

            if(m_strFileName.contains("8554/main") == true) m_bHWDec = true;

            //Log.d(TAG, "m_strFileName ::::::: " + m_strFileName);
            open1(m_strFileName, m_bTcp, m_bHWDec, m_iRESTFul_Port);

            m_MediaPlayer.release();
            m_MediaPlayer = null;
        }else if(m_strFileName.contains(".mp4") == true) {

            startRendering(m_strFileName);

            m_MediaPlayer.release();
            m_MediaPlayer = null;
        }else {
            m_MediaPlayer.setSurface(m_Surface);
            m_MediaPlayer.setOnPreparedListener(this);
            m_MediaPlayer.setOnCompletionListener(this);
            m_MediaPlayer.setOnErrorListener(this);

            m_MediaPlayer.prepareAsync();
        }

        return true;
    }


    synchronized public void onFrameAvailable(SurfaceTexture surface) {
        m_bUpdate = true;
    }


    public void UpdateVideoTexture() {
        //Log.d(TAG, "UpdateVideoTexture() : " + m_bUpdate);
        if(m_bUpdate == false) return;

        //Log.d(TAG, "m_MediaPlayer() : " + m_MediaPlayer);
        if((m_MediaPlayer != null) || (m4DLivePlayer != null) || (m_MediaPlayer != null)) {
            //Log.d(TAG, "UpdateVideoTexture / m_iCurrentState : " + m_iCurrentState);
            if(m_iCurrentState == MEDIAPLAYER_STATE.PLAYING || m_iCurrentState == MEDIAPLAYER_STATE.PAUSED) {
                SetManagerID(m_iNativeMgrID);

                boolean [] abValue = new boolean[1];
                GLES20.glGetBooleanv(GLES20.GL_DEPTH_TEST, abValue,0);
                GLES20.glDisable(GLES20.GL_DEPTH_TEST);
                m_SurfaceTexture.updateTexImage();

                float [] mMat = new float[16];

                m_SurfaceTexture.getTransformMatrix(mMat);

                RenderScene(mMat,m_iSurfaceTextureID,m_iUnityTextureID);

                if(abValue[0]) {
                    GLES20.glEnable(GLES20.GL_DEPTH_TEST);
                }else{
                }

                abValue = null;
            }
        }
    }


    public void SetRockchip(boolean bValue) {
        m_bRockchip = bValue;
    }


    public void SetLooping(boolean bLoop) {
        if(m_MediaPlayer != null)
            m_MediaPlayer.setLooping(bLoop);

        if(fdLocalPlayer != null)
            fdLocalPlayer.setLooping(bLoop);

        if(m4DLivePlayer != null)
            m4DLivePlayer.setLooping(bLoop);
    }

    public void SetVolume(float fVolume) {

        if(m_MediaPlayer != null) {
            m_MediaPlayer.setVolume(fVolume, fVolume);
        }
    }


    public void SetSeekPosition(int iSeek) {

        if(m_MediaPlayer != null) {
            if(m_iCurrentState == MEDIAPLAYER_STATE.READY || m_iCurrentState == MEDIAPLAYER_STATE.PLAYING || m_iCurrentState == MEDIAPLAYER_STATE.PAUSED) {
                m_MediaPlayer.seekTo(iSeek);
            }
        }

        if(m4DLivePlayer != null) {
            if (m_iCurrentState == MEDIAPLAYER_STATE.READY || m_iCurrentState == MEDIAPLAYER_STATE.PLAYING || m_iCurrentState == MEDIAPLAYER_STATE.PAUSED) {
                m4DLivePlayer.seek(iSeek);
            }
        }

        if(fdLocalPlayer != null) {
            if (m_iCurrentState == MEDIAPLAYER_STATE.READY || m_iCurrentState == MEDIAPLAYER_STATE.PLAYING || m_iCurrentState == MEDIAPLAYER_STATE.PAUSED) {
                fdLocalPlayer.seekTo(iSeek);
            }
        }
    }

    public int GetSeekPosition() {

        if(m_MediaPlayer != null) {
            if(m_iCurrentState == MEDIAPLAYER_STATE.READY || m_iCurrentState == MEDIAPLAYER_STATE.PLAYING  || m_iCurrentState == MEDIAPLAYER_STATE.PAUSED) {
                try {
                    m_iCurrentSeekPosition =  m_MediaPlayer.getCurrentPosition();
                } catch (SecurityException e) {
                    // TODO Auto-generated catch block
                    e.printStackTrace();
                } catch (IllegalStateException e) {
                    // TODO Auto-generated catch block
                    e.printStackTrace();
                }
            }
        }

        if(m4DLivePlayer != null) {
            //callback에서 처리 중..
            //m_iCurrentSeekPosition = frame;
        }

        if(fdLocalPlayer != null) {
            //m_iCurrentSeekPosition = frame;
        }

        return m_iCurrentSeekPosition;
    }

    public int GetCurrentSeekPercent() {
        return m_iCurrentSeekPercent;
    }


    public void Play(int iSeek) {

        Log.d(TAG, "?????????????? " + m4DLivePlayer);
        if(m_MediaPlayer != null) {
            if(m_iCurrentState == MEDIAPLAYER_STATE.READY || m_iCurrentState == MEDIAPLAYER_STATE.PAUSED || m_iCurrentState == MEDIAPLAYER_STATE.END ) {

                //m_MediaPlayer.seekTo(iSeek);
                m_MediaPlayer.start();

                m_iCurrentState = MEDIAPLAYER_STATE.PLAYING;

            }
        }

        if(m4DLivePlayer != null) {
            if(m_iCurrentState == MEDIAPLAYER_STATE.READY || m_iCurrentState == MEDIAPLAYER_STATE.PAUSED || m_iCurrentState == MEDIAPLAYER_STATE.END ) {
                m4DLivePlayer.play();
                m_iCurrentState = MEDIAPLAYER_STATE.PLAYING;
            }
        }

        if(fdLocalPlayer != null) {
            if(m_iCurrentState == MEDIAPLAYER_STATE.READY || m_iCurrentState == MEDIAPLAYER_STATE.PAUSED || m_iCurrentState == MEDIAPLAYER_STATE.END ) {
                //fdLocalPlayer.start(null);
                play();
                m_iCurrentState = MEDIAPLAYER_STATE.PLAYING;
            }
        }
    }

    public void Reset() {

        if(m_MediaPlayer != null) {
            if(m_iCurrentState == MEDIAPLAYER_STATE.PLAYING) {
                m_MediaPlayer.reset();
            }
        }

        if(m4DLivePlayer != null || fdLocalPlayer != null) {
            Log.d(TAG, "Need Check :: have not Reset funciton");
        }

        m_iCurrentState = MEDIAPLAYER_STATE.NOT_READY;
    }

    public void Stop() {

        if(m_MediaPlayer != null) {
            if(m_iCurrentState == MEDIAPLAYER_STATE.PLAYING) {
                m_MediaPlayer.stop();
            }
        }

        if(m4DLivePlayer != null) {
            if(m_iCurrentState == MEDIAPLAYER_STATE.PLAYING) {
                m4DLivePlayer.streamClose();
            }
        }

        m_iCurrentState = MEDIAPLAYER_STATE.NOT_READY;
    }

    public void RePlay()
    {
        if(m_MediaPlayer != null) {
            if(m_iCurrentState == MEDIAPLAYER_STATE.PAUSED){
                m_MediaPlayer.start();
                m_iCurrentState = MEDIAPLAYER_STATE.PLAYING;
            }
        }

        if(m4DLivePlayer != null) {
            if(m_iCurrentState == MEDIAPLAYER_STATE.PAUSED){
                m4DLivePlayer.play();
                m_iCurrentState = MEDIAPLAYER_STATE.PLAYING;
            }
        }

        if(fdLocalPlayer != null) {
            if(m_iCurrentState == MEDIAPLAYER_STATE.PLAYING) {
                play();
                m_iCurrentState = MEDIAPLAYER_STATE.PAUSED;
            }
        }
    }

    public void Pause() {
        if(m_MediaPlayer != null) {
            if(m_iCurrentState == MEDIAPLAYER_STATE.PLAYING) {
                m_MediaPlayer.pause();
                m_iCurrentState = MEDIAPLAYER_STATE.PAUSED;
            }
        }

        if(fdLocalPlayer != null) {
            if(m_iCurrentState == MEDIAPLAYER_STATE.PLAYING) {
                pause();
                m_iCurrentState = MEDIAPLAYER_STATE.PAUSED;
            }
        }

        Log.d(TAG, "?????????????? " + m4DLivePlayer);
        if(m4DLivePlayer != null) {
            if(m_iCurrentState == MEDIAPLAYER_STATE.PLAYING) {
                m4DLivePlayer.pause();
                m_iCurrentState = MEDIAPLAYER_STATE.PAUSED;
            }
        }
    }


    public void Speed(float _value) {

        Log.d(TAG, "speed set " + _value);
        if(m4DLivePlayer != null) {
            m4DLivePlayer.speed(_value);
            Log.d(TAG, "speed set in" + _value);
        }

    }


    public void setStreamOpenStartTS(int _value) {

        if(m4DLivePlayer != null) {
            m4DLivePlayer.setStreamOpenStartTS(_value);
            Log.d(TAG, "setStreamOpenStartTS set in" + _value);
        }

    }


    public int GetVideoWidth() {

        if(m_MediaPlayer != null) {
            return m_MediaPlayer.getVideoWidth();
        }

        if(m4DLivePlayer != null || fdLocalPlayer != null) {
            return m_iVideoWidth;
        }

        return 0;
    }

    public int GetVideoHeight() {

        if(m_MediaPlayer != null) {
            return m_MediaPlayer.getVideoHeight();
        }

        if(m4DLivePlayer != null || fdLocalPlayer != null) {
            return m_iVideoheight;
        }

        return 0;
    }


    public boolean IsUpdateFrame() {

        if(m_bUpdate == true) {
            return true;
        }else {
            return false;
        }
    }


    public void SetUnityTexture(int iTextureID) {
        m_iUnityTextureID = iTextureID;
        SetManagerID(m_iNativeMgrID);
        SetUnityTextureID(m_iUnityTextureID);
    }


    public void SetUnityTextureID(Object texturePtr) {

    }


    public void SetSplitOBB( boolean bValue,String strOBBName) {
        m_bSplitOBB = bValue;
        m_strOBBName = strOBBName;
    }

    public int GetDuration() {

        if(m_MediaPlayer != null) {
            return m_MediaPlayer.getDuration();
        }

        if(m4DLivePlayer != null || fdLocalPlayer != null) {
            return m_iDuration;
        }

        return 0;
    }


    public int InitNative(EasyMovieTexture obj) {

        m_iNativeMgrID = InitNDK(obj);
        m_objCtrl.add(this);

        return m_iNativeMgrID;
    }

    public void SetUnityActivity(Activity unityActivity) {

        SetManagerID(m_iNativeMgrID);
        m_UnityActivity = unityActivity;
        SetAssetManager(m_UnityActivity.getAssets());

    }


    public void NDK_SetFileName(String strFileName) {
        m_strFileName = strFileName;
    }

    /////add
    public void NDK_SET_IS_TCP(boolean _value) {
        m_bTcp = _value;
    }


    public void NDK_SET_IS_HWDEC(boolean _value) {
        m_bHWDec = _value;
    }


    public void NDK_SET_IS_RESTFUL_PORT(int _value) {
        m_iRESTFul_Port = _value;
    }
    ////add end


    public void InitJniManager() {
        SetManagerID(m_iNativeMgrID);
        InitApplication();
    }

    public int GetStatus() {
        return m_iCurrentState.GetValue();
    }

    public void SetNotReady() {
        m_iCurrentState = MEDIAPLAYER_STATE.NOT_READY;
    }

    public void SetWindowSize() {

        SetManagerID(m_iNativeMgrID);
        SetWindowSize(GetVideoWidth(),GetVideoHeight(),m_iUnityTextureID ,m_bRockchip);

    }

    public int GetError() {
        return m_iErrorCode;
    }

    public int GetErrorExtra() {
        return m_iErrorCodeExtra;
    }

    @Override
    public boolean onError(MediaPlayer arg0, int arg1, int arg2) {
        // TODO Auto-generated method stub

        if (arg0 == m_MediaPlayer) {
            String strError;

            switch (arg1) {
                case MediaPlayer.MEDIA_ERROR_NOT_VALID_FOR_PROGRESSIVE_PLAYBACK:
                    strError = "MEDIA_ERROR_NOT_VALID_FOR_PROGRESSIVE_PLAYBACK";
                    break;
                case MediaPlayer.MEDIA_ERROR_SERVER_DIED:
                    strError = "MEDIA_ERROR_SERVER_DIED";
                    break;
                case MediaPlayer.MEDIA_ERROR_UNKNOWN:
                    strError = "MEDIA_ERROR_UNKNOWN";
                    break;
                default:
                    strError = "Unknown error " + arg1;
            }

            m_iErrorCode = arg1;
            m_iErrorCodeExtra = arg2;

            m_iCurrentState = MEDIAPLAYER_STATE.ERROR;

            return true;
        }

        return false;
    }



    @Override
    public void onCompletion(MediaPlayer arg0) {
        // TODO Auto-generated method stub
        if (arg0 == m_MediaPlayer)
            m_iCurrentState = MEDIAPLAYER_STATE.END;
    }

    @Override
    public void onBufferingUpdate(MediaPlayer arg0, int arg1) {
        // TODO Auto-generated method stub
        if (arg0 == m_MediaPlayer)
            m_iCurrentSeekPercent = arg1;
    }

    @Override
    public void onPrepared(MediaPlayer arg0) {
        // TODO Auto-generated method stub
        if (arg0 == m_MediaPlayer)
        {
            m_iCurrentState = MEDIAPLAYER_STATE.READY;

            SetManagerID(m_iNativeMgrID);
            m_iCurrentSeekPercent = 0;
            m_MediaPlayer.setOnBufferingUpdateListener(this);

        }
    }


    public enum MEDIAPLAYER_STATE {
        NOT_READY       (0),
        READY           (1),
        END     		(2),
        PLAYING         (3),
        PAUSED          (4),
        STOPPED         (5),
        ERROR           (6);

        private int iValue;
        MEDIAPLAYER_STATE (int i)
        {
            iValue = i;
        }
        public int GetValue()
        {
            return iValue;
        }
    }


    private FDLivePlayerCallback m4DPlayerCallback = new FDLivePlayerCallback()
    {
        @Override
        public void getError(int code, String msg, String ls_ip)
        {
            Log.e(TAG, "getError " + code + " , " + msg + " , " + ls_ip);
            final String str = "getError," + code + "," + msg + "," + ls_ip;
            final int err_code = code;

            UnityPlayer.UnitySendMessage(_UNITY_ANDROID_, "CallBackFromFDPlayer", str);
        }

        @Override
        public void getVideoStreamInfo(int width, int height, int duration)
        {
            final String log = "############################# getVideoStreamInfo " + width + " , " + height + " , " + duration;
            Log.d(TAG, log);
            m_iVideoWidth = width;
            m_iVideoheight = height;
            m_iDuration = duration;

            String _value = "getVideoStreamInfo," + width + "," + height + "," + duration;

            //Log.d(TAG, "info ?????????????? " + m4DLivePlayer);
            UnityPlayer.UnitySendMessage(_UNITY_ANDROID_, "getVideoStreamInfo", _value);
        }

        @Override
        public void getCurrentPlayInfo(int channel, int frame, int frame_cycle, long time, String utc)
        {
            //Log.d(TAG, "getCurrentPlayInfo " + channel + " / " + frame_cycle + " / " + frame + " / " + time +  " / " + utc);
            //m_bUpdate = true;

            long cur_msec = time;
            long cur_frame = frame;

            final String info = "Play ch:" + channel + " /frame_cycle:" + frame_cycle + " /frame:" + frame + " /time:" + time;
            final String sUTC = "utc: " + utc;
            String _value = "getCurrentPlayInfo," + channel + "," + frame + "," + frame_cycle + "," + time + "," + utc;

            m_iCurrentSeekPosition = frame;

            UnityPlayer.UnitySendMessage(_UNITY_ANDROID_, "getCurrentPlayInfo", _value);
            //Log.d(TAG, info + " ?????????? " + m4DLivePlayer);

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

//            if(m_bUpdate) {
//
//                SetManagerID(m_iNativeMgrID);
//
//                boolean[] abValue = new boolean[1];
//                GLES20.glGetBooleanv(GLES20.GL_DEPTH_TEST, abValue, 0);
//                GLES20.glDisable(GLES20.GL_DEPTH_TEST);
//                m_SurfaceTexture.updateTexImage();
//
//                float[] mMat = new float[16];
//
//                m_SurfaceTexture.getTransformMatrix(mMat);
//
//                RenderScene(mMat, m_iSurfaceTextureID, m_iUnityTextureID);
//
//                if (abValue[0]) {
//                    GLES20.glEnable(GLES20.GL_DEPTH_TEST);
//                } else {
//
//                }
//
//                abValue = null;
//            }
        }

        @Override
        public void getStart(int code)
        {
            if (code == -1)
                Log.e(TAG, "getStart " + code);
            else if (code == 0)
                Log.d(TAG, "getStart " + code);

            final String log = "getStart " + code;
            m_iCurrentState = MEDIAPLAYER_STATE.PLAYING;
        }

        @Override
        public void getStop(int code) {
            Log.d(TAG, "getStop " + code);

            final String log = "getStop " + code;
            m_iCurrentState = MEDIAPLAYER_STATE.STOPPED;
        }

        @Override
        public void getPlay() {
            Log.d(TAG, "getPlay ");

            final String log = "getPlay ";
            m_iCurrentState = MEDIAPLAYER_STATE.PLAYING;
        }

        @Override
        public void getPause() {
            Log.d(TAG, "getPause ");

            final String log = "getPause ";
            m_iCurrentState = MEDIAPLAYER_STATE.PAUSED;
        }

        @Override
        public void getPlayDone() {
            Log.d(TAG, "getPlayDone ");

            final String log = "getPlayDone ";
            m_iCurrentState = MEDIAPLAYER_STATE.END;
            //close();
        }

        @Override
        public void getSlowReceiveFrame(int isSlow)
        {
            Log.d(TAG, "getSlowReceiveFrame : " + isSlow);

            final String log = "getSlowReceiveFrame : " + isSlow;
        }
    };


    public void close() {

        //Log.d(TAG, "?????????????? " + m4DLivePlayer);

        m4DLivePlayer.pause();
        m4DLivePlayer.streamClose();
        Log.d(TAG, "call streamClose ");
    }


    public void open1(String _url, boolean _isTCP, boolean _isHWDec, int _port) {

        //Log.d(TAG, "############ open1 Start");
        boolean isSub = false;

        if (m4DLivePlayer.isStreamOpened()) {
            m4DLivePlayer.streamClose();
            Log.d(TAG,"streamClose");
            return;
        }

        final String url = _url;//mEditURL.getText().toString();
        final boolean isTCP = _isTCP;//mCheckTCP.isChecked();
        final boolean isHWDec = _isHWDec;//mCheckHW.isChecked();
        Surface surfaceMain = m_Surface;

        final Surface surfaceMainEx = surfaceMain;

//        boolean _isOpened = m4DLivePlayer.isStreamOpened();
//
//        Log.d(TAG, "check _isOpened 000 :::  " + _isOpened);

        int ret = 1;
        //Log.d(TAG, "111 ?????????? " + m4DLivePlayer);
        if (!m4DLivePlayer.isStreamOpened()) {
            ret = m4DLivePlayer.streamOpen(url, surfaceMainEx, isTCP, isHWDec);
        }

        //Log.d(TAG, "############ streamOpen End ::: ret = " + ret);
//
//        _isOpened = m4DLivePlayer.isStreamOpened();
//
//        Log.d(TAG, "check _isOpened 111 :::  " + _isOpened);

        final int finalRet = ret;

        if (m4DLivePlayer.isStreamOpened()) {
            int port = _port;//Integer.parseInt(mEditRestFulPort.getText().toString());
            //String ip = mEditRestFulIP.getText().toString();

            String sub1 = url.substring(url.lastIndexOf("//")+2);
            String sub2 = sub1.substring(sub1.lastIndexOf("/")+1);
            String sub3 = sub2.substring(sub2.lastIndexOf("?")+1);

            //Log.d(TAG, "sub1 : " + sub1);
            //Log.d(TAG, "sub2 : " + sub2);
            //Log.d(TAG, "sub3 : " + sub3);

            String ss_ip = sub1.substring(0, sub1.indexOf("/"));
            if (ss_ip.indexOf(":") > 0)
                ss_ip = ss_ip.substring(0, ss_ip.indexOf(":"));

            //Log.d(TAG, "restful : " + ss_ip + " / " + port);
            //Log.d(TAG, "222 ?????????? " + m4DLivePlayer);

            if(m4DLivePlayer != null) {
                m4DLivePlayer.RESTFulOpen(ss_ip, port);
            }else {
                //Log.d(TAG, "m4DLivePlayer nullllllllllllll");
            }
            //Log.d(TAG, "333 ?????????? " + m4DLivePlayer);
        }
        //m4DLivePlayer.play();
        //Log.d(TAG, "444 ?????????? " + m4DLivePlayer);
        //m4DLivePlayer.setLooping(true);
        //Log.d(TAG, "open1 end");
        //Log.d(TAG, "555 ?????????? " + m4DLivePlayer);

        //m4DLivePlayer.pause();

    }


    //////local
    protected void startRendering(String filePath) {

        int numTracks;
        //int startVideoTrack; // start video track
        int numVideoTracks = 0; // video track count
        //int audioTrack; // current audio track

        MediaExtractor extractor = new MediaExtractor();

        try {
            extractor.setDataSource(filePath);

            numTracks = extractor.getTrackCount();
            startVideoTrack = -1;
            numVideoTracks = 0;
            audioTrack = -1;

            for (int i = 0; i < numTracks; ++i) {
                MediaFormat format = extractor.getTrackFormat(i);
                String mime = format.getString(MediaFormat.KEY_MIME);
                if (mime.startsWith("video/")) {
                    if (startVideoTrack == -1) {
                        startVideoTrack = i;
                    }
                    numVideoTracks++;
                } else if (mime.startsWith("audio/")) {
                    audioTrack = i;
                }
            }
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            extractor.release();
            extractor = null;
        }

        //startVideoTrack = startVideoTrack;
        videoTrack = numVideoTracks;
        //audioTrack = 0;

        fdLocalPlayer = new FDMediaPlayer(new File(filePath), m_Surface, startVideoTrack, videoTrack - 1, videoTrack / 2, audioTrack);
        if (fdLocalPlayer != null) {
            fdLocalPlayer.setCallback(fdLocalPlayerCallback);
            fdLocalPlayer.setLooping(false);
        }

        play();
    }

    private FDMediaPlayer.Callback fdLocalPlayerCallback = new FDMediaPlayer.Callback() {
        @Override
        public void onStart() {
            //Log.d(TAG, "onStart()");
            m_iCurrentState = MEDIAPLAYER_STATE.PLAYING;
        }

        @Override
        public void onStop() {
            //Log.d(TAG, "onStop()");
            m_iCurrentState = MEDIAPLAYER_STATE.STOPPED;
        }

        @Override
        public void onPause() {
            //Log.d(TAG, "onPause()");
            m_iCurrentState = MEDIAPLAYER_STATE.PAUSED;
        }

        @Override
        public void onVideoCurrentPosition(long pos) {
            //Log.d(TAG, "onVideoCurrentPosition() = " + pos);
            m_iCurrentSeekPosition = (int)pos;

            int channel = -1;
            int frame = -1;
            int frame_cycle = -1;
            int time = m_iCurrentSeekPosition;
            String utc = "";

            String _value = "getCurrentPlayInfo," + channel + "," + frame + "," + frame_cycle + "," + time + "," + utc;

            UnityPlayer.UnitySendMessage(_UNITY_ANDROID_, "getCurrentPlayInfo", _value);
        }

        @Override
        public void onVideoResolution(int width, int height) {
            //Log.d(TAG, "onVideoResolution() width = " + width +  " / height = " + height);
            m_iVideoWidth = width;
            m_iVideoheight = height;

            String _value = "getVideoStreamInfo," + width + "," + height + "," + m_iDuration;

            UnityPlayer.UnitySendMessage(_UNITY_ANDROID_, "getVideoStreamInfo", _value);
        }

        @Override
        public void onDuration(long duration) {
            //Log.d(TAG, "onDuration() = " + duration);
            m_iDuration = (int)duration;


        }

        @Override
        public void onCompletion() {
            //Log.d(TAG, "onCompletion()");
            m_iCurrentState = MEDIAPLAYER_STATE.END;
        }

    };

    /* ====================== Play Control ====================== */

    public void play() {
        if (fdLocalPlayer != null && !fdLocalPlayer.isPlaying()) {
            fdLocalPlayer.start(null);
        }
    }

    public void pause() {
        if (fdLocalPlayer != null && fdLocalPlayer.isPlaying()) {
            fdLocalPlayer.pause();
        }
    }

    public void stop() {
        if (fdLocalPlayer != null) {
            fdLocalPlayer.stop();
        }
    }

    public void seekTo(long time) {
        if (fdLocalPlayer != null) {
            fdLocalPlayer.seekTo(time);
        }
    }

    /* ====================== Interactive 제어 ====================== */

    public void onChangeChannelFrameLeft() {
        if (fdLocalPlayer != null) {
            //Log.d(TAG, "onChangeChannelFrameLeft");
            fdLocalPlayer.setChangeChannelFrame(1, 0, 0);
        }
    }

    public void onChangeChannelFrameRight() {
        if (fdLocalPlayer != null) {
            //Log.d(TAG, "onChangeChannelFrameRight");
            fdLocalPlayer.setChangeChannelFrame(-1, 0, 0);
        }
    }

    public void onTimeShiftRewind() {
        if (fdLocalPlayer != null) {
            //Log.d(TAG, "onTimeShiftRewind");
            fdLocalPlayer.setChangeChannelFrame(0, -1, 0);
        }
    }

    public void onTimeShiftForward() {
        if (fdLocalPlayer != null) {
            //Log.d(TAG, "onTimeShiftForward");
            fdLocalPlayer.setChangeChannelFrame(0, 1, 0);
        }
    }




}

