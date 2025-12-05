using UnityEngine;

public class FloatBlinkScaleAndClick : MonoBehaviour
{
    [Header("飘动范围半径")]
    public float moveRadius = 0.5f;

    [Header("飘动速度")]
    public float moveSpeed = 1f;

    [Header("闪烁速度（亮暗节奏）")]
    public float blinkSpeed = 3f;

    [Header("呼吸缩放速度")]
    public float scaleSpeed = 2f;

    [Header("缩放范围 (最小~最大)")]
    public float minScale = 0.8f;
    public float maxScale = 1.2f;

    [Header("亮度范围 (最暗~最亮)")]
    public float minAlpha = 0.3f;
    public float maxAlpha = 1f;

    [Header("达成条件：点击几次会触发事件")]
    public int requiredClicks = 5;

    [Header("达成点数后要跳出的物件")]
    public GameObject appearAfter;

    [Header("达成点数后要关闭的物件")]
    public GameObject objectToHide;

    private Vector3 origin;
    private SpriteRenderer sr;
    private float blinkTimer;
    private float scaleTimer;

    // ⭐ 所有星星共享计数
    public static int clickedCount = 0;

    void Start()
    {
        origin = transform.position;
        sr = GetComponent<SpriteRenderer>();

        blinkTimer = Random.value * 10f;
        scaleTimer = Random.value * 10f;

        // 默认隐藏出现物件
        if (appearAfter != null)
            appearAfter.SetActive(false);
    }

    void Update()
    {
        // ⭐ 飘动
        float x = Mathf.Sin(Time.time * moveSpeed + origin.x) * moveRadius;
        float y = Mathf.Cos(Time.time * moveSpeed + origin.y) * moveRadius;
        transform.position = origin + new Vector3(x, y, 0);

        // ⭐ 闪烁
        blinkTimer += Time.deltaTime * blinkSpeed;
        float a = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(blinkTimer) + 1) * 0.5f);

        if (sr)
        {
            Color c = sr.color;
            c.a = a;
            sr.color = c;
        }

        // ⭐ 呼吸缩放
        scaleTimer += Time.deltaTime * scaleSpeed;
        float s = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(scaleTimer) + 1) * 0.5f);
        transform.localScale = new Vector3(s, s, 1);
    }

    void OnMouseDown()
    {
        // ⭐ 点击后物件消失
        gameObject.SetActive(false);

        clickedCount++;

        // ⭐ 达成所需点击次数
        if (clickedCount >= requiredClicks)
        {
            // 显示新物件
            if (appearAfter != null)
                appearAfter.SetActive(true);

            // 隐藏旧物件
            if (objectToHide != null)
                objectToHide.SetActive(false);
        }
    }
}
