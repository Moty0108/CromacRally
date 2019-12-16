using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MappingWheelColider : MonoBehaviour
{
    public WheelCollider m_wheelCollider;

    private Vector3 m_center;
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_center = m_wheelCollider.transform.TransformPoint(m_wheelCollider.center);

        if (Physics.Raycast(m_center, -m_wheelCollider.transform.up, out hit, m_wheelCollider.suspensionDistance + m_wheelCollider.radius))
        {
            transform.position = hit.point + m_wheelCollider.transform.up * m_wheelCollider.radius;
        }
        else
        {
            transform.position = m_center - m_wheelCollider.transform.up * m_wheelCollider.suspensionDistance;
        }
    }
}
