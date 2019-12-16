using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform m_camPos;
    private Vector3 offset;
    public GameObject m_cam;
    public Transform m_camTarget;
    Rigidbody rb;
    RVP.VehicleParent vp;

    public float m_maxDistance = 0.1f;
    public float m_Delay = 0;
    public float m_ReverseDelay = 0;

    public float m_attendantsAmount;
    float m_prevPosition = 0;
    float m_prevPositionX = 0;
    float m_curPosition;
    float m_curPositionX;
    float m_offsetZ;
    float m_offsetX;
    float m_offsetY;

    [Header("카메라 흔들림")]
    public AnimationCurve m_shakeRange;
    public AnimationCurve m_shakeSpeed;
    public float m_minSpeed;
    public float m_maxSpeed;
    float m_shakeY = 0;
    float m_randomShakeY = 0;
    float m_shakeX = 0;
    float m_randomShakeX = 0;
    bool flag = false;
    bool flag2 = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        vp = GetComponent<RVP.VehicleParent>();
        m_offsetZ = m_camPos.localPosition.z;
        m_offsetX = m_camPos.localPosition.x;
        m_offsetY = m_camPos.localPosition.y;
        m_prevPosition = m_offsetZ;
        m_cam = Camera.main.gameObject;
    }

    private void FixedUpdate()
    {
        if (vp.localVelocity.z > 0)
        {
            m_curPosition = Mathf.Lerp(m_prevPosition, -((rb.velocity.magnitude * 3.6f) * m_maxDistance / 140f), Time.deltaTime * m_Delay);
        }
        else
        {
            m_curPosition = Mathf.Lerp(m_curPosition, m_offsetZ, Time.deltaTime * m_ReverseDelay);
        }

        
        m_curPositionX = Mathf.Lerp(m_prevPositionX, -(vp.localVelocity.x * m_maxDistance / m_attendantsAmount), Time.deltaTime * m_Delay);

        if (rb.velocity.magnitude * 3.6f > m_minSpeed && rb.velocity.magnitude * 3.6f < m_maxSpeed || rb.velocity.magnitude * 3.6f > m_maxSpeed)
        {
            float range = m_shakeRange.Evaluate(((rb.velocity.magnitude * 3.6f) - m_minSpeed) / m_maxSpeed);
            float speed = m_shakeSpeed.Evaluate(((rb.velocity.magnitude * 3.6f) - m_minSpeed) / m_maxSpeed);
            if (rb.velocity.magnitude * 3.6f > m_maxSpeed)
            {
                range = m_shakeRange.Evaluate(1);
            }

            if (flag)
            {
                m_shakeY += Time.deltaTime * speed;
                if (m_randomShakeY < m_shakeY)
                {
                    m_randomShakeY = Random.Range(-range, 0f);
                    flag = false;
                }
            }
            else
            {
                m_shakeY -= Time.deltaTime * speed;
                if (m_randomShakeY > m_shakeY)
                {
                    m_randomShakeY = Random.Range(0f, range);
                    flag = true;
                }
            }

            if (flag2)
            {
                m_shakeX += Time.deltaTime * speed;
                if (m_randomShakeX < m_shakeX)
                {
                    m_randomShakeX = Random.Range(range, 0f);
                    flag2 = false;
                }
            }
            else
            {
                m_shakeX -= Time.deltaTime * speed;
                if (m_randomShakeX > m_shakeX)
                {
                    m_randomShakeX = Random.Range(0f, range);
                    flag2 = true;
                }
            }
        }
        else
        {
            m_shakeX = Mathf.Lerp(m_shakeX, 0, Time.deltaTime);
            m_shakeY = Mathf.Lerp(m_shakeY, 0, Time.deltaTime);
        }


        m_camPos.transform.localPosition = new Vector3(m_prevPositionX + m_offsetX + m_shakeX, m_offsetY + m_shakeY, m_curPosition + m_offsetZ);
        m_prevPosition = m_curPosition;
        m_prevPositionX = m_curPositionX;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        //offset = transform.position - m_camPos.position;
        ////m_cam.transform.position = transform.position - offset;

        m_cam.transform.position = m_camPos.transform.position;
        m_cam.transform.rotation = m_camPos.transform.rotation;
        
    //    m_cam.transform.LookAt(m_camTarget);
    }

    
}
