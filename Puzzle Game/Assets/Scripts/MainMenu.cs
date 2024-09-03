using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject playerNamePanel;
    [SerializeField] private TextMeshProUGUI playerNameInput;
    [SerializeField] private TextMeshProUGUI errorText;
    private string filePath;
    public Texture2D customCursorTexture; // Twoja tekstura kursora
    private Vector2 hotSpot = Vector2.zero; // Punkt, kt�ry b�dzie centrum kursora. Mo�esz ustawi� (0,0) dla lewego g�rnego rogu lub (texture.width/2, texture.height/2) dla �rodka.
    public CursorMode cursorMode = CursorMode.Auto; 

    void Awake()
    {
        filePath = Application.persistentDataPath + "/highscores.json";
    }

    private void Start()
    {
        Cursor.SetCursor(customCursorTexture, hotSpot, cursorMode);

        AudioSource backgroundSong = AudioManager.Instance.GetSound(AudioManager.SoundName.BackgroundSong);

        if (!backgroundSong.isPlaying)
        {
            AudioManager.Instance.Play(AudioManager.SoundName.BackgroundSong);
        }
           
    }

    public void ShowPlayerInput()
    {
        playerNamePanel.SetActive(true);
    }    

    public void PlayGame()
    {
        if(SavePlayerName())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }

    }

    public void ShowCredits()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+2);
    }

    public void ShowHighScores()
    {
        SceneManager.LoadScene(3);
    }

    public void BackToMain()
    {
        SceneManager.LoadScene(0);
    }

    public bool SavePlayerName()
    {
        string playerName = playerNameInput.text;
        string cleanedPlayerName = Regex.Replace(playerName, @"\p{C}+", string.Empty).Trim();      // Usuni�cie znaku Zero Width Space i innych znak�w kontrolnych
        ValidationStatus status = ValidatePlayerName(cleanedPlayerName);

        switch (status)
        {
            case ValidationStatus.Success:
                PlayerData.playerName = playerName;
                return true;

            case ValidationStatus.TooLongName:
                errorText.text = "Nazwa mo�e posiada� maksymalnie 20 znak�w";
                return false;

            case ValidationStatus.EmptyName:
                errorText.text = "Wprowad� nazw� gracza";
                return false;

            case ValidationStatus.NameAlreadyExists:
                errorText.text = "Ta nazwa ju� jest zaj�ta";
                return false;

            case ValidationStatus.ContainSpecialCharacters:
                errorText.text = "Nazwa nie mo�e posiada� znak�w specjalnych";
                return false;
        }

        return false;
    }

    private enum ValidationStatus
    {
        Success,
        EmptyName,
        TooLongName,
        NameAlreadyExists,
        ContainSpecialCharacters
    }

    private ValidationStatus ValidatePlayerName(string playerName)
    {
        if (string.IsNullOrWhiteSpace(playerName))
            return ValidationStatus.EmptyName;
        if(playerName.Length > 20)
            return ValidationStatus.TooLongName;
        if (IsNameExisting(playerName))
            return ValidationStatus.NameAlreadyExists;
        if(ContainsSpecialCharacters(playerName))
            return ValidationStatus.ContainSpecialCharacters;

        return ValidationStatus.Success;
    }

    private bool ContainsSpecialCharacters(string input)
    {
        return input.Any(ch => !Char.IsLetterOrDigit(ch));
    }
    private bool IsNameExisting(string playerName)
    {
        HighscoreTable highscoreTable = LoadHighscores();

        for (int i = 0; i < highscoreTable.scoreList.Count; i++)
        {
            Score score = highscoreTable.scoreList[i];
            string cleanedPlayerName = Regex.Replace(score.playerName, @"\p{C}+", string.Empty).Trim();

            if (cleanedPlayerName == playerName)
            {
                return true;
            }
        }
        return false;

    }

    private HighscoreTable LoadHighscores()
    {

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<HighscoreTable>(json);
        }
        else
        {
            return new HighscoreTable(); // Zwr�� pusty ranking, je�li plik nie istnieje
        }
    }

    public void DeleteHighScores()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void HidePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}
