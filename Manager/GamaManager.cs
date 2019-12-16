using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamaManager : Singleton<GamaManager>
{
    public delegate void CounterFunction();
    public enum InputType
    {
        JOYSTICK, KEYBOARD
    }

    public int m_startTime;

    public TimeMode m_mode;
    public int m_carIndex;
    
    public GameTimerManager m_timeManager;
    public Vector3 m_respawnPoint;
    public Transform m_player;
    public Transform m_uiCanvas;
    public InputType m_inputType = InputType.KEYBOARD;
    public Counter counter;
    public DisplayBoard m_displayBoard;

    public CarData[] m_car;

    CarInputManager m_inputManager;
    RVP.BasicInput m_basicInput;

    int m_countTime = 0;

    public bool m_isPlaying = false;

    public bool m_debugMode = true;
    public bool m_noAnimation = false;
    public Recoder m_recoder;
    Ghost m_currentGhost;
    public GameObject[] m_ghostObject;

    // Start is called before the first frame update
    void Awake()
    {
        

        for (int i = 0; i < m_car.Length; i++)
        {
            m_car[i].m_car.SetActive(false);
        }

        if (!m_debugMode)
        {
            m_mode = MapCarSelectData.Instance.loadMapData.timeMode;
            m_carIndex = MapCarSelectData.Instance.loadMapData.car;
        }

        //if(GameObject.Find("CM vcam1"))
        //    GameObject.Find("CM vcam1").GetComponent<Cinemachine.CinemachineVirtualCamera>().m_LookAt = m_cars[Mathf.Clamp(m_carIndex, 0, m_cars.Length)].transform;

        //m_cars[Mathf.Clamp(m_carIndex, 0, m_cars.Length)].SetActive(true);
        //m_player.name = "Drift Car";
        //m_player = m_cars[Mathf.Clamp(m_carIndex, 0, m_cars.Length)].transform;

        

        if (LoadCarStatusData.Instance)
        {
            m_player = LoadCarStatusData.Instance.CreateCar(m_carIndex).transform;


            m_player.transform.position = Vector3.zero;
            m_player.gameObject.SetActive(true);


            m_inputManager = m_player.GetComponent<CarInputManager>();
            m_basicInput = m_player.GetComponent<RVP.BasicInput>();
            m_inputManager.enabled = false;
            m_basicInput.enabled = false;
        }
        else
        {

            if (m_car[m_carIndex] != null)
            {
                m_car[m_carIndex].m_car.SetActive(true);
                m_player = m_car[m_carIndex].m_car.transform;
                m_player.name = "Drift Car";
                m_car[m_carIndex].SetCarStatus();

                m_inputManager = m_player.GetComponent<CarInputManager>();
                m_basicInput = m_player.GetComponent<RVP.BasicInput>();
                m_inputManager.enabled = false;
                m_basicInput.enabled = false;
            }
        }



        Respawn();

        if(m_debugMode)
        {
            m_basicInput.enabled = true;
        }
    }

    private void Start()
    {
        switch (m_mode)
        {
            case TimeMode.GrandPrix:
                m_recoder = new Recoder(this);
                m_currentGhost = m_recoder.Load(SceneManager.GetActiveScene().name);

                foreach (GameObject obj in TrackManager.instance.m_points)
                {
                    if (obj.GetComponent<CheckPointClass>().m_pointType == CheckPointClass.CheckPointType.CHECKPOINT)
                    {
                        foreach (MeshRenderer mesh in obj.GetComponentsInChildren<MeshRenderer>())
                        {
                            mesh.enabled = false;
                        }
                    }
                }

                m_uiCanvas.GetComponentInChildren<UIRemainDistance>().gameObject.SetActive(false);
                if (m_uiCanvas.GetComponentInChildren<UITimer>().m_timerType == UITimer.TimerType.REMAIN)
                {
                    m_uiCanvas.GetComponentInChildren<UITimer>().gameObject.SetActive(false);
                }
                m_uiCanvas.GetComponentInChildren<UIAddTime>().gameObject.SetActive(false);
                GameTimerManager.Instance.m_maxTime = 99999f;

                break;

            case TimeMode.Time:
                break;
        }

        if(m_noAnimation)
            StartCoroutine(Counter(m_startTime, StartGame));
    }

    // Update is called once per frame
    void Update()
    {

        if(!m_isPlaying)
        {
            m_player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            m_player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
       

        if(Input.GetKeyDown(KeyCode.H))
        {
            m_recoder.Active();
        }

        if(Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            //Respawn(TrackManager.Instance.m_points[TrackManager.Instance.m_curPointNum].transform.position, TrackManager.Instance.m_points[(TrackManager.Instance.m_curPointNum + 1) % TrackManager.instance.m_points.Count].transform, TrackManager.Instance.m_points[TrackManager.Instance.m_curPointNum].GetComponent<CheckPointClass>().m_subtractTimeAmount);
            FadeInOut.Instance.StartFadeIO(InputRespawn);            
        }

        if(Input.GetKeyDown(KeyCode.J))
        {
            EndGame(false);
        }
    }

    void InputRespawn()
    {
        GameTimerManager.Instance.AddCurrentTime(5);
        StartCoroutine(Respawn(TrackManager.Instance.m_points[TrackManager.Instance.m_curPointNum].transform.position, TrackManager.Instance.m_points[(TrackManager.Instance.m_curPointNum + 1) % TrackManager.instance.m_points.Count].transform, TrackManager.Instance.m_points[TrackManager.Instance.m_curPointNum].GetComponent<CheckPointClass>().m_subtractTimeAmount));
    }

    public void StartGame()
    {
        GForce.Instance.m_startEffect = true;
        ForceFeedbackTest.Instance.isEffect = false;
        //Debug.Log("StartGame()");
        m_isPlaying = false;
        m_inputManager.enabled = false;
        m_basicInput.enabled = false;
        m_timeManager.StopTimer();
        Respawn();
        counter.StartEffect(InitGame, 3);
        if(m_displayBoard != null)
            m_displayBoard.StartEffect();
    }

    public void EndGame(bool finish)
    {
        if (m_recoder != null)
            m_recoder.EndRecoard(finish);
        //Debug.Log("게임 끝!");
        m_inputManager.enabled = false;
        m_basicInput.enabled = false;
        // 의자 및 핸들
        GForce.Instance.m_startEffect = false;
        GForce.Instance.SetEffectZero();
        ForceFeedbackTest.Instance.isEffect = false;
        //ForceFeedbackTest.Instance.forceFeedbackEnabled = false;
        //ForceFeedbackTest.Instance.ResetForceFeedback();
        //m_player.gameObject.SetActive(false);

        foreach(AudioSource source in m_player.GetComponentsInChildren<AudioSource>())
        {
            source.enabled = false;
        }
        SoundManager.Instance.gameObject.SetActive(false);
        m_player.GetComponent<ImpactSystem>().enabled = false;
        Destroy(m_player.GetComponent<DurabilitySystem>());//.enabled = false;
        TrackManager.Instance.gameObject.SetActive(false);

        

        GameTimerManager.Instance.StopTimer();
        if (finish)
        {

            m_player.GetComponent<CameraController>().enabled = false;
            StartCoroutine(Counter(3, Finish));
        }
        else
            StartCoroutine(NoFinish());
    }

    IEnumerator NoFinish()
    {
        int totalTime = (GameTimerManager.instance.m_curMinTime * 60 + GameTimerManager.Instance.m_curSecTime)
            * 100 + GameTimerManager.Instance.m_curMSecTime;
        BestRecord record = new BestRecord("", GameTimerManager.Instance.m_curMinTime,
            GameTimerManager.Instance.m_curSecTime, GameTimerManager.Instance.m_curMSecTime, totalTime);

        //UIEvent.Result(GameResult.RETIRE, record);
        while (true)
        {

            m_player.GetComponent<RVP.VehicleParent>().SetEbrake(1);
            if(m_player.GetComponent<Rigidbody>().velocity.magnitude < 1f)
            {
                m_isPlaying = false;
                UIEvent.Result(GameResult.RETIRE, record);
                break;
            }

            yield return null;
        }
    }

    public void Finish()
    {
        
        //Debug.Log("Result 호출!");
        int totalTime = (GameTimerManager.instance.m_curMinTime * 60 + GameTimerManager.Instance.m_curSecTime)
            * 100 + GameTimerManager.Instance.m_curMSecTime;
        m_isPlaying = false;
        BestRecord record = new BestRecord("", GameTimerManager.Instance.m_curMinTime,
            GameTimerManager.Instance.m_curSecTime, GameTimerManager.Instance.m_curMSecTime, totalTime);
        UIEvent.Result(GameResult.FINISH, record);
    }


    [ContextMenu("StartGame")]
    public void InitGame()
    {
        // LoadCarStatusData.Instance.m_cars[m_currentGhost.carIndex].m_createdObject

        if (m_recoder != null)
        {
            if (m_currentGhost != null)
                m_recoder.PlayRecord(m_ghostObject[m_currentGhost.carIndex], m_currentGhost);

            m_recoder.StartRecoard(m_player.transform, m_carIndex);
        }

        m_timeManager.ResetTimer();
        m_timeManager.StartTimer();
        ForceFeedbackTest.Instance.isEffect = true;
        GForce.Instance.m_startEffect = true;
        m_isPlaying = true;
        switch(m_inputType)
        {
            case InputType.JOYSTICK:
                m_inputManager.enabled = true;
                break;
            case InputType.KEYBOARD:
                m_basicInput.enabled = true;
                break;
        }
    }

    [ContextMenu("Respawn")]
    public void Respawn()
    {
        if (TrackManager.Instance != null)
        {
            m_player.position = TrackManager.Instance.m_points[0].transform.position + Vector3.down * 0.8f;
            m_player.transform.LookAt(TrackManager.Instance.m_points[1].transform);
        }
        else
            m_player.position = m_respawnPoint;

        m_player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        m_player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        //m_player.transform.eulerAngles = new Vector3(0, 0, 0);
        m_player.transform.Translate(Vector3.up, Space.World);
        m_player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        m_player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        
    }

    public void Respawn(Vector3 spawnPos, float rotateY)
    {
        m_player.position = spawnPos + Vector3.down * 0.8f;
        m_player.transform.eulerAngles = new Vector3(0, rotateY, 0);
        m_player.transform.Translate(Vector3.up, Space.World);
        m_player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        m_player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        m_timeManager.Subtract();
        
    }

    public IEnumerator Respawn(Vector3 spawnPos, Transform target, float subtractTime)
    {
        m_player.position = spawnPos + Vector3.down * 0.8f;
        //m_player.transform.eulerAngles = new Vector3(0, rotateY, 0);
        yield return new WaitForFixedUpdate();
        m_player.transform.LookAt(target);
        m_player.transform.Translate(Vector3.up, Space.World);
        m_player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        m_player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        m_timeManager.Subtract(subtractTime);   
    }

    public IEnumerator Counter(CounterFunction func)
    {
        m_countTime = 3;
        yield return new WaitForSecondsRealtime(1);
        m_countTime = 2;
        yield return new WaitForSecondsRealtime(1);
        m_countTime = 1;
        yield return new WaitForSecondsRealtime(1);
        m_countTime = 0;

        func();
        yield return null;
    }

    public IEnumerator Counter(int time, CounterFunction func)
    {
        for(int i =0; i<time; i++)
        {
            yield return new WaitForSecondsRealtime(1);
        }

        func();
        yield return null;
    }
}
