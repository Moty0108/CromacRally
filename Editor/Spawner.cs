using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[ExecuteInEditMode]
public class Spawner : MonoBehaviour
{
    public enum Type
    {
        NONE, ONEPIVOT, TWOPIVOT
    }

    Ray ray, ray2, ray3;
    public GameObject m_object;
    public Type m_type = Type.NONE;

    public bool m_start;

    public float m_height;
    public float m_twoDotRange;
    public float m_angle;
    public float m_angleAmount;
    public float m_objSize;
    public float m_sizeAmount;
    public bool leftright;
    public bool m_forwardBack;
    public bool m_magnet;
    public float m_magnetLength;

    [Space]
    public float m_minSize;
    public float m_maxSize;
    Quaternion m_meshQuat;

    private float m_prevObjSize;
    private float m_reverseAngle;
    private GameObject obj;

    Vector3 m_targetPos;
    Vector3 norm;
    Vector3 left, right;
    float x;

    Vector3 leftPos, rightPos;

    private void OnEnable()
    {
        if (!Application.isEditor)
        {
            Destroy(this);
        }

        SceneView.onSceneGUIDelegate += OnScene;
        
        // 실제 사용 시 주석 제거
        
    }

    private void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= OnScene;
    }

    void OnScene(SceneView scene)
    {
        Event e = Event.current;

        SceneView.RepaintAll();

        Vector3 mousePos = e.mousePosition;
        float ppp = EditorGUIUtility.pixelsPerPoint;
        mousePos.y = scene.camera.pixelHeight - mousePos.y * ppp;
        mousePos.x *= ppp;

        ray = scene.camera.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (mousePos.y > scene.camera.pixelHeight || mousePos.x > scene.camera.pixelWidth)
            return;

        if (m_type == Type.ONEPIVOT && m_start)
        {
            if (Physics.Raycast(ray, out hit))
            {

                m_targetPos = hit.point;


                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Q)
                {
                    m_angle += m_angleAmount;
                    m_angle %= 360;
                }

                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.E)
                {
                    m_angle -= m_angleAmount;
                    m_angle %= 360;
                }

                m_meshQuat = Quaternion.Euler(new Vector3(0, -m_angle,0));

                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Keypad8)
                {
                    m_objSize += m_sizeAmount;
                }
                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Keypad5)
                {
                   m_objSize -= m_sizeAmount;
                }

                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Z)
                {
                    m_angle = Random.Range(0, 360);
                }

                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.C)
                {
                    m_objSize = Random.Range(m_minSize, m_maxSize);
                }

                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Space)
                {
                    obj = Instantiate(m_object);
                    obj.transform.position = m_targetPos + Vector3.up * m_height;
                    obj.transform.Rotate(new Vector3(0, -m_angle, 0), Space.World);
                    obj.transform.localScale = new Vector3(m_objSize, m_objSize, m_objSize);
                }

            }
        }

        if (m_type == Type.TWOPIVOT && m_start)
        {   


            if (Physics.Raycast(ray, out hit))
            {
                m_targetPos = hit.point;
                norm = hit.point + Vector3.up * 10;


                m_prevObjSize = m_object.transform.localScale.x;
                if (m_magnet)
                {
                    m_objSize = m_prevObjSize;
                    if (hit.transform.root.tag == "GuardRail")
                    {
                        m_objSize = hit.transform.root.localScale.x;
                        Vector3 len = hit.transform.right * (hit.transform.GetComponentsInChildren<MeshCollider>()[0].bounds.size.x + m_object.transform.GetComponentsInChildren<MeshCollider>()[0].bounds.size.x);
                        if (leftright)
                        {
                            len *= -1;
                        }
                        //norm = hit.transform.root.position + Vector3.up * 10 + len;
                        m_targetPos = hit.transform.root.position + Vector3.down * m_object.transform.localScale.y + len;
                        //m_angle = hit.transform.root.localRotation.eulerAngles.y;

                        if (leftright)
                            norm = hit.transform.root.position - hit.transform.right * m_magnetLength;
                        else
                            norm = hit.transform.root.position + hit.transform.right * m_magnetLength;

                        norm += Vector3.up * 10;
                    }

                }

                //Vector3 left, right;


                if (!m_magnet)
                {
                    float x, z;
                    x = norm.x + Mathf.Cos(m_angle * Mathf.Deg2Rad) * m_twoDotRange;
                    z = norm.z + Mathf.Sin(m_angle * Mathf.Deg2Rad) * m_twoDotRange;


                    left = new Vector3(x, norm.y, z);

                    x = norm.x + Mathf.Cos((m_angle + 180) * Mathf.Deg2Rad) * m_twoDotRange;
                    z = norm.z + Mathf.Sin((m_angle + 180) * Mathf.Deg2Rad) * m_twoDotRange;
                    right = new Vector3(x, norm.y, z);
                }
                else
                {
                    if (hit.transform.root.tag == "GuardRail")
                    {
                        float x, z;
                        x = norm.x + Mathf.Cos(m_angle * Mathf.Deg2Rad) * m_twoDotRange;
                        z = norm.z + Mathf.Sin(m_angle * Mathf.Deg2Rad) * m_twoDotRange;

                        left = new Vector3(x, norm.y, z);
                        right = norm;
                    }
                    else
                    {
                        float x, z;
                        x = norm.x + Mathf.Cos(m_angle * Mathf.Deg2Rad) * 5;
                        z = norm.z + Mathf.Sin(m_angle * Mathf.Deg2Rad) * 5;

                        left = new Vector3(x, norm.y, z);
                        right = norm;
                    }
                }




                ray2.origin = left;
                ray2.direction = Vector3.down;
                ray3.origin = right;
                ray3.direction = Vector3.down;





                RaycastHit[] ray2hit;
                RaycastHit[] ray3hit;

                //Debug.DrawLine(norm, left, Color.blue);
                //Debug.DrawLine(norm, right, Color.blue);


                ray2hit = Physics.RaycastAll(ray2);

                for (int i = 0; i < ray2hit.Length; i++)
                {
                    if (ray2hit[i].transform.tag == "Terrain")
                        leftPos = ray2hit[i].point + Vector3.up * m_object.transform.localScale.y;
                }


                ray3hit = Physics.RaycastAll(ray3);

                for (int i = 0; i < ray3hit.Length; i++)
                {
                    if (ray3hit[i].transform.tag == "Terrain")
                        rightPos = ray3hit[i].point + Vector3.up * m_object.transform.localScale.y;
                }

                if (m_magnet)
                    m_targetPos = leftPos + Vector3.down * (m_object.transform.localScale.y + 0.2f);

                //Debug.DrawLine(leftPos, rightPos);

                Vector3 planeVec;
                planeVec = rightPos - leftPos;
                planeVec = new Vector3(planeVec.x, 0, planeVec.z);
                float a = Vector3.Dot(rightPos - leftPos, planeVec) / (rightPos - leftPos).magnitude / planeVec.magnitude;
                float rad = Mathf.Acos(a);


                if (leftPos.y < rightPos.y)
                    rad = -rad;



                m_meshQuat = Quaternion.identity;



                if (!float.IsNaN(rad * Mathf.Rad2Deg))
                    m_meshQuat = Quaternion.Euler(new Vector3(m_meshQuat.eulerAngles.x, -m_angle, rad * Mathf.Rad2Deg));


                if (e.type == EventType.MouseMove)
                {
                    m_targetPos = hit.point;
                }

                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Space)
                {
                    obj = Instantiate(m_object);
                    obj.transform.position = m_targetPos + Vector3.up * (m_objSize - 0.2f);
                    obj.transform.Rotate(new Vector3(0, -m_angle, 0), Space.World);
                    obj.transform.Rotate(new Vector3(0, 0, rad * Mathf.Rad2Deg), Space.Self);
                    obj.transform.localScale = new Vector3(m_objSize, m_objSize, m_objSize);
                }

                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Q)
                {
                    if (e.type == EventType.KeyDown && !e.shift)
                    {
                        m_angle += m_angleAmount;
                        m_angle %= 360;
                    }
                    else
                    {
                        Transform[] temp;
                        temp = Selection.transforms;
                        for (int i = 0; i < temp.Length; i++)
                        {
                            if (temp[i].root.tag == "GuardRail")
                            {
                                temp[i].root.transform.Translate(new Vector3(0, -0.1f, 0), Space.Self);
                            }
                        }
                    }
                }

                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.E)
                {
                    if (e.type == EventType.KeyDown && !e.shift)
                    {
                        m_angle -= m_angleAmount;
                        m_angle %= 360;
                    }
                    else
                    {
                        Transform[] temp;
                        temp = Selection.transforms;
                        for (int i = 0; i < temp.Length; i++)
                        {
                            if (temp[i].root.tag == "GuardRail")
                            {
                                temp[i].root.transform.Translate(new Vector3(0, 0.1f, 0), Space.Self);
                            }
                        }
                    }
                }

                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.A)
                {
                    if (e.type == EventType.KeyDown && !e.shift)
                    {
                        leftright = !leftright;
                    }
                    else
                    {
                        Transform[] temp;
                        temp = Selection.transforms;
                        for (int i = 0; i < temp.Length; i++)
                        {
                            if (temp[i].root.tag == "GuardRail")
                            {
                                temp[i].root.transform.Translate(new Vector3(0.1f, 0, 0), Space.Self);
                            }
                        }
                    }
                }

                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.D)
                {
                    if (e.type == EventType.KeyDown && !e.shift)
                    {
                        m_magnet = !m_magnet;
                    }
                    else
                    {
                        Transform[] temp;
                        temp = Selection.transforms;
                        for (int i = 0; i < temp.Length; i++)
                        {
                            if (temp[i].root.tag == "GuardRail")
                            {
                                temp[i].root.transform.Translate(new Vector3(-0.1f, 0, 0), Space.Self);
                            }
                        }
                    }
                }

                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.W)
                {
                    if (e.type == EventType.KeyDown && !e.shift)
                    {
                        m_objSize += m_sizeAmount;
                    }
                    else
                    {
                        Transform[] temp;
                        temp = Selection.transforms;
                        for (int i = 0; i < temp.Length; i++)
                        {
                            if (temp[i].root.tag == "GuardRail")
                            {
                                temp[i].root.transform.Translate(new Vector3(0, 0, -0.1f), Space.Self);
                            }
                        }
                    }
                }
                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.S)
                {
                    if (e.type == EventType.KeyDown && !e.shift)
                    {
                        m_objSize -= m_sizeAmount;
                    }
                    else
                    {
                        Transform[] temp;
                        temp = Selection.transforms;
                        for (int i = 0; i < temp.Length; i++)
                        {
                            if (temp[i].root.tag == "GuardRail")
                            {
                                temp[i].root.transform.Translate(new Vector3(0, 0, 0.1f), Space.Self);
                            }
                        }
                    }
                }
                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Z)
                {
                    if (e.type == EventType.KeyDown && !e.shift)
                        m_twoDotRange -= 0.1f;
                    else
                    {
                        m_magnetLength -= 0.1f;
                    }
                }
                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.C)
                {
                    if (e.type == EventType.KeyDown && !e.shift)
                        m_twoDotRange += 0.1f;
                    else
                    {
                        m_magnetLength += 0.1f;
                    }
                }

                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Z && e.alt)
                {
                    //Debug.Log("undo");
                    if (obj)
                        DestroyImmediate(obj);
                }

                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Y)
                {
                    Transform[] temp;
                    temp = Selection.transforms;
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].root.tag == "GuardRail")
                        {
                            temp[i].root.transform.Rotate(new Vector3(0, 0, 0.1f), Space.Self);
                        }
                    }
                }
                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.H)
                {
                    Transform[] temp;
                    temp = Selection.transforms;
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].root.tag == "GuardRail")
                        {
                            temp[i].root.transform.Rotate(new Vector3(0, 0, -0.1f), Space.Self);
                        }
                    }
                }
                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.G)
                {
                    Transform[] temp;
                    temp = Selection.transforms;
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].root.tag == "GuardRail")
                        {
                            temp[i].root.transform.Rotate(new Vector3(0, -0.1f, 0), Space.Self);
                        }
                    }
                }
                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.J)
                {
                    Transform[] temp;
                    temp = Selection.transforms;
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].root.tag == "GuardRail")
                        {
                            temp[i].root.transform.Rotate(new Vector3(0, 0.1f, 0), Space.Self);
                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        SceneView.RepaintAll();

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(norm, left);
        Gizmos.DrawLine(norm, right);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(leftPos, rightPos);

        if (Application.isEditor && m_type == Type.ONEPIVOT && m_start)
        {
            
            foreach (MeshFilter ms in m_object.GetComponentsInChildren<MeshFilter>())
            {
                //Gizmos.DrawWireMesh(ms.sharedMesh, m_targetPos + Vector3.up * m_height, m_meshQuat, new Vector3(m_objSize, m_objSize, m_objSize));
            }
        }

        if (Application.isEditor && m_type == Type.TWOPIVOT && m_start)
        {
            

            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(ray2);
            Gizmos.DrawRay(ray3);
            Gizmos.color = Color.white;

            Gizmos.DrawLine(m_targetPos, norm);

            //Gizmos.DrawWireMesh(temp.GetComponent<MeshFilter>().sharedMesh, m_targetPos + Direction * temp.GetComponent<MeshFilter>().sharedMesh.bounds.size.y / 2, dir);
            Gizmos.DrawWireMesh(m_object.GetComponentsInChildren<MeshFilter>()[0].sharedMesh, m_targetPos + Vector3.up * (m_objSize - 0.2f), m_meshQuat, new Vector3(m_objSize, m_objSize, m_objSize));
            Gizmos.DrawWireMesh(m_object.GetComponentsInChildren<MeshFilter>()[1].sharedMesh, m_targetPos + Vector3.up * (m_objSize - 0.2f), m_meshQuat, new Vector3(m_objSize, m_objSize, m_objSize));
            Gizmos.DrawWireMesh(m_object.GetComponentsInChildren<MeshFilter>()[3].sharedMesh, m_targetPos + Vector3.up * (m_objSize - 0.2f), m_meshQuat, new Vector3(m_objSize, m_objSize, m_objSize));
        }

        
    }
}
#endif