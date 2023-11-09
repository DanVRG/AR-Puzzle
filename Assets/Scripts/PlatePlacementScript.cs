using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlatePlacementScript : MonoBehaviour
{
    public GameObject plate;
    public GameObject plateMenu;
    public GameObject sizePiece;
    public TextMeshPro numberOfPiecesObject;
    public TextMeshPro errorObject;

    void OnEnable()
    {
        float pieceSpacing = sizePiece.transform.localScale.z * 1.4f;
        numberOfPiecesObject.text.Trim();
        bool isPicSelected = GameObject.Find("FileBrowserBtn").GetComponent<FilePicker>().isPicSelected;
        if (isPicSelected)
        {
            try
            {
                int numOfRows = int.Parse(numberOfPiecesObject.text.Split('*')[1]);
                int numOfCols = int.Parse(numberOfPiecesObject.text.Split('*')[0]);

                plate.SetActive(true);
                plateMenu.SetActive(true);

                plate.transform.localScale = new Vector3(pieceSpacing * numOfCols * 2.2f, sizePiece.transform.localScale.y * 0.5f, pieceSpacing * numOfRows * 1.3f);
                plate.GetComponent<Renderer>().material.color = Color.grey;
                errorObject.text = "";
            }
            catch (Exception e)
            {
                errorObject.text = "A méret értéke helytelenül lett megadva. Szabályos forma pl.: 6*4";
            }
            finally
            {
                this.GetComponent<PlatePlacementScript>().enabled = false;
            }
        }
        else
        {
            errorObject.text = "Nincs kiválasztva kép!";
            this.GetComponent<PlatePlacementScript>().enabled = false;
        }
        
    }

}
