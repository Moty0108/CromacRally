using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedOMeter : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int m_count = 0;
    float m_time = 0;

    

    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player" && m_count == 0)
        {
            m_count++;
            StartCoroutine(timer());
            //Debug.Log("SpeedoMeter In");
        }
        
        else if(other.tag == "Player" && m_count == 1)
        {
            StopAllCoroutines();
            //Debug.Log("Speed : " + m_time.ToString() + "s");
            //Debug.Log("SpeedoMeter Out");
            m_time = 0;
            m_count = 0;
        }
    }

    IEnumerator timer()
    {
        while(true)
        {
            m_time += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
