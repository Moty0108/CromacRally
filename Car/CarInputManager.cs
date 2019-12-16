using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using enums;

public class CarInputManager : MonoBehaviour
{
    RVP.VehicleParent vp;
    float ver, hor, clutch;
    
    public GEAR m_joystickInput;
    public ContollerMode m_contollerMode;

    // Start is called before the first frame update
    void Start()
    {
        vp = GetComponent<RVP.VehicleParent>();
        m_joystickInput = GEAR.STOP;
    }

    // Update is called once per frame
    void Update()
    {
        switch(m_contollerMode)
        {
            case ContollerMode.JOYSTICK:
                ver = Input.GetAxis("JoystickVertical");
                hor = Input.GetAxis("JoystickHorizontal");
                //ver = Input.GetAxis("Vertical");
                //hor = Input.GetAxis("Horizontal");
                clutch = Input.GetAxis("JoystickClutch");
                vp.SetSteer(hor);

                vp.SetEbrake(0);
                vp.SetBrake(0);
                vp.SetAccel(0);

                if (m_joystickInput == GEAR.FORWARD)
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        vp.SetEbrake(1);
                    }
                    else
                    {
                        vp.SetEbrake(0);
                    }

                    // 브레이크 페달 (Axis : 1 ~ 0)
                    if (ver > 0)
                    {
                        if (vp.localVelocity.z > 5)
                        {
                            vp.SetBrake(ver);
                        }
                        else
                        {
                            vp.SetBrake(0);
                            vp.SetEbrake(1);
                        }
                        
                    }

                    // 악셀 페달(Axis : 0 ~ -1)
                    else if (ver < 0)
                    {
                        
                        vp.SetAccel(-ver);
                    }
                    else
                    {
                        vp.SetBrake(0);
                        vp.SetAccel(0);
                    }
                }

                if (m_joystickInput == GEAR.BACK)
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        vp.SetEbrake(1);
                    }
                    else
                    {
                        vp.SetEbrake(0);
                    }

                    // 브레이크 페달 (Axis : 1 ~ 0)
                    if (ver > 0)
                    {
                        if(vp.localVelocity.z < -5)
                            vp.SetAccel(ver);
                        else
                        {
                            vp.SetAccel(0);
                            vp.SetEbrake(1);
                        }
                    }

                    // 악셀 페달(Axis : 0 ~ -1)
                    else if (ver < 0)
                    {
                        vp.SetBrake(-ver);
                    }
                    else
                    {
                        vp.SetBrake(0);
                        vp.SetAccel(0);
                    }
                }

                // 알크래프트 기어 버튼 인풋
                // 버튼 8, 10, 12 전진기어
                // 버튼 9, 11, 13 후진기어
                if (Input.GetKey(KeyCode.JoystickButton8) || Input.GetKey(KeyCode.JoystickButton10) || Input.GetKey(KeyCode.JoystickButton12) || Input.GetKey(KeyCode.Keypad1) || Input.GetKey(KeyCode.Joystick1Button4))
                {
                    m_joystickInput = GEAR.FORWARD;
                }
                else if (Input.GetKey(KeyCode.JoystickButton9) || Input.GetKey(KeyCode.JoystickButton11) || Input.GetKey(KeyCode.JoystickButton13) || Input.GetKey(KeyCode.Keypad2) || Input.GetKey(KeyCode.Joystick1Button5))
                {
                    m_joystickInput = GEAR.BACK;
                }
                break;
        }

        
        

       
        
    }

    

}
