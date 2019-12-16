using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleCarController : MonoBehaviour
{
    public enum STATE
    {
        FORWARD = 1, STOP = 0, BACK = -1
    }
    public STATE m_state = STATE.STOP;
    public float m_currentSpeed = 0;
    public float m_maxSpeed = 160f;
    public float m_backSpeed = 60f;
    public float m_maxHandleAngle = 45f;
    public float m_gravity = 9.8f;
    public float m_acceleration;
    public float m_deceleration;
    private bool m_isGround = true;

    public Transform m_center;
    public Text m_uiSpeed;
    

    Ray ray;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Move());
        //GetComponent<Rigidbody>().centerOfMass = m_center.localPosition ;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        InputGear();

        m_uiSpeed.text = "Gear : " + m_currentSpeed.ToString();

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = Vector3.up * 10f;
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }


        GetComponent<Rigidbody>().MovePosition(transform.position - transform.forward * m_currentSpeed * Time.deltaTime);
        //GetComponent<Rigidbody>().AddForce(-transform.forward * m_currentSpeed * 10, ForceMode.Impulse);
        
        if(!m_isGround)
        {
            //GetComponent<Rigidbody>().MovePosition(transform.position + Vector3.down * m_gravity * Time.deltaTime);
        }

        
        Turn();

        ray = new Ray();
        ray.origin = m_center.position;
        ray.direction = -transform.up;

        if(Physics.Raycast(ray, out hit, 5f))
        {
            m_isGround = true;
        }
        else
        {
            m_isGround = false;
            //GetComponent<Rigidbody>().MovePosition(transform.position + Vector3.down * m_gravity* Time.deltaTime);
            //GetComponent<Rigidbody>().AddForce(Vector3.down * m_gravity, ForceMode.Impulse);
        }
    }

    public void InputGear()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            m_state = STATE.FORWARD;
        }

        else if (Input.GetKey(KeyCode.DownArrow))
        {
            m_state = STATE.BACK;
        }

        else
        {
            m_state = STATE.STOP;
        }
    }

    public void Turn()
    {
        float h = 0;
        h = Input.GetAxis("Horizontal") * m_maxHandleAngle;

        if (m_currentSpeed > 0)
            GetComponent<Rigidbody>().MoveRotation(transform.rotation * Quaternion.Euler(new Vector3(1, h * Time.deltaTime, 1)));
        else if (m_currentSpeed < 0)
            GetComponent<Rigidbody>().MoveRotation(transform.rotation * Quaternion.Euler(new Vector3(1, -h * Time.deltaTime, 1)));
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray);
    }

    IEnumerator Move()
    {
        while (true)
        {
            

            switch (m_state)
            {
                case STATE.FORWARD:
                    if (m_currentSpeed <= 60)
                    {
                        m_currentSpeed += 2f;
                    }

                    else if (m_currentSpeed > 60 && m_currentSpeed <= 100)
                    {
                        m_currentSpeed += 1.4f;
                    }

                    else if (m_currentSpeed > 100 && m_currentSpeed <= m_maxSpeed)
                    {
                        m_currentSpeed += 1f;
                    }
                    else
                    {

                    }
                    break;

                case STATE.STOP:
                    if (m_currentSpeed > 5 && m_currentSpeed <= 60)
                    {
                        m_currentSpeed -= 4.5f;
                    }
                    else if (m_currentSpeed > 60 && m_currentSpeed < 100)
                    {
                        m_currentSpeed -= 3.6f;
                    }
                    else if (m_currentSpeed > 100)
                    {
                        m_currentSpeed -= 3f;
                    }
                    else if (m_currentSpeed < -5)
                    {
                        m_currentSpeed += 4.5f;
                    }
                    else
                    {
                        m_currentSpeed = 0;
                    }
                    break;

                case STATE.BACK:
                    if (m_currentSpeed > -m_backSpeed)
                    {
                        m_currentSpeed -= 2f;
                    }
                    else if (m_currentSpeed < -m_backSpeed)
                    {
                        m_currentSpeed = -m_backSpeed;
                    }
                    break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
