using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneSaver))]
public class SaveToCustomEngineEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        SceneSaver t = (SceneSaver)this.target;
        
        t.saveName = GUILayout.TextArea(t.saveName, 100);
        if (GUILayout.Button("Save"))
        {
            t.Save();
        }
        t.path = GUILayout.TextArea(t.path, 100);
        if (GUILayout.Button("Load"))
        {
            t.Load();
        }
        EditorGUI.EndChangeCheck();
    }


}
