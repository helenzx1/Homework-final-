using UnityEngine;

public class NotebookDrag : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;

    [Header("目标区域（把笔记本拖到这里）")]
    public Transform targetSpot;
    public Collider2D targetTrigger;

    [Header("目标区域视觉提示（SpriteRenderer）")]
    public SpriteRenderer targetHighlight;

    [Header("星星粒子方向控制器")]
    public ParticleDirectionController particleController;

    [Header("星星飞行动画")]
    public StarReturn starReturn;

    private bool moved = false;
    private Scene2Controller controller;

    void Awake()
    {
        // 🧡 Awake 只在游戏开始加载时执行一次
        // 如果你确保 highlight 初始就要隐藏 → 在 Inspector 里面先关掉 enabled
        if (targetHighlight != null)
            targetHighlight.enabled = false;
    }

    void OnEnable()
    {
        // 🧡 物件重新启用时，也强制隐藏提示框
        if (targetHighlight != null)
            targetHighlight.enabled = false;
    }

    void OnDisable()
    {
        // 🧡 物件被关闭时 → 强制隐藏（不会留在画面上）
        if (targetHighlight != null)
            targetHighlight.enabled = false;
    }

    void Start()
    {
        controller = FindFirstObjectByType<Scene2Controller>();
    }

    void OnMouseDown()
    {
        if (moved) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        offset = transform.position - mousePos;
        isDragging = true;

        if (targetHighlight != null)
            targetHighlight.enabled = true;
    }

    void OnMouseUp()
    {
        isDragging = false;

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
        if (moved) return;

        if (other == targetTrigger)
        {
            moved = true;

            if (targetHighlight != null)
                targetHighlight.enabled = false;

            transform.position = targetSpot.position;

            if (particleController != null)
                particleController.enabled = true;

            if (starReturn != null)
                starReturn.StartFly();

            if (controller != null)
                controller.OnLightFlyOutside();
        }
    }
}
