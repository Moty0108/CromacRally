using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Create")]
[SerializeField]
public class CarStatusData : ScriptableObject
{
    [Header("내구도")]
    [Space(10)]
    public float m_maxDurability;
    public float m_durabilityAmount;

    [Header("스티어링")]
    [Space(10)]
    public float m_steerRate;
    public AnimationCurve m_steerCurve;

    [Header("기어")]
    public RVP.Gear[] m_gear;

    [Header("무게중심")]
    public Vector3 m_Com;
    
}

[System.Serializable]
public class CarData
{
    public GameObject m_car;
    public CarStatusData m_status;

    public void SetCarStatus()
    {
        DurabilitySystem dur = m_car.GetComponent<DurabilitySystem>();
        dur.m_maxDurability = m_status.m_maxDurability;
        dur.m_multiply = m_status.m_durabilityAmount;

        RVP.SteeringControl steer = m_car.GetComponentInChildren<RVP.SteeringControl>();
        steer.steerRate = m_status.m_steerRate;
        steer.steerCurve = m_status.m_steerCurve;

        RVP.GearboxTransmission trans = m_car.GetComponentInChildren<RVP.GearboxTransmission>();
        //trans.gears = new RVP.Gear[m_status.m_gear.Length];
        trans.gears = m_status.m_gear;
        trans.CalculateRpmRanges();



        RVP.VehicleParent vp = m_car.GetComponent<RVP.VehicleParent>();
        vp.centerOfMassOffset = m_status.m_Com;


    }
}