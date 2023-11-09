using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class SetPieceSizeScript : MonoBehaviour
{
    public GameObject piece;

    void OnEnable()
    {
        string sizeTxt = this.gameObject.GetComponent<ButtonConfigHelper>().MainLabelText;
        if(sizeTxt == "Kicsi")
            piece.GetComponent<Transform>().localScale = new Vector3(0.03f, 0.015f, 0.03f);
        else if (sizeTxt == "Közepes")
            piece.GetComponent<Transform>().localScale = new Vector3(0.05f, 0.015f, 0.05f);
        else if (sizeTxt == "Nagy")
            piece.GetComponent<Transform>().localScale = new Vector3(0.07f, 0.015f, 0.07f);

        this.GetComponent<SetPieceSizeScript>().enabled = false;
    }

}
