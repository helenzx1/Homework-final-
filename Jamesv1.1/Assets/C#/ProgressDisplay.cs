using UnityEngine;
using TMPro;

public class ProgressDisplay : MonoBehaviour
{
    public TextMeshProUGUI txt;

    GameData data;

    void Start()
    {
        data = DataManager.Instance.data;
        UpdateDisplay();
    }

    void Update()
    {
        UpdateDisplay();   // 每帧更新（安全且轻量）
    }

    void UpdateDisplay()
    {
        if (txt == null || data == null) return; // ⭐止血关键

        txt.text =
            "Progress:\n" +
            "Scene ID: " + data.currentScene + "\n" +
            "Opening Watched: " + BoolToEN(data.openingWatched) + "\n" +
            "Desk Cleaned: " + BoolToEN(data.deskCleaned) + "\n" +
            "Notebook Found: " + BoolToEN(data.notebookFound) + "\n" +
            "Light Flew Outside: " + BoolToEN(data.lightFlewOutside) + "\n" +
            "Mirror Puzzle Done: " + BoolToEN(data.reflectionPuzzleDone) + "\n" +
            "Garden Hint Received: " + BoolToEN(data.gotHintToGarden) + "\n" +
            "Garden Puzzle Done: " + BoolToEN(data.gardenPuzzleDone) + "\n" +
            "Star 1 Collected: " + BoolToEN(data.star_1_collected);
    }


    string BoolToEN(bool value)
    {
        return value ? "Yes" : "No";
    }
}
