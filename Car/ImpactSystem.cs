using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSystem : MonoBehaviour
{
    public List<HandleSection> m_handleSection = new List<HandleSection>();
    
    FFBInfo m_curSection;
    Rigidbody rd;
    float m_curSpeed;

    public AudioSource m_audioSource;
    public AudioClip[] m_audioClip;

    int m_curClip = 0;
    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody>();
        m_audioSource = GetComponent<AudioSource>();   
    }

    private void FixedUpdate()
    {
        m_curSpeed = rd.velocity.magnitude * 3.6f;

        for (int i = 0; i < m_handleSection.Count; i++)
        {
            if (m_handleSection[i].m_min < m_curSpeed && m_curSpeed < m_handleSection[i].m_max)
            {
                m_curSection = m_handleSection[i].m_handleInfo;
                m_curClip = i;
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
            if (!collision.transform.CompareTag("Terrain"))
            {
                if (m_curClip < m_audioClip.Length && m_audioClip[m_curClip] != null)
                {//m_audioSource.PlayOneShot(m_audioClip[m_curClip]);

                    //Debug.Log(m_audioClip[m_curClip]);
                    SoundManager.Instance.Array[1].audioclip = m_audioClip[m_curClip];
                    SoundManager.Instance.PlayOneShot(1);
                }

            if(GamaManager.Instance.m_player.GetComponent<Rigidbody>().velocity.magnitude * 3.6f > 60f)
                GForce.Instance.ImpactEffect();
                //Debug.Log("충돌!");
                if (m_curSection)
                {
                    //Debug.Log("실행할 핸들 진동 : " + m_curSection.name);
                    m_curSection.StartEffect();
                }
                else
                {
                    //Debug.Log("핸들 정보 없음!");
                }
            }   
    }
}
