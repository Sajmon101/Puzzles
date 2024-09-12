using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinLoseManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject failPanel;
    [SerializeField] private GameObject darkening;
    [SerializeField] private TextMeshProUGUI winPanelBigText;
    [SerializeField] private TextMeshProUGUI winPanelSmallText;
    [SerializeField] private Transform puzzleBackground;
    [SerializeField] private Transform canvas;
    [SerializeField] private Timer timer;

    void Start()
    {
        PuzzleManager.Instance.OnGameVictory += OnGameVictory;
        timer.OnGameLose += OnGameLose;
    }

    private void OnGameVictory(object sender, EventArgs e)
    {
        winPanel.SetActive(true);
        winPanelBigText.text = "Gratulacje<line-height=130%>\n" + PlayerData.playerName + "!";
        winPanelSmallText.text = "Twój czas to: " + timer.GetCurrentTimerValue().minutes.ToString("00") + ":" + timer.GetCurrentTimerValue().seconds.ToString("00");
        AudioSource backgrounSound = AudioManager.Instance.GetSound(AudioManager.SoundName.BackgroundSong);
        backgrounSound.volume = 0.3f;
        AudioManager.Instance.Play(AudioManager.SoundName.WinSound);
    }

    private void OnGameLose(object sender, EventArgs e)
    {
        AudioSource backgrounSound = AudioManager.Instance.GetSound(AudioManager.SoundName.BackgroundSong);
        backgrounSound.volume = 0.3f;
        AudioManager.Instance.Play(AudioManager.SoundName.FailSound);
        timer.StopTimer();
        failPanel.SetActive(true);
        darkening.SetActive(true);
    }

    public void backGroundMusicToMax()
    {
        AudioSource backgrounSound = AudioManager.Instance.GetSound(AudioManager.SoundName.BackgroundSong);
        backgrounSound.volume = 1f;
    }
}
