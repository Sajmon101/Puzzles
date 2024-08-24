using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject playerNamePanel;
    [SerializeField] private TextMeshProUGUI playerNameInput;

    public void ShowPlayerInput()
    {
        playerNamePanel.SetActive(true);
    }    

    public void PlayGame()
    {
        SavePlayerName();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void ShowCredits()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+2);
    }

    public void BackToMain()
    {
        SceneManager.LoadScene(0);
    }

    public void SavePlayerName()
    {
        string playerName = playerNameInput.text;
        PlayerData.playerName = playerName;
    }

}
