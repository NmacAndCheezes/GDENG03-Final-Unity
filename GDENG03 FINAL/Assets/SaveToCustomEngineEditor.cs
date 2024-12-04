using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneSaver))]
public class SaveToCustomEngineEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        SceneSaver t = (SceneSaver)this.target;
        if(GUILayout.Button("Save"))
        {
            t.Save();
        }

        SceneSaver sceneSaver = (SceneSaver)this.target;
        sceneSaver.path = GUILayout.TextArea(sceneSaver.path, 100);
        if (GUILayout.Button("Load"))
        {
            t.Load();
        }
    }


}
