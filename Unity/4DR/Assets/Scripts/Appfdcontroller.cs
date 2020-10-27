using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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


    public void _SEND(string _url, string body) {

        string url = "http://app.4dlive.kr:7070" + "/4dapp/movie/swipe";

        WWW www;
        var formData = System.Text.Encoding.UTF8.GetBytes(body);
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");
        postHeader.Add("Contents-Length", formData.Length.ToString());

        
		Debug.Log("URL :: " + _url);        
        //WWW www = new WWW(_url, form);
		www = new WWW(url, formData, postHeader);
        
        StartCoroutine(WaitForRequest(www));

    }


    public IEnumerator _SEND_(string _url, string body) {

        string url = "http://app.4dlive.kr:7070" + "/4dapp/movie/swipe";
        //4dapp/movie/swipe
        
        WWWForm form = new WWWForm();
        form.AddField("POST", body);        
 
        Debug.Log("url : " + url);
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Contents-Length", body.Length.ToString());

        yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log(www.downloadHandler.text);
        }
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
