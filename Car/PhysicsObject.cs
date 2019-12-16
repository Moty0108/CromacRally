using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    bool isCrashed;
    // Start is called before the first frame update
    void Start()
    {
        isCrashed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isCrashed)
        {
            Destroy(gameObject, 10f);
        }
    }
}
