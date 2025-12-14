using UnityEngine;
using UnityEngine.SceneManagement;
using static QuestManager;

public class SceneChanger : MonoBehaviour
{
    [Header("要切换的场景名称")]
    public string nextSceneName;

    [Header("是否使用 Trigger 触发切换")]
    public bool useTrigger = false;

    [Header("玩家 Tag")]
    public string playerTag = "Player";

    // ───────────────────────────────
    // ⭐【新增扩充】免条件通行模式
    // ───────────────────────────────
    [Header("【特殊模式】无需任何侦测即可通过")]
    public bool allowFreePass = false;

    // ───────────────────────────────
    // ⭐ 方式 1：检查 GameData 布林条件
    // ───────────────────────────────
    [Header("【GameData 条件】全部满足才可通过")]
    public bool needDeskCleaned;
    public bool needNotebookFound;
    public bool needLightFlewOutside;
    public bool needReflectionDone;
    public bool needgetGardenDone;

    // ───────────────────────────────
    // ⭐ 方式 2：检查 Quest（组合任务）
    // ───────────────────────────────
    [Header("【可选】指定某个 QuestId 必须完成")]
    public QuestId requiredQuest = QuestId.None;

    // ───────────────────────────────
    // ⭐ 判断能否进入下一场景（最终整合）
    // ───────────────────────────────
    bool CanEnter()
    {
        // ⭐【扩充】如果打勾 → 不检测任何条件，直接过关
        if (allowFreePass)
            return true;

        // ⭐ 原本逻辑（完全不动）
        GameData data = DataManager.Instance?.data;
        if (data == null)
        {
            Debug.LogError("❌ DataManager.Instance 或 data 为 null！");
            return false;
        }

        // ✔ 检查 GameData 条件
        if (needDeskCleaned && !data.deskCleaned) return false;
        if (needNotebookFound && !data.notebookFound) return false;
        if (needLightFlewOutside && !data.lightFlewOutside) return false;
        if (needReflectionDone && !data.reflectionPuzzleDone) return false;
        if (needgetGardenDone && !data.gotHintToGarden) return false;

        // ✔ 检查 Quest 条件
        if (requiredQuest != QuestId.None)
        {
            if (!QuestManager.Instance.IsQuestCompleted(requiredQuest))
                return false;
        }

        return true;
    }

    // ───────────────────────────────
    // ⭐ Button 调用
    // ───────────────────────────────
    public void ChangeScene()
    {
        if (!CanEnter())
        {
            Debug.Log("❌ 条件未完成，无法切换场景！");
            return;
        }

        Debug.Log("🔄 正在切换场景 → " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }

    // ───────────────────────────────
    // ⭐ Trigger（2D）
    // ───────────────────────────────
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!useTrigger) return;
        if (!collision.CompareTag(playerTag)) return;

        if (!CanEnter())
        {
            Debug.Log("❌ Trigger：条件未完成");
            return;
        }

        ChangeScene();
    }
}
