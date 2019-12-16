using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainSpawner : MonoBehaviour
{
    public GameObject a;
    public float m_spawnAreaWidth;
    public float m_spawnAreaHeight;
    Vector3 LeftTop;
    Vector3 RightTop;
    Vector3 LeftBottom;
    Vector3 RightBottom;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawn());

        LeftTop = new Vector3(transform.position.x - m_spawnAreaWidth / 2, transform.position.y + m_spawnAreaHeight / 2, transform.position.z);
        RightTop = new Vector3(transform.position.x + m_spawnAreaWidth / 2, transform.position.y + m_spawnAreaHeight / 2, transform.position.z);
        LeftBottom = new Vector3(transform.position.x - m_spawnAreaWidth / 2, transform.position.y - m_spawnAreaHeight / 2, transform.position.z);
        RightBottom = new Vector3(transform.position.x + m_spawnAreaWidth / 2, transform.position.y - m_spawnAreaHeight / 2, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator spawn()
    {
        while(true)
        {
            Vector3 spawnPos;
            spawnPos = new Vector3(Random.Range(LeftTop.x, RightTop.x),  Random.Range(LeftTop.y, LeftBottom.y), transform.position.z);

            GameObject tmep = Instantiate(a, spawnPos, a.transform.rotation);
            tmep.transform.SetParent(transform);
            Destroy(tmep, 20f);
            yield return new WaitForSeconds(0.001f);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 LeftTop = new Vector3(transform.position.x - m_spawnAreaWidth / 2, transform.position.y + m_spawnAreaHeight / 2, transform.position.z);
        Vector3 RightTop = new Vector3(transform.position.x + m_spawnAreaWidth / 2, transform.position.y + m_spawnAreaHeight / 2, transform.position.z);
        Vector3 LeftBottom = new Vector3(transform.position.x - m_spawnAreaWidth / 2, transform.position.y - m_spawnAreaHeight / 2, transform.position.z);
        Vector3 RightBottom = new Vector3(transform.position.x + m_spawnAreaWidth / 2, transform.position.y - m_spawnAreaHeight / 2, transform.position.z);

        Gizmos.DrawLine(LeftTop, RightTop);
        Gizmos.DrawLine(LeftTop, LeftBottom);
        Gizmos.DrawLine(RightTop, RightBottom);
        Gizmos.DrawLine(LeftBottom, RightBottom);
    }
}
