using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1); // 载入 Build Settings 里 index 1 的场景
    }
}

