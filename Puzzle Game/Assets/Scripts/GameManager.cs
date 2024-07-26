using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform puzzleParent;
    [SerializeField] private Transform canvas;
    [SerializeField] private GameObject cornerPuzzle;
    [SerializeField] private GameObject sideTwoHolesPuzzle;
    [SerializeField] private GameObject sideOneHolePuzzle;
    [SerializeField] private GameObject innerPuzzle;

    private int puzzleBoardSize;
    private float singlePuzzleSize;
    int row = 0;
    int col = 0;


    private void Start()
    {
        puzzleBoardSize = 4;
        singlePuzzleSize = 1; //dopasuj na automatyczne!

        //creating board
        GeneratePuzzleBoard(puzzleBoardSize);
        puzzleParent.SetParent(canvas);
        puzzleParent.SetLocalPositionAndRotation(puzzleParent.position - new Vector3(300f,150f,0), Quaternion.identity); //na sztywno wpisane!
    }

    private void CreatePuzzle(GameObject puzzleType, int rotation)
    {
        if (col == puzzleBoardSize)
        {
            col = 0;
            row++;
        }
        col++;

        GameObject puzzle = Instantiate(puzzleType, puzzleParent);
        puzzle.transform.localPosition = new Vector3(col*1.25f, row*1.25f, 0f); //1.25 na razie wpisane na sztywno!
        puzzle.transform.localScale = new Vector3(singlePuzzleSize, singlePuzzleSize, 0f);
        puzzle.transform.rotation = Quaternion.Euler(0f, 0f, rotation);
    }

    private void GeneratePuzzleBoard(int size)
    {
        //pierwszy wiersz
        CreatePuzzle(cornerPuzzle, 90);
        for (int i = 0; i<(puzzleBoardSize-2)/2; i++)
        {
            CreatePuzzle(sideTwoHolesPuzzle, 180);
            CreatePuzzle(sideOneHolePuzzle, 180);
        }
        CreatePuzzle(cornerPuzzle, 180);

        //œrodkowe wiersze
        for(int j=0; j<(puzzleBoardSize-2)/2;j++)
        {
            CreatePuzzle(sideOneHolePuzzle, 90);
            for (int i = 0; i<(puzzleBoardSize-2)/2; i++)
            {
                CreatePuzzle(innerPuzzle, 90);
                CreatePuzzle(innerPuzzle, 0);
            }
            CreatePuzzle(sideTwoHolesPuzzle, -90);

            CreatePuzzle(sideTwoHolesPuzzle, 90);
            for (int i = 0; i<(puzzleBoardSize-2)/2; i++)
            {
                CreatePuzzle(innerPuzzle, 0);
                CreatePuzzle(innerPuzzle, 90);
            }
            CreatePuzzle(sideOneHolePuzzle, -90);
        }

        //ostatni wiersz
        CreatePuzzle(cornerPuzzle, 0);
        for (int i = 0; i<(puzzleBoardSize-2)/2; i++)
        {
            CreatePuzzle(sideOneHolePuzzle, 0);
            CreatePuzzle(sideTwoHolesPuzzle, 0);
        }
        CreatePuzzle(cornerPuzzle, -90);
    }
}
