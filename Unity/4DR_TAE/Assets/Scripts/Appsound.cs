using UnityEngine;
using System.Collections;
using System;

public enum SOUND_BGM_TYPE {

		TITLE_AND_MENU,
		IN_GAME_ONE,
		IN_GAME_TWO,
		IN_GAME_THREE,
		IN_GAME_FOUR,
		LENGTH
}


public enum SOUND_EFFECT_TYPE {

	BUTTON,
	WARING,
	GAME_START,
	GAME_MOVE,
	SUCCESS,
	TIME_OVER,
	RUBY_BUY,
	SUCCESS_NEW,
	FAIL_NEW,
	BUTTON2,
	CTL_MOVE,
	LENGTH

}

public class Appsound : MonoBehaviour {
	 
    public Appmain appmain;

    public AudioClip[] audioClipBGM;
    public AudioClip[] audioClipEffect;

    public AudioSource audioSourceBGM;    
    public AudioSource audioSourceEffect;
    
    public bool isCheckPlayEffectSound;
	public bool isCheckPlayRoomEnter;


	// Use this for initialization
	void Start () {
        appmain = (Appmain)FindObjectOfType(typeof(Appmain));
        
		initAudioSources();
	    initAudioClips();

        isCheckPlayEffectSound = false;
		isCheckPlayRoomEnter = false;

	}

	
    private void initAudioSources() {

        audioSourceBGM = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSourceBGM.playOnAwake = false;
        
        audioSourceEffect = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSourceEffect.playOnAwake = false;
        
    }
	

    public void initAudioClips() {

		string stringBGM = "Common/_Default_Sound/BGM/";
		string stringEffect = "Common/_Default_Sound/effect/";		

		// bgm
		int bgmClipCount = (int)SOUND_BGM_TYPE.LENGTH;

		if (audioClipBGM != null) audioClipBGM = null;

		audioClipBGM = new AudioClip[bgmClipCount];

		for(int i = 0; i<bgmClipCount; i++) {

		    audioClipBGM[i] = (AudioClip)Resources.Load( stringBGM + i);

		}

		int effecClipCount = Enum.GetValues(typeof(SOUND_EFFECT_TYPE)).Length;
		
		audioClipEffect = new AudioClip[effecClipCount];

		for (int i = 0; i < audioClipEffect.Length; i++) {

			audioClipEffect[i] = (AudioClip)Resources.Load(stringEffect + i);

		}
    }



    internal void playEffect(SOUND_EFFECT_TYPE type, float delay = 0.0f) {

        if (GET_SETTING_SOUND() == true) {
            StartCoroutine(this.coroutineEffect((int)type, delay));        
        }
    }


	internal void playEffectButton() {

        if (GET_SETTING_SOUND() == true) {
			int rand = UnityEngine.Random.Range(0, 2);
			int type = (int)((rand == 0) ? SOUND_EFFECT_TYPE.BUTTON:SOUND_EFFECT_TYPE.BUTTON2);

            StartCoroutine(this.coroutineEffect(type, 0.0f));        
        }
    }

    IEnumerator coroutineEffect(int index, float delay) {

        yield return new WaitForSeconds(delay);

		try {
			audioSourceEffect.PlayOneShot(audioClipEffect[index]);
		}catch(Exception e) {
			Debug.Log("e : " + e);
		}
    }
   


    internal void playBGM(SOUND_BGM_TYPE type, bool isLoop = true, float volume = 0.7f) {
		Debug.Log("ori playBGMRandom :: " + type);

		if (GET_SETTING_BGM() == true) {

			audioSourceBGM.loop = isLoop;
			audioSourceBGM.volume = volume;
			audioSourceBGM.clip = audioClipBGM[(int)type];
			audioSourceBGM.Play();
		}else {
            audioSourceBGM.Stop();
        }
    }


	internal void playBGMRandom(SOUND_BGM_TYPE type, bool isLoop = true, float volume = 0.7f) {

		Debug.Log("ran playBGMRandom :: " + type);

		int num = UnityEngine.Random.Range(0,2);

		if (GET_SETTING_BGM() == true) {

			audioSourceBGM.loop = isLoop;
			audioSourceBGM.volume = volume;
			audioSourceBGM.clip = audioClipBGM[(int)type + num];
			audioSourceBGM.Play();
		}else{
            audioSourceBGM.Stop();
        }
    }


	internal void stopBGM() {

        if (audioSourceBGM.isPlaying) audioSourceBGM.Stop();

    }


	public static bool GET_SETTING_BGM() {

		return Convert.ToBoolean(PlayerPrefs.GetInt(DEFINE.KEY_OPTION_BGM, 1));

	}


	public static bool GET_SETTING_SOUND() {

		return Convert.ToBoolean(PlayerPrefs.GetInt(DEFINE.KEY_OPTION_EFFECT_SOUND, 1));

	}
}
