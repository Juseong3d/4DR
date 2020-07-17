using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uisoPlayerInfo : MonoBehaviour
{
    public UILabel labelTeamName;
    public UILabel labelAgeand;
    public UILabel labelMainSkill;
    public UILabel labelCur;
    public UILabel labelPlayerName;


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
        labelPlayerName.text = _playerInfo.playerName;
        labelAgeand.text = string.Format("Age {0}\nTall {1}\nWeight {2}", _playerInfo.age, _playerInfo.tall, _playerInfo.weight);
        labelMainSkill.text = string.Format("[ffff64]MAIN SKILL[-]\n{0}", _playerInfo.s_skill);
        labelCur.text = string.Format("{0}", _playerInfo.spcial_info);

    }
}
