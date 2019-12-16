using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DurabilityGage : Singleton<DurabilityGage>
{
    public DurabilitySystem m_durability;
    public Image m_handle;
    public Image m_durabilityImage;
    public Image m_delayDurabilityImage;
    public Image m_shakedImage;
    public float m_minHandleOffsetX;
    public float m_maxHandleOffsetX;

    float m_curAngle;
    float m_curDurability;
    float m_maxDurability;


    float m_prevDurability;

    public float m_damgeRedzoneTime;
    public float m_damgeRedzoneSpeed;
    public float m_shakeTime;
    public float m_shakePower;
    public float m_colorEffecTime;
    public float m_colorEffectSpeed;
    float m_curShakeTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_durability = GamaManager.Instance.m_player.GetComponent<DurabilitySystem>();

        m_curDurability = m_durability.m_curDurability;
        m_maxDurability = m_durability.m_maxDurability;
        m_prevDurability = m_curDurability;
    }

    // Update is called once per frame
    void Update()
    {


        m_curDurability = m_durability.m_curDurability;
        m_durabilityImage.fillAmount = m_curDurability / m_maxDurability;
        m_delayDurabilityImage.fillAmount = m_prevDurability / m_maxDurability;
        m_handle.rectTransform.localPosition = new Vector3(m_minHandleOffsetX + m_curDurability * (m_maxHandleOffsetX - m_minHandleOffsetX) / m_durability.m_maxDurability, 0, 0);

        if(m_curShakeTime > 0)
        {
            m_shakedImage.rectTransform.localPosition = new Vector3(Random.Range(-m_shakePower, m_shakePower), Random.Range(-m_shakePower, m_shakePower), 0);
            m_curShakeTime -= Time.deltaTime;
        }
        else
        {
            m_shakedImage.rectTransform.localPosition = Vector3.zero;
            m_curShakeTime = 0;
        }

    }

    [ContextMenu("Shake")]
    public void Shake()
    {
        m_curShakeTime = m_shakeTime;
    }

    [ContextMenu("Shake and Color Effect")]
    public void StartEffect()
    {
        StopAllCoroutines();
        Shake();
        StartCoroutine(ColorEffect());
        StartCoroutine(DamageEffect());
    }

    IEnumerator DamageEffect()
    {
        yield return new WaitForSeconds(m_damgeRedzoneTime);


        while(true)
        {
            m_prevDurability -= Time.deltaTime * m_damgeRedzoneSpeed;
            if(m_prevDurability <= m_curDurability)
            {
                break;
            }

            yield return null;
        }

        m_prevDurability = m_curDurability;
    }


    IEnumerator ColorEffect()
    {
        m_shakedImage.color = new Color(255, 0, 0, 255);
        yield return new WaitForSeconds(m_colorEffecTime);
        float col = 0;

        while (true)
        {
            col += Time.deltaTime * m_colorEffectSpeed;
            
            if (col > 0)
            {
                m_shakedImage.color = new Color(255, 255, 255, 255);
            }

            m_shakedImage.color = new Color(255, col, col, 255);

            yield return null;
        }
    }
}
