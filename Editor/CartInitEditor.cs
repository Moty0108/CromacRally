using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CarIntialize))]
public class CartInitEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        if(GUILayout.Button("차량 설정"))
        {
            CarIntialize m_target = target as CarIntialize;

            if(m_target.gameObject.transform.GetComponent<RVP.VehicleParent>() == null)
                m_target.Init();
            else
            {
                Debug.Log("이미 생성 되어 있어!!");
            }
        }
    }


}
