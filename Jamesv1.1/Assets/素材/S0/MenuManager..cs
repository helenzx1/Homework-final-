using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject creditsPanel;
    public GameObject quitPanel;

    // --- Main Buttons ---
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); // 改成你的游戏场景名
    }

    public void ShowOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void ShowQuit()
    {
        quitPanel.SetActive(true);
    }

    // --- Close Panels ---
    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }

    public void CloseQuit()
    {
        quitPanel.SetActive(false);
    }

    // --- Quit Application (真实退出用) ---
    public void QuitGame()
    {
        Application.Quit();
    }
}
