using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterWithLoopSFX : MonoBehaviour
{
    [Header("文字相关")]
    public TextMeshProUGUI textUI;   // 你的PROTEXT / TextMeshProUGUI
    [TextArea]
    public string fullText;          // 要显示的完整文字
    public float charInterval = 0.05f; // 每个字出现的时间间隔

    [Header("音效相关")]
    public AudioSource audioSource;  // 放在同一个物件或别的物件都可以
    public AudioClip typingLoopClip; // 循环播放的打字声

    [Header("开局自动播放")]
    public bool playOnStart = false;  // ✔ 开局自动播放开关

    private Coroutine typingRoutine;


    void Start()
    {
        // ✔ 若启用自动播放，就自动跑打字机
        if (playOnStart)
        {
            StartTypewriter();
        }
    }


    /// <summary>
    /// 外部呼叫这个函数开始打字机效果（按钮用）
    /// </summary>
    public void StartTypewriter()
    {
        // 如果之前有在打字，先停掉
        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        typingRoutine = StartCoroutine(TypeRoutine());
    }


    private IEnumerator TypeRoutine()
    {
        // 初始化文字
        if (textUI != null)
            textUI.text = "";

        // 播放循环音效
        if (audioSource != null && typingLoopClip != null)
        {
            audioSource.clip = typingLoopClip;
            audioSource.loop = true;
            audioSource.Play();
        }

        // 打字机：一字一字出现
        foreach (char c in fullText)
        {
            textUI.text += c;
            yield return new WaitForSeconds(charInterval);
        }

        // 字打完，让音效把这一轮播完再停
        if (audioSource != null && typingLoopClip != null)
        {
            audioSource.loop = false;

            float remainTime = typingLoopClip.length - audioSource.time;
            if (remainTime > 0f)
                yield return new WaitForSeconds(remainTime);

            audioSource.Stop();
        }

        typingRoutine = null;
    }
}
