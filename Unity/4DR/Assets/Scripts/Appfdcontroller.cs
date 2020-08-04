using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appfdcontroller : MonoBehaviour
{

    public int _PORT = 7070;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void _SEND(string _url) {

        //WWWForm form = new WWWForm();
        Dictionary<string, string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
        headers.Add("Contents-Length", "10");

        //form.headers.Remove("Content-Type");
        //form.headers.Add("Content-Type", "application/json");
        //form.headers.Add("Contents-Length", "0");

        _url = "http://app.4dlive.kr:" + _PORT;
        _url = "http://app.4dlive.kr:7070";

		Debug.Log("URL :: " + _url);        
        //WWW www = new WWW(_url, form);
		WWW www = new WWW(_url, null, headers);
        
        StartCoroutine(WaitForRequest(www));

    }


    private IEnumerator WaitForRequest(WWW www) {

        yield return www;

        // check for errors
        if (www.error == null) {
            Debug.Log("recv : " + www.text);
        } else {
            Debug.Log("WWW Error: "+ www.error);
			
        }
    }
}
