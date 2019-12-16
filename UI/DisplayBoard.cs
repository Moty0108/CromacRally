using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisplayBoard : MonoBehaviour
{
    public GameObject m_times;
    public Counter m_counter;
    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartEffect()
    {
        m_times.SetActive(false);
        m_counter.StartAfterEffect(Init, 3);
    }

    


    public void Init()
    {
        m_times.SetActive(true);
    }
}
