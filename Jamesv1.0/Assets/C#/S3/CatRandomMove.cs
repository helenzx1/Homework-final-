using UnityEngine;

public class CatRandomMoveWithPlayerStop : MonoBehaviour
{
    [Header("移动速度")]
    public float speed = 1f;

    [Header("单次移动距离范围")]
    public float minStep = 0.5f;
    public float maxStep = 2f;

    [Header("移动前等待时间")]
    public float minWait = 1f;
    public float maxWait = 3f;

    [Header("玩家 Tag")]
    public string playerTag = "Player";

    [Header("玩家靠近时跳出的物件")]
    public GameObject objectToShow;

    private SpriteRenderer sr;
    private Animator anim;

    private bool isMoving = false;
    private bool isPausedByPlayer = false;

    private Coroutine moveRoutine;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (objectToShow != null)
            objectToShow.SetActive(false);

        moveRoutine = StartCoroutine(MoveLoop());
    }

    // ⭐ 主随机行动循环（不停走、停、翻方向）
    System.Collections.IEnumerator MoveLoop()
    {
        while (true)
        {
            // 若被玩家暂停 → 等待
            while (isPausedByPlayer)
                yield return null;

            // 随机等待
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));

            // 随机移动距离
            float step = Random.Range(minStep, maxStep);

            // 随机方向
            float dir = Random.value > 0.5f ? 1f : -1f;

            // 翻转方向
            if (sr != null)
                sr.flipX = dir < 0;

            // 启动 Walk 动画
            if (anim != null)
                anim.SetBool("Walk", true);

            Vector3 target = transform.position + new Vector3(dir * step, 0, 0);

            // 移动中
            isMoving = true;
            while (!isPausedByPlayer && Vector3.Distance(transform.position, target) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target,
                    speed * Time.deltaTime
                );
                yield return null;
            }
            isMoving = false;

            // 停止走路动画
            if (anim != null)
                anim.SetBool("Walk", false);
        }
    }

    // ⭐ 玩家进入触发范围：暂停所有行为 + 关闭 Animator
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag)) return;

        isPausedByPlayer = true;

        // 停止 Walk 动画
        if (anim != null)
            anim.SetBool("Walk", false);

        // ⭐ 完全关闭 Animator（猫保持当前静止姿势）
        if (anim != null)
            anim.enabled = false;

        // 显示提示物件
        if (objectToShow != null)
            objectToShow.SetActive(true);
    }

    // ⭐ 玩家离开触发范围：恢复行为 + 重启 Animator
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag)) return;

        isPausedByPlayer = false;

        // ⭐ 恢复 Animator
        if (anim != null)
            anim.enabled = true;

        // 关闭提示物件
        if (objectToShow != null)
            objectToShow.SetActive(false);
    }
}
