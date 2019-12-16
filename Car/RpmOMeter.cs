using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RpmOMeter : MonoBehaviour
{
    [Header("UI")]
    public RVP.Motor m_engine;
    public GameObject m_pointer;
    public float m_maxAngle;
    public float m_minAngle;
    float m_curAngle;
    float m_prevAngle = 0;
    float m_curPitch;
    float m_maxPitch;

    [Header("오브젝트")]
    public GameObject m_objectPointer;
    public float m_objectMaxAngle;
    public float m_objectMinAngle;
    float m_objectAngle;
    float m_objecetPrevAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_curPitch = m_engine.targetPitch;
        m_maxPitch = 220;
    }

    // Update is called once per frame
    void Update()
    {
        m_curPitch = m_engine.targetPitch;
        m_curAngle = Mathf.Lerp(m_prevAngle, (m_minAngle + m_curPitch * (m_maxAngle - m_minAngle) / m_engine.maxPitch), Time.deltaTime * 10);
        m_prevAngle = m_curAngle;
        m_pointer.GetComponent<RectTransform>().localRotation   = Quaternion.Euler(new Vector3(0, 0, m_curAngle));


        m_objectAngle = Mathf.Lerp(m_objecetPrevAngle, (m_objectMinAngle + m_curPitch * (m_objectMaxAngle - m_objectMinAngle) / m_engine.maxPitch), Time.deltaTime * 10);
        m_objecetPrevAngle = m_objectAngle;
        m_objectPointer.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0, 0, m_objectAngle));
    }
}
