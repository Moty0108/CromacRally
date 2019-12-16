using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyDataInfos : MonoBehaviour
{
    public Transform m_terrain;
    public TileInfo[] m_tileInfos;
    

    float m_terrainPositionX;
    float m_terrainPositionY;
    TerrainLayer[] m_terrainLayers;
    TerrainData m_terrainData;
    

    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.Find("Terrain"))
        {
            m_terrain = GameObject.Find("Terrain").GetComponent<Transform>();
        }

        m_terrainData = m_terrain.GetComponent<Terrain>().terrainData;
        m_terrainLayers = m_terrainData.terrainLayers;

        m_terrainPositionX = (float)m_terrain.position.x;
        m_terrainPositionY = (float)m_terrain.position.z;
    }

    // Update is called once per frame
    void Update()
    {

        if(m_tileInfos != null)
            SetEffect();

        
    }

    void SetEffect()
    {
        int x = (int)(Mathf.Abs(m_terrainPositionX - transform.position.x) / m_terrainData.size.x * 512);
        int y = (int)(Mathf.Abs(m_terrainPositionY - transform.position.z) / m_terrainData.size.z * 512);

        Color color;
        float maxColor = 0;
        int maxLayerNum = 0;

        for (int i = 0; i < m_terrainData.alphamapTextures.Length; i++)
        {
            color = m_terrainData.alphamapTextures[i].GetPixel(x, y);

            for (int j = 0; j < 4; j++)
            {
                if (color[j] > maxColor)
                {
                    maxColor = color[j];
                    maxLayerNum = (i * 4) + j;
                }

            }
        }

        if (TerrainLayerSoundData.Instance)
        {
            for (int i = 0; i < TerrainLayerSoundData.Instance.m_layerDatas.Length; i++)
            {
                if (TerrainLayerSoundData.Instance.m_layerDatas[i].layer == m_terrainLayers[maxLayerNum])
                {
                    TerrainLayerSoundData.Instance.m_curLayerNum = i;
                }
            }
        }

        for (int i = 0; i < m_tileInfos.Length; i++)
        {
            for (int j = 0; j < m_tileInfos[i].m_terrainLayers.Count; j++)
            {
                if (m_terrainLayers[maxLayerNum] == m_tileInfos[i].m_terrainLayers[j])
                {

                    for (int q = 0; q < m_tileInfos[i].m_handleSections.Count; q++)
                    {
                        if (m_tileInfos[i].m_handleSections[q].m_min < GetComponent<Rigidbody>().velocity.magnitude * 3 && m_tileInfos[i].m_handleSections[q].m_max > GetComponent<Rigidbody>().velocity.magnitude * 3)
                        {
                            if (m_tileInfos[i].m_handleSections[q].m_handleInfo == null)
                            {
                                FFBInfo.StopEffect();
                            }
                            else
                            {
                                m_tileInfos[i].m_handleSections[q].m_handleInfo.StartEffect();
                                //Debug.Log(m_tileInfos[i].m_handleSections[q].m_handleInfo.name);
                            }
                        }
                    }

                    for (int q = 0; q < m_tileInfos[i].m_chairSections.Count; q++)
                    {
                        if (m_tileInfos[i].m_chairSections[q].m_min < GetComponent<Rigidbody>().velocity.magnitude * 3 && m_tileInfos[i].m_chairSections[q].m_max > GetComponent<Rigidbody>().velocity.magnitude * 3)
                        {
                            if (m_tileInfos[i].m_chairSections[q].m_chairInfo == null)
                            {
                                ChairViveInfo.StopEffect();
                            }
                            else
                            {
                                m_tileInfos[i].m_chairSections[q].m_chairInfo.StartEffect();
                                //Debug.Log(m_tileInfos[i].m_chairSections[q].m_chairInfo.name);
                            }
                        }
                    }

                    return;
                }
            }
        }

        FFBInfo.StopEffect();
        ChairViveInfo.StopEffect();
    }
}
