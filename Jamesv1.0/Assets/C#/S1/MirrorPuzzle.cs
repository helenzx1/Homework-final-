using UnityEngine;

public class MirrorPuzzle : MonoBehaviour
{
    [Header("镜子谜题的 UI 或机关")]
    public GameObject puzzleUI;

    public void StartPuzzle()
    {
        Debug.Log("🪞【MirrorPuzzle】收到 Trigger 启动讯号 → 开始运行谜题");

        if (puzzleUI != null)
            puzzleUI.SetActive(true);
    }
}
