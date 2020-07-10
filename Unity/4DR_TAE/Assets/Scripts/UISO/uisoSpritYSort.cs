using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uisoSpritYSort : MonoBehaviour
{

    UISprite _sprite;

    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<UISprite>();
    }

    // Update is called once per frame
    void Update()
    {
        _sprite.depth = (int)(1000.0f - transform.localPosition.y);
    }
}
