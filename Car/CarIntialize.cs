using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RVP;

public class CarIntialize : MonoBehaviour
{
    public Mesh rimMesh;
    public Mesh tireMesh;
    public Transform suspensionFLPos;
    public Transform suspensionFRPos;
    public Transform suspensionRLPos;
    public Transform suspensionRRPos;

    public float tireWidth, tireradius;
    public Gear[] gears;

    public float downforce;

    public GameObject m_model;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        gameObject.name = "Drift Car";

        VehicleAssist va = CopyComponent<VehicleAssist>(m_model.GetComponent<VehicleAssist>(), gameObject);
        va.downforce = downforce;

        GameObject engine = new GameObject("engine");
        engine.transform.SetParent(transform);
        engine.transform.localPosition = Vector3.zero;
        engine.transform.localRotation = Quaternion.identity;
        engine.transform.localScale = Vector3.one;

        //GasMotor gasmotor = engine.AddComponent<GasMotor>();
        engine.AddComponent<AudioSource>();
        GasMotor gasmotor = CopyComponent<GasMotor>(m_model.GetComponentInChildren<GasMotor>(), engine);

        //gasmotor.ignition = true;
        //gasmotor.power = 1;
        //gasmotor.inputCurve = m_model.GetComponentInChildren<GasMotor>().inputCurve;
        //gasmotor.minPitch = 0.5f;
        //gasmotor.maxPitch = 1.5f;
        //gasmotor.canBoost = false;
        //gasmotor.strength = 1;
        //gasmotor.damagePitchWiggle = 0;
        //gasmotor.torqueCurve = m_model.GetComponentInChildren<GasMotor>().torqueCurve;
        //gasmotor.inertia = 0.8f;
        //gasmotor.pitchIncreaseBetweenShift = true;

        GameObject steeringwheel = new GameObject("steeringwheel");
        steeringwheel.transform.SetParent(transform);
        steeringwheel.transform.localPosition = Vector3.zero;
        steeringwheel.transform.localRotation = Quaternion.identity;
        steeringwheel.transform.localScale = Vector3.one;

        SteeringControl steeringControl = CopyComponent<SteeringControl>(m_model.GetComponentInChildren<SteeringControl>(), steeringwheel);

        GameObject transmission = new GameObject("transmission");
        transmission.transform.SetParent(transform);
        transmission.transform.localPosition = Vector3.zero;
        transmission.transform.localRotation = Quaternion.identity;
        transmission.transform.localScale = Vector3.one;

        GearboxTransmission gearbox = CopyComponent<GearboxTransmission>(m_model.GetComponentInChildren<GearboxTransmission>(), transmission);
        gearbox.gears = new Gear[m_model.GetComponentInChildren<GearboxTransmission>().gears.Length];
        gearbox.gears = m_model.GetComponentInChildren<GearboxTransmission>().gears;
        engine.GetComponent<GasMotor>().transmission = transmission.GetComponent<GearboxTransmission>();

        Transform[] temp;
        Suspension susFL = null;
        Suspension susFR = null;
        Suspension susRL = null;
        Suspension susRR = null;
        Wheel wFL = null;
        Wheel wFR = null;
        Wheel wRL = null;
        Wheel wRR = null;

        temp = m_model.GetComponentsInChildren<Transform>();

        foreach (Transform t in temp)
        {
            if (t.name == "suspensionFL")
            {
                susFL = t.GetComponent<Suspension>();
                wFL = t.GetComponentInChildren<Wheel>();
            }
            if (t.name == "suspensionFR")
            {
                susFR = t.GetComponent<Suspension>();
                wFR = t.GetComponentInChildren<Wheel>();
            }
            if (t.name == "suspensionRL")
            {
                susRL = t.GetComponent<Suspension>();
                wRL = t.GetComponentInChildren<Wheel>();
            }
            if (t.name == "suspensionRR")
            {
                susRR = t.GetComponent<Suspension>();
                wRR = t.GetComponentInChildren<Wheel>();
            }
        }

        // --------------------------------------------------------------------------------
        GameObject suspensionFL = new GameObject("suspensionFL");
        suspensionFL.transform.SetParent(transform);
        suspensionFL.transform.localPosition = suspensionFLPos.transform.localPosition;
        suspensionFL.transform.localRotation = Quaternion.Euler(0, -90, 0);
        suspensionFL.transform.localScale = Vector3.one;

        CopyComponent<Suspension>(susFL, suspensionFL);

        

        GameObject wheel = new GameObject("wheel");
        wheel.transform.SetParent(suspensionFL.transform);
        wheel.transform.localPosition = Vector3.zero;
        wheel.transform.localRotation = Quaternion.identity;
        wheel.transform.localScale = Vector3.one;

        wheel.AddComponent<Wheel>();
        

        suspensionFL.GetComponent<Suspension>().wheel = wheel.GetComponent<Wheel>();
        suspensionFL.GetComponent<Suspension>().oppositeWheel = suspensionFL.GetComponent<Suspension>();

        GameObject rim = new GameObject("rim");
        rim.transform.SetParent(wheel.transform);
        rim.transform.localPosition = Vector3.zero;
        rim.transform.localRotation = Quaternion.identity;
        rim.transform.localScale = Vector3.one;
        rim.AddComponent<MeshFilter>();
        rim.GetComponent<MeshFilter>().sharedMesh = rimMesh;
        rim.AddComponent<MeshRenderer>();

        wheel.GetComponent<Wheel>().GetWheelDimensions(tireradius, tireWidth);


        rim.SetActive(false);

        GameObject tire = new GameObject("tire");
        tire.transform.SetParent(rim.transform);
        tire.transform.localPosition = Vector3.zero;
        tire.transform.localRotation = Quaternion.identity;
        tire.transform.localScale = Vector3.one;
        tire.AddComponent<MeshFilter>();
        tire.GetComponent<MeshFilter>().sharedMesh = tireMesh;
        tire.AddComponent<MeshRenderer>();

        wheel.GetComponent<Wheel>().feedbackRpmBias = wFL.feedbackRpmBias;
        wheel.GetComponent<Wheel>().rpmBiasCurve = wFL.rpmBiasCurve;
        wheel.GetComponent<Wheel>().rpmBiasCurveLimit = wFL.rpmBiasCurveLimit;
        wheel.GetComponent<Wheel>().axleFriction = wFL.axleFriction;
        wheel.GetComponent<Wheel>().frictionSmoothness = wFL.frictionSmoothness;
        wheel.GetComponent<Wheel>().forwardFriction = wFL.forwardFriction;
        wheel.GetComponent<Wheel>().sidewaysFriction = wFL.sidewaysFriction;
        wheel.GetComponent<Wheel>().forwardRimFriction = wFL.forwardRimFriction;
        wheel.GetComponent<Wheel>().sidewaysRimFriction = wFL.sidewaysRimFriction;
        wheel.GetComponent<Wheel>().forwardCurveStretch = wFL.forwardCurveStretch;
        wheel.GetComponent<Wheel>().sidewaysCurveStretch = wFL.sidewaysCurveStretch;
        wheel.GetComponent<Wheel>().forwardFrictionCurve = wFL.forwardFrictionCurve;
        wheel.GetComponent<Wheel>().sidewaysFrictionCurve = wFL.sidewaysFrictionCurve;
        wheel.GetComponent<Wheel>().slipDependence = wFL.slipDependence;
        wheel.GetComponent<Wheel>().forwardSlipDependence = wFL.forwardSlipDependence;
        wheel.GetComponent<Wheel>().sidewaysSlipDependence = wFL.sidewaysSlipDependence;
        wheel.GetComponent<Wheel>().normalFrictionCurve = wFL.normalFrictionCurve;
        wheel.GetComponent<Wheel>().compressionFrictionFactor = wFL.compressionFrictionFactor;
        wheel.GetComponent<Wheel>().tirePressure = wFL.tirePressure;
        wheel.GetComponent<Wheel>().rimGlow = wFL.rimGlow;





        //--------------------------------------------------------------------------------
        GameObject suspensionFR = new GameObject("suspensionFR");
        suspensionFR.transform.SetParent(transform);
        suspensionFR.transform.localPosition = suspensionFRPos.transform.localPosition;
        suspensionFR.transform.localRotation = Quaternion.Euler(0, 90, 0);
        suspensionFR.transform.localScale = Vector3.one;

        CopyComponent<Suspension>(susFR, suspensionFR);

        GameObject wheelFR = new GameObject("wheel");
        wheelFR.transform.SetParent(suspensionFR.transform);
        wheelFR.transform.localPosition = Vector3.zero;
        wheelFR.transform.localRotation = Quaternion.identity;
        wheelFR.transform.localScale = Vector3.one;
        wheelFR.AddComponent<Wheel>();
        
        suspensionFR.GetComponent<Suspension>().wheel = wheelFR.GetComponent<Wheel>();
        suspensionFR.GetComponent<Suspension>().oppositeWheel = suspensionFR.GetComponent<Suspension>();

        GameObject rimFR = new GameObject("rim");
        rimFR.transform.SetParent(wheelFR.transform);
        rimFR.transform.localPosition = Vector3.zero;
        rimFR.transform.localRotation = Quaternion.identity;
        rimFR.transform.localScale = Vector3.one;
        rimFR.AddComponent<MeshFilter>();
        rimFR.GetComponent<MeshFilter>().sharedMesh = rimMesh;
        rimFR.AddComponent<MeshRenderer>();

        wheelFR.GetComponent<Wheel>().GetWheelDimensions(tireradius, tireWidth);

        rimFR.SetActive(false);

        GameObject tireFR = new GameObject("tire");
        tireFR.transform.SetParent(rimFR.transform);
        tireFR.transform.localPosition = Vector3.zero;
        tireFR.transform.localRotation = Quaternion.identity;
        tireFR.transform.localScale = Vector3.one;

        wheelFR.GetComponent<Wheel>().feedbackRpmBias = wFR.feedbackRpmBias;
        wheelFR.GetComponent<Wheel>().rpmBiasCurve = wFR.rpmBiasCurve;
        wheelFR.GetComponent<Wheel>().rpmBiasCurveLimit = wFR.rpmBiasCurveLimit;
        wheelFR.GetComponent<Wheel>().axleFriction = wFR.axleFriction;
        wheelFR.GetComponent<Wheel>().frictionSmoothness = wFR.frictionSmoothness;
        wheelFR.GetComponent<Wheel>().forwardFriction = wFR.forwardFriction;
        wheelFR.GetComponent<Wheel>().sidewaysFriction = wFR.sidewaysFriction;
        wheelFR.GetComponent<Wheel>().forwardRimFriction = wFR.forwardRimFriction;
        wheelFR.GetComponent<Wheel>().sidewaysRimFriction = wFR.sidewaysRimFriction;
        wheelFR.GetComponent<Wheel>().forwardCurveStretch = wFR.forwardCurveStretch;
        wheelFR.GetComponent<Wheel>().sidewaysCurveStretch = wFR.sidewaysCurveStretch;
        wheelFR.GetComponent<Wheel>().forwardFrictionCurve = wFR.forwardFrictionCurve;
        wheelFR.GetComponent<Wheel>().sidewaysFrictionCurve = wFR.sidewaysFrictionCurve;
        wheelFR.GetComponent<Wheel>().slipDependence = wFR.slipDependence;
        wheelFR.GetComponent<Wheel>().forwardSlipDependence = wFR.forwardSlipDependence;
        wheelFR.GetComponent<Wheel>().sidewaysSlipDependence = wFR.sidewaysSlipDependence;
        wheelFR.GetComponent<Wheel>().normalFrictionCurve = wFR.normalFrictionCurve;
        wheelFR.GetComponent<Wheel>().compressionFrictionFactor = wFR.compressionFrictionFactor;
        wheelFR.GetComponent<Wheel>().tirePressure = wFR.tirePressure;
        wheelFR.GetComponent<Wheel>().rimGlow = wFR.rimGlow;

        // --------------------------------------------------------------------------------
        GameObject suspensionRL = new GameObject("suspensionRL");
        suspensionRL.transform.SetParent(transform);
        suspensionRL.transform.localPosition = suspensionRLPos.transform.localPosition;
        suspensionRL.transform.localRotation = Quaternion.Euler(0, -90, 0);
        suspensionRL.transform.localScale = Vector3.one;

        CopyComponent<Suspension>(susRL, suspensionRL);

        GameObject wheelRL = new GameObject("wheel");
        wheelRL.transform.SetParent(suspensionRL.transform);
        wheelRL.transform.localPosition = Vector3.zero;
        wheelRL.transform.localRotation = Quaternion.identity;
        wheelRL.transform.localScale = Vector3.one;
        wheelRL.AddComponent<Wheel>();
        
        suspensionRL.GetComponent<Suspension>().wheel = wheelRL.GetComponent<Wheel>();
        suspensionRL.GetComponent<Suspension>().oppositeWheel = suspensionRL.GetComponent<Suspension>();

        GameObject rimRL = new GameObject("rim");
        rimRL.transform.SetParent(wheelRL.transform);
        rimRL.transform.localPosition = Vector3.zero;
        rimRL.transform.localRotation = Quaternion.identity;
        rimRL.transform.localScale = Vector3.one;
        rimRL.AddComponent<MeshFilter>();
        rimRL.GetComponent<MeshFilter>().sharedMesh = rimMesh;
        rimRL.AddComponent<MeshRenderer>();

        wheelRL.GetComponent<Wheel>().GetWheelDimensions(tireradius, tireWidth);

        rimRL.SetActive(false);

        GameObject tireRL = new GameObject("tire");
        tireRL.transform.SetParent(rimRL.transform);
        tireRL.transform.localPosition = Vector3.zero;
        tireRL.transform.localRotation = Quaternion.identity;
        tireRL.transform.localScale = Vector3.one;

        wheelRL.GetComponent<Wheel>().feedbackRpmBias = wRL.feedbackRpmBias;
        wheelRL.GetComponent<Wheel>().rpmBiasCurve = wRL.rpmBiasCurve;
        wheelRL.GetComponent<Wheel>().rpmBiasCurveLimit = wRL.rpmBiasCurveLimit;
        wheelRL.GetComponent<Wheel>().axleFriction = wRL.axleFriction;
        wheelRL.GetComponent<Wheel>().frictionSmoothness = wRL.frictionSmoothness;
        wheelRL.GetComponent<Wheel>().forwardFriction = wRL.forwardFriction;
        wheelRL.GetComponent<Wheel>().sidewaysFriction = wRL.sidewaysFriction;
        wheelRL.GetComponent<Wheel>().forwardRimFriction = wRL.forwardRimFriction;
        wheelRL.GetComponent<Wheel>().sidewaysRimFriction = wRL.sidewaysRimFriction;
        wheelRL.GetComponent<Wheel>().forwardCurveStretch = wRL.forwardCurveStretch;
        wheelRL.GetComponent<Wheel>().sidewaysCurveStretch = wRL.sidewaysCurveStretch;
        wheelRL.GetComponent<Wheel>().forwardFrictionCurve = wRL.forwardFrictionCurve;
        wheelRL.GetComponent<Wheel>().sidewaysFrictionCurve = wRL.sidewaysFrictionCurve;
        wheelRL.GetComponent<Wheel>().slipDependence = wRL.slipDependence;
        wheelRL.GetComponent<Wheel>().forwardSlipDependence = wRL.forwardSlipDependence;
        wheelRL.GetComponent<Wheel>().sidewaysSlipDependence = wRL.sidewaysSlipDependence;
        wheelRL.GetComponent<Wheel>().normalFrictionCurve = wRL.normalFrictionCurve;
        wheelRL.GetComponent<Wheel>().compressionFrictionFactor = wRL.compressionFrictionFactor;
        wheelRL.GetComponent<Wheel>().tirePressure = wRL.tirePressure;
        wheelRL.GetComponent<Wheel>().rimGlow = wRL.rimGlow;

        // --------------------------------------------------------------------------------
        GameObject suspensionRR = new GameObject("suspensionRR");
        suspensionRR.transform.SetParent(transform);
        suspensionRR.transform.localPosition = suspensionRRPos.transform.localPosition;
        suspensionRR.transform.localRotation = Quaternion.Euler(0, 90, 0);
        suspensionRR.transform.localScale = Vector3.one;

        CopyComponent<Suspension>(susRR, suspensionRR);

        GameObject wheelRR = new GameObject("wheel");
        wheelRR.transform.SetParent(suspensionRR.transform);
        wheelRR.transform.localPosition = Vector3.zero;
        wheelRR.transform.localRotation = Quaternion.identity;
        wheelRR.transform.localScale = Vector3.one;
        wheelRR.AddComponent<Wheel>();
        
        suspensionRR.GetComponent<Suspension>().wheel = wheelRR.GetComponent<Wheel>();
        suspensionRR.GetComponent<Suspension>().oppositeWheel = suspensionRR.GetComponent<Suspension>();

        GameObject rimRR = new GameObject("rim");
        rimRR.transform.SetParent(wheelRR.transform);
        rimRR.transform.localPosition = Vector3.zero;
        rimRR.transform.localRotation = Quaternion.identity;
        rimRR.transform.localScale = Vector3.one;
        rimRR.AddComponent<MeshFilter>();
        rimRR.GetComponent<MeshFilter>().sharedMesh = rimMesh;
        rimRR.AddComponent<MeshRenderer>();

        wheelRR.GetComponent<Wheel>().GetWheelDimensions(tireradius, tireWidth);

        rimRR.SetActive(false);

        GameObject tireRR = new GameObject("tire");
        tireRR.transform.SetParent(rimRR.transform);
        tireRR.transform.localPosition = Vector3.zero;
        tireRR.transform.localRotation = Quaternion.identity;
        tireRR.transform.localScale = Vector3.one;

        wheelRR.GetComponent<Wheel>().feedbackRpmBias = wRR.feedbackRpmBias;
        wheelRR.GetComponent<Wheel>().rpmBiasCurve = wRR.rpmBiasCurve;
        wheelRR.GetComponent<Wheel>().rpmBiasCurveLimit = wRR.rpmBiasCurveLimit;
        wheelRR.GetComponent<Wheel>().axleFriction = wRR.axleFriction;
        wheelRR.GetComponent<Wheel>().frictionSmoothness = wRR.frictionSmoothness;
        wheelRR.GetComponent<Wheel>().forwardFriction = wRR.forwardFriction;
        wheelRR.GetComponent<Wheel>().sidewaysFriction = wRR.sidewaysFriction;
        wheelRR.GetComponent<Wheel>().forwardRimFriction = wRR.forwardRimFriction;
        wheelRR.GetComponent<Wheel>().sidewaysRimFriction = wRR.sidewaysRimFriction;
        wheelRR.GetComponent<Wheel>().forwardCurveStretch = wRR.forwardCurveStretch;
        wheelRR.GetComponent<Wheel>().sidewaysCurveStretch = wRR.sidewaysCurveStretch;
        wheelRR.GetComponent<Wheel>().forwardFrictionCurve = wRR.forwardFrictionCurve;
        wheelRR.GetComponent<Wheel>().sidewaysFrictionCurve = wRR.sidewaysFrictionCurve;
        wheelRR.GetComponent<Wheel>().slipDependence = wRR.slipDependence;
        wheelRR.GetComponent<Wheel>().forwardSlipDependence = wRR.forwardSlipDependence;
        wheelRR.GetComponent<Wheel>().sidewaysSlipDependence = wRR.sidewaysSlipDependence;
        wheelRR.GetComponent<Wheel>().normalFrictionCurve = wRR.normalFrictionCurve;
        wheelRR.GetComponent<Wheel>().compressionFrictionFactor = wRR.compressionFrictionFactor;
        wheelRR.GetComponent<Wheel>().rimGlow = wRR.rimGlow;

        //--------------------------------------------------------------------------------------

        steeringwheel.GetComponent<SteeringControl>().steeredWheels = new Suspension[4];
        steeringwheel.GetComponent<SteeringControl>().steeredWheels[0] = suspensionRR.GetComponent<Suspension>();
        steeringwheel.GetComponent<SteeringControl>().steeredWheels[1] = suspensionFL.GetComponent<Suspension>();
        steeringwheel.GetComponent<SteeringControl>().steeredWheels[2] = suspensionFR.GetComponent<Suspension>();
        steeringwheel.GetComponent<SteeringControl>().steeredWheels[3] = suspensionRL.GetComponent<Suspension>();

        gearbox.outputDrives = new DriveForce[2];
        gearbox.outputDrives[0] = suspensionRR.GetComponent<DriveForce>();
        gearbox.outputDrives[1] = suspensionRL.GetComponent<DriveForce>();
        gearbox.automatic = true;

        gearbox.gears = gears;
        gearbox.CalculateRpmRanges();

        engine.GetComponent<GasMotor>().outputDrives = new DriveForce[1];
        engine.GetComponent<GasMotor>().outputDrives[0] = transmission.GetComponent<DriveForce>();


        //gameObject.AddComponent<Rigidbody>();

        //VehicleParent vp = CopyComponent<VehicleParent>(m_model.GetComponent<VehicleParent>(), gameObject);
        //vp.engine = GetComponentInChildren<GasMotor>();
        //vp.wheels = GetComponentsInChildren<Wheel>();

        

        GetComponent<VehicleParent>().engine = GetComponentInChildren<GasMotor>();
        GetComponent<VehicleParent>().wheels = GetComponentsInChildren<Wheel>();
        GetComponent<VehicleParent>().brakeIsReverse = true;
        GetComponent<VehicleParent>().holdEbrakePark = true;
        GetComponent<VehicleParent>().burnout = 0.9f;
        GetComponent<VehicleParent>().burnoutSpin = 5;
        GetComponent<VehicleParent>().burnoutSmoothness = 0.5f;
        GetComponent<VehicleParent>().centerOfMassOffset = new Vector3(0, -0.1f, 0);

        FlipControl cf = CopyComponent<FlipControl>(m_model.GetComponent<FlipControl>(), gameObject);
        VehicleDebug vd = CopyComponent<VehicleDebug>(m_model.GetComponent<VehicleDebug>(), gameObject);

        //gameObject.AddComponent<CarInputManager>();
        gameObject.AddComponent<CheckPoint>();
        //gameObject.AddComponent<Respawn>();

        GameObject campos = new GameObject("CamPos");
        campos.transform.SetParent(transform);
        campos.transform.localPosition = new Vector3(0, 1.5f, -0.27f);
        GameObject camTarget = new GameObject("CamTarget");
        camTarget.transform.SetParent(transform);
        camTarget.transform.localPosition = new Vector3(0, 1.2f, 2.22f);

        CameraController cc = gameObject.AddComponent<CameraController>();
        cc.m_cam = GameObject.Find("Main Camera");

        Transform[] tf = GetComponentsInChildren<Transform>();

        foreach(Transform t in tf)
        {
            if (t.name == "CamPos")
                cc.m_camPos = t;

            if (t.name == "CamTarget")
                cc.m_camTarget = t;
        }

        //gameObject.AddComponent<DisplaySpeed>();
        gameObject.AddComponent<GForce>();
        gameObject.AddComponent<ForceFeedbackTest>();
        gameObject.AddComponent<ApplyDataInfos>();

        CopyComponent<BasicInput>(m_model.GetComponent<BasicInput>(), gameObject);


        DestroyImmediate(this);
    }

    T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy as T;
    }
}

