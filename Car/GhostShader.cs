using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostShader : MonoBehaviour
{
    Transform m_target;
    public Renderer[] m_render;

    // Start is called before the first frame update
    void Start()
    {
        m_render = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, Camera.main.transform.position);

        foreach (Renderer renderer in m_render)
        {
            foreach (Material render in renderer.materials)
                render.SetFloat("_CurrentDistance", dist);
        }
    }
}
