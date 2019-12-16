using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CheckPointClass : MonoBehaviour
{
    public enum CheckPointType
    {
        STARTPOINT, CHECKPOINT, RESPAWNPOINT
    }

    public CheckPointType m_pointType = CheckPointType.CHECKPOINT;
    
    
    public int m_number;
    public int m_AddTimeAmount;
    public float m_subtractTimeAmount;
    public float m_respawnDistance;
    public int m_curNumberOfTime;
    public float[] m_AddTimeArray;

    // Start is called before the first frame update
    void Awake()
    {
        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = Vector3.down;
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            transform.position = hit.point;
        }

        m_curNumberOfTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(int number)
    {
        m_number = number;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.tag);
        if(!other.CompareTag("Player"))
        {
           if((TrackManager.Instance.m_curPointNum + 1) % TrackManager.Instance.m_points.Count == m_number)
            {
                switch (m_pointType)
                {
                    case CheckPointType.CHECKPOINT:
                        switch(GamaManager.Instance.m_mode)
                        {
                            case TimeMode.GrandPrix:
                                break;

                            case TimeMode.Time:
                                if (m_AddTimeArray.Length > 0)
                                    GameTimerManager.Instance.AddTime(m_AddTimeArray[Mathf.Clamp(m_curNumberOfTime, 0, m_AddTimeArray.Length - 1)]);
                                else
                                    GameTimerManager.Instance.AddTime(m_AddTimeAmount);
                                break;
                        }
                        
                        m_curNumberOfTime++;
                        //Debug.Log("체크포인트! : " + m_number);
                        TrackManager.Instance.m_curPointNum = m_number;
                        break;

                    case CheckPointType.STARTPOINT:
                        GameTimerManager.Instance.CheckBsetTime();
                        //Debug.Log("랩 추가!");
                        TrackManager.Instance.AddLap();
                        TrackManager.Instance.m_curPointNum = m_number;
                        break;

                    case CheckPointType.RESPAWNPOINT:
                        //Debug.Log("리스폰 포인트 지남!");
                        TrackManager.Instance.m_curPointNum = m_number;
                        break;
                }
                
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 temp = (TrackManager.Instance.m_points[(m_number + 1) % TrackManager.Instance.m_points.Count].transform.position - transform.position).normalized;
        Vector3 a = new Vector3(-temp.z, 0, temp.x) * m_respawnDistance + transform.position;
        Vector3 b = new Vector3(-temp.z, 0, temp.x) * -m_respawnDistance + transform.position;

        temp = transform.position - TrackManager.Instance.m_points[(m_number + 1) % TrackManager.Instance.m_points.Count].transform.position;

        Vector3 c = new Vector3(-temp.z, 0, temp.x).normalized * -m_respawnDistance + TrackManager.Instance.m_points[(m_number + 1) % TrackManager.Instance.m_points.Count].transform.position;
        Vector3 d = new Vector3(-temp.z, 0, temp.x).normalized * m_respawnDistance + TrackManager.Instance.m_points[(m_number + 1) % TrackManager.Instance.m_points.Count].transform.position;

        Gizmos.DrawLine(a, b);
        Gizmos.DrawLine(b, d);
        Gizmos.DrawLine(d, c);
        Gizmos.DrawLine(c, a);
    }
}
