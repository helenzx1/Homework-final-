using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetSceneAndData : MonoBehaviour
{
    void Awake()
    {
        // ⭐ 这个物件跨场景不被删除
        DontDestroyOnLoad(gameObject);
    }

    public void ReplayGame()
    {
        // ⭐ 不再重置 Data（避免当机）
        Debug.Log("🔄【REPLAY】回到开场场景（Build Index 0） without data reset");

        // ⭐ 直接跳回第一关
        SceneManager.LoadScene(0);
    }
}
