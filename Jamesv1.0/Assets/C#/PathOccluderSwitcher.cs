using UnityEngine;

public class PathOccluderSwitcher : MonoBehaviour
{
    [System.Serializable]
    public class Segment
    {
        [Range(0f, 1f)] public float tStart = 0f;
        [Range(0f, 1f)] public float tEnd = 1f;

        [Tooltip("进入这个区间时要启用的物件（例如：柱子前缘、藤蔓前景）")]
        public GameObject[] enableObjects;

        [Tooltip("进入这个区间时要关闭的物件（例如：另一侧前景）")]
        public GameObject[] disableObjects;
    }

    [Header("从你的 PathWalker 拿到同一个 t")]
    public PathWalker2D_WithAnim walker;   // 拖你的角色（或有 t 的脚本）进来

    [Header("分段规则（按 t 区间）")]
    public Segment[] segments;

    int currentSegment = -1;

    void Update()
    {
        if (!walker || segments == null || segments.Length == 0) return;

        float t = walker.t;
        int seg = FindSegment(t);

        if (seg != currentSegment)
        {
            ApplySegment(seg);
            currentSegment = seg;
        }
    }

    int FindSegment(float t)
    {
        for (int i = 0; i < segments.Length; i++)
        {
            if (t >= segments[i].tStart && t < segments[i].tEnd)
                return i;
        }
        return -1; // 不在任何段
    }

    void ApplySegment(int segIndex)
    {
        // ✅ 统一重置所有段提到过的物件
        for (int i = 0; i < segments.Length; i++)
        {
            SetActiveArray(segments[i].enableObjects, false);
            SetActiveArray(segments[i].disableObjects, true); // ⭐关键补这行
        }

        if (segIndex < 0) return;

        // 进入段：启用 / 关闭你指定的物件
        SetActiveArray(segments[segIndex].enableObjects, true);
        SetActiveArray(segments[segIndex].disableObjects, false);
    }


    void SetActiveArray(GameObject[] arr, bool active)
    {
        if (arr == null) return;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i]) arr[i].SetActive(active);
        }
    }
}
