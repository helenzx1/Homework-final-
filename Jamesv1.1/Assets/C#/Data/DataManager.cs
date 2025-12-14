using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    [Header("📦 存档数据")]
    public GameData data;

    [Header("🧪 开发模式（每次启动清档）")]
    [SerializeField] private bool devResetOnBoot = true;

    private string path;

    void Awake()
    {
        // =========================
        // Singleton 防重
        // =========================
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // =========================
        // 存档路径
        // =========================
        path = Path.Combine(Application.persistentDataPath, "save.json");

        // =========================
        // ⭐ 核心逻辑：启动流程唯一入口
        // =========================
#if UNITY_EDITOR
        if (devResetOnBoot)
        {
            HardReset();
            Debug.Log("🧪【开发模式】启动时已清档");
        }
        else
        {
            Load();
            Debug.Log("📂【开发模式】读取存档");
        }
#else
        Load();
        Debug.Log("🎮【正式版本】读取存档");
#endif
    }

    // =========================
    // 存档
    // =========================
    public void Save()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    // =========================
    // 读档
    // =========================
    public void Load()
    {
        if (!File.Exists(path))
        {
            data = new GameData();   // ⭐ 没档就新建
            return;
        }

        string json = File.ReadAllText(path);
        data = JsonUtility.FromJson<GameData>(json);
    }

    // =========================
    // 软重置（按钮用）
    // =========================
    public void ResetData()
    {
        data = new GameData();
        Save();
    }

    // =========================
    // 硬重置（启动 / Debug 用）
    // =========================
    private void HardReset()
    {
        if (File.Exists(path))
            File.Delete(path);

        data = new GameData();
    }
}
