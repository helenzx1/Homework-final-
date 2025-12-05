using UnityEngine;

public class DragObject : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;

    [Header("目标区域（拖进去才算完成）")]
    public Transform targetSpot;            // 吸附位置
    public Collider2D targetTrigger;        // 目标触发点

    [Header("目标区域视觉提示")]
    public SpriteRenderer targetHighlight;  // ← 目标区域的视觉框（在原 Trigger 上加的）

    private bool moved = false;

    private Scene2Controller controller;

    void Start()
    {
        controller = FindFirstObjectByType<Scene2Controller>();

        // 开始时隐藏高亮
        if (targetHighlight != null)
            targetHighlight.enabled = false;
    }

    void OnMouseDown()
    {
        if (moved) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        offset = transform.position - mousePos;
        isDragging = true;

        // 显示目标范围
        if (targetHighlight != null)
            targetHighlight.enabled = true;
    }

    void OnMouseUp()
    {
        isDragging = false;

        // 放开就隐藏目标范围
        if (targetHighlight != null)
            targetHighlight.enabled = false;
    }

    void Update()
    {
        if (isDragging && !moved)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos + offset;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("触发器 ENTER：" + other.name);

        if (moved) return;

        if (other == targetTrigger)
        {
            Debug.Log("== 匹配目标触发区 ==");

            moved = true;

            // 自动吸附
            transform.position = targetSpot.position;

            // 隐藏视觉范围
            if (targetHighlight != null)
                targetHighlight.enabled = false;

            // 通知控制器
            if (controller != null)
            {
                controller.OnMoveDeskItems();
                Debug.Log("已通知 Scene2Controller.OnMoveDeskItems()");
            }
            else
            {
                Debug.LogError("找不到 Scene2Controller！");
            }
        }
    }
}
