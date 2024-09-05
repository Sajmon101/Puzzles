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
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject playerNamePanel;
    [SerializeField] private GameObject promotionVideo;
    [SerializeField] private TextMeshProUGUI playerNameInput;
    [SerializeField] private TextMeshProUGUI errorText;
    private string filePath;
    public Texture2D customCursorTexture; // Twoja tekstura kursora
    private Vector2 hotSpot = Vector2.zero; // Punkt, który bêdzie centrum kursora. Mo¿esz ustawiæ (0,0) dla lewego górnego rogu lub (texture.width/2, texture.height/2) dla œrodka.
    public CursorMode cursorMode = CursorMode.Auto;
    private enum GameState { Menu, VideoPlaying }
    private GameState state;
    private Vector3 lastMousePosition;
    private float idleTime;
    private float timeToPlayVideo = 10f;


    void Awake()
    {
        filePath = Application.persistentDataPath + "/highscores.json";
    }

    private void Start()
    {
        promotionVideo.GetComponent<VideoPlayer>().loopPointReached += EndReached;
        state = GameState.Menu;
        Cursor.SetCursor(customCursorTexture, hotSpot, cursorMode);

        AudioSource backgroundSong = AudioManager.Instance.GetSound(AudioManager.SoundName.BackgroundSong);

        if (!backgroundSong.isPlaying)
        {
            AudioManager.Instance.Play(AudioManager.SoundName.BackgroundSong);
        }
           
    }

    void Update()
    {
        // Sprawdzanie ruchu myszy
        if (Input.anyKeyDown || Input.mousePosition != lastMousePosition)
        {
            lastMousePosition = Input.mousePosition;
            idleTime = 0f; // Zresetowanie licznika czasu bezczynnoœci

            if (state == GameState.VideoPlaying)
            {
                StopVideoAndShowMenu();
            }
        }
        else
        {
            // Zwiêkszanie licznika czasu bezczynnoœci, gdy nie ma ruchu myszy
            idleTime += Time.deltaTime;

            // Sprawdzenie, czy czas bezczynnoœci przekroczy³ ustalony próg
            if (idleTime >= timeToPlayVideo && state == GameState.Menu)
            {
                PlayVideo();
            }
        }
    }

    public void PlayVideo()
    {
        promotionVideo.SetActive(true);
        promotionVideo.GetComponent<VideoPlayer>().Play();
        state = GameState.VideoPlaying;
    }

    void StopVideoAndShowMenu()
    {
        idleTime = 0f; // Zresetowanie licznika czasu bezczynnoœci
        promotionVideo.GetComponent<VideoPlayer>().Stop();
        ShowMenu();
    }

    void ShowMenu()
    {
        state = GameState.Menu;
        promotionVideo.SetActive(false);
    }

    void EndReached(VideoPlayer vp)
    {
        StopVideoAndShowMenu();
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
            return new HighscoreTable(); // Zwróæ pusty ranking, jeœli plik nie istnieje
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

    public void Quit()
    {
        #if UNITY_EDITOR
                // Jeœli u¿ywasz edytora Unity
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                // Jeœli gra jest uruchomiona jako aplikacja
                Application.Quit();
        #endif
    }
}
