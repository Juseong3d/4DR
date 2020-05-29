using UnityEngine;
using System.Collections;

public class isoDDO : MonoBehaviour {

    void Awake() {
        DontDestroyOnLoad(transform.gameObject);	
	}
}


