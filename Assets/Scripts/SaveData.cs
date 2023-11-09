using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

#if WINDOWS_UWP
using System;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.Streams;
#endif

public class SaveData : MonoBehaviour
{
    public SavedGames sg;
    public Data data;

    private string filePath;



    public void Awake()
    {
        filePath = Application.persistentDataPath + "/SavedGames.json";
        if (!File.Exists(filePath))
        {
            var file = File.Create(filePath);
            StreamWriter sw = new StreamWriter(file);
            sw.WriteLine("{\"data\": [{\"Name\": \"asd0\",\"SizeOfPuzzle\": \"6 * 4\",\"Time\": \"14\",\"Picture\": \"asd\"}]}");
            sw.Flush();
            file.Close();
        }
    }

    public void OnEnable()
    {       
        this.GetComponent<SaveData>().enabled = false;
    }

    public void SaveToJSON(string username, string size, float time, string picName)
    {

        string gotData = "";
        gotData = System.IO.File.ReadAllText(filePath);

        sg = JsonUtility.FromJson<SavedGames>(gotData);
        //sg = new SavedGames();

        Data newData = new Data();
        newData.Name = username;
        newData.SizeOfPuzzle = size;
        newData.Time = time;
        newData.Picture = picName;
        Debug.Log(newData);
        sg.data.Add(newData);

        string savedGame = JsonUtility.ToJson(sg);
        Debug.Log(filePath);
        Debug.Log(savedGame);

        System.IO.File.WriteAllText(filePath, savedGame);
    }
    
}

[System.Serializable]
public class SavedGames
{
    public List<Data> data;
}

[System.Serializable]
public class Data
{
    public string Name;
    public string SizeOfPuzzle;
    public float Time;
    public string Picture;
}
