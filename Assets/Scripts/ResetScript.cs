using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using TMPro;

public class ResetScript : MonoBehaviour
{
    public GameObject plate;
    public GameObject hintPlate;
    public GameObject plateMenu;
    public GameObject counterObject;
    public GameObject timerObject;

    void OnEnable()
    {

        counterObject.GetComponent<CounterScript>().numOfPlacedPieces = 0;
        counterObject.GetComponent<TextMeshPro>().color = Color.white;
        counterObject.SetActive(false);

        timerObject.GetComponent<TimerScript>().currentTime = 0f;
        timerObject.SetActive(false);

        hintPlate.SetActive(false);
        plate.GetComponent<ObjectManipulator>().enabled = true;
        plate.GetComponent<RadialView>().enabled = true;
        plate.GetComponent<Transform>().localScale = new Vector3(0.2f, 0.01f, 0.1f);
        plate.GetComponent<Renderer>().material.color = Color.white;

        plateMenu.SetActive(true);

        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
        GameObject[] puzzlePositions = GameObject.FindGameObjectsWithTag("PuzzlePos");

        for (int i = 0; i < pieces.Length; i++)
        {
            Destroy(pieces[i]);
        }

        foreach (GameObject pPos in puzzlePositions)
        {
            Destroy(pPos);
        }



        this.GetComponent<ResetScript>().enabled = false;
    }
}
