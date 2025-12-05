using UnityEngine;

public class PickUpDataFlag : MonoBehaviour
{
    public enum DataFlag
    {
        openingWatched,
        deskCleaned,
        notebookFound,
        lightFlewOutside,
        reflectionPuzzleDone,
        gotHintToGarden,
        gardenPuzzleDone,
        star_1_collected
    }

    [Header("拾取后更新的 GameData 变量")]
    public DataFlag flagToSet;

    [Header("玩家靠近时显示的提示文字（可放多个物件）")]
    public GameObject[] interactHints;

    [Header("拾取后是否隐藏物件（不销毁，只隐藏）")]
    public bool hideOnPick = true;

    [Header("拾取后要显示的物件（可选）")]
    public GameObject showAfterPick;

    private bool playerInRange = false;

    private void Start()
    {
        if (interactHints != null)
        {
            foreach (var hint in interactHints)
                if (hint != null) hint.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ApplyFlag();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactHints != null)
                foreach (var hint in interactHints)
                    if (hint != null) hint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactHints != null)
                foreach (var hint in interactHints)
                    if (hint != null) hint.SetActive(false);
        }
    }

    private void ApplyFlag()
    {
        GameData data = DataManager.Instance.data;

        switch (flagToSet)
        {
            case DataFlag.openingWatched: data.openingWatched = true; break;
            case DataFlag.deskCleaned: data.deskCleaned = true; break;
            case DataFlag.notebookFound: data.notebookFound = true; break;
            case DataFlag.lightFlewOutside: data.lightFlewOutside = true; break;
            case DataFlag.reflectionPuzzleDone: data.reflectionPuzzleDone = true; break;
            case DataFlag.gotHintToGarden: data.gotHintToGarden = true; break;
            case DataFlag.gardenPuzzleDone: data.gardenPuzzleDone = true; break;
            case DataFlag.star_1_collected: data.star_1_collected = true; break;
        }

        DataManager.Instance.Save();

        Debug.Log($"已更新 GameData：{flagToSet} = true");

        // 显示目标物件
        if (showAfterPick != null)
            showAfterPick.SetActive(true);

        // ⭐ 隐藏自己，不删除
        if (hideOnPick)
            gameObject.SetActive(false);
    }
}
