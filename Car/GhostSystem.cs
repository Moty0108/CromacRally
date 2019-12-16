using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

[System.Serializable]
public struct SerializableVector3
{
    /// <summary>
    /// x component
    /// </summary>
    public float x;

    /// <summary>
    /// y component
    /// </summary>
    public float y;

    /// <summary>
    /// z component
    /// </summary>
    public float z;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rX"></param>
    /// <param name="rY"></param>
    /// <param name="rZ"></param>
    public SerializableVector3(float rX, float rY, float rZ)
    {
        x = rX;
        y = rY;
        z = rZ;
    }

    /// <summary>
    /// Returns a string representation of the object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.Format("[{0}, {1}, {2}]", x, y, z);
    }

    /// <summary>
    /// Automatic conversion from SerializableVector3 to Vector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Vector3(SerializableVector3 rValue)
    {
        return new Vector3(rValue.x, rValue.y, rValue.z);
    }

    /// <summary>
    /// Automatic conversion from Vector3 to SerializableVector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator SerializableVector3(Vector3 rValue)
    {
        return new SerializableVector3(rValue.x, rValue.y, rValue.z);
    }
}

[System.Serializable]
public class Ghost
{
    [SerializeField]
    public int carIndex;
    [SerializeField]
    public float bestTime;
    [SerializeField]
    public List<SerializableVector3> position = new List<SerializableVector3>();
    [SerializeField]
    public List<SerializableVector3> rotation = new List<SerializableVector3>();
}

public class Recoder
{
    MonoBehaviour behaviour;
    bool isRecoarding;
    GameObject obj;
    GameObject m_ghostObj;
    bool isActive;
    bool isClear = false;

    public Recoder(MonoBehaviour _behaviour)
    {
        behaviour = _behaviour;
        isRecoarding = false;
        isActive = true;
    }

    public void StartRecoard(Transform transform, int _index)
    {
        isRecoarding = true;
        behaviour.StartCoroutine(Recoarder(transform, _index));
        obj = transform.gameObject;
    }

    public void EndRecoard(bool _isClear)
    {
        isClear = _isClear;
        isRecoarding = false;
    }

    public void Active()
    {
        isActive = !isActive;
        m_ghostObj.SetActive(isActive);
    }

    public Transform GetGhostTransform()
    {
        if (m_ghostObj)
            return m_ghostObj.transform;
        else
            return null;
    }

    Dictionary<float, SerializableVector3> position;
    Dictionary<float, SerializableVector3> rotation;

    public void PlayRecord(GameObject _obj, Ghost _ghost)
    {
        behaviour.StartCoroutine(PlayRecoard(_obj, _ghost));
    }

    IEnumerator PlayRecoard(GameObject _obj, Ghost _ghost)
    {
        m_ghostObj = Object.Instantiate(_obj);

        m_ghostObj.transform.position = _ghost.position[0];
        m_ghostObj.transform.eulerAngles = _ghost.rotation[0];
        int currentTime = 0;
        while (currentTime < _ghost.position.Count - 1)
        {
            currentTime = (int)(GameTimerManager.Instance.m_curTime * 10);
            m_ghostObj.transform.position = Vector3.Lerp(m_ghostObj.transform.position, _ghost.position[currentTime], Mathf.Clamp01(Time.deltaTime * 5));
            m_ghostObj.transform.rotation = Quaternion.Lerp(m_ghostObj.transform.rotation, Quaternion.Euler(_ghost.rotation[currentTime]), Mathf.Clamp01(Time.deltaTime * 5));

            yield return null;
            
        }
    }

    IEnumerator Recoarder(Transform transform, int _index)
    {
        position = new Dictionary<float, SerializableVector3>();
        rotation = new Dictionary<float, SerializableVector3>();
        Ghost ghost = new Ghost();
        
        while (isRecoarding)
        {
            int currentTime = (int)(GameTimerManager.Instance.m_curTime * 10);
            
            if (!position.ContainsKey(currentTime))
                position.Add(currentTime, transform.position);
            if (!rotation.ContainsKey(currentTime))
                rotation.Add(currentTime, transform.eulerAngles);

            yield return null;
        }
        
        ghost.position = position.Values.ToList();
        ghost.rotation = rotation.Values.ToList();
        ghost.carIndex = _index;
        ghost.bestTime = GameTimerManager.Instance.m_curTime;
        Ghost prevGhost = Load(SceneManager.GetActiveScene().name);
        
        if(prevGhost == null && isClear)
        Save(ghost, SceneManager.GetActiveScene().name);

        if (prevGhost != null && prevGhost.bestTime > ghost.bestTime && isClear)
        {
            Debug.Log("고스트저장");
            Save(ghost, SceneManager.GetActiveScene().name);
        }

    }

    public void Delete(string path)
    {
        FileInfo file = new FileInfo(Application.persistentDataPath + "/" + path + ".bin");

        if(file.Exists)
        {
            file.Delete();
        }
    }

    public void Save(Ghost info, string path)
    {
        BinarySerialize(info, Application.persistentDataPath + "/" + path + ".bin");
    }

    public Ghost Load(string path)
    {
        Ghost temp = null;
        try
        {
            temp = BinaryDeserialize<Ghost>(Application.persistentDataPath + "/" + path + ".bin");
        }
        catch (FileNotFoundException e)
        {
            Debug.Log(e);
            Debug.Log("고스트 정보 없음!");
        }

        return temp;
    }

    void BinarySerialize<T>(T info, string filePath)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(filePath, FileMode.Create);
        formatter.Serialize(stream, info);
        stream.Close();
    }

    T BinaryDeserialize<T>(string filePath)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(filePath, FileMode.Open);
        T info = (T)formatter.Deserialize(stream);
        stream.Close();
        return info;
    }
}

public class GhostSystem : MonoBehaviour
{
    Ghost m_ghostInfo;
    public GameObject m_ghostObj;
    public GameObject m_obj;

    Recoder m_recoder;

    // Start is called before the first frame update
    void Start()
    {
        m_recoder = new Recoder(this);
        m_ghostInfo = m_recoder.Load(SceneManager.GetActiveScene().name);

        if (m_ghostInfo != null)
            m_recoder.PlayRecord(m_ghostObj, m_ghostInfo);

        m_recoder.StartRecoard(m_obj.transform, 1);
    }

    
}
