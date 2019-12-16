using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrackPoint
{
    public bool m_isCheckPoint;
    public float m_respawnDistance;
    public Vector3 m_position;
    public List<Vector3> horPoints;


    public TrackPoint(bool check, float distance, Vector3 position)
    {
        m_isCheckPoint = check;
        m_respawnDistance = distance;
        m_position = position;
        horPoints = new List<Vector3>();
    }
}
