using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uisoDamage : MonoBehaviour
{

    public UILabel[] labelScore;
    public UISprite spriteMain;


    private void Awake() {

        labelScore = this.gameObject.GetComponentsInChildren<UILabel>();
        if(spriteMain == null) {
            spriteMain = this.gameObject.GetComponent<UISprite>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SET_INFO(PARTS where, WHAT_TEAM_COLOR who, int score) {

        string _who = who.ToString().ToLower();

        if(spriteMain != null) {
            spriteMain.spriteName = string.Format("img_{0}_{1}", where.ToString(), _who);
        }

        if(labelScore != null) {
            for(int i = 0; i<labelScore.Length; i++) {
                labelScore[i].text = score.ToString();
            }
        }

    }

}
