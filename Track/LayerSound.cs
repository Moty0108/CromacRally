using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSound : MonoBehaviour
{
    Rigidbody m_rd;
    AudioSource m_audioSource;
    AudioClip m_audioClip;
    AudioClip m_driftAudioClip;
    RVP.VehicleParent m_vp;

    private void Awake()
    {
        m_rd = GetComponentInParent<Rigidbody>();
        m_audioSource = GetComponent<AudioSource>();
        m_vp = GetComponentInParent<RVP.VehicleParent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource.clip = m_audioClip;
        m_audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_audioSource.isPlaying && m_audioSource.enabled)
        {
            m_audioSource.Play();
        }
    }

    private void FixedUpdate()
    {
        //if (m_vp.localVelocity.x > 5 || m_vp.localVelocity.x < -5)
        //    m_audioSource.clip = m_driftAudioClip;
        //else
        //    m_audioSource.clip = m_audioClip;

        m_audioSource.volume = (m_rd.velocity.magnitude * 3.6f) / 160;
    }

    public void SetAudioClip(AudioClip clip, AudioClip driftClip)
    {
        m_audioClip = clip;
        m_audioSource.clip = m_audioClip;
        m_driftAudioClip = driftClip;
    }
}
