using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void StartGame()
    {
        //CameraManager.Instance.TurnOffVirtualCamera();
        //GetComponent<Animator>().enabled = false;
        //GamaManager.Instance.StartGame();
        FadeInOut.Instance.StartFadeIO(StartGameFade);
    }

    public void StartGameFade()
    {
        CameraManager.Instance.TurnOffVirtualCamera();
        GetComponent<Animator>().enabled = false;
        //GamaManager.Instance.StartGame();
        GamaManager.Instance.StartCoroutine(GamaManager.Instance.Counter(GamaManager.Instance.m_startTime, GamaManager.Instance.StartGame));
    }
}
