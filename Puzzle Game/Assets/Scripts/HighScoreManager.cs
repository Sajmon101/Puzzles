using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using static Timer;
using System;

public class HighscoreManager : MonoBehaviour
{
    private string filePath;
    [SerializeField] private Timer timer;

    void Awake()
    {
        filePath = Application.persistentDataPath + "/highscores.json";
    }

    private void Start()
    {
        PuzzleManager.Instance.OnGameVictory += OnGameEnd;
    }

    private void OnGameEnd(object sender, System.EventArgs e)
    {
        string playerName = PlayerData.playerName; 
        TimerValue finishTime = timer.GetCurrentTimerValue();
        float finishTimeInSeconds = finishTime.seconds + finishTime.minutes * 60;
        float puzzlesAmount = (float)Math.Pow(PuzzleManager.Instance.GetPuzzleBoardSize(), 2f);
        float finalScore = (100f * (float)Math.Pow(puzzlesAmount, 1.6f))/(finishTimeInSeconds);

        AddNewScore(playerName, finalScore);
    }

    public void SaveHighscores(HighscoreTable highscoreTable)
    {
        string json = JsonUtility.ToJson(highscoreTable, true);
        File.WriteAllText(filePath, json);
    }

    public HighscoreTable LoadHighscores()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<HighscoreTable>(json);
        }
        else
        {
            return new HighscoreTable(); // Zwróæ pusty ranking, jeœli plik nie istnieje
        }
    }

    public void AddNewScore(string playerName, float score)
    {
        HighscoreTable highscoreTable = LoadHighscores();
        Score newScore = new Score { playerName = playerName, score = score };
        highscoreTable.scoreList.Add(newScore);

        // Sortuj wyniki rosn¹co po czasie (mniej czasu to lepszy wynik)
        highscoreTable.scoreList.Sort((x, y) => y.score.CompareTo(x.score));

        SaveHighscores(highscoreTable);
    }

}
