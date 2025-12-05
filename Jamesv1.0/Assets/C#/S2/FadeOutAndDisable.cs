using UnityEngine;

public class FadeOutAndDisable : MonoBehaviour
{
    public float fadeDuration = 5f;      // 淡化时间
    public bool reverse = false;         // ✔ 勾选 = 反向（淡入）
    public bool disableOnEnd = true;     // ✔ 不想自动关闭可取消

    private CanvasGroup cg;
    private float timer;

    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        if (cg == null)
            cg = gameObject.AddComponent<CanvasGroup>();

        // 反向模式：起始透明
        if (reverse)
            cg.alpha = 0f;
        else
            cg.alpha = 1f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / fadeDuration;

        if (!reverse)
        {
            // 正常模式：淡出 1 → 0
            cg.alpha = 1f - t;
        }
        else
        {
            // 反向模式：淡入 0 → 1
            cg.alpha = t;
        }

        if (t >= 1f && disableOnEnd)
        {
            gameObject.SetActive(false); // 完成后自动关掉
        }
    }
}
