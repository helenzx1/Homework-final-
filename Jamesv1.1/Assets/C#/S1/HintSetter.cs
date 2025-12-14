using UnityEngine;

public class HintSetter : MonoBehaviour
{
    // 按钮点击会调用这个函数
    public void SetHintGot()
    {
        DataManager.Instance.data.gotHintToGarden = true;
        DataManager.Instance.Save();

        Debug.Log("🌿 已达成：gotHintToGarden = true");
    }
}
