using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckPassword : MonoBehaviour
{
    [SerializeField] TMP_InputField inputPassword;
    [SerializeField] GameObject passwordPanel;
    [SerializeField] GameObject wasDeletedPanel;
    [SerializeField] GameObject errorText;
    private string correctPassword = "zielonybridge";

    private string filePath;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/highscores.json";
    }

    private bool isPasswordCorrect(string password)
    {
        if(password == correctPassword) 
            return true;
        else
            return false;
    }

    public void DeleteScoresIfAuthorized()
    {
        string cleanedInputPassword = Regex.Replace(inputPassword.text, @"\p{C}+", string.Empty).Trim();

        if (isPasswordCorrect(cleanedInputPassword))
        {
            passwordPanel.SetActive(false);
            wasDeletedPanel.SetActive(true);
            DeleteHighScores();
        }
        else
        {
            errorText.GetComponent<TextMeshProUGUI>().text = "Niepoprawne has³o";
        }
    }

    public void DeleteHighScores()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
