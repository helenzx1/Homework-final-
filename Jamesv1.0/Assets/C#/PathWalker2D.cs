using UnityEngine;

public class PathWalker2D_WithAnim : MonoBehaviour
{
    [Header("路径")]
    public Transform[] pathPoints;

    [Header("移动参数")]
    public float pathSpeed = 0.35f;
    public float lateralOffset = 0.12f;
    public float swaySpeed = 4f;

    [Range(0f, 1f)]
    public float t = 0f;

    [Header("动画")]
    public Animator animator;

    float swayTimer;

    // =========================
    // 移动 & 路径逻辑
    // =========================
    void Update()
    {
        if (pathPoints == null || pathPoints.Length < 2) return;

        // 1) 输入
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(h, v);

        // 没输入就停动画
        if (input.sqrMagnitude < 0.001f)
        {
            UpdateAnimation(Vector2.zero, false);
            return;
        }

        // 2) 当前路径段
        int count = pathPoints.Length;
        float scaled = t * (count - 1);
        int index = Mathf.Clamp(Mathf.FloorToInt(scaled), 0, count - 2);
        int nextIndex = index + 1;
        float lerpT = scaled - index;

        Vector3 p0 = pathPoints[index].position;
        Vector3 p1 = pathPoints[nextIndex].position;

        Vector3 basePos = Vector3.Lerp(p0, p1, lerpT);

        // 3) 路径切线 & 法线
        Vector2 tangent = ((Vector2)(p1 - p0)).normalized;
        Vector2 normal = new Vector2(-tangent.y, tangent.x);

        // 4) 输入投影到路径方向
        float moveInput =
            Vector2.Dot(input.normalized, tangent) * input.magnitude;

        // 5) 推进路径
        t += moveInput * pathSpeed * Time.deltaTime;
        t = Mathf.Clamp01(t);

        // 6) 摆动
        swayTimer += Time.deltaTime * swaySpeed;
        float sway =
            Mathf.Sin(swayTimer) * lateralOffset * Mathf.Abs(moveInput);

        transform.position = basePos + (Vector3)(normal * sway);

        // 7) 动画
        Vector2 moveDir = tangent * Mathf.Sign(moveInput);
        UpdateAnimation(moveDir, true);
    }

    // =========================
    // 动画控制
    // =========================
    void UpdateAnimation(Vector2 moveDir, bool isMoving)
    {
        if (!animator) return;

        animator.SetBool("IsMoving", isMoving);

        if (!isMoving) return;

        animator.SetFloat("MoveX", Mathf.Sign(moveDir.x));
        animator.SetFloat("MoveY", Mathf.Sign(moveDir.y));
    }
}
