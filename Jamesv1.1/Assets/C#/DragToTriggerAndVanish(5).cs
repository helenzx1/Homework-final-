using UnityEngine;

public class DragToTriggerAndVanish : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private bool done = false;

    [Header("触发区（必须是 Collider2D + IsTrigger）")]
    public Collider2D targetTrigger;

    [Header("成功后淡出的速度")]
    public float fadeSpeed = 1f;

    [Header("淡化完后要启用的物件们（可扩充）")]
    public GameObject[] objectsToEnable;

    [Header("开启 Debug Mode？")]
    public bool debugMode = true;

    // ⭐⭐ 新增：达成条件（例如需要 3 颗星）
    [Header("需要多少颗进入 Trigger 才算达成")]
    public static int goalCount = 6;

    [Header("已进入 Trigger 的数量（自动累计）")]
    public static int currentCount = 0;

    private SpriteRenderer sr;
    private bool fading = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (debugMode)
        {
            if (targetTrigger == null)
                Debug.LogError("❌ targetTrigger 没指定！");

            if (GetComponent<Collider2D>() == null)
                Debug.LogError("❌ 缺少 Collider2D！");

            if (targetTrigger != null && !targetTrigger.isTrigger)
                Debug.LogError("❌ 目标区 IsTrigger 没勾！");
        }
    }

    void Update()
    {
        // ⭐ 渐渐淡化
        if (fading && sr != null)
        {
            Color c = sr.color;
            c.a -= Time.deltaTime * fadeSpeed;

            if (c.a <= 0)
            {
                c.a = 0;
                sr.color = c;

                fading = false;

                if (debugMode)
                    Debug.Log("✨ 淡化完成 → 启用指定物件群");

                EnableObjects();

                gameObject.SetActive(false);
                return;
            }

            sr.color = c;
        }
    }

    void OnMouseDown()
    {
        if (done) return;

        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0;
        offset = transform.position - mouse;
        isDragging = true;

        if (debugMode)
            Debug.Log("🟡 开始拖曳：" + gameObject.name);
    }

    void OnMouseDrag()
    {
        if (!isDragging || done) return;

        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0;
        transform.position = mouse + offset;
    }

    void OnMouseUp()
    {
        isDragging = false;

        if (debugMode)
            Debug.Log("🟡 放开拖曳：" + gameObject.name);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (done) return;

        if (debugMode)
            Debug.Log("🟦 Trigger → 撞到：" + other.name);

        if (other == targetTrigger)
        {
            // ⭐⭐ 累积数量
            currentCount++;

            if (debugMode)
                Debug.Log("⭐ 达成进度：" + currentCount + " / " + goalCount);

            done = true;   // 每颗星只能进一次

            // ⭐⭐ 只有当累计达到目标才执行淡出 + 解锁物件
            if (currentCount >= goalCount)
            {
                if (debugMode)
                    Debug.Log("🎉 全部达成 → 开始淡化");

                fading = true;
            }
            else
            {
                // ⭐ 未达成 → 该物件直接隐藏（不触发淡化与启用物件）
                if (debugMode)
                    Debug.Log("🔸 尚未达成目标数量 → 只隐藏自己");

                gameObject.SetActive(false);
            }
        }
    }

    // ⭐ 淡化结束后启用所有物件
    void EnableObjects()
    {
        foreach (var obj in objectsToEnable)
        {
            if (obj != null)
            {
                obj.SetActive(true);

                if (debugMode)
                    Debug.Log("🌟 已启用：" + obj.name);
            }
        }
    }
}
