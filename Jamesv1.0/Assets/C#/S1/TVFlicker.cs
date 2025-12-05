using UnityEngine;

public class TVFlicker : MonoBehaviour
{
    SpriteRenderer sr;

    [Header("亮度变化范围")]
    public float minBrightness = 0.65f;
    public float maxBrightness = 1.25f;

    [Header("闪烁速度（推荐 20~50）")]
    public float flickerSpeed = 30f;

    [Header("偏色强度（老电视感）")]
    public float colorShift = 0.05f;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float t = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);

        float brightness = Mathf.Lerp(minBrightness, maxBrightness, t);

        // 加一点点 RGB 不同步变化 → 像老电视的轻微色偏
        float r = brightness + Random.Range(-colorShift, colorShift);
        float g = brightness + Random.Range(-colorShift, colorShift);
        float b = brightness + Random.Range(-colorShift, colorShift);

        sr.color = new Color(r, g, b, 1f);
    }
}
