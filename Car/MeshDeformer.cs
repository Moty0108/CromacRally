using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class MeshDeformer : MonoBehaviour
{
    Mesh deformingMesh;
    Vector3[] originalVertices, displacedVertices;
    Vector3[] vertexVelocities;
    public GameObject m_targetMesh;
    public GameObject m_targetCollider;

    // Start is called before the first frame update
    void Start()
    {
        deformingMesh = Instantiate(m_targetMesh.GetComponent<MeshFilter>().mesh) as Mesh;

        originalVertices = deformingMesh.vertices;
        displacedVertices = deformingMesh.vertices;
        vertexVelocities = new Vector3[originalVertices.Length];
        //GetComponent<MeshFilter>().mesh = deformingMesh;
        m_targetMesh.GetComponent<MeshFilter>().mesh = deformingMesh;
        m_targetCollider.GetComponent<MeshCollider>().sharedMesh = deformingMesh;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void AddDeformingForce(Vector3 point, float force)
    {
        

        Stopwatch sw = new Stopwatch();
        sw.Reset();
        sw.Start();

        UnityEngine.Debug.DrawLine(Camera.main.transform.position, point);
        point = transform.InverseTransformPoint(point);

        //UnityEngine.Debug.Log(displacedVertices.Length);
        Vector3 pointToVertex;
        float attenuatedForce;

        

        for (int i = 0;i <displacedVertices.Length;i++)
        {
            pointToVertex = displacedVertices[i] - point;
            attenuatedForce = force * Mathf.Cos(5 * Mathf.Clamp(pointToVertex.magnitude, 0, Mathf.PI / 10));
            vertexVelocities[i] = pointToVertex.normalized * attenuatedForce * 0.01f;


            displacedVertices[i] += vertexVelocities[i];
        }


        //for (int i = 0; i < displacedVertices.Length; i++)
        //{
        //    displacedVertices[i] += vertexVelocities[i];
        //}

        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals();

        sw.Stop();
        //UnityEngine.Debug.Log( "실행시간 : " + (float)sw.ElapsedMilliseconds / 1000f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        AddDeformingForce(collision.contacts[0].point - collision.contacts[0].normal * 0.1f, 10f);
    }
}
