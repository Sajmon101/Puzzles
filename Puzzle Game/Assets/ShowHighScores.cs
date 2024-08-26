using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ShowHighScores : MonoBehaviour
{
    private string filePath;
    private GameObject currentPlayerHihgscoreTile;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] private GameObject scoreRecordPrefab;
    [SerializeField] private GameObject currentPlayerScoreRecordPrefab;
    [SerializeField] private RectTransform scoreTableContent;

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

        for (int i = 0; i < highscoreTable.scoreList.Count; i++)
        {
            Score score = highscoreTable.scoreList[i];
            GameObject scoreEntry;

            if (score.playerName == PlayerData.playerName)
            {
                scoreEntry = Instantiate(currentPlayerScoreRecordPrefab, scoreTableContent);
                currentPlayerHihgscoreTile = scoreEntry;
            }
            else
            {
                scoreEntry = Instantiate(scoreRecordPrefab, scoreTableContent);
            }

            // Ustaw tekst lub inne dane
            TextMeshProUGUI scoreText = scoreEntry.GetComponentInChildren<TextMeshProUGUI>();
            scoreText.text = (i+1).ToString() + ". " + score.playerName + ": " + score.score.minutes.ToString("00") + ":" + score.score.seconds.ToString("00.00");
        }

        if (currentPlayerHihgscoreTile != null)
        {
            StartCoroutine(ScrollToPlayer());
        }
    }

    private IEnumerator ScrollToPlayer()
    {
        // Poczekaj na koniec klatki, aby zaktualizowaæ uk³ad UI
        yield return new WaitForEndOfFrame();

        RectTransform target = currentPlayerHihgscoreTile.GetComponent<RectTransform>();

        float contentHeight = scoreTableContent.rect.height;
        float targetY = Mathf.Abs(target.localPosition.y);
        Debug.Log(contentHeight);
        Debug.Log(targetY);

        float normalizedPosition = 1.0f - (targetY / contentHeight);
        normalizedPosition = Mathf.Clamp01(normalizedPosition);

        scrollRect.verticalNormalizedPosition = normalizedPosition;
        Debug.Log(scrollRect.verticalNormalizedPosition);
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
