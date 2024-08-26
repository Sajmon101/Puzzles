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
        // Usuniêcie znaku Zero Width Space i innych znaków kontrolnych
        string cleanedPlayerName = Regex.Replace(playerName, @"\p{C}+", string.Empty).Trim();
        Debug.Log($"Cleaned playerName: '{cleanedPlayerName}', Length: {cleanedPlayerName.Length}");
        //string playerName2 = "";
        //Debug.Log("Nazwa gracza:" + playerName + "D³ugoœæ" + playerName.Length);
        //Debug.Log("Nazwa gracza:" + playerName2 + "D³ugoœæ" + playerName2.Length);
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
