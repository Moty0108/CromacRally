using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SpawnWindowEditor : EditorWindow
{

    static List<Object> m_prefabs;
    
    static List<string> m_prefabPreviews;
    Object m_selectObejct;

    static PreviewRenderUtility m_prevRender;

    [MenuItem("Window/SpawnerEditor")]
    static void Open()
    {
        SpawnWindowEditor exampleWindow = CreateInstance<SpawnWindowEditor>();
        exampleWindow.Show();
        exampleWindow.position = new Rect(500, 100, 500, 500);
        m_prefabs = new List<Object>();
        //m_prefabPreviews = new List<Texture>();
        var info = new DirectoryInfo("Assets/SpawnObject");
        foreach(FileInfo temp in info.GetFiles())
        {
            if (temp.FullName.Contains(".prefab") && !temp.FullName.Contains(".meta"))
            {
                Debug.Log(temp.FullName);
                //m_prefabs.Add(AssetDatabase.LoadAssetAtPath(temp.FullName, typeof(GameObject)));
                m_prefabs.Add(PrefabUtility.LoadPrefabContents(temp.FullName));
            }
        }


        
        for (int i = 0; i < m_prefabs.Count; i++)
        {
            //m_prefabPreviews.Add(AssetPreview.GetAssetPreview(m_prefabs[i]));
        }
        

        //SceneView.onSceneGUIDelegate += OnScene;
    }

    private void OnDestroy()
    {
        
        //SceneView.onSceneGUIDelegate -= OnScene;
        
    }

    private void OnDisable()
    {
        
    }

    static void OnScene(SceneView sceneView)
    {
        Event curEvent = Event.current;
        //ay raytemp = sceneView.camera.ViewportPointToRay(curEvent.mousePosition);
        Ray raytemp = HandleUtility.GUIPointToWorldRay(new Vector2(curEvent.mousePosition.x, curEvent.mousePosition.y));


        RaycastHit hittemp;
        if (Physics.Raycast(raytemp, out hittemp))
        {
            Debug.DrawLine(hittemp.point, hittemp.point + Vector3.up * 100, Color.red);

        }

        if (curEvent.type == EventType.MouseUp && curEvent.button == 0 && !curEvent.alt)
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
            }
            curEvent.Use();

        }
    }
    Object temp;
    int selected = 0;
    Vector2 scrollPos = Vector2.zero;

    private void OnGUI()
    {

        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height / 3));
        //selected = GUILayout.SelectionGrid(selected, m_prefabs.ToArray(), 3, GUILayout.Width(100), GUILayout.Height(100));
        GUILayout.EndScrollView();

        m_selectObejct = m_prefabs[selected];

        GUILayout.Label("현재 선택된 오브젝트");
        GUILayout.TextArea(m_prefabs[selected].name);
        
        
    }
}
