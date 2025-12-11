using UnityEngine;

public class CatSpriteController : MonoBehaviour
{
    [Header("小猫贴图")]
    public Sprite idleSprite;
    public Sprite walkSprite1;
    public Sprite walkSprite2;

    [Header("切换速度")]
    public float switchTime = 0.15f;

    [Header("走路速度")]
    public float moveSpeed = 2f;

    [Header("左右边界")]
    public float leftX = -2f;
    public float rightX = 2f;

    [Header("中心点（走到这里会Idle一次）")]
    public float centerX = 0f;

    private float idleDuration = 0.5f;

    private SpriteRenderer sr;
    private float timer;
    private bool useFirst = true;
    private float dir = 1f;
    private bool isIdle = false;
    private float idleTimer = 0f;
    private bool hasStoppedAtCenter = false;

    private bool wasIdleLastFrame = false;  // ⭐ 新增：记录前一帧是否 idle

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // ★ Idle 状态
        if (isIdle)
        {
            sr.sprite = idleSprite;
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleDuration)
            {
                isIdle = false;
                idleTimer = 0f;
            }

            wasIdleLastFrame = true;
            return;
        }

        // ★ 一开始动起来时 → 马上换第一张走路图
        if (wasIdleLastFrame)
        {
            useFirst = true;
            sr.sprite = walkSprite1;
            timer = 0f;
        }

        wasIdleLastFrame = false;  // 现在不是 idle 状态

        // ★ 动画切换节奏
        timer += Time.deltaTime;
        if (timer >= switchTime)
        {
            timer = 0;
            useFirst = !useFirst;
            sr.sprite = useFirst ? walkSprite1 : walkSprite2;
        }

        // ★ 边界换方向
        if (transform.position.x >= rightX)
        {
            dir = -1f;
            hasStoppedAtCenter = false;
        }
        else if (transform.position.x <= leftX)
        {
            dir = 1f;
            hasStoppedAtCenter = false;
        }

        // ★ 中心点停住
        if (!hasStoppedAtCenter && Mathf.Abs(transform.position.x - centerX) < 0.05f)
        {
            isIdle = true;
            hasStoppedAtCenter = true;
            sr.sprite = idleSprite;
            return;
        }

        // ★ 翻转方向
        sr.flipX = dir < 0;

        // ★ 移动
        transform.position += new Vector3(dir * moveSpeed * Time.deltaTime, 0, 0);
    }
}
