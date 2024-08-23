using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ShowHighScores : MonoBehaviour
{
    private string filePath;
    [SerializeField] private GameObject scoreRecord;
    [SerializeField] private Transform scoreTableContent;

    void Awake()
    {
        filePath = Application.persistentDataPath + "/highscores.json";
    }

    void Start()
    {
        ShowScores();
    }

    private void ShowScores()
    {
        HighscoreTable highscoreTable = LoadHighscores();

        foreach (Score score in highscoreTable.scoreList)
        {
            // Instantiuj prefab
            GameObject scoreEntry = Instantiate(scoreRecord, scoreTableContent);

            // Pobierz komponent RectTransform
            RectTransform rectTransform = scoreEntry.GetComponent<RectTransform>();

            // Wyœrodkuj prefab
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localScale = Vector3.one;

            // Ustaw tekst lub inne dane
            TextMeshProUGUI scoreText = scoreEntry.GetComponentInChildren<TextMeshProUGUI>();
            scoreText.text = score.playerName + ": " + score.score.minutes.ToString("00") + ":" + score.score.seconds.ToString("00.00");
        }
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
}
