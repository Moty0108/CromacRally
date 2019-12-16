using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : Singleton<CameraManager>
{
    public CinemachineVirtualCamera[] m_vCams;

    private void Awake()
    {
        if(m_vCams == null)
            m_vCams = GameObject.FindObjectsOfType<Cinemachine.CinemachineVirtualCamera>();
        TurnOffVirtualCamera();

        foreach (CinemachineVirtualCamera vcams in m_vCams)
        {
            vcams.m_LookAt = GamaManager.Instance.m_player;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOffVirtualCamera()
    {
        foreach(CinemachineVirtualCamera vcams in m_vCams)
        {
            vcams.gameObject.SetActive(false);
        }
    }

    public void TurnOnVirtualCamera(int number)
    {
        TurnOffVirtualCamera();
        m_vCams[number].gameObject.SetActive(true);
    }
}
