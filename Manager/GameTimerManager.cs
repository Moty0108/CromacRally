using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimerManager : Singleton<GameTimerManager>
{
    public UIAddTime m_uiAddTime;
    public float m_maxTime;
    float m_remainTime;
    public int m_minTime, m_secTime, m_msecTime;
    public float m_addTimeAmount;
    public float m_subTimeAmount;
    bool m_isTimer = false;

    public float m_curTime = 0;
    public int m_curMinTime, m_curSecTime, m_curMSecTime;

    float m_bestTime = 0;
    float m_curBestTime = 0;
    public int m_BestMinTime = 0, m_BestSecTime = 0, m_BestMSecTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        m_remainTime = m_maxTime;   
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isTimer)
        {
            m_remainTime -= Time.deltaTime;
            m_curTime += Time.deltaTime;
            m_curBestTime += Time.deltaTime;
        }

        m_minTime = (int)(m_remainTime / 60);
        m_secTime = (int)(m_remainTime % 60);
        m_msecTime = (int)((m_remainTime - Mathf.Floor(m_remainTime)) * 100);

        m_curMinTime = (int)(m_curTime / 60);
        m_curSecTime = (int)(m_curTime % 60);
        m_curMSecTime = (int)((m_curTime - Mathf.Floor(m_curTime)) * 100);

        if(m_isTimer && m_remainTime <= 0)
        {
            StopTimer();
            m_remainTime = 0;
            GamaManager.Instance.EndGame(false);
        }

    }

    public void CheckBsetTime()
    {
        if(m_bestTime == 0 || m_bestTime > m_curBestTime)
        {
            m_bestTime = m_curBestTime;

            m_BestMinTime = (int)(m_bestTime / 60);
            m_BestSecTime = (int)(m_bestTime % 60);
            m_BestMSecTime = (int)((m_bestTime - Mathf.Floor(m_bestTime)) * 100);
        }
        m_curBestTime = 0;
    }

    [ContextMenu("Start")]
    public void StartTimer()
    {
        m_isTimer = true;
    }

    [ContextMenu("Stop")]
    public void StopTimer()
    {
        m_isTimer = false;
    }

    [ContextMenu("Reset")]
    public void ResetTimer()
    {
        m_remainTime = m_maxTime;
        m_curTime = 0;
        m_curBestTime = 0;
    }

    public void AddTime(float time)
    {
        m_remainTime += time;
        m_uiAddTime.StartPlusEffect((int)time);
    }

    [ContextMenu("Add")]
    public void AddTime()
    {
        m_remainTime += m_addTimeAmount;
        m_uiAddTime.StartPlusEffect((int)m_addTimeAmount);
    }

    public void AddCurrentTime(float time)
    {
        m_curTime += time;
    }

    public void Subtract(float time)
    {
        m_remainTime -= time;
        if(m_uiAddTime.gameObject.activeInHierarchy)
            m_uiAddTime.StartMinusEffect((int)time);
    }

    [ContextMenu("Sub")]
    public void Subtract()
    {
        m_remainTime -= m_subTimeAmount;
    }

    
}
