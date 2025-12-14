using UnityEngine;

public class PickAndTriggerStar : MonoBehaviour
{
    [Header("星星飞行脚本（拖入 StarReturn 的物件）")]
    public StarReturn starReturn;

    private bool playerInRange = false;

    void Update()
    {
        // 玩家在区域内 + 按下 E
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TriggerStar();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    private void TriggerStar()
    {
        if (starReturn != null)
            starReturn.StartFly();

        Debug.Log("按 E → 触发星星飞行");
    }
}
