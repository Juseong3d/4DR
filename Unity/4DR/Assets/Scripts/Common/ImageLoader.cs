using UnityEngine;
using System.Collections;
using System;
 
public class ImageLoader : MonoBehaviour {
 
    // 다운로드 받을 이미지 주소 - 에디터에서 설정한다.
    public string imageUrl = null;
	public UITexture texture;
	public Texture _texture;

	public bool isDestory;

	public bool isPixelPerface;
 
	public bool isNeedChange;
    // 객체 생성시 코루틴 시작
    // --> 이미지 로딩이 전체 실행에 영향을 주지 않도록...
    void Start () {
        
		isNeedChange = false;

    }


	public void SET(string url, UITexture texture, bool isDestory = false, bool isPixelPerface = true, bool isNeedChange = false) {

		this.imageUrl = url;
		this.texture = texture;
		this.isDestory = isDestory;
		this.isPixelPerface = isPixelPerface;
		this.isNeedChange = isNeedChange;
	}


	public void SET(string url, Texture texture, bool isDestory = false, bool isPixelPerface = true, bool isNeedChange = false) {

		this.imageUrl = url;
		this._texture = texture;
		this.isDestory = isDestory;
		this.isPixelPerface = isPixelPerface;
		this.isNeedChange = isNeedChange;
	}


	public void START() {

		bool ishave = false;

		if(Appmain.appimg.tempTexture != null) {
			for(int i = 0; i<Appmain.appimg.tempTexture.Length; i++) {

				if(Appmain.appimg.tempTexture[i].url.Equals(imageUrl) == true) {
					if(Appmain.appimg.tempTexture[i].texture != null) {
						texture.mainTexture = Appmain.appimg.tempTexture[i].texture;
						if(isPixelPerface == true) {
							texture.MakePixelPerfect();
						}
						if(isDestory == true) {
							NGUITools.Destroy(this.gameObject);
						}
					}else {
						StartCoroutine(waitDownloadAndSetting(imageUrl));
					}

					ishave = true;
					break;
				}
			}
		}

		if(ishave == false) {
			StartCoroutine(loadImage(imageUrl, isNeedChange));
		}
	}


	IEnumerator waitDownloadAndSetting(string url) {

		yield return new WaitForEndOfFrame();

		if(Appmain.appimg.tempTexture != null) {
			for(int i = 0; i<Appmain.appimg.tempTexture.Length; i++) {

				if(Appmain.appimg.tempTexture[i].url.Equals(url) == true) {

					if(Appmain.appimg.tempTexture[i].texture != null) {
					
						texture.mainTexture = Appmain.appimg.tempTexture[i].texture;
						texture.MakePixelPerfect();

						if(isDestory == true) {
							NGUITools.Destroy(this.gameObject);
						}
					}else {
						StartCoroutine(waitDownloadAndSetting(imageUrl));
					}
				}
			}
		}else {

		}
	}


    IEnumerator loadImage(string url, bool isNeedChange = false) {
        if (url != null) {
            WWW www = new WWW(url);

			int i = 0;

			if(Appmain.appimg.tempTexture != null) {
				for(i = 0; i<Appmain.appimg.tempTexture.Length; i++) {
					if(string.IsNullOrEmpty(Appmain.appimg.tempTexture[i].url) == true) {
						Appmain.appimg.tempTexture[i].url = url;
						break;
					}
				}
			}

            yield return www;

			try {
				if(www.error == null) {
					if(texture != null) {
						NGUITools.SetActive(texture.gameObject, true);
						texture.mainTexture = www.texture;						
					}
					_texture = www.texture;

					if(isPixelPerface == true) {
						texture.MakePixelPerfect();
					}

					if(Appmain.appimg.tempTexture != null) {
						if(Appmain.appimg.tempTexture[i].texture == null) {
							Appmain.appimg.tempTexture[i].texture = www.texture;												
						}
					}
					
				}else {
					Debug.Log("url : " + url);
					Debug.Log("error : [" + www.error + "]");
										
				}
			}catch(Exception e) {
				Debug.Log("exception... : " + e);
			}

			if(isDestory == true) {
				//NGUITools.Destroy(texture);
				NGUITools.Destroy(this.gameObject);
			}
			//www.Dispose();
        }
    }
}
