using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isoUVAnimaiton : MonoBehaviour
{

    public Material _material;
    public Vector2 _texOffset;

    // Start is called before the first frame update
    void Start()
    {
        _material = this.gameObject.GetComponent<Renderer>().material;
        _texOffset = _material.GetTextureOffset("_MainTex");

    }

    // Update is called once per frame
    void Update()
    {
        _texOffset.y += Time.deltaTime; 

        if(_texOffset.y >= 1.0f) {
            _texOffset.y = 0.0f;
        }
        _material.SetTextureOffset("_MainTex", _texOffset);
        
    }
}
