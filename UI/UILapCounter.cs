using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILapCounter : MonoBehaviour
{
    public Text m_curLapText, m_maxLapText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_curLapText.text = TrackManager.Instance.m_curLap.ToString();
        m_maxLapText.text = TrackManager.Instance.m_maxLap.ToString();
    }
}
