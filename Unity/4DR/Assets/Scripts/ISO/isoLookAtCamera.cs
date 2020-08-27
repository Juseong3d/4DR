using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isoLookAtCamera : MonoBehaviour
{

    public Camera _camera;
    public Renderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        if(Appmain.appui != null) 
            _camera = Appmain.appui.mainCamera3D;

        //_renderer = GetComponentInChildren<Renderer>();
        _renderer.material.renderQueue -= 10;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_camera != null)
            this.transform.LookAt(this.transform.position + _camera.transform.parent.rotation * Vector3.forward, _camera.transform.parent.rotation * Vector3.up);        
    }
}
