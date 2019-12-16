using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CombineMesh : MonoBehaviour
{
    public MeshFilter[] meshFilters;

    // Start is called before the first frame update
    void Start()
    {
        //MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for(int i = 0; i<meshFilters.Length;i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        //Debug.Log(b.Count);
        
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.GetComponent<MeshRenderer>().sharedMaterial = meshFilters[0].GetComponent<MeshRenderer>().sharedMaterial;
        transform.gameObject.SetActive(true);
    }



    //[ContextMenu("SaveMesh")]
    //public void saveMesh()
    //{
    //    CombineInstance[] combine = new CombineInstance[meshFilters.Length];

    //    for (int i = 0; i < meshFilters.Length; i++)
    //    {
    //        combine[i].mesh = meshFilters[i].sharedMesh;
    //        combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
    //        //meshFilters[i].gameObject.SetActive(false);
    //    }


    //    Mesh mesh = new Mesh();
    //    mesh.CombineMeshes(combine);

    //    //transform.GetComponent<MeshRenderer>().sharedMaterial = meshFilters[0].GetComponent<MeshRenderer>().sharedMaterial;
    //    //transform.gameObject.SetActive(true);

    //    AssetDatabase.CreateAsset(mesh, "Assets/CreatedMesh" + transform.name + ".asset");
    //    AssetDatabase.SaveAssets();
    //}
}
