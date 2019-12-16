using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rainparticle : MonoBehaviour
{
    RVP.VehicleParent vp;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        vp = GameObject.Find("Drift Car").GetComponent<RVP.VehicleParent>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(new Vector2(-vp.localAngularVel.y * 5, vp.localVelocity.z * 0.1f));
    }
}
