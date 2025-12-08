using UnityEngine;
using TMPro;
using System.Collections;

public class TypingNarrationSystem : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI textUI;

    [Header("Settings")]
    public float typingSpeed = 0.04f;   // 打字机速度
    public float holdTime = 2f;         // 全部显示后停留时间
    public float fadeOutTime = 1f;      // 淡出时间

    private CanvasGroup canvasGroup;
    private Coroutine typingRoutine;

    void Awake()
    {
        canvasGroup = textUI.gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    public void ShowNarration(string content)
    {
        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        typingRoutine = StartCoroutine(TypingSequence(content));
    }

    IEnumerator TypingSequence(string content)
    {
        textUI.text = "";
        canvasGroup.alpha = 1f;

        // Typing effect
        foreach (char c in content)
        {
            textUI.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Hold when finished
        yield return new WaitForSeconds(holdTime);

        // Fade out
        float t = 0f;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = 1 - (t / fadeOutTime);
            yield return null;
        }

        canvasGroup.alpha = 0;
    }
}
