using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTreeCollider : MonoBehaviour
{
    TerrainData m_terrainData;
    TreeInstance[] m_trees;

    // Start is called before the first frame update
    void Start()
    {
        m_terrainData = GetComponent<Terrain>().terrainData;
        //Debug.Log("나무 개수 : " + m_terrainData.treeInstanceCount);
        m_trees = new TreeInstance[m_terrainData.treeInstanceCount];
        m_trees = m_terrainData.treeInstances;
        GameObject treeColliders = new GameObject("TreeColliders");

        for (int i = 0; i< m_trees.Length;i++)
        {
            GameObject temp = new GameObject("Tree" + i);
            temp.transform.SetParent(treeColliders.transform);
            temp.transform.position = new Vector3(m_trees[i].position.x * m_terrainData.size.x, m_trees[i].position.y * m_terrainData.size.y, m_trees[i].position.z * m_terrainData.size.z) + transform.position;
            temp.tag = "TreeCollider";
            CapsuleCollider cc = temp.AddComponent<CapsuleCollider>();
            cc.radius = m_trees[i].widthScale;
            cc.height = m_trees[i].heightScale * 20;
            cc.center = new Vector3(0, 10, 0);
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
