using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{

    [SerializeField]
    protected LineRenderer m_LineRenderer;
    [SerializeField]
    protected Camera m_Camera;
    protected List<Vector3> m_Points;

    public virtual LineRenderer lineRenderer
    {
        get
        {
            return m_LineRenderer;
        }
    }

    public virtual new Camera camera
    {
        get
        {
            return m_Camera;
        }
    }

    public virtual List<Vector3> points
    {
        get
        {
            return m_Points;
        }
    }

    protected virtual void Awake()
    {
        if (m_LineRenderer == null)
        {
            CreateDefaultLineRenderer();
        }
        if (m_Camera == null)
        {
            CreateDefaultCamera();
        }
        m_Points = new List<Vector3>();
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount == 2)
        {
            Reset();
        }

        if (Input.GetMouseButton(0) || Input.touchCount == 1)
        {
#if !UNITY_EDITOR && (PLATFORM_ANDROID || PLATFORM_IOS)
            Touch touch = Input.GetTouch(0);
            Vector3 mousePosition = m_Camera.ScreenToWorldPoint(touch.position);
#else
            Vector3 mousePosition = m_Camera.ScreenToWorldPoint(Input.mousePosition);
#endif
            RaycastHit _hit;
            Ray _ray = Appmain.appui.mainCamera3D.ScreenPointToRay(Input.mousePosition);

            //Debug.DrawRay(_ray, Vector3.forward);
			if(Physics.Raycast(_ray, out _hit)) {

                //mousePosition.z = m_LineRenderer.transform.position.z;
                if (!m_Points.Contains(_hit.point))
                {
                    m_Points.Add(_hit.point);
                    m_LineRenderer.positionCount = m_Points.Count;
                    m_LineRenderer.SetPosition(m_LineRenderer.positionCount - 1, mousePosition);
                }

            }
            
        }
    }

    protected virtual void Reset()
    {
        if (m_LineRenderer != null)
        {
            m_LineRenderer.positionCount = 0;
        }
        if (m_Points != null)
        {
            m_Points.Clear();
        }
    }

    protected virtual void CreateDefaultLineRenderer()
    {
        m_LineRenderer = gameObject.AddComponent<LineRenderer>();
        m_LineRenderer.positionCount = 0;
        m_LineRenderer.material = new Material(Shader.Find("Mobile/Particles/Additive"));
        m_LineRenderer.startColor = Color.white;
        m_LineRenderer.endColor = Color.white;
        m_LineRenderer.startWidth = 0.3f;
        m_LineRenderer.endWidth = 0.3f;
        m_LineRenderer.useWorldSpace = false;
    }

    protected virtual void CreateDefaultCamera()
    {
        m_Camera = Camera.main;
        if (m_Camera == null)
        {
            m_Camera = gameObject.AddComponent<Camera>();
        }
        m_Camera.orthographic = true;
    }

}