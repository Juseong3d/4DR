using UnityEngine;
using System.Collections;

public class MedaiPlayerSampleGUI : MonoBehaviour {
	
	public MediaPlayerCtrl scrMedia;
	
	public bool m_bFinish = false;
	public int _nowSeek;

	public int _index = 0;
	// Use this for initialization
	void Start () {

		scrMedia.OnEnd += OnEnd;
		scrMedia.OnReady += OnReady;

		_index = 0;
		_nowSeek = 0;

	}

	
	// Update is called once per frame
	void Update () {


	
	}
	
	void OnGUI() {
		
		string status = scrMedia.GetSeekPosition().ToString();
	
		if( GUI.Button(new Rect(50,50,200,100), status))
		{
			string _url = string.Format("rtsp://192.168.0.62:1935/vod/unpack_1.mp4?videoindex={0}", _index);
			scrMedia.Load(_url);
			m_bFinish = false;
		}

		if( GUI.Button(new Rect(600,50,100,100),"prev"))
		{
			_nowSeek = scrMedia.GetSeekPosition();
			_index --;

			string _url = string.Format("rtsp://192.168.0.62:1935/vod/unpack_1.mp4?videoindex={0}", _index);
			scrMedia.Load(_url);			
			m_bFinish = false;
		}

		if( GUI.Button(new Rect(700,50,100,100),"next"))
		{
			_nowSeek = scrMedia.GetSeekPosition();
			_index ++;

			string _url = string.Format("rtsp://192.168.0.62:1935/vod/unpack_1.mp4?videoindex={0}", _index);
			scrMedia.Load(_url);			
			m_bFinish = false;
		}
		
		if( GUI.Button(new Rect(50,200,100,100),"Play"))
		{
			scrMedia.Play();
			m_bFinish = false;
		}
	 	
		if( GUI.Button(new Rect(50,350,100,100),"stop"))
		{
			scrMedia.Stop();
		}
		
		if( GUI.Button(new Rect(50,500,100,100),"pause"))
		{
			scrMedia.Pause();
		}
		
		if( GUI.Button(new Rect(50,650,100,100),"Unload"))
		{
			scrMedia.UnLoad();
		}
		
		if( GUI.Button(new Rect(50,800,100,100), " " + m_bFinish))
		{
		
		}
		
		if( GUI.Button(new Rect(200,50,100,100),"SeekTo"))
		{
			scrMedia.SeekTo(_nowSeek);
		}


		if( scrMedia.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
		{
			if( GUI.Button(new Rect(200,200,100,100),scrMedia.GetSeekPosition().ToString()))
			{
				//_nowSeek = scrMedia.GetSeekPosition();
			}
			
			if( GUI.Button(new Rect(200,350,100,100),scrMedia.GetDuration().ToString()))
			{
				
			}

			if( GUI.Button(new Rect(200,450,100,100),scrMedia.GetVideoWidth().ToString()))
			{
				
			}

			if( GUI.Button(new Rect(200,550,100,100),scrMedia.GetVideoHeight().ToString()))
			{
				
			}
		}

		if( GUI.Button(new Rect(200,650,100,100),scrMedia.GetCurrentSeekPercent().ToString()))
		{
			
		}
	

	}


	
	void OnEnd()
	{
		m_bFinish = true;
	}


	void OnReady() {

		Debug.Log("_nowSeek : " + _nowSeek);	
		scrMedia.SeekTo(_nowSeek);
	}


	IEnumerator _SET_SEEK(float _value) {

		yield return new WaitForSeconds(_value);

		scrMedia.SeekTo(_nowSeek);

	}
}
