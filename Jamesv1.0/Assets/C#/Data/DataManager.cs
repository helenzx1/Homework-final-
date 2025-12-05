using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public GameData data = new GameData();

    private string path;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            path = Application.persistentDataPath + "/save.json";
            Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    public void Load()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<GameData>(json);
        }
    }
    public void ResetData()
    {
        data = new GameData();
        Save();
    }
}
