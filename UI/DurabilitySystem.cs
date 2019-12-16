using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DurabilitySystem : MonoBehaviour
{
    public DurabilityGage m_durabilityGageUI;

    public float m_maxDurability = 100f;
    public float m_curDurability = 100f;
    public float m_delay = 1f;
    public float m_multiply = 1;

    GameObject m_prevCollider;
    Rigidbody m_rd;
    float m_speed;
    bool m_isCoolTime;
    // Start is called before the first frame update
    void Start()
    {
        m_isCoolTime = false;
        m_durabilityGageUI = DurabilityGage.Instance;
        m_curDurability = m_maxDurability;
        m_rd = GetComponent<Rigidbody>();
        Physics.IgnoreLayerCollision(0, 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        m_speed = m_rd.velocity.magnitude * 3.6f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag != "Terrain" && collision.transform.tag != "NonCrashObject")
        {
            if (!m_isCoolTime)
            {
                m_isCoolTime = true;
                StopAllCoroutines();

                if(collision.transform.CompareTag("PhysicsObject"))
                {
                    collision.transform.tag = "NonCrashObject";
                    Destroy(collision.gameObject, 10f);
                }
                
                m_curDurability -= m_speed * m_speed / 4000 * m_multiply;
                if(m_curDurability <= 0)
                {
                    GamaManager.Instance.EndGame(false);
                }
                StartCoroutine(CollisionTimer());
                m_durabilityGageUI.StartEffect();
            }
        }
    }

    IEnumerator CollisionTimer()
    {
        
        yield return new WaitForSeconds(m_delay);

        m_isCoolTime = false;
    }
}
