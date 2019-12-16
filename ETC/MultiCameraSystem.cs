using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MultiCameraSystem : MonoBehaviour
{
    public Camera m_main;
    public Camera m_left;
    public Camera m_right;
    public bool m_multi = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("SetCamera")]
    void CameraSetting()
    {
        if (!m_multi)
        {
            m_left.gameObject.SetActive(true);
            m_right.gameObject.SetActive(true);
            m_main.rect = new Rect(0.33333f, 0, 0.33333f, 1);
            m_left.rect = new Rect(0, 0, 0.33333f, 1);
            m_right.rect = new Rect(0.66666f, 0, 0.33333f, 1);
            m_multi = !m_multi;
        }
        else
        {
            m_left.gameObject.SetActive(false);
            m_right.gameObject.SetActive(false);
            m_main.rect = new Rect(0, 0, 1, 1);
            m_multi = !m_multi;
        }
        
        
    }
}
