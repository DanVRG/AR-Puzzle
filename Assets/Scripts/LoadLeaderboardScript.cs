using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

#if WINDOWS_UWP
using System;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.Streams;
#endif

public class LoadLeaderboardScript : MonoBehaviour
{
    SavedGames sg;

    public GameObject dataLinePrefab;
    public GameObject startingPoint;
    public GameObject thumbnail;
    public TextMeshPro errorObject;

    [HideInInspector]
    public string nameOfPicture;

    // Start is called before the first frame update
    void OnEnable()
    {
        sg = GameObject.Find("SaveObject").GetComponent<SaveData>().sg;
        SetImg();
        GenerateLines();
        this.GetComponent<LoadLeaderboardScript>().enabled = false;
    }

    public void LoadFromJSON()
    {     
        string gotData = "";
        string filePath = Application.persistentDataPath + "/SavedGames.json";
        gotData = System.IO.File.ReadAllText(filePath);

        sg = JsonUtility.FromJson<SavedGames>(gotData);        
    }

    private void GenerateLines()
    {
        LoadFromJSON();
        

        GameObject sizeObj = GameObject.Find("SizeOutput");
        string sizeStr = sizeObj.GetComponent<TextMeshPro>().text;
        try
        {
            errorObject.text = "";

            string sizeX = sizeStr.Split('*')[0];
            string sizeY = sizeStr.Split('*')[1];
            string sizeReversed = sizeY + "*" + sizeX;

            GameObject lbSizeTxt = GameObject.Find("LeaderboardSize");
            lbSizeTxt.GetComponent<TextMeshPro>().text = "Méret: " + sizeStr + " v. " + sizeReversed;
            if (sizeStr == sizeReversed)
                lbSizeTxt.GetComponent<TextMeshPro>().text = "Méret: " + sizeStr;

            GameObject sm = GameObject.Find("SetupMenu");
            float offSet = 0;

            sg.data.Sort(((x, y) => x.Time.CompareTo(y.Time)));

            Vector3 startPos = startingPoint.transform.position;
            int rank = 1;
            foreach (Data d in sg.data)
            {
                if (d.Picture == nameOfPicture && (d.SizeOfPuzzle == sizeStr || d.SizeOfPuzzle == sizeReversed))
                {
                    GameObject dataLine = Instantiate(dataLinePrefab, new Vector3(startPos.x, startPos.y - offSet, startPos.z), startingPoint.transform.rotation, sm.transform);
                    dataLine.transform.GetChild(0).GetComponent<TextMeshPro>().text = d.Name;
                    dataLine.transform.GetChild(1).GetComponent<TextMeshPro>().text = d.SizeOfPuzzle;

                    float minutes = Mathf.FloorToInt(d.Time / 60);
                    float seconds = Mathf.FloorToInt(d.Time % 60);
                    string outputTime = string.Format("{00:00}:{1:00}", minutes, seconds);
                    dataLine.transform.GetChild(2).GetComponent<TextMeshPro>().text = outputTime;

                    dataLine.transform.GetChild(3).GetComponent<TextMeshPro>().text = d.Picture;
                    dataLine.transform.GetChild(4).GetComponent<TextMeshPro>().text = rank.ToString();
                    offSet += 0.03f;
                    rank++;

                    if (rank > 10)
                        break;
                }
            }
        }
        catch (Exception e)
        {
            errorObject.text = "A méret értéke helytelenül lett megadva. Szabályos forma pl.: 6*4";
        }
    }

    private void SetImg()
    {
        thumbnail.SetActive(true);
        Texture2D img = GameObject.Find("FileBrowserBtn").GetComponent<FilePicker>().customTexture;
        float ratio = (float)img.width / (float)img.height;
        float size = 0.3f;
        thumbnail.GetComponent<Transform>().localScale = new Vector3(size * ratio, size, 0.01f);
        Material mat = new Material(Shader.Find("Standard"));
        mat.SetTexture("_MainTex", img);
        thumbnail.GetComponent<MeshRenderer>().material = mat;
    }

}
