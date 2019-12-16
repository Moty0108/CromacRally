using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandleSection
{
    public float m_min;
    public float m_max;
    public FFBInfo m_handleInfo;

    public HandleSection()
    {
        m_min = 0;
        m_max = 0;
        m_handleInfo = null;
    }
}

[System.Serializable]
public class ChairSection
{
    public float m_min;
    public float m_max;
    public ChairViveInfo m_chairInfo;

    public ChairSection()
    {
        m_min = 0;
        m_max = 0;
        m_chairInfo = null;
    }
}

[ExecuteInEditMode]
[CreateAssetMenu(fileName ="TileInfo")]
[SerializeField]
public class TileInfo : ScriptableObject
{
    [Header("타일 정보")]
    public List<TerrainLayer> m_terrainLayers;
    
    public List<HandleSection> m_handleSections;
    public List<ChairSection> m_chairSections;
    public float m_minRange;
    public float m_maxRange;

    public TileInfo()
    {
        m_terrainLayers = new List<TerrainLayer>();
        m_handleSections = new List<HandleSection>();
        m_chairSections = new List<ChairSection>();
        m_minRange = 0;
        m_maxRange = 100;
    }
}
