/*******************************************************
 * QuestManager（任务管理器）
 * -----------------------------------------------------
 * 核心目标：
 * 1. 让任务可扩充、可重复使用
 * 2. 每个任务都能在 Inspector 里用下拉菜单与勾选组合条件
 * 3. 避免硬编码，让任务逻辑集中管理
 *
 * 使用方式：
 * - 在场景中放一个 QuestManager 物件（推荐第一个场景）
 * - 在 Inspector 中新增任务 → 下拉选任务名 → 勾选完成条件
 * - 其他脚本只要呼叫：
 *      QuestManager.Instance.IsQuestCompleted(QuestId.Scene2_AllDone)
 *
 *******************************************************/

using UnityEngine;
using System;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    private GameData data;

    /*******************************************************
     * 任务编号（Task/Quest 的名字）
     * -----------------------------------------------------
     * 你可以自由扩充
     *******************************************************/
    public enum QuestId
    {
        None,

        // ⭐ 你要的新任务编号
        Scene2_AllDone,           // desk + notebook + lightFlew + reflectionPuzzle

        // 旧例子也保留
        Scene2_DeskAndNotebook,
        Scene2_LightFlewOutside,

        Garden_PuzzleDone,
        Hint_ToGarden_Got,
        EventA_ReadyToPlay
    }

    /*******************************************************
     * 🔻 可编辑任务结构（给 Inspector 使用）
     *******************************************************/
    [Serializable]
    public class QuestDefinition
    {
        [Header("任务编号（下拉菜单）")]
        public QuestId questId;

        [Header("📌 完成条件（Scene2 可组合）")]
        public bool need_deskCleaned;
        public bool need_notebookFound;
        public bool need_lightFlewOutside;
        public bool need_reflectionPuzzleDone;

        [Header("📌 更多条件可继续扩充…")]
        public bool need_gardenPuzzleDone;
        public bool need_gotHintToGarden;
        public bool need_star1;
    }

    [Header("这里列出所有任务（可自由加）")]
    public QuestDefinition[] quests;


    /*******************************************************
     * 单例初始化
     *******************************************************/
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        data = DataManager.Instance.data;
        Debug.Log("QuestManager 已成功启动！");
    }


    /*******************************************************
     * 对外接口：检查任务是否完成
     *******************************************************/
    public bool IsQuestCompleted(QuestId id)
    {
        if (id == QuestId.None) return false;

        foreach (var q in quests)
        {
            if (q.questId != id) continue;

            // ⭐ 检查所有勾选过的条件
            if (q.need_deskCleaned && !data.deskCleaned) return false;
            if (q.need_notebookFound && !data.notebookFound) return false;
            if (q.need_lightFlewOutside && !data.lightFlewOutside) return false;
            if (q.need_reflectionPuzzleDone && !data.reflectionPuzzleDone) return false;

            if (q.need_gardenPuzzleDone && !data.gardenPuzzleDone) return false;
            if (q.need_gotHintToGarden && !data.gotHintToGarden) return false;
            if (q.need_star1 && !data.star_1_collected) return false;

            // ✔ 所有条件都符合
            return true;
        }

        Debug.LogWarning($"⚠ Quest {id} 未在 Inspector 中定义！");
        return false;
    }



    /*******************************************************
     * 写入任务（可选：按任务反向写 GameData）
     *******************************************************/
    public void CompleteQuest(QuestId id)
    {
        switch (id)
        {
            case QuestId.Scene2_DeskAndNotebook:
                data.deskCleaned = true;
                data.notebookFound = true;
                break;

            case QuestId.Scene2_LightFlewOutside:
                data.lightFlewOutside = true;
                break;

            case QuestId.Garden_PuzzleDone:
                data.gardenPuzzleDone = true;
                break;

            case QuestId.Hint_ToGarden_Got:
                data.gotHintToGarden = true;
                break;

            case QuestId.EventA_ReadyToPlay:
                // 这个通常是组合任务，不改 data
                break;

            case QuestId.Scene2_AllDone:
                // 如果你想自动设定，也可以写：
                data.deskCleaned = true;
                data.notebookFound = true;
                data.lightFlewOutside = true;
                data.reflectionPuzzleDone = true;
                break;
        }

        DataManager.Instance.Save();


    }

}
