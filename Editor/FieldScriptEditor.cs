using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldScript))]
public class FieldScriptEditor : Editor
{
    bool[] m_foldout = new bool[255];
    bool m_last = false;

    public void Awake()
    {
        for(int i=0;i<255;i++)
        {
            m_foldout[i] = false;
        }
    }

    public override void OnInspectorGUI()
    {
        

        FieldScript _target = target as FieldScript;
        EditorGUILayout.BeginVertical();

        _target.m_length = EditorGUILayout.FloatField("맵 넓이", _target.m_length);

        if (GUILayout.Button("포인트 추가"))
        {
            if (_target.pos.Count == 0)
                _target.pos.Add(new BezierPoint(Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero));
            else
            {
                Vector3 temp = new Vector3(10, 0, 0);

                _target.pos.Add(new BezierPoint(_target.pos[_target.pos.Count - 1].m_endPos,
                    _target.pos[_target.pos.Count - 1].m_endPos,
                    _target.pos[_target.pos.Count - 1].m_endPos,
                    _target.pos[_target.pos.Count - 1].m_endPos + temp));

                
            }
        }
        

        if(GUILayout.Button("마지막 포인트 삭제"))
        {
            _target.pos.RemoveAt(_target.pos.Count-1);
        }

        for(int i=0;i<_target.pos.Count;i++)
        {
            
            m_foldout[i] = EditorGUILayout.Foldout(m_foldout[i], "[" + (i+1) + "] Position ");
            if (m_foldout[i])
            {
                _target.pos[i].m_startPos = EditorGUILayout.Vector3Field("StartPosition", _target.pos[i].m_startPos);
                _target.pos[i].m_startBezier = EditorGUILayout.Vector3Field("StartBezier", _target.pos[i].m_startBezier);
                _target.pos[i].m_endBezier = EditorGUILayout.Vector3Field("EndBezier", _target.pos[i].m_endBezier);
                _target.pos[i].m_endPos = EditorGUILayout.Vector3Field("EndPosition", _target.pos[i].m_endPos);
                EditorGUILayout.Space();
                _target.pos[i].m_field = (enums.Field)EditorGUILayout.EnumPopup("필드 종류", _target.pos[i].m_field);
            }
        }

        if(GUILayout.Button("필드 종류 추가"))
        {
            _target.m_fieldinfos.Add(new FieldInfo());
        }

        if(GUILayout.Button("마지막 필드 종류 삭제"))
        {
            if(_target.m_fieldinfos.Count != 0)
                _target.m_fieldinfos.RemoveAt(_target.m_fieldinfos.Count - 1);
        }

        for(int i=0;i<_target.m_fieldinfos.Count;i++)
        {
            _target.m_fieldinfos[i].m_field = (enums.Field)EditorGUILayout.EnumPopup("필드 종류", _target.m_fieldinfos[i].m_field);

            _target.m_fieldinfos[i].m_fieldFirstValue = EditorGUILayout.FloatField("속도 구간 1(0 ~ ?)", _target.m_fieldinfos[i].m_fieldFirstValue);
            _target.m_fieldinfos[i].m_ffbinfo = (FFBInfo)EditorGUILayout.ObjectField("휠 정보", _target.m_fieldinfos[i].m_ffbinfo ,typeof(FFBInfo));
            
            EditorGUILayout.LabelField("------------------------------------------------------------------------");
            _target.m_fieldinfos[i].m_fieldSecondValue = EditorGUILayout.FloatField("속도 구간 2(1 ~ ?)", _target.m_fieldinfos[i].m_fieldSecondValue);
            _target.m_fieldinfos[i].m_ffbStronginfo = (FFBInfo)EditorGUILayout.ObjectField("휠 정보", _target.m_fieldinfos[i].m_ffbStronginfo, typeof(FFBInfo));
            //_target.m_fieldinfos[i].m_ffbinfo.m_forceSpeed = EditorGUILayout.FloatField("스티어링 속도", _target.m_fieldinfos[i].m_ffbinfo.m_forceSpeed);
            //_target.m_fieldinfos[i].m_ffbinfo.m_forceMaxSteering = EditorGUILayout.FloatField("스티어링 최대 각도", _target.m_fieldinfos[i].m_ffbinfo.m_forceMaxSteering);
            EditorGUILayout.LabelField("======================================");
            EditorGUILayout.Space();
            EditorGUILayout.Space();

        }

        //EditorGUILayout.BeginHorizontal();

        //_target.rotateSpeed = EditorGUILayout.IntSlider("Rotate Speed(degree)", (int)_target.rotateSpeed, 1, 360);

        //if (GUILayout.Button("+"))
        //{
        //    _target.rotateSpeed += 10f;
        //}
        //if (GUILayout.Button("-"))
        //{
        //    _target.rotateSpeed -= 10f;
        //}

        //EditorGUILayout.EndHorizontal();

        //_target.centerPos = EditorGUILayout.Vector3Field("Center", _target.centerPos);

        //GUILayout.Label("테스트 플레이");
        //string label = EditorApplication.isPlaying ? "Stop" : "Play";

        //if (GUILayout.Button(label))
        //    EditorApplication.ExecuteMenuItem("Edit/Play");
        EditorGUILayout.EndVertical();
    }

    public void OnSceneGUI()
    {
        FieldScript _target = target as FieldScript;

        if (_target.pos.Count != 0)
        {
            for (int i = 0; i < _target.pos.Count; i++)
            {
                _target.pos[i].m_startPos = Handles.PositionHandle(_target.pos[i].m_startPos, Quaternion.identity);
                _target.pos[i].m_startBezier = Handles.PositionHandle(_target.pos[i].m_startBezier, Quaternion.identity);
                _target.pos[i].m_endBezier = Handles.PositionHandle(_target.pos[i].m_endBezier, Quaternion.identity);
                _target.pos[i].m_endPos = Handles.PositionHandle(_target.pos[i].m_endPos, Quaternion.identity);
                
            }
        }
        
        
        

        //Vector3 dir = _target.transform.position - _target.centerPos;
        //float len = Handles.RadiusHandle(Quaternion.LookRotation(dir), _target.centerPos, dir.magnitude);
        //_target.transform.position = (len * dir.normalized) + _target.centerPos;
    }

    
}
