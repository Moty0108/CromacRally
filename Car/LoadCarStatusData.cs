using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string file)
    {
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(file) as TextAsset;
        //Debug.Log(data.text);

        string[] lines = Regex.Split(data.text, LINE_SPLIT_RE); // 개행으로 구분해서 최대 줄 수와 데이터를 받아옴
        //Debug.Log("데이터 개수" + lines.Length);

        if (lines.Length <= 1)  // 줄이 없으면 그대로 반환
        {
            //Debug.Log("CSV 데이터 없음");
            return list;
        }

        string[] header = Regex.Split(lines[0], SPLIT_RE);  // 헤더를 읽어옴

        for(int i = 1; i<lines.Length;i++)  // 데이터부분을 읽어옴
        {
            string[] values = Regex.Split(lines[i], SPLIT_RE);

            if (values.Length == 0 || values[0] == "")
                continue;

            Dictionary<string, object> entry = new Dictionary<string, object>();

            for(int j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");  // 앞뒤 공백 제거
                object finalvalue = value;
                int n;
                float f;


                if(int.TryParse(value, out n))  // 정수면 정수로 저장
                {
                    finalvalue = n;
                }
                else if(float.TryParse(value, out f))   // 실수면 실수로 저장
                {
                    finalvalue = f;
                }
                // 해당사항 없으면 문자열 그대로 저장
                
                entry[header[j]] = finalvalue;  // 헤더(이름) 딕셔너리에 값 저장
            }
            list.Add(entry);
        }

        return list;
    }
}

[System.Serializable]
public class FinalCarData
{
    public GameObject m_selectSceneModel;
    public GameObject m_gameScenePrefab;
    [HideInInspector]
    public GameObject m_createdObject;
    public Sprite m_carBackGroundSprite;
    public Vector3 m_carLogoPosition;   //차 선택UI 로고 위치 조절
    public Vector2 m_carLogoSize;       //차 선택UI 로고 크기 조절
    public Vector3 m_carLogoRotate;     //차 선택UI 로고 회전
    public string m_carName;
    public string m_carDescription;
    public int m_durabilityLevel;
    public int m_durabilityAmountLevel;
    public int m_maxSpeedLevel;
    public int m_accelLevel;
    public int m_corneringLevel;
    public int m_comLevel;
}

public class LoadCarStatusData : Singleton<LoadCarStatusData>
{
    public string m_dataName;
    public FinalCarData[] m_cars;
    List<Dictionary<string, object>> data;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        data = CSVReader.Read(m_dataName);
        

        

        //데이터 복사
        if (StringData.instance != null)
        {
            if (m_cars != null)
            {
                StringData.instance.SetCarData();
            }
            //else
                //Debug.Log("No Car Data");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject CreateCar(int index)
    {
        GameObject temp = Instantiate(m_cars[index].m_gameScenePrefab);
        temp.name = "Drift Car";
        temp.SetActive(false);

        SetStatus(temp, index);

        return temp;
    }


    public void SetStatus(GameObject obj, int i)
    {
            float f;
            float.TryParse(data[m_cars[i].m_durabilityLevel]["Dur"].ToString(), out f);
            obj.GetComponent<DurabilitySystem>().m_maxDurability = f;
            float.TryParse(data[m_cars[i].m_durabilityLevel]["DurAmount"].ToString(), out f);
            obj.GetComponent<DurabilitySystem>().m_multiply = f;

            float.TryParse(data[0]["Gear0"].ToString(), out f);
            obj.GetComponentInChildren<RVP.GearboxTransmission>().gears[0].ratio = f;
            float.TryParse(data[0]["Gear1"].ToString(), out f);
            obj.GetComponentInChildren<RVP.GearboxTransmission>().gears[1].ratio = f;
            float.TryParse(data[m_cars[i].m_accelLevel]["Accel1"].ToString(), out f);
            obj.GetComponentInChildren<RVP.GearboxTransmission>().gears[2].ratio = f;
            float.TryParse(data[m_cars[i].m_accelLevel]["Accel2"].ToString(), out f);
            obj.GetComponentInChildren<RVP.GearboxTransmission>().gears[3].ratio = f;
            float.TryParse(data[m_cars[i].m_accelLevel]["Accel3"].ToString(), out f);
            obj.GetComponentInChildren<RVP.GearboxTransmission>().gears[4].ratio = f;
            float.TryParse(data[m_cars[i].m_accelLevel]["Accel4"].ToString(), out f);
            obj.GetComponentInChildren<RVP.GearboxTransmission>().gears[5].ratio = f;
            float.TryParse(data[m_cars[i].m_maxSpeedLevel]["MaxSpeed"].ToString(), out f);
            obj.GetComponentInChildren<RVP.GearboxTransmission>().gears[6].ratio = f;

            float x, y, z;
            float.TryParse(data[m_cars[i].m_comLevel]["ComX"].ToString(), out x);
            float.TryParse(data[m_cars[i].m_comLevel]["ComY"].ToString(), out y);
            float.TryParse(data[m_cars[i].m_comLevel]["ComZ"].ToString(), out z);

            obj.GetComponent<RVP.VehicleParent>().centerOfMassOffset = new Vector3(x, y, z);

            float.TryParse(data[m_cars[i].m_corneringLevel]["ConeringF"].ToString(), out f);


            obj.GetComponentInChildren<RVP.SteeringControl>().steerCurve.MoveKey(0, new Keyframe(0, f));
            
            float.TryParse(data[m_cars[i].m_corneringLevel]["ConeringR"].ToString(), out f);
            obj.GetComponentInChildren<RVP.SteeringControl>().steerCurve.MoveKey(1, new Keyframe(3, f));
    }
}
