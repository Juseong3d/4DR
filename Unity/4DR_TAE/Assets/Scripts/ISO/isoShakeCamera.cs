using UnityEngine;
using System.Collections;

public class isoShakeCamera : MonoBehaviour
{
    public float updownDecaytime_0 = 1.0f;
    public float leftrightDecaytime_0 = 1.0f;
	public float updownDecaytime = 0.0f;
    public float leftrightDecaytime = 0.0f;
    public Vector3 offset = new Vector3(0, 0, 0);
    public Vector3 position;
    public float startTime;
    public float amp = 0.15f; // 진폭 . 상하로 이동하는 정도. 값이 클수록 더 크게 진동 .
	//public float amp = 0.3f; // 진폭 . 상하로 이동하는 정도. 값이 클수록 더 크게 진동 .
    public float freq = 7f; // 진동수. 초당 상하 운동 횟수. 값이 클수록 더 빨 진동 .
    public float phase = 0f; // 위상 . 함수의 시작 포지ㅅㄴ.. 모르겠다 꾀꼬리 복붙은 위대해 

    public bool updown;
    public bool leftright;
	// Use this for initialization
	void Start ()
	{
        //StartUpDownShake(10.0f);
	}

	//void OnGUI()
	//{
	//    if (GUI.Button(new Rect(100, 100, 100, 100), ""))
	//    {
	//        StartUpDownShake(updownDecaytime_0);
	//    }
	//    if (GUI.Button(new Rect(200, 100, 100, 100), ""))
	//    {
	//        StartLeftRightShake(leftrightDecaytime_0);
	//    }
	//    if (GUI.Button(new Rect(300, 100, 100, 100), ""))
	//    {
	//        StartUpDownShake(updownDecaytime_0);
	//        StartLeftRightShake(leftrightDecaytime_0);
	//    }
	//}
	
	public void StartUpDownShake( float decay )
	{		
		startTime = Time.time;
		updownDecaytime = decay + Time.fixedTime - startTime;
		position = this.gameObject.transform.localPosition;
		updown = true;
	}
	
	public void StartLeftRightShake( float decay )
	{
		startTime = Time.time;
		leftrightDecaytime = decay + Time.fixedTime - startTime;
		position = this.gameObject.transform.localPosition;
		leftright = true;
	}
	
	// Update is called once per frame
	public void Update ()
	{
		
	}
	
	public void FixedUpdate ()
	{
		if(updown) UpDownShake();
		if(leftright) LeftRightShake();
	}
	
	public void SetOption(float _amp, float _freq) // 진폭, 진동수 
	{
		amp = _amp;
		freq = _freq;
	}
	
	public void UpDownShake()
	{
		float totaltime = Time.fixedTime - startTime;
		if( totaltime < updownDecaytime )
		{
			Vector3 pos = this.gameObject.transform.localPosition;
			
			pos -= offset;
			
			offset.y = Mathf.Sin( 2 * 3.14159f * (totaltime * freq) + phase ) * amp * (updownDecaytime - totaltime) / updownDecaytime ; 
			
			pos += offset;
			
			this.gameObject.transform.localPosition = pos;
		}
		else
		{
			updown = false;
			this.gameObject.transform.localPosition = position;
			offset = new Vector3(0,0,0);
		}
	}
	public void LeftRightShake()
	{
		float totaltime = Time.fixedTime - startTime;
		if( totaltime < leftrightDecaytime )
		{
			Vector3 pos = this.gameObject.transform.localPosition;
			
			pos -= offset;
			
			offset.x = Mathf.Sin( 2 * 3.14159f * (totaltime * freq) + phase ) * amp * (leftrightDecaytime - totaltime) / leftrightDecaytime ; 
			
			pos += offset;
			
			this.gameObject.transform.localPosition = pos;
		}
		else
		{
			offset = new Vector3(0,0,0);
			this.gameObject.transform.localPosition = position;
			leftright = false;
		}
	}
}
