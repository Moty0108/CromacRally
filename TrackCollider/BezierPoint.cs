using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using enums;

[System.Serializable]
public class BezierPoint
{
    public Vector3 m_startPos, m_startBezier, m_endBezier, m_endPos;

    public BezierPoint(Vector3 sp, Vector3 sb, Vector3 eb, Vector3 ep)
    {
        m_startPos = sp;
        m_startBezier = sb;
        m_endBezier = eb;
        m_endPos = ep;
    }
     
    public Field m_field;

    public Vector3[] uppos = new Vector3[11];
    public Vector3[] downpos = new Vector3[11];
    public Vector3[] midpos = new Vector3[11];
    public Vector3[] polyPos = new Vector3[22];
}
