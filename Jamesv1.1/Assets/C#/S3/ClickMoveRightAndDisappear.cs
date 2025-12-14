using UnityEngine;

public class ClickMoveRightAndDisappear : MonoBehaviour
{
    [Header("移动速度")]
    public float moveSpeed = 2f;

    [Header("移动多远后消失")]
    public float moveDistance = 5f;

    [Header("消失前要开启的物件")]
    public GameObject objectToEnable;

    private bool isClicked = false;
    private bool triggered = false;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;

        // 物件默认关掉（如果你希望）
        if (objectToEnable != null)
            objectToEnable.SetActive(false);
    }

    void OnMouseDown()
    {
        isClicked = true;
    }

    void Update()
    {
        if (isClicked)
        {
            // ⭐ 往右移动
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;

            // ⭐ 还没触发过，并且到达终点 → 开启物件
            if (!triggered &&
                Vector3.Distance(startPos, transform.position) >= moveDistance)
            {
                triggered = true;  // 确保只触发一次

                if (objectToEnable != null)
                    objectToEnable.SetActive(true);

                // ⭐ 最后一帧再消失
                gameObject.SetActive(false);
            }
        }
    }
}
