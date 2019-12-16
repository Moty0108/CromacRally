using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RVP;

public class DisplaySpeed : MonoBehaviour
{
    public Text m_speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_speed.text = (GetComponent<Rigidbody>().velocity.magnitude * 3.6f).ToString();
    }
}
