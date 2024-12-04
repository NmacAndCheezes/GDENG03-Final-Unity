using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SceneSaver))]
public class SaveToCustomEngineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SceneSaver t = (SceneSaver)this.target;
        if(GUILayout.Button("Test"))
        {
            t.Save();
        }
    }


}
