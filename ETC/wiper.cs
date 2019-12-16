using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wiper : MonoBehaviour
{

    public float speed;
    public float max;
    public GameObject m_wiper;
    public float offsetz;
    // Start is called before the first frame update
    void Start()
    {
        offsetz = transform.localEulerAngles.z;
    }

    private void FixedUpdate()
    {
        if (flag)
        {
            x += Time.deltaTime * speed;
            if (x > max)
                flag = false;
        }
        else
        {
            x -= Time.deltaTime * speed;
            if (x < 0)
                flag = true;
        }
        transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, x + offsetz);
        m_wiper.transform.localRotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z - 90);
    }

    bool flag = true;
    float x = 0;
    // Update is called once per frame
    void Update()
    {
        
    }
}
