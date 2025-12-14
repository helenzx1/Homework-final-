using UnityEngine;

public class FloatBreathFollow : MonoBehaviour
{
    [Header("漂浮参数")]
    public float floatAmplitude = 0.1f;    // 浮动高度
    public float floatSpeed = 1.2f;        // 浮动速度

    [Header("呼吸缩放")]
    public float scaleAmplitude = 0.05f;   // 呼吸变化幅度
    public float scaleSpeed = 2f;          // 呼吸速度

    private Vector3 dragBasePos;   // 拖动后的基础位置（不包含漂浮）
    private Vector3 startScale;    // 原始缩放
    private bool isDragging;       // 外部可设定是否正在拖动

    // 外部可调用：开始拖动
    public void BeginDrag()
    {
        isDragging = true;
    }

    // 外部可调用：拖动中更新位置
    public void DragMove(Vector3 worldPos)
    {
        dragBasePos = worldPos;
        transform.position = dragBasePos;
    }

    // 外部可调用：结束拖动
    public void EndDrag()
    {
        isDragging = false;
        dragBasePos = transform.position;  // 拖完后重新记录停靠位置
    }

    void Start()
    {
        dragBasePos = transform.position;
        startScale = transform.localScale;
    }

    void Update()
    {
        float t = Time.time;

        // 🎈 呼吸缩放 (一直生效，拖动中也会呼吸，不会影响位置)
        float scale = 1 + Mathf.Sin(t * scaleSpeed) * scaleAmplitude;
        transform.localScale = startScale * scale;

        // 🎈 只有非拖动时，才加漂浮效果
        if (!isDragging)
        {
            transform.position = dragBasePos +
                new Vector3(0, Mathf.Sin(t * floatSpeed) * floatAmplitude, 0);
        }
    }
}
