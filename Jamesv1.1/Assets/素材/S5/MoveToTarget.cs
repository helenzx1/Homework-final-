using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    public Transform target;     // 要走过去的目标
    public float speed = 2f;     // 移动速度
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (target == null) return;

        // 计算方向
        Vector3 direction = (target.position - transform.position);
        float distance = direction.magnitude;

        // 设置动画速度（0 = idle，>0 = walk）
        animator.SetFloat("Speed", distance);

        if (distance > 0.05f)
        {
            // 归一化方向
            Vector3 move = direction.normalized * speed * Time.deltaTime;

            // 移动角色
            transform.position += move;

            // 翻转（左右判断）
            spriteRenderer.flipX = (direction.x < 0);

            // 设置 Facing（0前，1后，2侧）
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                animator.SetInteger("Facing", 2); // side
            }
            else
            {
                if (direction.y > 0)
                    animator.SetInteger("Facing", 1); // back
                else
                    animator.SetInteger("Facing", 0); // front
            }
        }
        else
        {
            animator.SetFloat("Speed", 0f); // 停止
        }
    }
}
