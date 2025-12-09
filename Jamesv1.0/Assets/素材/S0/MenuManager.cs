using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;     // 主菜单（Start / Options / Help / Credits）
    public GameObject optionsPanel; // Options 面板
    public GameObject helpPanel;    // Help 面板
    public GameObject creditsPanel; // Credits 面板（如果你不用可以为空）

    // --- Main Menu ---
    public void OpenOptions()
    {
        mainMenu.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void OpenHelp()
    {
        mainMenu.SetActive(false);
        helpPanel.SetActive(true);
    }

    public void OpenCredits()
    {
        mainMenu.SetActive(false);
        creditsPanel.SetActive(true);
    }

    // --- Back Button ---
    public void BackToMenu()
    {
        optionsPanel.SetActive(false);
        helpPanel.SetActive(false);
        if (creditsPanel != null)
            creditsPanel.SetActive(false);

        mainMenu.SetActive(true);
    }

    // --- Start Game ---
    public void StartGame()
    {
        // 你的场景名字修改这里
        UnityEngine.SceneManagement.SceneManager.LoadScene("S0");
    }

    // --- Quit ---
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
    }
}
