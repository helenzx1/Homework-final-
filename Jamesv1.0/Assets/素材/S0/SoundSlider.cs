using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    public AudioSource audioSource;  // 要控制的音效
    public Slider slider;            // UI 的滑杆

    void Start()
    {
        slider.onValueChanged.AddListener(SetVolume);
        slider.value = audioSource.volume; // 初始值同步
    }

    void SetVolume(float v)
    {
        audioSource.volume = v;
    }
}
