using UnityEngine;

public class StarReturn : MonoBehaviour
{
    public Transform targetPoint;
    public Transform returnPoint;
    public float speed = 5f;

    public GameObject objectToActivate;

    private bool goingOut = false;   // ⭐默认不飞
    private bool finished = false;

    void Update()
    {
        if (!goingOut || finished) return;

        if (goingOut && !finished)
        {
            // 正在往外飞
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPoint.position,
                speed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
            {
                // 到达外点 → 开始飞回来
                goingOut = false;
                StartReturn();
            }
        }
    }

    void StartReturn()
    {
        StartCoroutine(ReturnRoutine());
    }

    System.Collections.IEnumerator ReturnRoutine()
    {
        while (Vector3.Distance(transform.position, returnPoint.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                returnPoint.position,
                speed * Time.deltaTime
            );
            yield return null;
        }

        finished = true;

        // 1️⃣ 开启目标物件
        if (objectToActivate != null)
            objectToActivate.SetActive(true);

        // ⭐ 2️⃣ 更新 GameData（星星已飞出去）
        DataManager.Instance.data.lightFlewOutside = true;
        DataManager.Instance.Save();
        Debug.Log("已更新 GameData.lightFlewOutside = true");

        // ⭐⭐ 3️⃣ 通知 Scene2Controller：“光飞出去了，请开始你的后续流程”
        // 这是唯一关键！你必须加这一行！
        Scene2Controller controller = FindFirstObjectByType<Scene2Controller>();
        if (controller != null)
        {
            controller.OnLightFlyOutside();
        }
        else
        {
            Debug.LogError("❌ Scene2Controller 未找到，无法通知光飞出去事件！");
        }

        // 4️⃣ 把自己关掉
        gameObject.SetActive(false);

        Debug.Log("星星飞回来 → 开启物件 + 自己关闭");
    }

    // ⭐外部触发飞行
    public void StartFly()
    {
        goingOut = true;
        finished = false;
    }
}
