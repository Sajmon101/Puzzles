using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ValidateAndPlay: MonoBehaviour
{
    private string playerName;
    [SerializeField] private TextMeshProUGUI errorText;
    private string filePath;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/highscores.json";
    }

    public void OnStartButtonClick()
    {
        if (SavePlayerName())
        {
            SceneManager.LoadScene(SceneNames.Scenes.Puzzles.ToString());
        }
    }

    public bool SavePlayerName()
    {
        playerName = GetComponent<TMP_InputField>().text;
        string cleanedPlayerName = Regex.Replace(playerName, @"\p{C}+", string.Empty).Trim();      // Usuniêcie znaku Zero Width Space i innych znaków kontrolnych
        ValidationStatus status = ValidatePlayerName(cleanedPlayerName);

        switch (status)
        {
            case ValidationStatus.Success:
                PlayerData.playerName = playerName;
                return true;

            case ValidationStatus.TooLongName:
                errorText.text = "Nazwa mo¿e posiadaæ maksymalnie 20 znaków";
                return false;

            case ValidationStatus.EmptyName:
                errorText.text = "WprowadŸ nazwê gracza";
                return false;

            case ValidationStatus.NameAlreadyExists:
                errorText.text = "Ta nazwa ju¿ jest zajêta";
                return false;

            case ValidationStatus.ContainSpecialCharacters:
                errorText.text = "Nazwa nie mo¿e posiadaæ znaków specjalnych";
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
        if (playerName.Length > 20)
            return ValidationStatus.TooLongName;
        if (IsNameExisting(playerName))
            return ValidationStatus.NameAlreadyExists;
        if (ContainsSpecialCharacters(playerName))
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
            return new HighscoreTable(); // Zwróæ pusty ranking, jeœli plik nie istnieje
        }
    }
}
