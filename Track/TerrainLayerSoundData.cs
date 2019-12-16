using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainLayerSoundData : Singleton<TerrainLayerSoundData>
{
    [System.Serializable]
    public class LayerData
    {
        public TerrainLayer layer;
        public AudioClip tireAudio;
        public AudioClip DriftAudio;
    }

    public LayerData[] m_layerDatas;
    public int m_curLayerNum = 0;
    LayerSound m_layerSound;

    // Start is called before the first frame update
    void Start()
    {
        m_layerSound = GamaManager.Instance.m_player.GetComponentInChildren<LayerSound>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_layerDatas[m_curLayerNum].tireAudio != null && m_layerSound != null)
            m_layerSound.SetAudioClip(m_layerDatas[m_curLayerNum].tireAudio, m_layerDatas[m_curLayerNum].DriftAudio);
    }

    [ContextMenu("GetLayerData")]
    public void GetLayerData()
    {
        m_layerDatas = new LayerData[GetComponent<Terrain>().terrainData.terrainLayers.Length];
        
        for(int i = 0; i<m_layerDatas.Length;i++)
        {
            m_layerDatas[i] = new LayerData();
            m_layerDatas[i].layer = GetComponent<Terrain>().terrainData.terrainLayers[i];
        }
    }
}
