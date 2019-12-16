using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CheckPointManager : MonoBehaviour
{
    public GameObject m_pointPrefab;
    public List<CheckPointClass> m_checkPoints;
    public int m_curPointNum = 0;
    public int m_curLap = 0;
    public int m_maxLap = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Init()
    {
        m_curPointNum = 0;
        m_curLap = 0;
    }

    public void AddLap()
    {
        m_curLap++;

        if(m_curLap == m_maxLap)
        {
            Init();
            GamaManager.Instance.StartGame();
        }
    }

    [ContextMenu("Add Point")]
    public void AddPoint()
    {
        GameObject pointObj = Instantiate(m_pointPrefab);
        pointObj.name = "Point " + (m_checkPoints.Count + 1);
        pointObj.transform.SetParent(transform);
        pointObj.tag = "check_point";
        CheckPointClass cp = pointObj.AddComponent<CheckPointClass>();
        //cp.Init(this, m_checkPoints.Count);

        m_checkPoints.Add(cp);
    }

    public void DeletePoint(int num)
    {
        m_checkPoints.RemoveAt(num);
    }

    
}
