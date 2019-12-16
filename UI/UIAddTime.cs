using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAddTime : MonoBehaviour
{
    public Text m_addTimeText;
    public float m_fadeSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartPlusEffect(int time)
    {
        StopAllCoroutines();
        StartCoroutine(Effect(time));
    }

    public void StartMinusEffect(int time)
    {
        StopAllCoroutines();
        StartCoroutine(MinusEffect(time));
    }

    IEnumerator Effect(int time)
    {
        m_addTimeText.gameObject.SetActive(true);

        m_addTimeText.text = "+" + time.ToString("00") + ":00";

        Color temp = Color.green;
        float alpha = 1;
        

        while(true)
        {
            alpha -= Time.deltaTime * m_fadeSpeed;
            
            m_addTimeText.color = new Color(temp.r, temp.g, temp.b, alpha);
            if (alpha <= 0)
            {
                m_addTimeText.gameObject.SetActive(false);
                break;
            }


            yield return null;
        }
    }

    IEnumerator MinusEffect(int time)
    {
        m_addTimeText.gameObject.SetActive(true);

        m_addTimeText.text = "-" + time.ToString("00") + ":00";

        Color temp = Color.red;
        float alpha = 1;


        while (true)
        {
            alpha -= Time.deltaTime * m_fadeSpeed;

            m_addTimeText.color = new Color(temp.r, temp.g, temp.b, alpha);
            if (alpha <= 0)
            {
                m_addTimeText.gameObject.SetActive(false);
                break;
            }


            yield return null;
        }
    }
}
