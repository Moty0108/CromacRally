using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TrackManager : Singleton<TrackManager>
{
    public List<GameObject> m_points = new List<GameObject>();
    public GameObject m_CheckPointObject;
    public GameObject m_respawnPointObject;
    public GameObject m_startPointObject;

    public float m_remainDistance;

    public int m_curPointNum = 0;
    public int m_curLap = 0;
    public int m_maxLap = 0;


    private void Start()
    {

    }

    private void Update()
    {
        if (m_points.Count > 0)
        {
            Vector3 a = Vector3.zero, b = Vector3.zero;

            a = m_points[m_curPointNum].transform.position;
            b = m_points[(m_curPointNum + 1) % m_points.Count].transform.position;

            Vector3 temp;
            temp = Vector3.Project(GamaManager.Instance.m_player.transform.position - a, (b - a).normalized) + a;

            if(!GamaManager.Instance.m_debugMode && Vector3.Distance(temp, GamaManager.Instance.m_player.transform.position) > m_points[m_curPointNum].GetComponent<CheckPointClass>().m_respawnDistance)
            {
                //GamaManager.Instance.Respawn(m_points[m_curPointNum].transform.position, m_points[(m_curPointNum + 1) % m_points.Count].transform, m_points[m_curPointNum].GetComponent<CheckPointClass>().m_subtractTimeAmount);
                if (GamaManager.Instance.m_isPlaying)
                {
                    FadeInOut.Instance.StartFadeIO(Resapwn);
                }
            }

            m_remainDistance = Vector3.Distance(temp, m_points[(m_curPointNum + 1) % m_points.Count].transform.position);

            int i = (m_curPointNum + 1) % m_points.Count;
            while (true)
            {
                if (m_points[i].GetComponent<CheckPointClass>().m_pointType == CheckPointClass.CheckPointType.CHECKPOINT)
                {
                    break;
                }
                if (m_points[i].GetComponent<CheckPointClass>().m_pointType == CheckPointClass.CheckPointType.STARTPOINT)
                {
                    break;
                }
                m_remainDistance += Vector3.Distance(m_points[i].transform.position, m_points[(i + 1) % m_points.Count].transform.position);

                i = (i + 1) % m_points.Count;
            }
        }
    }

    public void Resapwn()
    {
        GameTimerManager.Instance.AddCurrentTime(5);
        StartCoroutine(GamaManager.Instance.Respawn(m_points[m_curPointNum].transform.position, m_points[(m_curPointNum + 1) % m_points.Count].transform, m_points[m_curPointNum].GetComponent<CheckPointClass>().m_subtractTimeAmount));
    }

    public void Init()
    {
        m_curPointNum = 0;
        m_curLap = 0;
    }

    public void AddLap()
    {
        m_curLap++;
        
        if(Lab.Instance)
        Lab.Instance.FadeOutLabText("LAB " + m_curLap + "/" + m_maxLap);
        
        if (m_curLap == m_maxLap)
        {
            if(SoundManager.Instance.Array[4] != null)
                SoundManager.Instance.PlayOneShot(4);
            GamaManager.Instance.EndGame(true);
        }
    }

    [ContextMenu("Sort")]
    public void SortList()
    {
        m_points.Clear();

        GameObject[] temp = GetComponentsInChildren<GameObject>();
        
        foreach(GameObject a in temp)
        {
            m_points.Add(a);
        }
    }

    
    public void CreateTestPoint()
    {
        GameObject temp = new GameObject();
        temp.transform.SetParent(transform);
        m_points.Add(temp);
    }

    [ContextMenu("CreateStartPoint")]
    public void CreateStartPoint()
    {
        GameObject temp = Instantiate(m_startPointObject);
        temp.name = "StartPoint " + m_points.Count.ToString();
        //GameObject temp = new GameObject("StartPoint" + m_points.Count.ToString());
        temp.GetComponent<Transform>().SetParent(transform);
        temp.transform.localPosition = Vector3.zero;
        CheckPointClass cp = temp.AddComponent<CheckPointClass>();
        cp.m_number = 0;
        cp.m_pointType = CheckPointClass.CheckPointType.STARTPOINT;
        if(m_points.Count > 0)
            temp.transform.position = m_points[m_points.Count - 1].transform.position;
        m_points.Add(temp);
    }

    [ContextMenu("CreateCheckPoint")]
    public void CreateCheckPoint()
    {
        GameObject temp = Instantiate(m_CheckPointObject);
        temp.name = "CheckPoint " + m_points.Count.ToString();
        //GameObject temp = new GameObject("CheckPoint " + m_points.Count.ToString());
        temp.GetComponent<Transform>().SetParent(transform);
        temp.transform.localPosition = Vector3.zero;
        CheckPointClass cp = temp.AddComponent<CheckPointClass>();
        cp.m_number = m_points.Count;
        cp.m_pointType = CheckPointClass.CheckPointType.CHECKPOINT;

        if (m_points.Count > 0)
            temp.transform.position = m_points[m_points.Count - 1].transform.position;

        m_points.Add(temp);
    }

    [ContextMenu("CreateRespawnPoint")]
    public void CreateRespawnPoint()
    {
        GameObject temp = Instantiate(m_respawnPointObject);
        temp.name = "RspawnPoint " + m_points.Count.ToString();
        //GameObject temp = new GameObject("RspawnPoint " + m_points.Count.ToString());
        temp.GetComponent<Transform>().SetParent(transform);
        temp.transform.localPosition = Vector3.zero;
        CheckPointClass cp = temp.AddComponent<CheckPointClass>();
        cp.m_number = m_points.Count;
        cp.m_pointType = CheckPointClass.CheckPointType.RESPAWNPOINT;

        if (m_points.Count > 0)
            temp.transform.position = m_points[m_points.Count - 1].transform.position;

        m_points.Add(temp);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        if (m_points.Count > 0)
        {
            for (int i = 0; i < m_points.Count - 1; i++)
            {
                Gizmos.DrawLine(m_points[i].transform.position, m_points[i + 1].transform.position);
            }
            Gizmos.DrawLine(m_points[m_points.Count - 1].transform.position, m_points[0].transform.position);
        }
    }
}
