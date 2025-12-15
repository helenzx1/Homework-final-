using UnityEngine;
using UnityEngine.EventSystems;

public class Puzzle4 : MonoBehaviour
{
    [Header("4 片碎片（顺序 = 碎片 1~4）")]
    public RectTransform[] pieces;

    [Header("4 个正确位置（顺序 = 左上、右上、左下、右下）")]
    public RectTransform[] spots;

    [Header("拼对后吸附距离（UI 单位）")]
    public float snapDistance = 60f;

    [Header("完成提示 UI")]
    public GameObject completeUI;

    Canvas canvas;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();

        // 给每片碎片自动添加拖拽脚本
        for (int i = 0; i < pieces.Length; i++)
        {
            PuzzlePieceDrag drag = pieces[i].gameObject.AddComponent<PuzzlePieceDrag>();
            drag.puzzle = this;
        }

        Shuffle();
    }

    //────────────────────────────────────────
    // 随机洗牌（保证不会一开始就完成）
    //────────────────────────────────────────
    void Shuffle()
    {
        int safety = 0;

        do
        {
            for (int i = 0; i < pieces.Length; i++)
            {
                int r = Random.Range(0, pieces.Length);
                Vector2 temp = pieces[i].anchoredPosition;
                pieces[i].anchoredPosition = pieces[r].anchoredPosition;
                pieces[r].anchoredPosition = temp;
            }

            safety++;
            if (safety > 20)
            {
                Debug.LogWarning("Shuffle safety break");
                break;
            }

        } while (IsCompleted());
    }

    //────────────────────────────────────────
    // 判断是否全部拼对（UI 坐标）
    //────────────────────────────────────────
    bool IsCompleted()
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            if (Vector2.Distance(
                pieces[i].anchoredPosition,
                spots[i].anchoredPosition
            ) > snapDistance)
                return false;
        }
        return true;
    }

    //────────────────────────────────────────
    // 每次放开碎片时调用
    //────────────────────────────────────────
    public void CheckPuzzle()
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            if (Vector2.Distance(
                pieces[i].anchoredPosition,
                spots[i].anchoredPosition
            ) < snapDistance)
            {
                pieces[i].anchoredPosition = spots[i].anchoredPosition;
            }
        }

        if (IsCompleted())
        {
            Debug.Log("🎉 4片拼图完成！");
            if (completeUI) completeUI.SetActive(true);

            // 更新任务 & 存档
            DataManager.Instance.data.reflectionPuzzleDone = true;
            DataManager.Instance.Save();

            if (QuestManager.Instance.IsQuestCompleted(
                QuestManager.QuestId.Scene2_AllDone))
            {
                Debug.Log("🌟 Scene2 所有任务完成！");
            }
        }
    }
}

//────────────────────────────────────────
// 拖拽控制脚本（UI 专用）
//────────────────────────────────────────
public class PuzzlePieceDrag : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
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
        rect.SetAsLastSibling(); // 拖动时显示在最上层
    }

    public void OnDrag(PointerEventData e)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rootCanvas.transform as RectTransform,
            e.position,
            rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : rootCanvas.worldCamera,
            out pos
        );

        rect.anchoredPosition = pos;
    }

    public void OnEndDrag(PointerEventData e)
    {
        puzzle.CheckPuzzle();
    }
}
