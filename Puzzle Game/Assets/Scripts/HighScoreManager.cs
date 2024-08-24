using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static Timer;

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
        GameManager.Instance.OnGameEnd += OnGameEnd;
    }

    private void OnGameEnd(object sender, System.EventArgs e)
    {
        string playerName = PlayerData.playerName; 
        TimerValue finishTime = timer.GetCurrentTimerValue();
        AddNewScore(playerName, finishTime);
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

    public void AddNewScore(string playerName, TimerValue score)
    {
        HighscoreTable highscoreTable = LoadHighscores();
        Score newScore = new Score { playerName = playerName, score = score };
        highscoreTable.scoreList.Add(newScore);

        // Sortuj wyniki rosn¹co po czasie (mniej czasu to lepszy wynik)
        highscoreTable.scoreList.Sort((x, y) => CompareTimerValues(x.score, y.score));

        SaveHighscores(highscoreTable);
    }

    public int CompareTimerValues(TimerValue x, TimerValue y)
    {
        if (x.minutes != y.minutes)
        {
            return x.minutes.CompareTo(y.minutes); // Porównaj najpierw minuty
        }
        else
        {
            return x.seconds.CompareTo(y.seconds); // Jeœli minuty s¹ równe, porównaj sekundy
        }
    }





}
