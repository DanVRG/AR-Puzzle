using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadData : MonoBehaviour
{
    Data data;

    public void OnEnable()
    {
        data = GameObject.Find("SaveBtn").GetComponent<SaveData>().data;
        LoadFromJSON();
        this.GetComponent<LoadData>().enabled = false;
    }

    public void LoadFromJSON()
    {
        
        string filePath = Application.persistentDataPath + "/SavedData.json";
        string gotData = System.IO.File.ReadAllText(filePath);

        data = JsonUtility.FromJson<Data>(gotData);
        GameObject.Find("LoadOutput").GetComponent<TextMeshPro>().text = data.Name +": "+data.SizeOfPuzzle;
    }

}
