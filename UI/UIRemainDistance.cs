using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRemainDistance : MonoBehaviour
{
    public Text m_remainDisText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_remainDisText.text = Mathf.Floor(TrackManager.Instance.m_remainDistance).ToString() + "M";
    }
}
