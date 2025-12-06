using UnityEngine;

public class DragToTriggerAndVanish : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private bool done = false;

    [Header("触发区（必须是 Collider2D + IsTrigger）")]
    public Collider2D targetTrigger;

    [Header("成功后要跳去哪（可选）")]
    public Transform popOutPosition;

    [Header("成功后要让物件消失？")]
    public bool destroyAfterSuccess = false;

    [Header("成功后启用的物件（可选）")]
    public GameObject objectToEnable;

    [Header("开启 Debug Mode？")]
    public bool debugMode = true;

    void Start()
    {
        if (debugMode)
        {
            if (targetTrigger == null)
                Debug.LogError("❌ targetTrigger 没指定！脚本无法匹配触发区。");

            if (GetComponent<Collider2D>() == null)
                Debug.LogError("❌ 拖曳物件没有 Collider2D！无法触发 OnMouse 或 OnTriggerEnter2D！");

            if (targetTrigger != null && !targetTrigger.isTrigger)
                Debug.LogError("❌ targetTrigger 的 IsTrigger 没勾！不会触发 OnTriggerEnter2D。");
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
        if (debugMode)
        {
            Debug.Log("🟦 OnTriggerEnter2D 触发 → 撞到：" + other.name);

            if (other == targetTrigger)
                Debug.Log("🟢 这是目标触发区！（匹配成功）");
            else
                Debug.Log("🔴 撞到的不是 targetTrigger，本次不算。");
        }

        if (done) return;

        if (other == targetTrigger)
        {
            if (debugMode)
                Debug.Log("🎉== 成功进入 Trigger，执行成功逻辑 ==🎉");

            done = true;

            if (popOutPosition != null)
            {
                transform.position = popOutPosition.position;
                if (debugMode)
                    Debug.Log("✨ 物件跳到 popOutPosition");
            }

            // ⭐ 启用指定物件
            if (objectToEnable != null)
            {
                objectToEnable.SetActive(true);
                if (debugMode)
                    Debug.Log("🌟 已启用物件：" + objectToEnable.name);
            }

            if (destroyAfterSuccess)
            {
                if (debugMode)
                    Debug.Log("💥 物件已 Destroy()");
                Destroy(gameObject);
            }
        }
        else
        {
            if (debugMode)
                Debug.Log("⚠ 不是目标 collider，忽略。");
        }
    }
}
