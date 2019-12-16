using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : AnimationEvent
{
    public void Awake()
    {
        m_animations.Add(new AnimationInfo(0.9f, One));
    }

    private void Start()
    {
        EventSceneManager.Instance.StartEventScene(this);
        CameraManager.Instance.TurnOnVirtualCamera(0);
    }


    public void One()
    {
        FadeInOut.Instance.StartFadeIO(StartGameFade);
    }

    public void StartGameFade()
    {
        CameraManager.Instance.TurnOffVirtualCamera();
        GamaManager.Instance.StartCoroutine(GamaManager.Instance.Counter(GamaManager.Instance.m_startTime, GamaManager.Instance.StartGame));
    }
}
