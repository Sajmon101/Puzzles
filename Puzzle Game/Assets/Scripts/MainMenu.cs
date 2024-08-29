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

    void Awake()
    {
        filePath = Application.persistentDataPath + "/highscores.json";
    }

    private void Start()
    {
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
        }

        return false;
    }

    private enum ValidationStatus
    {
        Success,
        EmptyName,
        TooLongName,
        NameAlreadyExists
    }

    private ValidationStatus ValidatePlayerName(string playerName)
    {
        if (string.IsNullOrWhiteSpace(playerName))
            return ValidationStatus.EmptyName;
        if(playerName.Length > 20)
            return ValidationStatus.TooLongName;
        if (IsNameExisting(playerName))
            return ValidationStatus.NameAlreadyExists;

        return ValidationStatus.Success;
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
