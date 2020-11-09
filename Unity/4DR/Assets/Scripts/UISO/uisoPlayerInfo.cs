using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uisoPlayerInfo : MonoBehaviour
{
    public UILabel labelAge;

    public UILabel labelPlayerName;
    public UILabel labelSpcialInfo;

    public UILabel labelTeamName;
    public UILabel labelAgeand;

    public UILabel labelTotalPoint;

    public UILabel labelMainSkill;
    
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SET_INFO(DEFAULT_PLAYER_LIST _playerInfo) {

        labelTeamName.text = string.Format("{0} {1}", _playerInfo.country, _playerInfo.teamName);
        labelPlayerName.text = string.Format("{0} ({1})", _playerInfo.playerName, _playerInfo.age);
        labelAgeand.text = string.Format("{1}cm/{2}kg", _playerInfo.age, _playerInfo.tall, _playerInfo.weight);
        labelAge.text = string.Format("{0}", _playerInfo.age);
        //labelMainSkill.text = string.Format("[ffff64]MAIN SKILL[-]\n{0}", _playerInfo.s_skill);
        labelMainSkill.text = string.Format("{0}", _playerInfo.s_skill);
        labelSpcialInfo.text = string.Format("{0}", _playerInfo.spcial_info);

        labelTotalPoint.text = string.Format("-");

    }
}
