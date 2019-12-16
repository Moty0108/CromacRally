using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class AnimationInfo
{
    public bool m_isPlay = false;
    public float m_frame;
    public Action m_function;

    public AnimationInfo(float frame, Action function)
    {
        m_frame = frame;
        m_function = function;
    }
}

public class EventSceneManager : Singleton<EventSceneManager>
{
    Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GamaManager.Instance.m_player.GetComponent<Animator>();
        //m_animator.enabled = false;
    }

    public void StartEventScene(AnimationEvent eventScene)
    {
        if (!m_animator)
        {
            m_animator = GamaManager.Instance.m_player.GetComponent<Animator>();
        }
        m_animator.enabled = true;
        m_animator.SetTrigger(eventScene.m_triggerName);

        //Debug.Log(eventScene.m_animations.Count);
        StartCoroutine(EventScene(eventScene.m_animations));
    }

    public void StartEventScene(AnimationEvent eventScene, string name)
    {
        if (!m_animator)
        {
            m_animator = GamaManager.Instance.m_player.GetComponent<Animator>();
        }
        m_animator.enabled = true;
        m_animator.SetTrigger(eventScene.m_triggerName);

        
        StartCoroutine(EventScene(eventScene.m_animations, name));
    }

    public void OffAnimation()
    {
        m_animator.enabled = false;
    }

    IEnumerator EventScene(List<AnimationInfo> info)
    {
        
        while(true)
        {
            for (int i = 0; i < info.Count;i++)
            {
                if(!info[i].m_isPlay)
                {
                    if(m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > info[i].m_frame)
                    {
                        info[i].m_isPlay = true;
                        info[i].m_function();
                        info.RemoveAt(i);
                    }
                }
            }
            if(info.Count <= 0 && m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
            {
                //Debug.Log("애니메이션 끝!");
                OffAnimation();
                break;
            }

            yield return null;
        }
    }

    IEnumerator EventScene(List<AnimationInfo> info, string name)
    {

        while (true)
        {
            if ( m_animator.GetCurrentAnimatorStateInfo(0).IsName(name))
            {
                for (int i = 0; i < info.Count; i++)
                {
                    if (!info[i].m_isPlay)
                    {
                        if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > info[i].m_frame)
                        {
                            info[i].m_isPlay = true;
                            info[i].m_function();
                            info.RemoveAt(i);
                        }
                    }
                }
                if (info.Count <= 0 && m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f && m_animator.GetCurrentAnimatorStateInfo(0).IsName(name))
                {
                    //Debug.Log("애니메이션 끝!");
                    OffAnimation();
                    break;
                }
            }

            yield return null;
        }
    }
}
