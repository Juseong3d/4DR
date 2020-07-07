using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uisoEffectText : MonoBehaviour
{

    public UISprite _sprite;
    public UILabel _label;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SET_SPRITE_COLOR(string _clr) {

        Color _color;
        bool isResult = ColorUtility.TryParseHtmlString(_clr, out _color);

        if(isResult == true) {
            _sprite.color = _color;
        }
    }


    public void SET_LABEL(string _value) {

        _label.text = _value;

    }

}
