using UnityEngine;

public class PixelPlayerAnim : MonoBehaviour
{
    public float moveSpeed = 3f;

    Rigidbody2D rb;
    Animator anim;
    Vector2 input;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        // 防止斜方向加速
        if (input.magnitude > 1f)
            input.Normalize();

        anim.SetFloat("MoveX", input.x);
        anim.SetFloat("MoveY", input.y);
        anim.SetFloat("Speed", input.sqrMagnitude);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + input * moveSpeed * Time.fixedDeltaTime);
    }
}
