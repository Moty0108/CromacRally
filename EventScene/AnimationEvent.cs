using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationEvent : MonoBehaviour
{
    public string m_triggerName;

    public List<AnimationInfo> m_animations = new List<AnimationInfo>();

}
