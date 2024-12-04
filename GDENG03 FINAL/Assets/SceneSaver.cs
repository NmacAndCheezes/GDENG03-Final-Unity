using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using Object = UnityEngine.Object;

public class SceneSaver : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Save()
    {
        SceneData sData = new();
        Object[] objects = FindObjectsByType(typeof(GameObject), FindObjectsSortMode.InstanceID);
        sData.sceneName = SceneManager.GetActiveScene().name;
        List<GameObjectData> dataList = new();
        foreach (Object obj in objects)
        {
            GameObject go = obj as GameObject;
            if(go == this.gameObject) continue;

            GameObjectData goData = new GameObjectData();
            goData.Scale = go.transform.localScale;
            goData.Position = go.transform.localPosition;
            goData.Rotation = go.transform.localEulerAngles;
            goData.Name = go.name;
            goData.IsEnabled = go.activeInHierarchy;
            goData.Id = go.GetInstanceID();
            goData.parentID = go.transform.parent != null ? go.transform.parent.gameObject.GetInstanceID() : -1;

            if(go.TryGetComponent(out MeshFilter meshFilter))
            {
                goData.Mesh = new MeshData(meshFilter.sharedMesh);
            }
            dataList.Add(goData);
        }

        sData.GameObjects = dataList.ToArray();
        string data = JsonUtility.ToJson(sData, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/unityScene.level", data);
        Debug.Log($"Saved in {Application.persistentDataPath + "/unityScene.level"}");

    }
}

[Serializable]
public class SceneData
{
    public string sceneName;
    public GameObjectData[] GameObjects;
}

[Serializable]
public class MeshData
{
    public int[] triangles;
    public Vector3[] vertices;

    // add whatever properties of the mesh you need...

    public MeshData(Mesh mesh)
    {
        this.vertices = mesh.vertices;
        this.triangles = mesh.triangles;
        // further properties...
    }
}

[Serializable]
public class GameObjectData
{
    public int Id;
    public bool IsEnabled;
    public int parentID;
    public string Name;
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale;
    public MeshData Mesh;
}
