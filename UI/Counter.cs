using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    public delegate void CounterFunction();

    public Text m_countText;
    public int m_curTime;
    public AudioSource m_audioSource;
    public AudioClip[] m_audioClips;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartEffect(CounterFunction cf, int time)
    {
        StartCoroutine(CounterCoroutine(cf, time));
    }

    public void StartAfterEffect(CounterFunction cf, int time)
    {
        StartCoroutine(CounterAfterCoroutine(cf, time));
    }

    IEnumerator CounterCoroutine(CounterFunction cf, int time)
    {
        m_countText.gameObject.SetActive(true);
        m_curTime = time;
        m_countText.text = m_curTime.ToString();
        if (m_audioSource != null)
        {
            m_audioSource.PlayOneShot(m_audioClips[time]);
        }
        yield return new WaitForSecondsRealtime(1);
        for (int i = 0; i<time;i++)
        {
            m_curTime--;
            m_countText.text = m_curTime.ToString();

            if (m_curTime == 0)
            {
                if (m_audioSource != null)
                {
                        m_audioSource.PlayOneShot(m_audioClips[0]);
                }

                m_countText.text = "START";
                cf();
            }
            else
            {
                if (m_audioSource != null)
                {
                    if (m_audioClips[time - i - 1] != null)
                        m_audioSource.PlayOneShot(m_audioClips[time - i - 1]);
                }
            }
            yield return new WaitForSecondsRealtime(1);
        }

        m_countText.gameObject.SetActive(false);

    }

    IEnumerator CounterAfterCoroutine(CounterFunction cf, int time)
    {
        m_countText.gameObject.SetActive(true);
        m_curTime = time;
        m_countText.text = m_curTime.ToString();
        if (m_audioSource != null)
        {
            m_audioSource.PlayOneShot(m_audioClips[time]);
        }
        yield return new WaitForSecondsRealtime(1);
        for (int i = 0; i < time; i++)
        {
            m_curTime--;
            m_countText.text = m_curTime.ToString();

            if (m_curTime == 0)
            {
                if (m_audioSource != null)
                {
                    m_audioSource.PlayOneShot(m_audioClips[0]);
                }

                m_countText.text = "START";
            }
            else
            {
                if (m_audioSource != null)
                {
                    if (m_audioClips[time - i - 1] != null)
                        m_audioSource.PlayOneShot(m_audioClips[time - i - 1]);
                }
            }
            yield return new WaitForSecondsRealtime(1);
        }

        m_countText.gameObject.SetActive(false);
        cf();
    }
}
