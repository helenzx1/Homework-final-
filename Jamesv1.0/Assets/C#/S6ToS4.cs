using UnityEngine;
using UnityEngine.SceneManagement;

public class S6ToS4 : MonoBehaviour
{
    [Header("要跳转到的场景名称")]
    public string targetScene = "S4";

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 只认玩家（防止路人、特效、子弹误触）
        if (!other.CompareTag("Player")) return;

        SceneManager.LoadScene(targetScene);
    }
}
