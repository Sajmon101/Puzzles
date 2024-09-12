using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private GameObject cornerPuzzle;
    [SerializeField] private GameObject sideTwoHolesPuzzle;
    [SerializeField] private GameObject sideOneHolePuzzle;
    [SerializeField] private GameObject innerPuzzle;
    [SerializeField] private Transform puzzleTargetPlace;
    [SerializeField] private GameObject logo;

    private float puzzleBoardSize;
    public event EventHandler OnGameVictory;
    private float boardSize;
    private float puzzleScale;
    private float puzzleOffset;
    Vector3 leftDownBoardCorner;
    int row = 0;
    int col = 0;
    private List<Transform> puzzlesList = new();
    public static PuzzleManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        puzzleBoardSize = GetPuzzleBoardSize();
        puzzleScale = 800f/puzzleBoardSize;
        puzzleOffset = 1.26f *puzzleScale;
        leftDownBoardCorner = new Vector3((-puzzleBoardSize/2*puzzleOffset + 0.5f * puzzleOffset), (-puzzleBoardSize/2*puzzleOffset + 0.5f * puzzleOffset), 0f);

        GeneratePuzzleBoard((int)puzzleBoardSize);
        MakePuzzlePile();
        SpawnRandomPuzzle();
    }

    private void CreatePuzzle(GameObject puzzleType, int rotation)
    {
        GameObject puzzle = Instantiate(puzzleType, puzzleTargetPlace);
        puzzle.transform.localPosition = leftDownBoardCorner + new Vector3(col * puzzleOffset, row * puzzleOffset, 0f);
        puzzle.transform.localScale = new Vector3(puzzleScale, puzzleScale, 0f);
        puzzle.transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        puzzle.GetComponent<Puzzle>().correctPosition = puzzle.transform.position;

        GameObject logoObj = Instantiate(logo);
        logoObj.transform.SetParent(puzzleTargetPlace);
        logoObj.transform.localPosition = Vector3.zero;
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
        for (int j = 0; j<(puzzleBoardSize-2)/2; j++)
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
        List<Transform> children = new List<Transform>();

        foreach (Transform child in puzzleTargetPlace)
        {
            children.Add(child);
        }

        foreach (Transform childTransform in children)
        {
            childTransform.SetParent(transform);
            childTransform.localPosition = Vector3.zero;
        }
    }


    public bool SpawnRandomPuzzle()
    {

        if (puzzlesList.Count == 0)
        {
            OnGameVictory?.Invoke(this, EventArgs.Empty);
            return false; 
        }

        int randomIndex = UnityEngine.Random.Range(0, puzzlesList.Count);

        Transform selectedPuzzle = puzzlesList[randomIndex];

        puzzlesList.RemoveAt(randomIndex);

        selectedPuzzle.gameObject.SetActive(true);
        return true;
    }

    public float GetPuzzleBoardSize()
    {
        if (PlayerPrefs.HasKey("SliderValue"))
        {
            float sliderValue = PlayerPrefs.GetFloat("SliderValue");

            puzzleBoardSize = 4f + sliderValue * 2;

            return puzzleBoardSize;
        }

        return 0;
    }
}
