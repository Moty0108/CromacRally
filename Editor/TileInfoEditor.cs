using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(TileInfo))]
public class TileInfoEditor : Editor
{
    TileInfo m_tileInfo = null;

    private void OnEnable()
    {
        m_tileInfo = (TileInfo)target;
    }

    public override void OnInspectorGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontStyle = FontStyle.Bold;

        EditorGUILayout.HelpBox("의자와 핸들의 진동이 적용될 터레인 레이어를 연결한다.", MessageType.None);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Terrain Layers", style);
        
        if(GUILayout.Button("+"))
        {
            m_tileInfo.m_terrainLayers.Add(new TerrainLayer());
        }

        if(GUILayout.Button("-") && m_tileInfo.m_terrainLayers.Count > 0)
        {
            m_tileInfo.m_terrainLayers.RemoveAt(m_tileInfo.m_terrainLayers.Count-1);
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        for (int i = 0; i < m_tileInfo.m_terrainLayers.Count; i++)
        {
            m_tileInfo.m_terrainLayers[i] = (TerrainLayer)EditorGUILayout.ObjectField("레이어 " + (i + 1), m_tileInfo.m_terrainLayers[i], typeof(TerrainLayer));
        }



        GUILayout.Space(50);

        EditorGUILayout.HelpBox("구간(속도)의 최소, 최대 (의자, 핸들 공유)", MessageType.None);
        EditorGUILayout.BeginHorizontal();
        m_tileInfo.m_minRange = EditorGUILayout.FloatField("Min", m_tileInfo.m_minRange);
        m_tileInfo.m_maxRange = EditorGUILayout.FloatField("Max", m_tileInfo.m_maxRange);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.HelpBox("각 구간(속도) 별로 적용할 핸들 진동 데이터를 연결한다.", MessageType.None);
        GUILayout.BeginHorizontal();

        GUILayout.Label("HandleSection", style);
        if (GUILayout.Button("+"))
        {
            m_tileInfo.m_handleSections.Add(new HandleSection());
        }

        if (GUILayout.Button("-") && m_tileInfo.m_handleSections.Count > 0)
        {
            m_tileInfo.m_handleSections.RemoveAt(m_tileInfo.m_handleSections.Count - 1);
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        for (int i = 0; i < m_tileInfo.m_handleSections.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            m_tileInfo.m_handleSections[i].m_min = EditorGUILayout.FloatField(m_tileInfo.m_handleSections[i].m_min, GUILayout.Width(30));
            EditorGUILayout.MinMaxSlider(ref m_tileInfo.m_handleSections[i].m_min, ref m_tileInfo.m_handleSections[i].m_max, m_tileInfo.m_minRange, m_tileInfo.m_maxRange);
            m_tileInfo.m_handleSections[i].m_max = EditorGUILayout.FloatField(m_tileInfo.m_handleSections[i].m_max, GUILayout.Width(30));
            EditorGUILayout.EndHorizontal();

            if (i == 0)
            {
                m_tileInfo.m_handleSections[i].m_min = m_tileInfo.m_minRange;
            }

            if (i > 0)
            {
                m_tileInfo.m_handleSections[i - 1].m_max = m_tileInfo.m_handleSections[i].m_min;
            }
            if (i < m_tileInfo.m_handleSections.Count - 1)
            {
                m_tileInfo.m_handleSections[i + 1].m_min = m_tileInfo.m_handleSections[i].m_max;
            }

            if (i == m_tileInfo.m_handleSections.Count - 1)
            {
                //m_tileInfo.m_sections[i].m_max = m_tileInfo.m_maxRange;
                m_tileInfo.m_handleSections[i].m_max = m_tileInfo.m_maxRange;
            }



            m_tileInfo.m_handleSections[i].m_handleInfo = (FFBInfo)EditorGUILayout.ObjectField("핸들 진동", m_tileInfo.m_handleSections[i].m_handleInfo, typeof(FFBInfo));

            EditorGUILayout.Space();
        }


        GUILayout.Space(50);

        EditorGUILayout.HelpBox("각 구간(속도) 별로 적용할 의자 진동 데이터를 연결 한다.", MessageType.None);
        GUILayout.BeginHorizontal();

        GUILayout.Label("ChairSection", style);
        if (GUILayout.Button("+"))
        {
            m_tileInfo.m_chairSections.Add(new ChairSection());
        }

        if (GUILayout.Button("-") && m_tileInfo.m_chairSections.Count > 0)
        {
            m_tileInfo.m_chairSections.RemoveAt(m_tileInfo.m_chairSections.Count - 1);
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        for (int i = 0; i < m_tileInfo.m_chairSections.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            m_tileInfo.m_chairSections[i].m_min = EditorGUILayout.FloatField(m_tileInfo.m_chairSections[i].m_min, GUILayout.Width(30));
            EditorGUILayout.MinMaxSlider(ref m_tileInfo.m_chairSections[i].m_min, ref m_tileInfo.m_chairSections[i].m_max, m_tileInfo.m_minRange, m_tileInfo.m_maxRange);
            m_tileInfo.m_chairSections[i].m_max = EditorGUILayout.FloatField(m_tileInfo.m_chairSections[i].m_max, GUILayout.Width(30));
            EditorGUILayout.EndHorizontal();

            if (i == 0)
            {
                m_tileInfo.m_chairSections[i].m_min = m_tileInfo.m_minRange;
            }

            if (i > 0)
            {
                m_tileInfo.m_chairSections[i - 1].m_max = m_tileInfo.m_chairSections[i].m_min;
            }
            if (i < m_tileInfo.m_chairSections.Count - 1)
            {
                m_tileInfo.m_chairSections[i + 1].m_min = m_tileInfo.m_chairSections[i].m_max;
            }

            if (i == m_tileInfo.m_chairSections.Count - 1)
            {
                //m_tileInfo.m_sections[i].m_max = m_tileInfo.m_maxRange;
                m_tileInfo.m_chairSections[i].m_max = m_tileInfo.m_maxRange;
            }



            m_tileInfo.m_chairSections[i].m_chairInfo = (ChairViveInfo)EditorGUILayout.ObjectField("의자 진동", m_tileInfo.m_chairSections[i].m_chairInfo, typeof(ChairViveInfo));

            EditorGUILayout.Space();
        }
        
        if(GUILayout.Button("Apply"))
        {
            EditorUtility.SetDirty(m_tileInfo);
        }
    }
}
