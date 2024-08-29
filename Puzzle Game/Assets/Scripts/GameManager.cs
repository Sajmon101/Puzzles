using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI winPanelBigText;
    [SerializeField] private TextMeshProUGUI winPanelSmallText;
    [SerializeField] private Transform puzzlePile;
    [SerializeField] private Transform puzzleTargetPlace;
    [SerializeField] private Transform puzzleBackground;
    [SerializeField] private Transform canvas;
    [SerializeField] private GameObject cornerPuzzle;
    [SerializeField] private GameObject sideTwoHolesPuzzle;
    [SerializeField] private GameObject sideOneHolePuzzle;
    [SerializeField] private GameObject innerPuzzle;
    [SerializeField] private GameObject logo;
    [SerializeField] private int puzzleBoardSize = 4;
    [SerializeField] private Timer timer;

    private float boardSize;
    private float puzzleScale;
    private float puzzleOffset;
    int row = 0;
    int col = 0;
    private List<Transform> puzzlesList = new();
    public event EventHandler OnGameEnd;

    public static GameManager Instance;

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

        OnGameEnd += EndGameActions;

    }

    private void Start()
    {
        puzzleScale = 800f/puzzleBoardSize;
        puzzleOffset = 1.26f *puzzleScale;

        GeneratePuzzleBoard(puzzleBoardSize);
        MakePuzzlePile();
        SpawnRandomPuzzle();
    }

    private void CreatePuzzle(GameObject puzzleType, int rotation)
    {
        GameObject puzzle = Instantiate(puzzleType, puzzleTargetPlace);
        Vector3 leftDownBoardCorner = new Vector3((-puzzleBoardSize/2*puzzleOffset + 0.5f * puzzleOffset), (-puzzleBoardSize/2*puzzleOffset + 0.5f * puzzleOffset), 0f);
        puzzle.transform.localPosition = leftDownBoardCorner + new Vector3(col * puzzleOffset, row * puzzleOffset, 0f);
        puzzle.transform.localScale = new Vector3(puzzleScale, puzzleScale, 0f);
        puzzle.transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        puzzle.GetComponent<Puzzle>().correctPosition = puzzle.transform.position;

        //dodaj logo
        // Utwórz instancjê prefab
        GameObject logoObj = Instantiate(logo);
        logoObj.transform.SetParent(puzzleTargetPlace);
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
        // Tworzymy tymczasow¹ listê do przechowywania dzieci
        List<Transform> children = new List<Transform>();

        // Przechodzimy przez ka¿de dziecko w puzzleTargetPlace i dodajemy je do listy
        foreach (Transform child in puzzleTargetPlace)
        {
            children.Add(child);
        }

        // Teraz przenosimy ka¿de dziecko z listy do puzzlePile
        foreach (Transform childTransform in children)
        {
            childTransform.SetParent(puzzlePile.transform);
            childTransform.localPosition = Vector3.zero;
        }
    }


    public bool SpawnRandomPuzzle()
    {
        
        if (puzzlesList.Count == 0)
        {
            OnGameEnd?.Invoke(this, EventArgs.Empty);
            return false;  // Zwróæ null, jeœli lista jest pusta
        }

        // Losuj indeks z dostêpnych elementów
        int randomIndex = UnityEngine.Random.Range(0, puzzlesList.Count);

        // Pobierz wylosowany element
        Transform selectedPuzzle = puzzlesList[randomIndex];

        // Usuñ element z listy, aby nie móg³ byæ wylosowany ponownie
        puzzlesList.RemoveAt(randomIndex);

        // Zwróæ wylosowany GameObject
        selectedPuzzle.gameObject.SetActive(true);
        return true;
    }

    private void EndGameActions(object sender, EventArgs e)
    {
        winPanel.SetActive(true);
        winPanelBigText.text = "Gratulacje " + PlayerData.playerName + "!";
        winPanelSmallText.text = "Twój czas to: " + timer.GetCurrentTimerValue().minutes.ToString("00") + ":" + timer.GetCurrentTimerValue().seconds.ToString("00");
        AudioSource backgrounSound = AudioManager.Instance.GetSound(AudioManager.SoundName.BackgroundSong);
        backgrounSound.volume = 0.35f;
        AudioManager.Instance.Play(AudioManager.SoundName.WinSound);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(3);
    }
}
