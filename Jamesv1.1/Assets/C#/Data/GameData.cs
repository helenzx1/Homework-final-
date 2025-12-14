[System.Serializable]
public class GameData
{
    // 📌 当前所在场景编号（用于场景进度储存）
    public int currentScene = 1;

    // 📌 是否已经观看开场剧情（避免重复播放）
    public bool openingWatched = false;

    // 📌 桌面是否已经整理（书本被拾取后设为 true）
    public bool deskCleaned = false;

    // 📌 是否已经找到笔记本（Notebook）
    public bool notebookFound = false;

    // 📌 星星是否已经飞出去（StarReturn → 更新 true）
    public bool lightFlewOutside = false;

    // 📌 镜子反射谜题是否完成
    public bool reflectionPuzzleDone = false;

    // 📌 是否已经获得去花园的提示
    public bool gotHintToGarden = false;

    // 📌 花园谜题是否完成
    public bool gardenPuzzleDone = false;

    // 📌 第一颗星星是否被收集（用于星星图鉴或进度）
    public bool star_1_collected = false;
}
