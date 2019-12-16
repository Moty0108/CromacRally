using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class DurabilityUI : MonoBehaviour
{
    public DurabilitySystem m_durability;
    public GameObject m_meter;
    public GameObject m_pointer;
    public float m_maxAngle;
    public float m_minAngle;
    public float m_angleLength;
    float m_curAngle;
    float m_curDurability;
    float m_maxDurability;

    // Start is called before the first frame update
    void Start()
    {
        m_curDurability = m_durability.m_curDurability;
        m_maxDurability = m_durability.m_maxDurability;
    }

    // Update is called once per frame
    void Update()
    {
        m_curDurability = m_durability.m_curDurability;
        m_curAngle = -(m_minAngle + m_curDurability * (m_minAngle - m_maxAngle) / m_maxDurability);
        m_meter.GetComponent<Image>().fillAmount = m_curDurability / m_maxDurability;
        m_pointer.GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, m_curAngle));
    }

    private void OnDrawGizmos()
    {
        Vector3 a, b;
        a = new Vector3(m_pointer.GetComponent<RectTransform>().position.x + Mathf.Cos(m_minAngle * Mathf.Deg2Rad) * m_angleLength, m_pointer.GetComponent<RectTransform>().position.y + Mathf.Sin(m_minAngle * Mathf.Deg2Rad) * m_angleLength, 0);
        b = new Vector3(m_pointer.GetComponent<RectTransform>().position.x + Mathf.Cos(m_maxAngle * Mathf.Deg2Rad) * m_angleLength, m_pointer.GetComponent<RectTransform>().position.y + Mathf.Sin(m_maxAngle * Mathf.Deg2Rad) * m_angleLength, 0);
        Gizmos.DrawLine(m_pointer.GetComponent<RectTransform>().position, a);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(m_pointer.GetComponent<RectTransform>().position, b);
    }
}
