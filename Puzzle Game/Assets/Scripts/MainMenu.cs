using System.Collections;
using System.Collections.Generic;
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
        // Usuni�cie znaku Zero Width Space i innych znak�w kontrolnych
        string cleanedPlayerName = Regex.Replace(playerName, @"\p{C}+", string.Empty).Trim();
        Debug.Log($"Cleaned playerName: '{cleanedPlayerName}', Length: {cleanedPlayerName.Length}");
        //string playerName2 = "";
        //Debug.Log("Nazwa gracza:" + playerName + "D�ugo��" + playerName.Length);
        //Debug.Log("Nazwa gracza:" + playerName2 + "D�ugo��" + playerName2.Length);
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
        }

        return false;
    }

    private enum ValidationStatus
    {
        Success,
        EmptyName,
        TooLongName
    }

    private ValidationStatus ValidatePlayerName(string playerName)
    {
        Debug.Log($"'playerName' is '{playerName}' and its length is {playerName.Length}");
        if (string.IsNullOrWhiteSpace(playerName))
            return ValidationStatus.EmptyName;
        if(playerName.Length >= 20)
            return ValidationStatus.TooLongName;

        return ValidationStatus.Success;
    }

}
