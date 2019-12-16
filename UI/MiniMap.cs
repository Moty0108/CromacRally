using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public Transform m_player;
    public Terrain m_terrain;
    public Sprite m_minimapImage;
    public Sprite m_playerIcon;
    public Sprite m_checkPointIcon;
    public Sprite m_ghostSprite;
    public Transform m_checkPoint;
    

    public Image m_imageMap;
    public Image m_imagePlayer;
    public Image m_ghostImage;
    public RectTransform m_rotationParent;
    public RectTransform m_scaleParent;

    public float m_minimapDefaultSize;
    public float m_minimapMaxDistance;
    public float m_minimapMaxSize;
    public float m_playerIconRotaionOffset;
    public float m_playerIconSize = 1;
    public float m_checkPointSize;
    public float m_ghostSize;
    Vector3 m_terrainOffset;
    public GameObject[] m_checkPointObj;
    public List<CheckPointClass> m_checkPoints;
    

    // Start is called before the first frame update
    void Start()
    {
        m_player = GamaManager.Instance.m_player;

        m_imageMap.sprite = m_minimapImage;
        m_imagePlayer.sprite = m_playerIcon;
        
        m_terrainOffset = m_terrain.transform.position;

        if (GamaManager.Instance.m_mode == TimeMode.GrandPrix)
        {
            m_ghostImage.gameObject.SetActive(true);
            m_ghostImage.sprite = m_ghostSprite;
        }
        else
            m_ghostImage.gameObject.SetActive(false);

        ResetCheckPoint();
    }

    // Update is called once per framew
    void Update()
    {
        float x, y;
        x = (m_player.position.x - (m_terrainOffset.x + m_terrain.terrainData.size.x / 2)) * m_imageMap.rectTransform.sizeDelta.x / m_terrain.terrainData.size.x;
        y = (m_player.position.z - (m_terrainOffset.z + m_terrain.terrainData.size.z / 2)) * m_imageMap.rectTransform.sizeDelta.y / m_terrain.terrainData.size.z;

        m_imageMap.rectTransform.localPosition = new Vector3(-x, -y, 0);


        m_rotationParent.localRotation = Quaternion.Euler(0, 0, m_player.transform.localRotation.eulerAngles.y);
        m_imagePlayer.rectTransform.localRotation = Quaternion.Euler(0, 0, m_rotationParent.localRotation.eulerAngles.y - m_player.transform.localRotation.eulerAngles.y + +m_playerIconRotaionOffset);
        m_scaleParent.localScale = new Vector3(m_minimapDefaultSize - ( m_player.GetComponent<Rigidbody>().velocity.magnitude * 3.6f) * m_minimapMaxSize / 200, m_minimapDefaultSize - (m_player.GetComponent<Rigidbody>().velocity.magnitude * 3.6f) * m_minimapMaxSize / 200, m_minimapDefaultSize);
        //m_scaleParent.localPosition = new Vector3(-(m_player.GetComponent<Rigidbody>().velocity.magnitude * 3.6f) * m_minimapMaxDistance / 200, -(m_player.GetComponent<Rigidbody>().velocity.magnitude * 3.6f) * m_minimapMaxDistance / 200, 1);
        Vector3 temp = new Vector3(m_player.GetComponent<Rigidbody>().velocity.normalized.x, m_player.GetComponent<Rigidbody>().velocity.normalized.z, 0);

        m_scaleParent.localPosition = temp * -(m_player.GetComponent<Rigidbody>().velocity.magnitude * 3.6f) * m_minimapMaxDistance / 200;
        m_imagePlayer.rectTransform.localScale = new Vector3(m_playerIconSize + (m_player.GetComponent<Rigidbody>().velocity.magnitude * 3.6f) * 1.2f * m_minimapMaxSize / 200, m_playerIconSize + (m_player.GetComponent<Rigidbody>().velocity.magnitude * 3.6f) * 1.2f * m_minimapMaxSize / 200, m_playerIconSize);

        if (GamaManager.Instance.m_mode == TimeMode.Time)
        {
            for (int i = 0; i < m_checkPointObj.Length; i++)
            {
                float tempx, tempz;
                tempx = m_checkPoints[i].transform.position.x;
                tempz = m_checkPoints[i].transform.position.z;
                x = (tempx - (m_terrainOffset.x + m_terrain.terrainData.size.x / 2)) * m_imageMap.rectTransform.sizeDelta.x / m_terrain.terrainData.size.x;
                y = (tempz - (m_terrainOffset.z + m_terrain.terrainData.size.z / 2)) * m_imageMap.rectTransform.sizeDelta.y / m_terrain.terrainData.size.z;

                m_checkPointObj[i].GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
                m_checkPointObj[i].GetComponent<RectTransform>().localScale = new Vector3(m_checkPointSize + (m_player.GetComponent<Rigidbody>().velocity.magnitude * 3.6f) * 0.1f * m_minimapMaxSize / 200, m_checkPointSize + (m_player.GetComponent<Rigidbody>().velocity.magnitude * 3.6f) * 0.1f * m_minimapMaxSize / 200, m_checkPointSize);
                m_checkPointObj[i].GetComponent<RectTransform>().eulerAngles = new Vector3(0, m_rotationParent.localRotation.eulerAngles.y, 0);
            }
        }

        if(GamaManager.Instance.m_mode == TimeMode.GrandPrix)
        {
            if (GamaManager.Instance.m_recoder.GetGhostTransform() != null)
            {
                m_ghostImage.gameObject.SetActive(true);
                float ghostX, ghostY;
                ghostX = GamaManager.Instance.m_recoder.GetGhostTransform().position.x;
                ghostY = GamaManager.Instance.m_recoder.GetGhostTransform().position.z;
                x = (ghostX - (m_terrainOffset.x + m_terrain.terrainData.size.x / 2)) * m_imageMap.rectTransform.sizeDelta.x / m_terrain.terrainData.size.x;
                y = (ghostY - (m_terrainOffset.z + m_terrain.terrainData.size.z / 2)) * m_imageMap.rectTransform.sizeDelta.y / m_terrain.terrainData.size.z;

                m_ghostImage.GetComponent<RectTransform>().localPosition = new Vector3(x + m_imageMap.rectTransform.localPosition.x, y + m_imageMap.rectTransform.localPosition.y, 0);
                m_ghostImage.GetComponent<RectTransform>().localScale = new Vector3(m_ghostSize + (m_player.GetComponent<Rigidbody>().velocity.magnitude * 3.6f) * 0.1f * m_minimapMaxSize / 200, m_ghostSize + (m_player.GetComponent<Rigidbody>().velocity.magnitude * 3.6f) * 0.1f * m_minimapMaxSize / 200, m_ghostSize);
                m_ghostImage.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, m_rotationParent.localRotation.eulerAngles.y);
            }
            else
            {
                m_ghostImage.gameObject.SetActive(false);
            }
        }
    }

    //[ContextMenu("GetCheckPointData")]
    //public void ResetCheckPoint()
    //{
    //    m_checkPointObj = new GameObject[m_checkPoint.GetComponentsInChildren<CheckPointData>(true).Length];
    //    m_checkPointData = m_checkPoint.GetComponentsInChildren<CheckPointData>(true);
    //    Debug.Log(m_checkPointObj.Length);
    //    for (int i = 0; i < m_checkPointObj.Length; i++)
    //    {
    //        m_checkPointObj[i] = new GameObject();
    //        m_checkPointObj[i].name = "CheckPoint" + (i + 1);
    //        m_checkPointObj[i].transform.SetParent(m_imageMap.rectTransform);
    //        m_checkPointObj[i].AddComponent<CanvasRenderer>();
    //        m_checkPointObj[i].AddComponent<Image>();
    //        m_checkPointObj[i].GetComponent<Image>().sprite = m_checkPointIcon;
    //        m_checkPointObj[i].GetComponent<Image>().SetNativeSize();
    //    }

    //}

    [ContextMenu("GetCheckPointData")]
    public void ResetCheckPoint()
    {
        m_checkPoints.Clear();

        if (GamaManager.Instance.m_mode == TimeMode.Time)
        {
            for (int i = 0; i < TrackManager.Instance.m_points.Count; i++)
            {
                if (TrackManager.Instance.m_points[i].GetComponent<CheckPointClass>().m_pointType == CheckPointClass.CheckPointType.CHECKPOINT)
                {
                    m_checkPoints.Add(TrackManager.Instance.m_points[i].GetComponent<CheckPointClass>());
                }
            }

            m_checkPointObj = new GameObject[m_checkPoints.Count];

            for (int i = 0; i < m_checkPointObj.Length; i++)
            {
                m_checkPointObj[i] = new GameObject();
                m_checkPointObj[i].name = "CheckPoint" + (i + 1);
                m_checkPointObj[i].transform.SetParent(m_imageMap.rectTransform);
                m_checkPointObj[i].AddComponent<CanvasRenderer>();
                m_checkPointObj[i].AddComponent<Image>();
                m_checkPointObj[i].GetComponent<Image>().sprite = m_checkPointIcon;
                m_checkPointObj[i].GetComponent<Image>().SetNativeSize();
            }
        }
    }
}
