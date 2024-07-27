using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform puzzleParent;
    [SerializeField] private Transform canvas;
    [SerializeField] private GameObject cornerPuzzle;
    [SerializeField] private GameObject sideTwoHolesPuzzle;
    [SerializeField] private GameObject sideOneHolePuzzle;
    [SerializeField] private GameObject innerPuzzle;

    private int puzzleBoardSize;
    private float boardSize;
    private float puzzleSize;
    int row = 0;
    int col = 0;


    private void Start()
    {
        puzzleSize = 1.25f;
        puzzleBoardSize = 12;
        boardSize = 6.0f/puzzleBoardSize;

        //creating board
        GeneratePuzzleBoard(puzzleBoardSize);
        puzzleParent.localScale = new Vector3(boardSize, boardSize, boardSize);
        puzzleParent.SetParent(canvas);

    }

    private void CreatePuzzle(GameObject puzzleType, int rotation)
    {


        GameObject puzzle = Instantiate(puzzleType, puzzleParent);
        puzzle.transform.localPosition = new Vector3(-puzzleBoardSize/2+ col * puzzleSize, -puzzleBoardSize/2 + row * puzzleSize, 0f);
        puzzle.transform.localScale = new Vector3(1f, 1f, 0f);
        puzzle.transform.rotation = Quaternion.Euler(0f, 0f, rotation);

        col++;
        if (col == puzzleBoardSize)
        {
            col = 0;
            row++;
        }
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
