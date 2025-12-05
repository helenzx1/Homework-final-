using UnityEngine;
using static QuestManager;

public class Scene2Controller : MonoBehaviour
{
    private GameData data;

    [Header("笔记本（拖进来）")]
    public GameObject notebook;

    void Start()
    {
        data = DataManager.Instance.data;
        Debug.Log("Scene2Controller Start() OK");
        Debug.Log("【运行时 Data】lightFlewOutside = " + data.lightFlewOutside);
    }

    //──────────────────────────────
    // ① 桌面整理事件
    //──────────────────────────────
    public void OnMoveDeskItems()
    {
        data.deskCleaned = true;
        data.notebookFound = true;
        DataManager.Instance.Save();

        notebook.SetActive(true);
        Debug.Log("📘 笔记本出现");
    }

    //──────────────────────────────
    // ② 光飞出窗外 → 检测镜子是否可启动
    //──────────────────────────────
    public void OnLightFlyOutside()
    {
        // ⚠ 移除阻挡逻辑，否则第一次事件会被误判成重复
        Debug.Log("✨ 收到光飞出去事件通知 → 开始检查镜子谜题条件…");

        //⭐ 这里不再 return，而是直接继续流程
        data.lightFlewOutside = true;
        DataManager.Instance.Save();

        OnMirrorPuzzleStart();
    }

    //──────────────────────────────
    // ③ 镜子谜题开始（用任务系统侦测）
    //──────────────────────────────
    public void OnMirrorPuzzleStart()
    {
        Debug.Log($"[镜子检查] data.lightFlewOutside = {data.lightFlewOutside}");

        bool canStartMirror =
            QuestManager.Instance != null &&
            QuestManager.Instance.IsQuestCompleted(QuestId.Scene2_LightFlewOutside);

        Debug.Log($"[镜子检查] 条件成立？ {canStartMirror}");

        if (!canStartMirror)
        {
            Debug.Log("⚠ 镜子谜题不能开始");
            return;
        }

        Debug.Log("🪞 镜子谜题开始！（条件成立）");

        // ⭐⭐ 开启真正的镜子谜题
        MirrorPuzzle puzzle = FindFirstObjectByType<MirrorPuzzle>();
        if (puzzle != null)
        {
            puzzle.StartPuzzle();
        }
        else
        {
            Debug.LogError("❌ MirrorPuzzle 脚本不存在！无法启动镜子谜题");
        }
    }

    //──────────────────────────────
    // ④ 镜子谜题完成
    //──────────────────────────────
    public void OnMirrorPuzzleCompleted()
    {
        // 之后再补
    }
}
