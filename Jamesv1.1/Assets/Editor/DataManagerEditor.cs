using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DataManager))]
public class DataManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DataManager manager = (DataManager)target;

        GUILayout.Space(10);

        if (GUILayout.Button("清除存档（Reset Save）"))
        {
            manager.ResetData();
        }
    }
}
