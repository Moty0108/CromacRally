using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BoxColliderCreator : EditorWindow
{
    static List<Vector3> m_points;
    static bool m_isInSceneView = false;
    static bool m_isCreatePointMode;

    
    static bool m_isDrag;
    static int m_button;
    static float m_height = 100;

    static Stack<List<Vector3>> m_prevPoints;


    List<Vector3> templist;
    [MenuItem("Window/BoxColliderCreator")]
    static void Open()
    {
        BoxColliderCreator exampleWindow = CreateInstance<BoxColliderCreator>();
        exampleWindow.Show();
        exampleWindow.position = new Rect(500, 100, 200, 300);
        
        m_points = new List<Vector3>();
        m_prevPoints = new Stack<List<Vector3>>();
        m_isCreatePointMode = false;
        m_isDrag = false;
        SceneView.onSceneGUIDelegate += SceneGUI;

        
    }

    private void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= SceneGUI;
        if(m_points != null)
            m_points.Clear();
    }

    private void OnFocus()
    {
        m_isInSceneView = true;
    }

    private void OnLostFocus()
    {
        m_isInSceneView = false;
    }

    static void SceneGUI(SceneView sceneView)
    {
        sceneView.Repaint();
        Event curEvent = Event.current;
        //ay raytemp = sceneView.camera.ViewportPointToRay(curEvent.mousePosition);
        if (m_isCreatePointMode)
        {
            Ray raytemp = HandleUtility.GUIPointToWorldRay(new Vector2(curEvent.mousePosition.x, curEvent.mousePosition.y));


            RaycastHit hittemp;
            if (Physics.Raycast(raytemp, out hittemp))
            {
                Debug.DrawLine(hittemp.point, hittemp.point + Vector3.up * m_height, Color.red);

            }

          

            if (curEvent.type == EventType.MouseDrag)
            {
                m_isDrag = true;
            }

            if (curEvent.type == EventType.MouseUp && curEvent.button == 0 && !curEvent.alt && !m_isDrag)
            {
                
                Vector3 mousePos = curEvent.mousePosition;
                float ppp = EditorGUIUtility.pixelsPerPoint;
                mousePos.y = sceneView.camera.pixelHeight - mousePos.y * ppp;
                mousePos.x *= ppp;

                Ray ray = sceneView.camera.ScreenPointToRay(mousePos);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.point);
                    m_points.Add(hit.point);
                }
                curEvent.Use();
            }

            if (curEvent.type == EventType.MouseUp)
                m_isDrag = false;

            if (curEvent.type == EventType.KeyDown && curEvent.keyCode == KeyCode.Z && curEvent.control)
            {
                curEvent.Use();
                if(m_points.Count > 0)
                    m_points.RemoveAt(m_points.Count - 1);
            }



            if (m_points != null)
            {
                for (int i = 0; i < m_points.Count - 1; i++)
                {
                    Debug.DrawLine(m_points[i], m_points[i + 1], Color.cyan);
                }

                for (int i = 0; i < m_points.Count; i++)
                {
                    m_points[i] = Handles.PositionHandle(m_points[i], Quaternion.identity);
                }
            }
        }
        else
        {
            if (curEvent.type == EventType.KeyDown && curEvent.keyCode == KeyCode.Z && curEvent.control)
            {
                curEvent.Use();
                
            }
        }
    }

    private void OnGUI()
    {

        m_height = EditorGUILayout.FloatField("높이", m_height);

        if (GUILayout.Button("콜라이더 생성"))
        {
            if (m_points.Count != 0)
                CreateColiider(m_points);
            else
                Debug.Log("생성된 포인트 없음!");
        }

        if (!m_isCreatePointMode)
        {
            if (GUILayout.Button("포인트 생성 OFF"))
            {
                m_isCreatePointMode = true;
            }
        }
        else
        {
            if (GUILayout.Button("포인트 생성 ON"))
            {
                m_isCreatePointMode = false;
            }
        }

        if (m_prevPoints.Count > 0)
        {
            if (GUILayout.Button("이전 포인트 되돌리기"))
            {
                m_isCreatePointMode = true;


                m_points = m_prevPoints.Pop();
            }
        }

        GUILayout.Box("높이 : 콜라이더 높이");
        GUILayout.Box("Ctrl + Z : 마지막으로 지정한 포인트 삭제");

        if (m_prevPoints.Count > 0)
        {
            GUILayout.Box("이전 포인트 되돌리기 : 콜라이더 생성 전 지정한 포인트 복원");
        }
    }

    static void CreateColiider(List<Vector3> points)
    {
        GameObject parent = new GameObject("BoxCollider");

        
        for(int i = 0; i < points.Count - 1;i++)
        {
            GameObject temp = new GameObject(i.ToString());
            temp.transform.position = Vector3.Lerp(points[i], points[i+1], 0.5f);
            temp.transform.SetParent(parent.transform);
            temp.transform.LookAt(points[i + 1]);
            temp.transform.localEulerAngles = new Vector3(0, temp.transform.localEulerAngles.y, 0);

            BoxCollider col = temp.AddComponent<BoxCollider>();
            col.size = new Vector3(0, m_height, Vector3.Distance(points[i], points[i + 1]));
            
        }

        Selection.activeObject = parent;

        if (EditorUtility.DisplayDialog("경고!", "적용할거야??", "응", "아니"))
        {
            m_prevPoints.Push(new List<Vector3>(points));
            m_isCreatePointMode = false;
            m_points.Clear();
        }
        else
        {
            DestroyImmediate(parent);
        }
        
        
    }
}
