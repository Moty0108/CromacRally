using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track3VCamController : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera[] m_vCams;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < m_vCams.Length; i++)
        {
            m_vCams[i].m_LookAt = GamaManager.Instance.m_player;
        }
        m_vCams[1].m_Follow = GamaManager.Instance.m_player;

        CameraManager.Instance.TurnOffVirtualCamera();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAni()
    {

    }
}
