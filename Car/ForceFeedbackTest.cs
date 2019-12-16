using System.Runtime.InteropServices;
using UnityEngine;
using enums;

/// <summary>
/// Maschine simulations test of DirectInput force feedback
/// by Cameron Bonde ('Vectrex' on the forums)
/// www.kartsim.com
/// www.maschinesimulations.com
/// 
/// nb. Use at your own risk. So if your haptics enabled cat-suit throttles you, don't blame us ;)
/// </summary>
public class ForceFeedbackTest : Singleton<ForceFeedbackTest> {



    // Import system function to get the current window handle, which DirectInput needs (for no good reason I can think of)
#if UNITY_STANDALONE_WIN
    private int force;
    public bool forceFeedbackEnabled = false;
    public bool isEffect = false;

	[DllImport("user32")]
	private static extern int GetForegroundWindow();

	// Import functions from DirectInput c++ wrapper dll
	[DllImport("UnityForceFeedback")]
	private static extern int InitDirectInput();

	[DllImport("UnityForceFeedback")]
	private static extern int InitForceFeedback(int HWND);

	[DllImport("UnityForceFeedback")]
	private static extern int DetectForceFeedbackDevice();

	[DllImport("UnityForceFeedback")]
	private static extern int SetDeviceForcesXY(int x, int y);

	[DllImport("UnityForceFeedback")]
	private static extern bool StartEffect();

	[DllImport("UnityForceFeedback")]
	private static extern bool StopEffect();

	[DllImport("UnityForceFeedback")]
	public static extern bool SetAutoCenter(bool autoCentre);

	[DllImport("UnityForceFeedback")]
	private static extern void FreeForceFeedback();

	[DllImport("UnityForceFeedback")]
	private static extern void FreeDirectInput();

	public void Awake()
	{
        if(instance != null)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

		InitDirectInput();

        if (DetectForceFeedbackDevice() >= 0 && !forceFeedbackEnabled)
        {
            InitialiseForceFeedback();
            forceFeedbackEnabled = true;
        }
    }

	public void ResetForceFeedback()
	{
		FreeForceFeedback();
		forceFeedbackEnabled = false;
	}

	public void Update()
	{


        if (forceFeedbackEnabled)
        {
            //int result = DetectForceFeedbackDevice();
            //if (result < 0)
            //{
            //    // If device disconnected (currently doesn't work)
            //    //Debug.LogWarning("ForceFeedback device disconnected");
            //    StopEffect();
            //    FreeForceFeedback();
            //    forceFeedbackEnabled = false;
            //}
            //force = -(int)(Input.GetAxis("Vertical") * 10000f);
            //SetDeviceForcesXY(force, 0); // You might need to set force on the Y depending on your device. Also, be carefull with the update rate of new force data.
            
        }

        if(isEffect)
        {
            ForceWeak();
        }

    }

    float m_speed = 0, m_maxsteering = 0;
    public float m_curSteering = 0;
    bool flag = false;
    public void ForceWeak()
    {
            if (flag)
            {
                m_curSteering += m_speed * Time.deltaTime;
                if (m_curSteering > m_maxsteering)
                    flag = false;
            }
            else
            {
                m_curSteering -= m_speed * Time.deltaTime;
                if (m_curSteering < -m_maxsteering)
                    flag = true;
            }

            SetDeviceForcesXY((int)(m_curSteering * 10000), 0);
    }

    public void SetSteeringCenter()
    {
        if (forceFeedbackEnabled)
        {
            m_speed = 0;
            m_maxsteering = 0;
            m_curSteering = 0;
        }
    }

    public void SetForce(float spped, float maxsteering)
    {
        m_speed = spped;
        m_maxsteering = maxsteering;
    }

	public void OnApplicationQuit()
	{
		ShutDownForceFeedback();
	}

	private void InitialiseForceFeedback()
	{
		if (forceFeedbackEnabled)
		{
			//Debug.Log("WARNING: Force feedback attempted to initialise but was aleady running!");
			return;
		}
		int hwnd = GetForegroundWindow();
		print("Window HWND = " + hwnd);
		if (InitForceFeedback(hwnd) >= 0)
		{
			StartEffect();
			forceFeedbackEnabled = true;
			SetAutoCenter(false);
		}
	}

	private void ShutDownForceFeedback()
	{
		if (forceFeedbackEnabled)
		{
			StopEffect();
			FreeDirectInput();
		}
	}

	//public void OnGUI()
	//{
	//	if (forceFeedbackEnabled)
	//	{
	//		GUILayout.Label("Press up/down to test force amounts (This is hardcoded to one axis at the moment)");
	//		GUILayout.Space(50);
	//		GUILayout.Label("Force = " + force);
	//		GUILayout.Label("Wheel = " + Input.GetAxis("Horizontal"));
	//	}
	//	else
	//	{
	//		GUILayout.Label("No Force Feedback device detected.");
	//	}
	//	if (GUILayout.Button("Reset Force Feedback"))
	//	{
	//		ResetForceFeedback();
	//	}
	//}

	//public void OnApplicationFocus(bool focused)
	//{
	//	if(!focused)
	//	{
	//		//ResetForceFeedback();
	//	}
	//}
#else
	public void OnGUI()
	{
		GUILayout.Label("Silly rabbit! Force Feedback plugin is for Windows! :(");
	}
#endif
}
