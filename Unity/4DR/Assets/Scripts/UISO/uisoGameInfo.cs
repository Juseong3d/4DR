using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class uisoGameInfo : MonoBehaviour
{

    public GAME_INFO_TAE _info;

    public DEFAULT_PLAYER_LIST _blue;
    public DEFAULT_PLAYER_LIST _red;

    public UILabel labelRoundTime;
    public UILabel labelRoundCnt;

    public UISlider sliderBlue;
    public UISlider sliderRed;

    public UISlider sliderBlueBack;
    public UISlider sliderRedBack;

    public UISlider sliderBluePenalty;
    public UISlider sliderRedPenalty;    

    public UIToggle[] toggleRed;
    public UIToggle[] toggleBlue;

    public UILabel labelBlueName;
    public UILabel labelRedName;
    
    [Header("* up score -----------------")]
    public UILabel labelScoreRed;
    public UILabel labelScoreBlue;

    public UILabel labelMatchCnt;

    public UILabel labelCountryRed;
    public UILabel labelCountryBlue;

    public UISprite spriteCountryRed;
    public UISprite spriteCountryBlue;

    public ParticleSystem psPenaltyRed;
    public ParticleSystem psPenaltyBlue;
    
    public ParticleSystem psHPRed;
    public ParticleSystem psHPBlue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_info.isPlaying == true) {

            _info.nowRoundTime -= Time.deltaTime;

            if(_info.nowRoundTime < 0f) {
                _info.nowRoundTime = 0f;
                _info.isPlaying = false;
            }
#if _TAE_
            SET_ROUND_TIME();
#endif
        }        

        if(_info.gameType == GAME_TYPE_TAE.MINUS) {
            if(_info.roundInfo[_info.nowRoundCnt].blue.isPenalty == true) {
                _info.roundInfo[_info.nowRoundCnt].blue.nowPenaltyTime -= Time.deltaTime;

                if(_info.roundInfo[_info.nowRoundCnt].blue.nowPenaltyTime < 0f) {
                    _info.roundInfo[_info.nowRoundCnt].blue.nowPenaltyTime = 0f;
                    _info.roundInfo[_info.nowRoundCnt].blue.isPenalty = false;
                    NGUITools.SetActive(sliderBluePenalty.gameObject, false);

                    NGUITools.SetActive(psPenaltyBlue.gameObject, false);                    
                }
                {
                    sliderBluePenalty.value = _info.roundInfo[_info.nowRoundCnt].blue.nowPenaltyTime / DEFINE.MAX_PENALTY_TIME;
                }
            }

            if(_info.roundInfo[_info.nowRoundCnt].red.isPenalty == true) {
                _info.roundInfo[_info.nowRoundCnt].red.nowPenaltyTime -= Time.deltaTime;

                if(_info.roundInfo[_info.nowRoundCnt].red.nowPenaltyTime < 0f) {
                    _info.roundInfo[_info.nowRoundCnt].red.nowPenaltyTime = 0f;
                    _info.roundInfo[_info.nowRoundCnt].red.isPenalty = false;
                    NGUITools.SetActive(sliderRedPenalty.gameObject, false);

                    NGUITools.SetActive(psPenaltyRed.gameObject, false);                    
                }
                {
                    sliderRedPenalty.value = _info.roundInfo[_info.nowRoundCnt].red.nowPenaltyTime / DEFINE.MAX_PENALTY_TIME;
                }            
            }

            if(_info.roundInfo[_info.nowRoundCnt].prevBlueScore != _info.roundInfo[_info.nowRoundCnt].blueScore) {

                _info.roundInfo[_info.nowRoundCnt].prevBlueScore --;
                sliderBlue.value = ((float)_info.roundInfo[_info.nowRoundCnt].prevBlueScore) / DEFINE.MAX_MINUS_GAME_SCORE;            

            }

            if(_info.roundInfo[_info.nowRoundCnt].prevRedScore != _info.roundInfo[_info.nowRoundCnt].redScore) {

                _info.roundInfo[_info.nowRoundCnt].prevRedScore --;
                sliderRed.value = ((float)_info.roundInfo[_info.nowRoundCnt].prevRedScore) / DEFINE.MAX_MINUS_GAME_SCORE;

            }
        }
    }

#if _TAE_
    public void SET_INFO(GAME_INFO_TAE _info) {

        this._info = _info;

        if(_info.gameType == GAME_TYPE_TAE.MINUS) {

            labelRoundCnt.text = string.Format("{0}R", _info.nowRoundCnt);

            //3라운드 5라운드를 박아스 쓰좌...
            if(_info.nowStageCnt == 0) {

            }else {
                NGUITools.SetActive(toggleBlue[2].gameObject, false);
                NGUITools.SetActive(toggleRed[2].gameObject, false);
            }

            NGUITools.SetActive(sliderBluePenalty.gameObject, false);
            NGUITools.SetActive(sliderRedPenalty.gameObject, false);

            for(int i = 0; i<toggleBlue.Length; i++) {
                if(i < _info.roundInfo[_info.nowRoundCnt].blueWinCnt) {
                    toggleBlue[i].value = true;
                }else {
                    toggleBlue[i].value = false;
                }
            }

            for(int i = 0; i<toggleRed.Length; i++) {
                if(i < _info.roundInfo[_info.nowRoundCnt].redWinCnt) {
                    toggleRed[i].value = true;
                }else {
                    toggleRed[i].value = false;
                }
            }

            labelBlueName.text = string.Format("{0}", _info.roundInfo[_info.nowRoundCnt].blue.playerName);
            labelRedName.text = string.Format("{0}", _info.roundInfo[_info.nowRoundCnt].red.playerName);

            //psPenaltyBlue.Stop();
            //psPenaltyRed.Stop();
            NGUITools.SetActive(psPenaltyBlue.gameObject, false);
            NGUITools.SetActive(psPenaltyRed.gameObject, false);
            
        }else if(_info.gameType == GAME_TYPE_TAE.PLUS) {

            if(labelMatchCnt != null) 
                labelMatchCnt.text = string.Format("MATCH\n{0}", _info.nowStageCnt);

            if(labelRoundCnt != null)
                labelRoundCnt.text = string.Format("R{0}", _info.nowRoundCnt);

            labelBlueName.text = string.Format("{0}", _info.roundInfo[_info.nowRoundCnt].blue.playerName);
            labelRedName.text = string.Format("{0}", _info.roundInfo[_info.nowRoundCnt].red.playerName);

            if(labelCountryBlue != null)
                labelCountryBlue.text = string.Format("{0}", _info.roundInfo[_info.nowRoundCnt].blue.country);

            if(labelCountryRed != null)
                labelCountryRed.text = string.Format("{0}", _info.roundInfo[_info.nowRoundCnt].red.country);

            COUNTRY_CODE _blue = Appdoc.GET_COUNTRY_CODE(_info.roundInfo[_info.nowRoundCnt].blue.country);
            COUNTRY_CODE _red = Appdoc.GET_COUNTRY_CODE(_info.roundInfo[_info.nowRoundCnt].red.country);

            spriteCountryBlue.spriteName = _blue.alpha2Code.ToLower();
            spriteCountryRed.spriteName = _red.alpha2Code.ToLower();

            labelScoreBlue.text = string.Format("{0}", _info.roundInfo[_info.nowRoundCnt].blueScore);
            labelScoreRed.text = string.Format("{0}", _info.roundInfo[_info.nowRoundCnt].redScore);

        }

        SET_ROUND_TIME();
    }
#endif

#if _TAE_
    public void SET_ROUND_TIME() {

        if(_info.nowRoundTime <= 10f) {
            labelRoundTime.text = string.Format("{0:0.00}", _info.nowRoundTime);
        }else {
            TimeSpan time = TimeSpan.FromSeconds(_info.nowRoundTime);
            labelRoundTime.text = string.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);
        }
    }


    public void UPDATE_SCORE(Q_COMMAND_CTL_CAMERA _cmd) {

        if(_info.gameType == GAME_TYPE_TAE.MINUS) {

            WHAT_TEAM_COLOR _who = _cmd.setScoreWho;

            sliderBlue.value = ((float)_info.roundInfo[_info.nowRoundCnt].prevBlueScore) / DEFINE.MAX_MINUS_GAME_SCORE;
            sliderRed.value = ((float)_info.roundInfo[_info.nowRoundCnt].prevRedScore) / DEFINE.MAX_MINUS_GAME_SCORE;

            GameObject _prfab = Appimg.LoadResource4Prefab4UI(UIDEFINE.PATH_EFFECT_DAMAGE);
            UILabel _label = _prfab.GetComponentInChildren<UILabel>();

            _label.text = string.Format("{0}", _cmd.setScore);            

            switch(_who) {
                case WHAT_TEAM_COLOR.BLUE:
                    _prfab.transform.SetParent(sliderBlue.thumb.transform);
                    if(psHPBlue != null) {
                        psHPBlue.Play();
                    }
                    break;
                case WHAT_TEAM_COLOR.RED:
                    _prfab.transform.SetParent(sliderRed.thumb.transform);
                    if(psHPRed != null) {
                        psHPRed.Play();
                    }
                    break;
            }
            
            StartCoroutine(SET_BACK_GAUAGE(0.5f));

        }else if(_info.gameType == GAME_TYPE_TAE.PLUS) {
            
            WHAT_TEAM_COLOR _who = _cmd.setScoreWho;

            if(_who == WHAT_TEAM_COLOR.BLUE) {
                labelScoreBlue.text = string.Format("{0}", _info.roundInfo[_info.nowRoundCnt].blueScore);
            }else if(_who == WHAT_TEAM_COLOR.RED) {
                labelScoreRed.text = string.Format("{0}", _info.roundInfo[_info.nowRoundCnt].redScore);
            }
        }
    }


    public void SET_PERNALTY(WHAT_TEAM_COLOR _who) {

        if(_info.gameType != GAME_TYPE_TAE.MINUS) return;

        if(_who == WHAT_TEAM_COLOR.BLUE) {
            _info.roundInfo[_info.nowRoundCnt].blue.isPenalty = true;
            _info.roundInfo[_info.nowRoundCnt].blue.nowPenaltyTime = DEFINE.MAX_PENALTY_TIME;

            if(sliderBluePenalty != null) {
                NGUITools.SetActive(sliderBluePenalty.gameObject, true);
            }

            if(psPenaltyBlue != null) {
                NGUITools.SetActive(psPenaltyBlue.gameObject, true);
                
            }
        }else if(_who == WHAT_TEAM_COLOR.RED) {
            _info.roundInfo[_info.nowRoundCnt].red.isPenalty = true;
            _info.roundInfo[_info.nowRoundCnt].red.nowPenaltyTime = DEFINE.MAX_PENALTY_TIME;

            if(sliderBluePenalty != null) {
                NGUITools.SetActive(sliderRedPenalty.gameObject, true);
            }

            if(psPenaltyRed != null) {
                NGUITools.SetActive(psPenaltyRed.gameObject, true);
            }
        }
    }


    IEnumerator SET_BACK_GAUAGE(float delay = 0.5f) {

        yield return new WaitForSeconds(delay);

        sliderBlueBack.value = sliderBlue.value;
        sliderRedBack.value = sliderRed.value;

    }
#endif
}
