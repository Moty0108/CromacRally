using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using enums;

[System.Serializable]
public class SpeedArray
{
    public float m_motorTorque;
    public float m_speed;
}


public class CarController : MonoBehaviour
{

    GEAR m_state;

    [Header("Time Scale")]
    [Tooltip("게임 전체의 시간")]
    public float m_timeScale;


    [Header("Gear System")]
    [Tooltip("기어 시스템의 사용 유무")]
    public bool m_gearSystem = false;
    [Tooltip("기어 시스템 사용 시 첫 번째 기어")]
    [Range(0, 11)]
    public int m_FirstGear;
    [Tooltip("기어 시스템 사용 시 두 번째 기어")]
    [Range(0, 11)]
    public int m_SecondGear;
    [Tooltip("기어 시스템 사용 시 세 번째 기어")]
    [Range(0, 11)]
    public int m_ThirdGear;
    [Tooltip("기어 시스템 사용 시 네 번째 기어")]
    [Range(0, 11)]
    public int m_FourGear;
    [Tooltip("기어 시스템 사용 시 다섯 번째 기어")]
    [Range(0, 11)]
    public int m_FiveGear;

    [Header("Torque")]
    [Tooltip("전진할 때 모터의 토크\n기어 시스템 사용 시 사용 X\n기어 시스템 미사용 시 사용 O")]
    public float m_motorTorque;
    [Tooltip("후진할 때 모터의 토크")]
    public float m_backMotorTorque;

    [Header("ETC")]
    [Tooltip("브레이크의 강도")]
    public float m_brakePower = 10000f;
    [Tooltip("핸들링 각도")]
    public float m_maxSteeringAngle;

    [HideInInspector]   // 사용안함
    public float m_maxSpeed = 160f;
    [HideInInspector]   // 사용안함
    public float m_backMaxSpeed = 60f;


    private float m_displaySpeed;
    private float m_currentSpeed = 0;
    private float m_acceleration;
    private float m_deceleration;
    private float m_brakeTorque;
    private float m_verticalAxis;
    private Transform m_getField;
    private bool m_handBrake = false;

    
    public List<AxleInfo> m_axleInfos;
    [HideInInspector]
    public Transform m_centerOfMass;
    [HideInInspector]
    public List<Transform> m_tfTires;
    [HideInInspector]
    public Text m_uiSpeed;

    public SpeedArray[] m_speedArray;


    [HideInInspector]public Rigidbody rd;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = m_timeScale;
        rd = GetComponent<Rigidbody>();
        rd.centerOfMass = m_centerOfMass.localPosition;
        m_getField = GameObject.Find("GetField").transform;
    }

    private void FixedUpdate()
    {
        GetField();
        TransformTire();
        //GearSystem();

        // 수정전
        //Move(m_motorTorque * m_verticalAxis, m_brakeTorque);

        // 수정후
        Move2(m_motorTorque * m_verticalAxis, m_brakeTorque);
    }

    void GetField()
    {
        
        m_getField.transform.position = new Vector3(transform.position.x, GameObject.Find("FieldCollider").GetComponent<FieldScript>().pos[0].m_startPos.y, transform.position.z);
    }


    // Update is called once per frame
    void Update()
    {
        DisplaySpeed();

        if (Input.GetKey(KeyCode.Q))
        {
            //진동 테스트
        }
    }

    // 타이어의 각도 및 회전
    void TransformTire()
    {
        m_tfTires[2].Rotate(m_axleInfos[1].leftWheel.rpm * Time.deltaTime, 0f, 0f);
        m_tfTires[3].Rotate(m_axleInfos[1].rightWheel.rpm * Time.deltaTime, 0f, 0f);

        m_tfTires[0].localEulerAngles = new Vector3(m_tfTires[0].localEulerAngles.x, m_axleInfos[0].rightWheel.steerAngle - m_tfTires[0].localEulerAngles.z, m_tfTires[0].localScale.z);
        m_tfTires[1].localEulerAngles = new Vector3(m_tfTires[1].localEulerAngles.x, m_axleInfos[0].leftWheel.steerAngle - m_tfTires[1].localEulerAngles.z, m_tfTires[1].localScale.z);
    }

    // 휠 콜라이더의 각도(핸들링) 변경
    // public void Steering(float _axis)
    // _axis : 사용자로부터 입력받는 Axis 인풋값
    public void Steering(float _axis)
    {
        float steering = m_maxSteeringAngle * _axis;

        foreach (AxleInfo axleInfo in m_axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
        }

    }
    
    // 차의 위치 초기화
    // public void ResetCar()
    public void ResetCar()
    {
        transform.position = Vector3.up * 10f;
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void HandBrake(bool flag)
    {
        if(flag)
        {
            m_handBrake = flag;
        }
        else
        {
            m_handBrake = flag;
        }
    }

    // 브레이크 - 수정
    // public void Brake()
    // 차의 magnitude값이 5보다 크면 속도가 서서히 감소
    // 리지드바디의 drag값을 조정하여 속도가 서서히 줄게 함
    // 차의 magnitude값이 5보다 작으면 완전히 멈춤
    public void Brake(bool flag)
    {
        if (flag)
            m_brakeTorque = m_brakePower;
        else
            m_brakeTorque = 0;
    }

    // 브레이크 초기화
    // public void NonBrake()
    // 차의 브레이크 값 0으로 초기화
    // 리지드바디의 drag값 초기값으로 지정
    public void NonBrake()
    {
        m_brakeTorque = 0;
    }

    // 전진
    // public void Accelerate(float _axis)
    // _axis : 사용자로 부터 입력받는 Axis 인풋값
    public void Accelerate(float _axis)
    {
        m_verticalAxis = _axis;   
    }

    // 후진
    // public void Accelerate(float _axis)
    // _axis : 사용자로 부터 입력받는 Axis 인풋값
    public void Decelerate(float _axis)
    {
        m_verticalAxis = -_axis;
    }

    public void SIdeSlip(bool flag)
    {

        if (flag)
        {
        rd.AddForce(-transform.right * Time.deltaTime * 5 * m_axleInfos[0].leftWheel.steerAngle, ForceMode.Acceleration);
            WheelFrictionCurve temp = new WheelFrictionCurve();
            temp.asymptoteSlip = m_axleInfos[1].leftWheel.sidewaysFriction.asymptoteSlip;
            temp.asymptoteValue = m_axleInfos[1].leftWheel.sidewaysFriction.asymptoteValue;
            temp.extremumSlip = m_axleInfos[1].leftWheel.sidewaysFriction.extremumSlip;
            temp.extremumValue = m_axleInfos[1].leftWheel.sidewaysFriction.extremumValue;
            temp.stiffness = 0.5f;

            m_axleInfos[1].leftWheel.sidewaysFriction = temp;
            m_axleInfos[1].rightWheel.sidewaysFriction = temp;
            
        }
        else
        {
            WheelFrictionCurve temp = new WheelFrictionCurve();
            temp.asymptoteSlip = m_axleInfos[1].leftWheel.sidewaysFriction.asymptoteSlip;
            temp.asymptoteValue = m_axleInfos[1].leftWheel.sidewaysFriction.asymptoteValue;
            temp.extremumSlip = m_axleInfos[1].leftWheel.sidewaysFriction.extremumSlip;
            temp.extremumValue = m_axleInfos[1].leftWheel.sidewaysFriction.extremumValue;
            temp.stiffness = 1f;

            m_axleInfos[1].leftWheel.sidewaysFriction = temp;
            m_axleInfos[1].rightWheel.sidewaysFriction = temp;
            
        }
    }

    // 차의 이동
    // private Move(float _motor, float _brake)
    // _motor 휠콜라이더의 모터토크 값
    // _brake 휠콜라이더의 브레이크토크 값
    void Move(float _motor, float _brake)
    {
        foreach (AxleInfo axleInfo in m_axleInfos)
        {
            axleInfo.leftWheel.motorTorque = _motor;
            axleInfo.rightWheel.motorTorque = _motor;
            axleInfo.leftWheel.brakeTorque = _brake;
            axleInfo.rightWheel.brakeTorque = _brake;
        }
    }

    // 차의 이동
    // private Move(float _motor, float _brake)
    // _motor 휠콜라이더의 모터토크 값
    // _brake 휠콜라이더의 브레이크토크 값
    void Move2(float _motor, float _brake)
    {
        if (!m_handBrake)
        {
            foreach (AxleInfo axleInfo in m_axleInfos)
            {
                if (axleInfo.motor)
                {
                    axleInfo.leftWheel.motorTorque = _motor;
                    axleInfo.rightWheel.motorTorque = _motor;
                    axleInfo.leftWheel.brakeTorque = _brake;
                    axleInfo.rightWheel.brakeTorque = _brake;
                }
            }
        }
        else
        {
            m_axleInfos[1].leftWheel.motorTorque = 0;
            m_axleInfos[1].rightWheel.motorTorque = 0;

            m_axleInfos[1].leftWheel.brakeTorque = 1000;
            m_axleInfos[1].rightWheel.brakeTorque = 1000;
        }
    }

    void Move(float _brake)
    {
        foreach (AxleInfo axleInfo in m_axleInfos)
        {
            axleInfo.leftWheel.brakeTorque = _brake;
            axleInfo.rightWheel.brakeTorque = _brake;
        }
    }

    // 기어
    // private void GearSystem()
    // 현재 모터토크에서 차의 속도가 최대 속도에 달하였을때
    // 모터토크의 크기를 키움
    void GearSystem()
    {
        if (m_gearSystem && m_state == GEAR.FORWARD)
        {
            if (rd.velocity.magnitude > 0 && rd.velocity.magnitude < m_speedArray[m_FirstGear].m_speed)
            {
                m_motorTorque = m_speedArray[m_FirstGear].m_motorTorque;
            }
            else if (rd.velocity.magnitude >= m_speedArray[m_FirstGear].m_speed && rd.velocity.magnitude < m_speedArray[m_SecondGear].m_speed)
            {
                m_motorTorque = m_speedArray[m_SecondGear].m_motorTorque;
            }
            else if (rd.velocity.magnitude >= m_speedArray[m_SecondGear].m_speed && rd.velocity.magnitude < m_speedArray[m_ThirdGear].m_speed)
            {
                m_motorTorque = m_speedArray[m_ThirdGear].m_motorTorque;
            }
            else if (rd.velocity.magnitude >= m_speedArray[m_ThirdGear].m_speed && rd.velocity.magnitude < m_speedArray[m_FourGear].m_speed)
            {
                m_motorTorque = m_speedArray[m_FourGear].m_motorTorque;
            }
            else if (rd.velocity.magnitude >= m_speedArray[m_FourGear].m_speed && rd.velocity.magnitude < m_speedArray[m_FirstGear].m_speed)
            {
                m_motorTorque = m_speedArray[m_FiveGear].m_motorTorque;
            }
        }

        if (m_state == GEAR.BACK)
        {
            m_motorTorque = m_backMotorTorque;
        }
    }

    // 속도 출력
    void DisplaySpeed()
    {
        m_displaySpeed = rd.velocity.magnitude;
        m_uiSpeed.text = (rd.velocity.magnitude * 3.8f).ToString();
        m_uiSpeed.text = string.Format("{0:N2}", rd.velocity.magnitude * 3.8f);
        // 실제 속도 계산법
        // magnitude * 2 * 3.6
    }

    // 차의 현재상태 변경
    public void StateChange(GEAR _gear)
    {
        if (_gear == GEAR.FORWARD)
        {
            m_state = GEAR.FORWARD;
        }

        else if (_gear == GEAR.BACK)
        {
            m_state = GEAR.BACK;
        }

        else if(_gear == GEAR.STOP)
        {
            m_state = GEAR.STOP;
        }
    }
    
}
