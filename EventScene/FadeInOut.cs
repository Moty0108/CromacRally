using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeInOut : Singleton<FadeInOut>
{
    public delegate void Function();

    public Image m_image;
    public float m_time = 0.5f;
    public AnimationCurve m_curve;

    Coroutine cor;

    // Start is called before the first frame update
    void Start()
    {
        m_image = GetComponent<Image>();
        m_image.canvasRenderer.SetAlpha(0);
        m_image.color = new Color(0, 0, 0, 1);
    }

    private void OnEnable()
    {
        m_image.canvasRenderer.SetAlpha(0);
        m_image.color = new Color(0, 0, 0, 1);
    }


    void Update()
    {
    }

    [ContextMenu("Set")]
    public void set()
    {
        StartFadeIO();
    }

    public void StartFadeIn()
    {
        if(cor == null)
            cor = StartCoroutine(FadeIn());
    }

    public void StartFadeIn(float time)
    {
        if (cor == null)
            cor = StartCoroutine(FadeIn(time));
    }

    public void StartFadeIn(float time, Function func)
    {
        if (cor == null)
            cor = StartCoroutine(FadeIn(time, func));
        
    }

    public void StartFadeOut()
    {
        if (cor == null)
            cor = StartCoroutine(FadeOut());
    }

    public void StartFadeOut(float time)
    {
        if (cor == null)
            cor = StartCoroutine(FadeOut(time));
    }

    public void StartFadeOut(float time, Function func)
    {
        if (cor == null)
            cor = StartCoroutine(FadeOut(time, func));
    }

    public void StartFadeIO()
    {
        if (cor == null)
            cor = StartCoroutine(FadeIO());
    }

    public void StartFadeIO(Function func)
    {
        if (cor == null)
            cor = StartCoroutine(FadeIO(func));
    }


    IEnumerator FadeIO()
    {
        float time = 0;

        while(true)
        {
            time += Time.deltaTime;
            m_image.canvasRenderer.SetAlpha(m_curve.Evaluate(time));
            
            if(time >= m_curve.keys[m_curve.keys.Length-1].time)
            {
                m_image.canvasRenderer.SetAlpha(0);
                cor = null;
                break;
            }

            yield return null;
        }
    }

    IEnumerator FadeIO(Function func)
    {
        float time = 0;
        bool flag = true;

        while (true)
        {
            time += Time.deltaTime;
            m_image.canvasRenderer.SetAlpha(m_curve.Evaluate(time));

            if(m_image.canvasRenderer.GetAlpha() >= 1 && flag)
            {
                func();
                flag = false;
            }

            if (time >= m_curve.keys[m_curve.keys.Length - 1].time)
            {
                m_image.canvasRenderer.SetAlpha(0);
                cor = null;
                break;
            }

            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        while(true)
        {
            m_image.CrossFadeAlpha(1, m_time, false);
            if(m_image.canvasRenderer.GetAlpha() > 0.9f)
            {
                m_image.canvasRenderer.SetAlpha(1);
                cor = null;
                break;
            }
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        while(true)
        {
            m_image.CrossFadeAlpha(0, m_time, false);
            if (m_image.canvasRenderer.GetAlpha() < 0.1f)
            {
                m_image.canvasRenderer.SetAlpha(0);
                cor = null;
                break;
            }
            yield return null;
        }
    }

    IEnumerator FadeIn(float time)
    {
        while (true)
        {
            m_image.CrossFadeAlpha(1, time, false);
            if (m_image.canvasRenderer.GetAlpha() > 0.9f)
            {
                m_image.canvasRenderer.SetAlpha(1);
                cor = null;
                break;
            }
            yield return null;
        }
    }

    IEnumerator FadeOut(float time)
    {
        while (true)
        {
            m_image.CrossFadeAlpha(0, time, false);
            if (m_image.canvasRenderer.GetAlpha() < 0.1f)
            {
                m_image.canvasRenderer.SetAlpha(0);
                cor = null;
                break;
            }
            yield return null;
        }
    }

    IEnumerator FadeIn(float time, Function func)
    {
        while (true)
        {
            m_image.CrossFadeAlpha(1, time, false);
            if (m_image.canvasRenderer.GetAlpha() > 0.9f)
            {
                m_image.canvasRenderer.SetAlpha(1);
                func();
                cor = null;
                break;
            }
            yield return null;
        }
    }

    IEnumerator FadeOut(float time, Function func)
    {
        while (true)
        {
            m_image.CrossFadeAlpha(0, time, false);
            if (m_image.canvasRenderer.GetAlpha() < 0.1f)
            {
                m_image.canvasRenderer.SetAlpha(0);
                func();
                cor = null;
                break;
            }
            yield return null;
        }
    }
}
