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

    public UISlider sliderBluePenalty;
    public UISlider sliderRedPenalty;    

    public UIToggle[] toggleRed;
    public UIToggle[] toggleBlue;

    public UILabel labelBlueName;
    public UILabel labelRedName;
    

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

            SET_ROUND_TIME();
        }        

        if(_info.roundInfo[_info.nowRoundCnt].blue.isPenalty == true) {
            _info.roundInfo[_info.nowRoundCnt].blue.nowPenaltyTime -= Time.deltaTime;

            if(_info.roundInfo[_info.nowRoundCnt].blue.nowPenaltyTime < 0f) {
                _info.roundInfo[_info.nowRoundCnt].blue.nowPenaltyTime = 0f;
                _info.roundInfo[_info.nowRoundCnt].blue.isPenalty = false;
                NGUITools.SetActive(sliderBluePenalty.gameObject, false);
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
            }
            {
                sliderBluePenalty.value = _info.roundInfo[_info.nowRoundCnt].blue.nowPenaltyTime / DEFINE.MAX_PENALTY_TIME;
            }

            
        }
    }


    public void SET_INFO(GAME_INFO_TAE _info) {

        this._info = _info;

        labelRoundCnt.text = string.Format("{0}R", _info.nowStageCnt);

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

        SET_ROUND_TIME();
        
    }


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

            sliderBlue.value = ((float)_info.roundInfo[_info.nowRoundCnt].blueScore) / DEFINE.MAX_MINUS_GAME_SCORE;
            sliderRed.value = ((float)_info.roundInfo[_info.nowRoundCnt].redScore) / DEFINE.MAX_MINUS_GAME_SCORE;

            GameObject _prfab = Appimg.LoadResource4Prefab4UI(UIDEFINE.PATH_EFFECT_DAMAGE);
            UILabel _label = _prfab.GetComponentInChildren<UILabel>();

            _label.text = string.Format("{0}", _cmd.setScore);

            if(_who == WHAT_TEAM_COLOR.BLUE) {
                _prfab.transform.SetParent(sliderBlue.thumb.transform);
            }else if(_who == WHAT_TEAM_COLOR.RED) {
                _prfab.transform.SetParent(sliderRed.thumb.transform);
            }
            
        }
    }


    public void SET_PERNALTY(WHAT_TEAM_COLOR _who) {

        if(_who == WHAT_TEAM_COLOR.BLUE) {
            _info.roundInfo[_info.nowRoundCnt].blue.isPenalty = true;
            _info.roundInfo[_info.nowRoundCnt].blue.nowPenaltyTime = DEFINE.MAX_PENALTY_TIME;
            NGUITools.SetActive(sliderBluePenalty.gameObject, true);
        }else if(_who == WHAT_TEAM_COLOR.RED) {
            _info.roundInfo[_info.nowRoundCnt].red.isPenalty = true;
            _info.roundInfo[_info.nowRoundCnt].red.nowPenaltyTime = DEFINE.MAX_PENALTY_TIME;
            NGUITools.SetActive(sliderRedPenalty.gameObject, true);
        }
    }
}
