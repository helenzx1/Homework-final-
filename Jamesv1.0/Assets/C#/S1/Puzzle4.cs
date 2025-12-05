using UnityEngine;
using UnityEngine.EventSystems;

public class Puzzle4 : MonoBehaviour
{
    [Header("4 片碎片（顺序 = 碎片 1~4）")]
    public RectTransform[] pieces;

    [Header("4 个正确位置（顺序 = 左上、右上、左下、右下）")]
    public RectTransform[] spots;

    [Header("拼对后吸附距离")]
    public float snapDistance = 60f;

    [Header("完成提示 UI")]
    public GameObject completeUI;

    Canvas canvas;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();

        // ⭐ 给每片碎片加入拖动组件（不用你手动设）
        for (int i = 0; i < pieces.Length; i++)
        {
            PuzzlePieceDrag drag = pieces[i].gameObject.AddComponent<PuzzlePieceDrag>();
            drag.index = i;
            drag.puzzle = this;
           
        }

        Shuffle();
    }

    //────────────────────────────────────────
    // 随机洗牌（不会洗成正确答案）
    //────────────────────────────────────────
    void Shuffle()
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            int r = Random.Range(0, pieces.Length);
            Vector3 t = pieces[i].anchoredPosition;
            pieces[i].anchoredPosition = pieces[r].anchoredPosition;
            pieces[r].anchoredPosition = t;
        }

        if (IsCompleted()) Shuffle();
    }

    //────────────────────────────────────────
    // 判断是否全对
    //────────────────────────────────────────
    bool IsCompleted()
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            if (Vector3.Distance(pieces[i].position, spots[i].position) > snapDistance)
                return false;
        }
        return true;
    }

    //────────────────────────────────────────
    // 每次放开碎片时检查
    //────────────────────────────────────────
    public void CheckPuzzle()
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            if (Vector3.Distance(pieces[i].position, spots[i].position) < snapDistance)
            {
                pieces[i].position = spots[i].position;
            }
        }

        if (IsCompleted())
        {
            Debug.Log("🎉 4片拼图完成！");
            if (completeUI) completeUI.SetActive(true);

            // ⭐ 更新任务 & 存档
            DataManager.Instance.data.reflectionPuzzleDone = true;
            DataManager.Instance.Save();

            if (QuestManager.Instance.IsQuestCompleted(QuestManager.QuestId.Scene2_AllDone))
                Debug.Log("🌟 Scene2 所有任务完成！");
        }
    }
}


//────────────────────────────────────────
// ⭐⭐ 完整拖曳控制脚本（自动加在每片碎片上）
//────────────────────────────────────────
public class PuzzlePieceDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int index;
    public Puzzle4 puzzle;

    RectTransform rect;
    Canvas rootCanvas;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        rootCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData e)
    {
    }

    public void OnDrag(PointerEventData e)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rootCanvas.transform as RectTransform,
            e.position,
            rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : rootCanvas.worldCamera,
            out pos);

        rect.anchoredPosition = pos;
    }

    public void OnEndDrag(PointerEventData e)
    {
        puzzle.CheckPuzzle();
    }
}

