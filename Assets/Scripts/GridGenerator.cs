using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject piecePrefab;
    public GameObject puzzPosPrefab;
    public GameObject puzzlePositionPrefab;
    public GameObject plate;
    public GameObject pieceStartingPointObject;
    public GameObject puzzleStartingPointObject;
    public GameObject hintPlate;
    public GameObject counterObject;
    public GameObject timerObject;
    public GameObject sizePiece;

    public TextMeshPro numberOfPiecesObject;
    public TextMeshPro sizeOfPieces;

    public Texture2D myTexture;

    [HideInInspector]
    public List<GameObject> pieces;
    [HideInInspector]
    public List<GameObject> positions;
    [HideInInspector]
    public int numOfPieces;

    private int numOfRows, numOfCols;

    private Vector3 pieceStartingPoint, puzzleStartingPoint;

    private void OnEnable()
    {
        //Setting up size of grid
        numberOfPiecesObject.text.Trim();
        numOfRows = int.Parse(numberOfPiecesObject.text.Split('*')[1]);
        numOfCols = int.Parse(numberOfPiecesObject.text.Split('*')[0]);
        numOfPieces = this.numOfCols * this.numOfRows;

        //Activate counter
        counterObject.SetActive(true);
        timerObject.SetActive(true);
        timerObject.GetComponent<TimerScript>().enabled = true;
        timerObject.GetComponent<TimerScript>().currentTime = 0f;

        //Initializing arrays
        pieces = new List<GameObject>(numOfPieces);
        positions = new List<GameObject>(numOfPieces);


        //Resize test
        Vector3 size = sizePiece.transform.localScale;
        piecePrefab.GetComponent<Transform>().localScale = new Vector3(size.x, size.y, size.z);
        puzzlePositionPrefab.GetComponent<Transform>().localScale = new Vector3(size.x, size.y, size.z);

        //Grid
        float pieceSpacing = piecePrefab.transform.localScale.z*1.4f;
        float puzzleSpacing = piecePrefab.transform.localScale.z;
        float rotationAngle = plate.transform.rotation.eulerAngles.y;

        //Tabel resize
        this.transform.localScale = new Vector3(pieceSpacing*numOfCols*2.2f, piecePrefab.transform.localScale.y*0.5f, pieceSpacing*numOfRows*1.3f);
        this.GetComponent<Renderer>().material.color = Color.grey;
        Debug.Log("Trasformed");


        //Setting grid startingpoints
        pieceStartingPoint = pieceStartingPointObject.transform.position;
        puzzleStartingPoint = puzzleStartingPointObject.transform.position;

        // Convert the angle to radians
        float rotationRadians = -rotationAngle * (float)Mathf.PI / 180.0f;
        float cosTheta = (float)Mathf.Cos(rotationRadians);
        float sinTheta = (float)Mathf.Sin(rotationRadians);

        // Calculate the coordinates of the grid points
        float[] pieceXCoords = new float[numOfCols];
        float[] pieceZCoords = new float[numOfRows];
        float[] puzzleXCoords = new float[numOfCols];
        float[] puzzleZCoords = new float[numOfRows];
        for (int i = 0; i < numOfCols; i++)
        {
            pieceXCoords[i] = (i * pieceSpacing) + pieceStartingPoint.x;
            puzzleXCoords[i] = (i * puzzleSpacing) + puzzleStartingPoint.x;
        }
        for (int j = 0; j < numOfRows; j++)
        {
            pieceZCoords[j] = (j * pieceSpacing) + pieceStartingPoint.z;
            puzzleZCoords[j] = (j * puzzleSpacing) + puzzleStartingPoint.z;

        }

        //Setting up the Texture
        Texture2D customTexture = GameObject.Find("FileBrowserBtn").GetComponent<FilePicker>().customTexture;

        HintplateSetup(customTexture);

        //Subdividing the image
        Material[] customMaterials = SubdivideTexture(customTexture);      

        int pieceNumber = 0;

        // Instantiate the prefabs at each point in the grid
        for (int i = 0; i < numOfCols; i++)
        {
            for (int j = 0; j < numOfRows; j++)
            {
                float rotatedPuzzleX = (puzzleXCoords[i] - puzzleStartingPoint.x) * cosTheta - (puzzleZCoords[j] - puzzleStartingPoint.z) * sinTheta + puzzleStartingPoint.x;
                float rotatedPuzzleZ = (puzzleXCoords[i] - puzzleStartingPoint.x) * sinTheta + (puzzleZCoords[j] - puzzleStartingPoint.z) * cosTheta + puzzleStartingPoint.z;

                // Instantiate the prefabs at the rotated point
                GameObject piece = Instantiate(piecePrefab, new Vector3(0, pieceStartingPoint.y + 0.01f, 0), plate.transform.rotation);
                piece.transform.GetChild(0).GetComponent<MeshRenderer>().material = customMaterials[pieceNumber];
                pieces.Add(piece);
                
                GameObject pos = Instantiate(puzzlePositionPrefab, new Vector3(rotatedPuzzleX, puzzleStartingPoint.y, rotatedPuzzleZ), plate.transform.rotation);
                positions.Add(pos);

                pieceNumber++;
            }
        }

        PlacePiecesInRandomOrder(pieceXCoords,pieceZCoords,cosTheta,sinTheta);

        
        Debug.Log("Placed");

        this.GetComponent<GridGenerator>().enabled = false;
    }

    private Material[] SubdivideTexture(Texture2D customTexture)
    {
        Material[] customMats = new Material[numOfPieces];
        int pieceNumber = 0;
        Texture2D[] subImages = new Texture2D[numOfPieces];
        int subWidth = customTexture.width / numOfCols;
        int subHeight = customTexture.height / numOfRows;

        for (int i = 0; i < numOfCols; i++)
        {
            for (int j = 0; j < numOfRows; j++)
            {
                Texture2D subImg = new Texture2D(subWidth, subHeight);
                subImg.SetPixels(customTexture.GetPixels(i * subWidth, j * subHeight, subWidth, subHeight));
                subImg.Apply();
                subImages[pieceNumber] = subImg;
                Material newMat = new Material(Shader.Find("Standard"));
                newMat.SetTexture("_MainTex", subImages[pieceNumber]);
                customMats[pieceNumber] = newMat;
                pieceNumber++;
            }
        }

        return customMats;
    }

    private void HintplateSetup(Texture2D customTexture)
    {
        //Setting up Hintplate
        hintPlate.SetActive(true);
        float ratio = (float)customTexture.width / (float)customTexture.height;
        Debug.Log(customTexture.width + "*" + customTexture.height);
        Debug.Log(ratio);

        float hintSize = 0.4f;
        Debug.Log(hintSize);
        hintPlate.GetComponent<Transform>().localScale = new Vector3(hintSize * ratio, hintSize, 0.01f);
        Debug.Log(hintPlate.transform.localScale);

        Vector3 plateStartPos = GameObject.Find("PlateStartPoint").transform.position;
        hintPlate.GetComponent<Transform>().position = new Vector3(plateStartPos.x, plateStartPos.y + hintSize * 0.7f, plateStartPos.z+0.02f);
        hintPlate.GetComponent<Transform>().rotation = GameObject.Find("PlateStartPoint").transform.rotation;

        Material hintMaterial = new Material(Shader.Find("Standard"));
        hintMaterial.SetTexture("_MainTex", customTexture);
        GameObject.Find("HintQuad").GetComponent<MeshRenderer>().material = hintMaterial;
    }

    private void PlacePiecesInRandomOrder(float[] pieceXCoords, float[] pieceZCoords, float cosTheta, float sinTheta)
    {
        List<GameObject> tempPieces = new List<GameObject>(pieces);
        for (int i = 0; i < numOfCols; i++)
        {
            for (int j = 0; j < numOfRows; j++)
            {
                // Apply the rotation matrix to each point
                float rotatedPieceX = (pieceXCoords[i] - pieceStartingPoint.x) * cosTheta - (pieceZCoords[j] - pieceStartingPoint.z) * sinTheta + pieceStartingPoint.x;
                float rotatedPieceZ = (pieceXCoords[i] - pieceStartingPoint.x) * sinTheta + (pieceZCoords[j] - pieceStartingPoint.z) * cosTheta + pieceStartingPoint.z;
                int id = Random.Range(0, tempPieces.Count);

                tempPieces[id].transform.position = new Vector3(rotatedPieceX, pieceStartingPoint.y + 0.01f, rotatedPieceZ);
                tempPieces.RemoveAt(id);
            }
        }
    }
}