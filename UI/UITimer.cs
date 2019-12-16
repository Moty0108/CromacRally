using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{
    public enum TimerType
    {
        REMAIN, CURRENT, BEST
    }

    public TimerType m_timerType = TimerType.REMAIN;
    public Text m_minText, m_secText, m_mSecText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_timerType)
        {
            case TimerType.REMAIN:
                m_minText.text = GameTimerManager.Instance.m_minTime.ToString("00");
                m_secText.text = GameTimerManager.Instance.m_secTime.ToString("00");
                m_mSecText.text = GameTimerManager.Instance.m_msecTime.ToString("00");
                break;

            case TimerType.CURRENT:
                m_minText.text = GameTimerManager.Instance.m_curMinTime.ToString("00");
                m_secText.text = GameTimerManager.Instance.m_curSecTime.ToString("00");
                m_mSecText.text = GameTimerManager.Instance.m_curMSecTime.ToString("00");
                break;

            case TimerType.BEST:
                m_minText.text = GameTimerManager.Instance.m_BestMinTime.ToString("00");
                m_secText.text = GameTimerManager.Instance.m_BestSecTime.ToString("00");
                m_mSecText.text = GameTimerManager.Instance.m_BestMSecTime.ToString("00");
                break;
        }
    }
}
