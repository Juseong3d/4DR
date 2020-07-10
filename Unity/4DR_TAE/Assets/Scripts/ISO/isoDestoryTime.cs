using UnityEngine;
using System.Collections;

public class isoDestoryTime : MonoBehaviour {

	
	public bool isStatus;
	public float time;
	float lifeTime;

	// Use this for initialization
	void Awake () {

		isStatus = false;
		lifeTime = 0.0f;

		if(time != 0) {
			isStatus = true;
		}

	
	}
	
	// Update is called once per frame
	void Update () {

		if(isStatus == false)
			return;

		if(lifeTime > time) {
			NGUITools.Destroy(this.gameObject);
		}

		lifeTime += Time.deltaTime;
	
	}


	public void SET_DESTROY_TIMER(float t) {

		this.time = t;
		isStatus = true;

	}
}
