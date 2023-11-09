using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterScript : MonoBehaviour
{

    [HideInInspector]
    public int numOfPlacedPieces;

    // Start is called before the first frame update
    void Awake()
    {
        numOfPlacedPieces = 0;
    }
}
