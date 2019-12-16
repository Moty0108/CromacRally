using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedOMeterUI : MonoBehaviour
{
    [Header("UI")]
    public Rigidbody m_car;
    public GameObject m_pointer;
    public float m_maxAngle;
    public float m_minAngle;
    float m_curAngle;
    float m_prevAngle = 0;
    float m_curSpeed;
    float m_maxSpeed;

    [Header("오브젝트")]
    public GameObject m_objectPointer;
    public float m_objectMaxAngle;
    public float m_objectMinAngle;
    float m_objectAngle;
    float m_objecetPrevAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_curSpeed = m_car.velocity.magnitude * 3.6f;
        m_maxSpeed = 220;
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        m_curSpeed = m_car.velocity.magnitude * 3.6f;
        m_curAngle = Mathf.Lerp(m_prevAngle, (m_minAngle + m_curSpeed * (m_maxAngle - m_minAngle) / 220), Time.deltaTime * 5f);
        m_prevAngle = m_curAngle;
        m_pointer.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, m_curAngle));

        m_objectAngle = Mathf.Lerp(m_objecetPrevAngle, (m_objectMinAngle + m_curSpeed * (m_objectMaxAngle - m_objectMinAngle) / 220), Time.deltaTime * 5f);
        m_objecetPrevAngle = m_objectAngle;
        m_objectPointer.transform.localEulerAngles = new Vector3(m_objectPointer.transform.localEulerAngles.x, m_objectPointer.transform.localEulerAngles.y, m_objectAngle);
    }
}
