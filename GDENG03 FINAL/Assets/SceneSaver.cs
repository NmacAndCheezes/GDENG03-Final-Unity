using UnityEngine;

public class SceneSaver : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Save()
    {
        Object[] objects = FindObjectsByType(typeof(GameObject), FindObjectsSortMode.InstanceID);

    }
}
