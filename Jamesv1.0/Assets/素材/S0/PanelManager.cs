using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [Header("Panels")]
    public CanvasGroup mainMenuPanel;
    public CanvasGroup helpPanel;
    public CanvasGroup optionsPanel;
    public CanvasGroup creditsPanel;

    private void HideAll()
    {
        Hide(mainMenuPanel);
        Hide(helpPanel);
        Hide(optionsPanel);
        Hide(creditsPanel);
    }

    private void Show(CanvasGroup panel)
    {
        HideAll();
        panel.alpha = 1;
        panel.interactable = true;
        panel.blocksRaycasts = true;
    }

    private void Hide(CanvasGroup panel)
    {
        panel.alpha = 0;
        panel.interactable = false;
        panel.blocksRaycasts = false;
    }

    // ——按钮调用——
    public void OpenHelp()        => Show(helpPanel);
    public void OpenOptions()     => Show(optionsPanel);
    public void OpenCredits()     => Show(creditsPanel);

    public void BackToMenu()      => Show(mainMenuPanel);

    public void QuitGame()        => Application.Quit();
}
