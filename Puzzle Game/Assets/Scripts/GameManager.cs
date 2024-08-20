using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform puzzleParent;
    [SerializeField] private Transform canvas;
    [SerializeField] private GameObject cornerPuzzle;
    [SerializeField] private GameObject sideTwoHolesPuzzle;
    [SerializeField] private GameObject sideOneHolePuzzle;
    [SerializeField] private GameObject innerPuzzle;
    [SerializeField] private GameObject logo;
    [SerializeField] private int puzzleBoardSize = 4;

    private float boardSize;
    private float puzzleScale;
    private float puzzleOffset;
    int row = 0;
    int col = 0;
    private List<Transform> puzzlesList = new();


    private void Start()
    {
        puzzleScale = 800f/puzzleBoardSize;
        puzzleOffset = 1.26f *puzzleScale;

        GeneratePuzzleBoard(puzzleBoardSize);
        MakePuzzlePile();
        GetRandomPuzzle()?.SetActive(true);
    }

    private void CreatePuzzle(GameObject puzzleType, int rotation)
    {
        GameObject puzzle = Instantiate(puzzleType, puzzleParent);
        Vector3 leftDownBoardCorner = new Vector3((-puzzleBoardSize/2*puzzleOffset + 0.5f * puzzleOffset), (-puzzleBoardSize/2*puzzleOffset + 0.5f * puzzleOffset), 0f);
        puzzle.transform.localPosition = leftDownBoardCorner + new Vector3(col * puzzleOffset, row * puzzleOffset, 0f);
        puzzle.transform.localScale = new Vector3(puzzleScale, puzzleScale, 0f);
        puzzle.transform.rotation = Quaternion.Euler(0f, 0f, rotation);

        //dodaj logo
        // Utwórz instancjê prefab
        GameObject logoObj = Instantiate(logo);
        logoObj.transform.SetParent(puzzleParent);
        logoObj.transform.localPosition = Vector3.zero;

        // Ustaw rodzica dla nowo utworzonego obiektu
        logoObj.transform.SetParent(puzzle.transform);
        logoObj.transform.localScale = new Vector3(0.175f * puzzleBoardSize, 0.175f * puzzleBoardSize, 1.0f); //na sztywno na razie!

        puzzlesList.Add(puzzle.transform);
        puzzle.SetActive(false);

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

    void MakePuzzlePile()
    {
        foreach(Transform transform in puzzleParent)
        {
            transform.localPosition = Vector3.zero;
        }
    }

    GameObject GetRandomPuzzle()
    {
        if (puzzlesList.Count == 0)
        {
            Debug.LogWarning("No more puzzle pieces to draw.");
            return null;  // Zwróæ null, jeœli lista jest pusta
        }

        // Losuj indeks z dostêpnych elementów
        int randomIndex = UnityEngine.Random.Range(0, puzzlesList.Count);

        // Pobierz wylosowany element
        Transform selectedPuzzle = puzzlesList[randomIndex];

        // Usuñ element z listy, aby nie móg³ byæ wylosowany ponownie
        puzzlesList.RemoveAt(randomIndex);

        // Zwróæ wylosowany GameObject
        return selectedPuzzle.gameObject;
    }
}
