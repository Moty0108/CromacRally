using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Runtime.InteropServices;
using System;

public class GForce : MonoBehaviour
{
    private static GForce _instnace = null;

    public static GForce Instance
    {
        get
        {
            if(!_instnace)
            {
                _instnace = GameObject.FindObjectOfType<GForce>();

                if(_instnace == null)
                {
                    Debug.Log("Singleton error");
                }
            }
            return _instnace;
        }
    }


    [Range(-15, 15)]
    public float x;
    [Range(-20, 20)]
    public float y;

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    struct DataPacket
    {
        [MarshalAs(UnmanagedType.I4)]
        public float temp1;
        [MarshalAs(UnmanagedType.I4)]
        public float temp2;
        [MarshalAs(UnmanagedType.I4)]
        public float temp3;
        [MarshalAs(UnmanagedType.I4)]
        public float temp4;
        [MarshalAs(UnmanagedType.I4)]
        public float temp5;
        [MarshalAs(UnmanagedType.I4)]
        public float temp6;
        [MarshalAs(UnmanagedType.I4)]
        public float temp7;
        [MarshalAs(UnmanagedType.I4)]
        public float y;
        [MarshalAs(UnmanagedType.I4)]
        public float x;
        [MarshalAs(UnmanagedType.I4)]
        public float temp8;
        [MarshalAs(UnmanagedType.I4)]
        public float temp9;
        [MarshalAs(UnmanagedType.I4)]
        public float temp10;
        [MarshalAs(UnmanagedType.I4)]
        public float temp11;
        [MarshalAs(UnmanagedType.I4)]
        public float temp12;
        [MarshalAs(UnmanagedType.I4)]
        public float temp13;
        [MarshalAs(UnmanagedType.I4)]
        public float temp14;

        public byte[] Serialize()
        {
            var buffer = new byte[Marshal.SizeOf(typeof(DataPacket))];

            var gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var pBuffer = gch.AddrOfPinnedObject();

            Marshal.StructureToPtr(this, pBuffer, false);
            gch.Free();

            return buffer;
        }
    }

    UdpClient m_client;
    public bool m_startEffect = false;
    public float m_speed;
    public float m_pivotX;
    public float m_pivotY;
    public float m_effectX = 0;
    public float m_effectY = 0;
    public float m_impactX = 0;
    public float m_impactY = 0;
    public float m_speedY = 0;
    public float m_effectRangeX;
    public float m_effectRangeY;
    Rigidbody m_playerRd;
    RVP.VehicleParent vp;


    // Start is called before the first frame update
    void Start()
    {
        m_client = new UdpClient();
        vp = GetComponent<RVP.VehicleParent>();
        m_playerRd = GamaManager.Instance.m_player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        //m_pivotX = vp.localVelocity.x;
        //m_pivotY = Mathf.Clamp(GetComponent<Rigidbody>().velocity.magnitude, 0, 15);

        if (m_startEffect)
        {
            SetEffect();
            //SpeedEffect();
        }



        x = m_pivotX + m_effectX + m_impactX;
        y = m_pivotY + m_effectY + m_impactY + m_speedY;

        if (m_startEffect)
        {
            SendCPP();
        }
    }

    bool flag = false;
    int sign = 1;

    public void SetEffectPower(float _speed, float _range)
    {
        m_speed = _speed;
        m_effectRangeX = _range;
    }
    public void SetEffectZero()
    {
        m_speed = 0;
        m_effectX = 0;
        m_effectY = 0;
        m_effectRangeX = 0;
        m_effectRangeY = 0;
    }

    public void SetEffect()
    {
        m_effectX += sign * m_speed * Time.deltaTime;

        if (flag)
        {
            sign = 1;
            if (m_effectX > m_effectRangeX)
                flag = false;
        }
        else
        {
            sign = -1;
            if (m_effectX < -m_effectRangeX)
                flag = true;
        }
    }

    int sign2 = 1;
    bool flag2 = false;
    
    public void SpeedEffect()
    {
        m_speedY += sign2 * 150f * Time.deltaTime;
        float max = (m_playerRd.velocity.magnitude * 3.6f * 10f) / 160f;

        if(flag2)
        {
            sign2 = 1;
            if(m_speedY > max)
            {
                flag2 = false;
                SendCPP();
            }
        }
        else
        {
            sign2 = -1;
            if (m_speedY < max)
            {
                flag2 = true;
                SendCPP();
            }
        }
    }

    public void SetZero()
    {
        x = 0;
        y = 0;
    }

    public void SetPivotY(float _y)
    {
        m_pivotY = _y;
    }

    // suspension에서 호출
    public void SetPivotX(float _x)
    {
        m_pivotX = _x;
    }

    public void SendCPP()
    {
        DataPacket temp = new DataPacket();
        if (y == 0)
            temp.x = 1;
        else
            temp.x = -y;

        if (x == 0)
            temp.y = 1;
        else
            temp.y = x;

        byte[] buffer = temp.Serialize();

        m_client.Send(buffer, sizeof(int) * 8 * 2, "127.0.0.1", 20777);

    }

    public void ImpactEffect()
    {
        if(m_startEffect)
            StartCoroutine(Impact());
    }
    
    // 충돌했을때 방향에 따른 충돌
    IEnumerator Impact()
    {

        //Debug.Log("의자 충돌 효과 실행!");
        m_impactX = UnityEngine.Random.Range(0, -1);
        m_impactY = UnityEngine.Random.Range(0, -1);

        for (int i = 0; i < 1; i++)
        {
            if (m_impactX < 0)
                m_impactX = UnityEngine.Random.Range(15, 5);
            else
                m_impactX = UnityEngine.Random.Range(-15, -5);

            if (m_impactY < 0)
                m_impactY = UnityEngine.Random.Range(15, 5);
            else
                m_impactY = UnityEngine.Random.Range(-15, -5);
            
            SendCPP();
            yield return new WaitForSeconds(0.2f);
        }

        m_impactX = 0;
        m_impactY = 0;
        SendCPP();
    }
}
