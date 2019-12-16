using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("의자진동"))]
public class ChairViveInfo : ScriptableObject
{
    [SerializeField]
    public float m_spped;

    [SerializeField]
    public float m_maxX;

    public void StartEffect()
    {
        GForce.Instance.SetEffectPower(m_spped, m_maxX);
    }

    public static void StopEffect()
    {
        GForce.Instance.SetEffectZero();
    }
}
