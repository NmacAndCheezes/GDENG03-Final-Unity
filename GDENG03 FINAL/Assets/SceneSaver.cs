using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Object = UnityEngine.Object;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Android;

public class SceneSaver : MonoBehaviour
{
    public string saveName;
    public string path = "Hello World\nI've got 2 lines...";

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
        string directXPath = "C:\\Users\\Nathan\\Documents\\GitHub\\GDENG03-Scene-Editor-Group-1\\GDENG03-Activities";
        File.WriteAllText(directXPath + $"/{saveName}.level", data);
        Debug.Log($"Saved in {directXPath + $"/{saveName}.level"}");
    }

    public void Load()
    {
        
        string fullpath = "C:\\Users\\Nathan\\Documents\\GitHub\\GDENG03-Scene-Editor-Group-1\\GDENG03-Activities\\" + path;
        Object[] objects = FindObjectsByType(typeof(GameObject), FindObjectsSortMode.InstanceID);
        foreach (var v in objects)
        {
            if (v.GameObject() == this.gameObject) continue;
            DestroyImmediate(v);
        }
        string data = File.ReadAllText(fullpath);
        JObject sceneData = JObject.Parse(data);

        Debug.Log($"Loading {path}");
        List<JToken> jObj = sceneData["GameObjects"].Children().ToList();
        foreach(JToken obj in jObj)
        {
            GameObject go = new GameObject();
            go.name = obj["Name"].ToString() + "\n";
            string result = go.name;
            //go.SetActive((bool)sceneData["IsEnabled"]);
            go.transform.localPosition = new Vector3(
                (float)obj["Position"]["x"],
                (float)obj["Position"]["y"],
                (float)obj["Position"]["z"]
                );
            go.transform.localEulerAngles = new Vector3(
                (float)obj["Rotation"]["x"],
                (float)obj["Rotation"]["y"],
                (float)obj["Rotation"]["z"]
                );
            go.transform.localScale = new Vector3(
                (float)obj["Scale"]["x"],
                (float)obj["Scale"]["y"],
                (float)obj["Scale"]["z"]
                );

            Mesh mesh = new Mesh();
            mesh.name = obj["Name"].ToString();
            JArray jVertices = obj["vertices"] as JArray;
            List<Vector3> vertices = new();
            foreach (JToken v in jVertices)
            {
                Vector3 vec = new Vector3((float)v["x"], (float)v["y"], (float)v["z"]);
                result += vec.ToString() + "\n";
                vertices.Add(vec);
                
            }
            mesh.vertices = vertices.ToArray();

            JArray jIndices = obj["indices"] as JArray;
            List<int> indices = new();
            foreach (JToken i in jIndices)
            {
                indices.Add((int)i);
                result += i.ToString() + "\n";
            }
            mesh.triangles = indices.ToArray();
            go.AddComponent<MeshFilter>().sharedMesh = mesh;
            go.AddComponent<MeshRenderer>();
            Debug.Log(result);
        }

        
        //mesh.SetVertices();
    }
}

[Serializable]
public class SceneData
{
    public string sceneName;
    public GameObjectData[] GameObjects;
}

[Serializable]
public class ImportedScene
{
    public string sceneName;
    public ImportedGO[] GameObjects;
    public class ImportedGO
    {
        public string Name;
        public bool IsEnabled;
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
    }
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
