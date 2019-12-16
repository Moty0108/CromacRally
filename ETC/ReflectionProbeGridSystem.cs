using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionProbeGridSystem : MonoBehaviour
{
    public int m_x;
    public int m_y;
    public int m_xLength;
    public int m_yLength;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Set")]
    public void SetProbe()
    {
        int x = 0, y = 0;

        ReflectionProbe[] probe = GetComponentsInChildren<ReflectionProbe>();

        //for(int i = 0; i < m_x; i++)
        //{
        //    for(int j = 0; j < m_y; j++)
        //    {
        //        probe[(i * m_y) + j].transform.position = new Vector3(i * m_xLength, 0, j * m_yLength);
        //    }
        //}

        for(int i = 0; i<probe.Length;i++)
        {
            probe[i].transform.position = new Vector3(x * m_xLength, 0, y * m_yLength);
            y++;
            if(y == m_y)
            {
                x++;
                y = 0;
            }
        }
    }
}
