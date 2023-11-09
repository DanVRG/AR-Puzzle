using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class PuzzleManager : MonoBehaviour
{
    [HideInInspector]
    GameObject plate;

    [HideInInspector]
    GridGenerator gg;

    [HideInInspector]
    GameObject counterObject;

    [HideInInspector]
    CounterScript cs;

    [HideInInspector]
    GameObject timerObject;

    [HideInInspector]
    TimerScript ts;

    SaveData sd;

    private void Awake()
    {
        plate = GameObject.Find("Plate");
        gg = plate.GetComponent<GridGenerator>();

        counterObject =  GameObject.Find("CounterTMP");
        cs = counterObject.GetComponent<CounterScript>();

        timerObject = GameObject.Find("TimerTMP");
        ts = timerObject.GetComponent<TimerScript>();

        sd = GameObject.Find("SaveObject").GetComponent<SaveData>();

        counterObject.GetComponent<TextMeshPro>().text = gg.numOfPieces +"/"+ cs.numOfPlacedPieces;
    }
    void OnEnable()
    {
        Object[] allObjects = Object.FindObjectsOfType<GameObject>();     

        /*
        for (int i = 0; i < gg.pieces.Count; i++)
        {
            Debug.Log(i + ".: " + gg.pieces[i].GetInstanceID());
        }

        Debug.Log("This: " + this.gameObject.GetInstanceID());
        */

        Vector3 piecePos = this.gameObject.transform.position;

        bool stop = false;

        foreach (GameObject go in allObjects)
        {
            if (go.tag.Equals("PuzzlePos"))
            {
                Vector3 puzzPos = go.transform.position;
                Vector3 puzzScale= go.transform.localScale;
                float maxPuzzPosX = puzzPos.x + puzzScale.x / 2;
                float minPuzzPosX = puzzPos.x - puzzScale.x / 2;
                float maxPuzzPosY = puzzPos.y + puzzScale.y / 2;
                float minPuzzPosY = puzzPos.y - puzzScale.y / 2;
                float maxPuzzPosZ = puzzPos.z + puzzScale.z / 2;
                float minPuzzPosZ = puzzPos.z - puzzScale.z / 2;

                if (piecePos.x < maxPuzzPosX && piecePos.x > minPuzzPosX)
                    if (piecePos.y < maxPuzzPosY && piecePos.y > minPuzzPosY)
                        if (piecePos.z < maxPuzzPosZ && piecePos.z > minPuzzPosZ)
                        {
                            this.gameObject.transform.position = go.transform.position;
                            this.gameObject.transform.rotation = go.transform.rotation;
                            //Debug.Log(go.name + " " + go.transform.position.ToString("F3") + ", id: " + go.GetInstanceID() + ", size: " + go.transform.localScale.ToString("F3"));

                            GameObject counterObject = GameObject.Find("CounterTMP");
                            if (gg.positions.IndexOf(go) == gg.pieces.IndexOf(this.gameObject))
                            {
                                this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                                this.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
                                this.gameObject.GetComponent<ObjectManipulator>().enabled = false;
                                Destroy(go);
                                cs.numOfPlacedPieces++;
                                counterObject.GetComponent<TextMeshPro>().text = gg.numOfPieces + "/" + cs.numOfPlacedPieces;
                                stop = true;

                                //Check if game is finished
                                if (gg.numOfPieces == cs.numOfPlacedPieces)
                                {
                                    counterObject.GetComponent<TextMeshPro>().color = Color.green;
                                    ts.enabled = false;

                                    string username = GameObject.Find("UsernameOutput").GetComponent<TextMeshPro>().text;
                                    string size = GameObject.Find("SizeOutput").GetComponent<TextMeshPro>().text;
                                    float time = ts.currentTime;
                                    string picName = GameObject.Find("FileBrowserBtn").GetComponent<FilePicker>().nameOfPic;
                                    sd.SaveToJSON(username, size, time, picName);

                                    //Refresh leaderboard
                                    GameObject lb = GameObject.Find("Leaderboard");
                                    FilePicker fp = GameObject.Find("FileBrowserBtn").GetComponent<FilePicker>();
                                    GameObject[] dataLines = GameObject.FindGameObjectsWithTag("Dataline");
                                    foreach (GameObject d in dataLines)
                                    {
                                        Destroy(d);
                                    }
                                    lb.GetComponent<LoadLeaderboardScript>().nameOfPicture = fp.nameOfPic;
                                    lb.GetComponent<LoadLeaderboardScript>().enabled = true;
                                }            
                            }
                        }
            }
        }

        if(!stop)
            this.GetComponent<PuzzleManager>().enabled = false;
    }
}
