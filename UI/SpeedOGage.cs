using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RVP;

public class SpeedOGage : MonoBehaviour
{
    public Image m_gage;
    public Rigidbody m_player;
    public Text m_speedText;
    GasMotor m_gasMotor;

    float m_curSpeed;

    DriveForce nextOutput;
    Transmission nextTrans;
    GearboxTransmission nextGearbox;
    ContinuousTransmission nextConTrans;
    Suspension nextSus;
    bool reachedEnd = false;
    float topSpeed;

   

    // Start is called before the first frame update
    void Start()
    {
        //if(m_gasMotor == null)
        //{
        //    m_gasMotor = GameObject.Find("Drift Car").GetComponentInChildren<GasMotor>();
        //}

        m_player = GamaManager.Instance.m_player.GetComponent<Rigidbody>();
        m_gasMotor = m_player.GetComponentInChildren<GasMotor>();

        GetTopSpeed();
        topSpeed = topSpeed * 3.6f;
    }

    private void FixedUpdate()
    {
        m_curSpeed = m_player.velocity.magnitude * 3.6f;
        m_speedText.text = ((int)m_curSpeed).ToString();
        //m_speedText.text = m_curSpeed.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        m_gage.fillAmount = m_curSpeed / topSpeed;

    }

    void GetTopSpeed()
    {
        if (m_gasMotor.outputDrives != null)
        {
            if (m_gasMotor.outputDrives.Length > 0)
            {
                topSpeed = m_gasMotor.torqueCurve.keys[m_gasMotor.torqueCurve.length - 1].time * 1000;
                nextOutput = m_gasMotor.outputDrives[0];

                while (!reachedEnd)
                {
                    if (nextOutput)
                    {
                        if (nextOutput.GetComponent<Transmission>())
                        {
                            nextTrans = nextOutput.GetComponent<Transmission>();

                            if (nextTrans is GearboxTransmission)
                            {
                                nextGearbox = (GearboxTransmission)nextTrans;
                                topSpeed /= nextGearbox.gears[nextGearbox.gears.Length - 1].ratio;
                            }
                            else if (nextTrans is ContinuousTransmission)
                            {
                                nextConTrans = (ContinuousTransmission)nextTrans;
                                topSpeed /= nextConTrans.maxRatio;
                            }

                            if (nextTrans.outputDrives.Length > 0)
                            {
                                nextOutput = nextTrans.outputDrives[0];
                            }
                            else
                            {
                                topSpeed = -1;
                                reachedEnd = true;
                            }
                        }
                        else if (nextOutput.GetComponent<Suspension>())
                        {
                            nextSus = nextOutput.GetComponent<Suspension>();

                            if (nextSus.wheel)
                            {
                                topSpeed /= Mathf.PI * 100;
                                topSpeed *= nextSus.wheel.tireRadius * 2 * Mathf.PI;
                            }
                            else
                            {
                                topSpeed = -1;
                            }

                            reachedEnd = true;
                        }
                        else
                        {
                            topSpeed = -1;
                            reachedEnd = true;
                        }
                    }
                    else
                    {
                        topSpeed = -1;
                        reachedEnd = true;
                    }
                }
            }
            else
            {
                topSpeed = -1;
            }
        }
        else
        {
            topSpeed = -1;
        }
    }
}
