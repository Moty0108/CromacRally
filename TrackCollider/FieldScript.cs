using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using enums;

public class FieldScript : MonoBehaviour
{
    public List<BezierPoint> pos = new List<BezierPoint>();
    public float m_length = 0.5f;
 
    public Transform playerPos;
    public Rigidbody m_carRgd;
    public Field m_field;
    public List<FieldInfo> m_fieldinfos;
    ForceFeedbackTest m_forceFeedBack;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("GetField").GetComponent<Transform>();
        m_forceFeedBack = GameObject.Find("ForceFeedBack").GetComponent<ForceFeedbackTest>();
        m_carRgd = GameObject.Find("Drift Car").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetField();
        SetForceFeedback();
    }

    void SetForceFeedback()
    {
        
        for(int i=0;i<m_fieldinfos.Count; i++)
        {
            if (m_field == m_fieldinfos[i].m_field)
            {
                if(m_carRgd.velocity.magnitude *3.8f > 0 && m_carRgd.velocity.magnitude * 3.8f <= m_fieldinfos[i].m_fieldFirstValue)
                {
                    m_forceFeedBack.SetForce(m_fieldinfos[i].m_ffbinfo.m_forceSpeed, m_fieldinfos[i].m_ffbinfo.m_forceMaxSteering);

                }
                else if (m_carRgd.velocity.magnitude * 3.8f > m_fieldinfos[i].m_fieldFirstValue && m_carRgd.velocity.magnitude * 3.8f < m_fieldinfos[i].m_fieldSecondValue)
                {
                    m_forceFeedBack.SetForce(m_fieldinfos[i].m_ffbStronginfo.m_forceSpeed, m_fieldinfos[i].m_ffbStronginfo.m_forceMaxSteering);

                }

                return;
            }

            if (m_field == Field.DEFAULT)
                m_forceFeedBack.SetSteeringCenter();
        }
    }

    void GetField()
    {
        for (int i = 0; i < pos.Count; i++)
        {
            if (isInside(playerPos.position, pos[i].polyPos))
            {
                m_field = pos[i].m_field;
                //Debug.Log(pos[i].m_field);
                return;
            }
        }

        m_field = Field.DEFAULT;
    }

    private void OnDrawGizmosSelected()
    {

    }

    

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.cyan;
        for (int i = 0; i < pos.Count; i++)
        {
            Gizmos.DrawLine(pos[i].m_startPos, pos[i].m_startBezier);
            Gizmos.DrawLine(pos[i].m_endPos, pos[i].m_endBezier);
        }

        DrawBezier();

        
    }


    void DrawBezier()
    {
        Gizmos.color = Color.yellow;
        List<Vector3> temp;
        

        for (int j = 0; j < pos.Count; j++)
        {
            
            temp = GetPosition(pos[j], 10, out pos[j].uppos, out pos[j].midpos, out pos[j].downpos);
            

            for (int i = 0; i < temp.Count-1; i++)
            {
                Gizmos.DrawLine(temp[i], temp[i + 1]);
                Gizmos.DrawLine(temp[i], (pos[j].uppos[i] * m_length) + pos[j].midpos[i]);
                Gizmos.DrawLine(temp[i], (pos[j].downpos[i] * m_length) + pos[j].midpos[i]);
            }
            Gizmos.DrawLine(temp[temp.Count-1], (pos[j].uppos[pos[j].uppos.Length-1] * m_length) + pos[j].midpos[pos[j].midpos.Length-1]);
            Gizmos.DrawLine(temp[temp.Count-1], (pos[j].downpos[pos[j].downpos.Length-1] * m_length) + pos[j].midpos[pos[j].midpos.Length-1]);

            Vector3[] polypath = new Vector3[pos[j].uppos.Length + pos[j].downpos.Length];
            for (int i = 0; i < pos[j].uppos.Length; i++)
            {
                    polypath[i] = new Vector3(pos[j].uppos[i].x *m_length + pos[j].midpos[i].x, pos[j].midpos[i].y , pos[j].uppos[i].z * m_length + pos[j].midpos[i].z);
                    polypath[pos[j].uppos.Length + i] = new Vector3(pos[j].downpos[pos[j].downpos.Length - i - 1].x * m_length + pos[j].midpos[pos[j].downpos.Length - i - 1].x, pos[j].midpos[pos[j].midpos.Length - i - 1].y,  pos[j].downpos[pos[j].downpos.Length - i - 1].z * m_length + pos[j].midpos[pos[j].downpos.Length - i - 1].z);
                    
            }
            //m_poly.SetPath(j, polypath);
            pos[j].polyPos = polypath;

            Gizmos.color = Color.magenta;
            for(int i=0;i<polypath.Length-1;i++)
            {
                Gizmos.DrawLine(polypath[i], polypath[i + 1]);
            }
            Gizmos.color = Color.yellow;
        }

        
    }

    List<Vector3> GetPosition(BezierPoint bezierpos, float segment,out Vector3[] start, out Vector3[] mid, out Vector3[] end)
    {
        List<Vector3> pos = new List<Vector3>();
        start = new Vector3[11];
        mid = new Vector3[11];
        end = new Vector3[11];

        Vector3 temp, temp2, temp3;
        for(int i = 0;i<segment+1;i++)
        {
            pos.Add(BezierCurve(bezierpos.m_startPos, bezierpos.m_startBezier, bezierpos.m_endBezier, bezierpos.m_endPos, i / segment, out temp, out temp2, out temp3));
            if (i == segment)
                break;
            start[i] = temp;
            mid[i] = temp2;
            end[i] = temp3;
        }

        pos.Add(BezierCurve(bezierpos.m_endPos, bezierpos.m_endBezier, bezierpos.m_startBezier, bezierpos.m_startPos, 0, out temp, out temp2, out temp3));
        start[10] = -temp;
        mid[10] = temp2;
        end[10] = -temp3;

        return pos;
    }
    
    Vector3 Lerp(Vector3 a, Vector3 b, float t)
    {
        return (1f - t) * a + t * b;
    }

    Vector3 GetVertical(Vector3 a, Vector3 b)
    {
        Vector3 temp = (a - b).normalized;

        return new Vector3(-temp.z, 0, temp.x);
    }

    Vector3 BezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t, out Vector3 start, out Vector3 mid, out Vector3 end)
    {
        Vector3 a = Lerp(p0, p1, t);
        Vector3 b = Lerp(p1, p2, t);
        Vector3 c = Lerp(p2, p3, t);
        Vector3 d = Lerp(a, b, t);
        Vector3 e = Lerp(b, c, t);
        Vector3 pointOnCurve = Lerp(d, e, t);

        mid = pointOnCurve;
        start = -GetVertical(pointOnCurve, e);
        end = GetVertical(pointOnCurve, e);
        

        return pointOnCurve;
    }

    bool isInside(Vector3 B, Vector3[] p)
    {
    //crosses는 점q와 오른쪽 반직선과 다각형과의 교점의 개수
        int crosses = 0;
        for(int i = 0 ; i<p.Length ; i++)
        {
            int j = (i + 1) % p.Length;
            //점 B가 선분 (p[i], p[j])의 y좌표 사이에 있음
            if((p[i].z > B.z) != (p[j].z > B.z) )
            {
                //atX는 점 B를 지나는 수평선과 선분 (p[i], p[j])의 교점
                double atX = (p[j].x - p[i].x) * (B.z - p[i].z) / (p[j].z - p[i].z) + p[i].x;
                //atX가 오른쪽 반직선과의 교점이 맞으면 교점의 개수를 증가시킨다.
                if(B.x<atX)
                crosses++;
            }
        }
    return crosses % 2 > 0;
    }

}
