using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallScene : AnimationEvent
{
    Rigidbody[] rd;
    public AudioClip m_slowSound;
    public AudioClip m_impactSound;
    public GameObject m_ui;
    AudioSource m_audioSource;
    AudioSource[] m_playerAudioSource;

    public void Awake()
    {
        m_animations.Add(new AnimationInfo(0f, One));
        m_animations.Add(new AnimationInfo(0.2f, Two));
        m_animations.Add(new AnimationInfo(0.5f, Three));
        m_animations.Add(new AnimationInfo(0.8f, Four));
        m_animations.Add(new AnimationInfo(0.025f, SlowSound));
        m_animations.Add(new AnimationInfo(0.938f, Impact));
        m_animations.Add(new AnimationInfo(0.96f, OnInput));
        rd = GetComponentsInChildren<Rigidbody>();

        m_audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        m_playerAudioSource = GamaManager.Instance.m_player.GetComponentsInChildren<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.transform.root.CompareTag("Player"))
        {
            m_ui.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
            GameTimerManager.Instance.StopTimer();
            //Debug.Log("충돌 | " + gameObject.name);
            EventSceneManager.Instance.StartEventScene(this, "03Track");
            for (int i = 0; i < rd.Length; i++)
            {
                rd[i].isKinematic = false;
            }
        }
    }

    private void SlowSound()
    {
        
        if (m_slowSound)
        {
            //Debug.Log("슬로우 사운드");
            m_audioSource.clip = m_slowSound;
            m_audioSource.PlayOneShot(m_slowSound);
        }
    }

    private void Impact()
    {
        if(m_impactSound)
            m_audioSource.PlayOneShot(m_impactSound);
        GForce.Instance.ImpactEffect();
        
    }

    private void OnInput()
    {
        //GamaManager.Instance.m_player.GetComponent<Animator>().updateMode = AnimatorUpdateMode.AnimatePhysics;
        StopAllCoroutines();
        //m_smallDustParticle.Stop();
        Rigidbody playerRigid = GamaManager.Instance.m_player.GetComponent<Rigidbody>();
        playerRigid.AddForce(GamaManager.Instance.m_player.forward * 30f, ForceMode.VelocityChange);
        EventSceneManager.Instance.OffAnimation();
    }

    private void One()
    {
        //m_smallDustParticle.Play();
        //StartCoroutine(SmallDust());
        foreach(AudioSource source in m_playerAudioSource)
        {
            source.enabled = false;
        }
        GamaManager.Instance.m_player.GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
        CameraManager.Instance.TurnOnVirtualCamera(1);
    }

    private void Two()
    {
        CameraManager.Instance.TurnOffVirtualCamera();
        CameraManager.Instance.TurnOnVirtualCamera(2);
        
    }

    private void Three()
    {
        CameraManager.Instance.TurnOffVirtualCamera();
        CameraManager.Instance.TurnOnVirtualCamera(3);
    }

    private void Four()
    {
        foreach (AudioSource source in m_playerAudioSource)
        {
            source.enabled = true;
        }
        CameraManager.Instance.TurnOffVirtualCamera();
        GetComponent<BoxCollider>().enabled = true;
        GameTimerManager.Instance.StartTimer();
        m_ui.SetActive(true);
    }

    bool flag = true;

}
