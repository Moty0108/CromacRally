using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName =("스티어링진동"))]
public class FFBInfo : ScriptableObject
{
    [SerializeField]
    public float m_forceSpeed;
    [SerializeField] 
    public float m_forceMaxSteering;

    public void StartEffect()
    {
        ForceFeedbackTest.Instance.SetForce(m_forceSpeed, m_forceMaxSteering);
    }

    public static void StopEffect()
    {
        ForceFeedbackTest.Instance.SetSteeringCenter();
    }
}
