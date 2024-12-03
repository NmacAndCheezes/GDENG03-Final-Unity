using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SceneSaver))]
public class SaveToCustomEngineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Test"))
        {
            Debug.Log("giid");
        }
    }
}
