using UnityEngine;

[ExecuteAlways]  // 允许编辑模式就能影响粒子
[RequireComponent(typeof(ParticleSystem))]
public class ParticleDirectionController : MonoBehaviour
{
    [Header("粒子飞行方向 (角度)")]
    public Vector3 directionEuler = new Vector3(0, 0, 0);

    private ParticleSystem ps;
    private ParticleSystem.ShapeModule shape;

    void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
        shape = ps.shape;
        ApplyDirection();
    }

    void Update()
    {
        ApplyDirection();  // 编辑模式 & 运行模式 都实时更新
    }

    private void ApplyDirection()
    {
        shape.rotation = directionEuler;
    }
}
