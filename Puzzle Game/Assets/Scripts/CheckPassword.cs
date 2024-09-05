using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckPassword : MonoBehaviour
{
    [SerializeField] TMP_InputField inputPassword;
    [SerializeField] MainMenu mainMenu;
    [SerializeField] GameObject passwordPanel;
    [SerializeField] GameObject wasDeletedPanel;
    [SerializeField] GameObject errorText;
    private string correctPassword = "zielonybridge";

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

        Debug.Log(inputPassword.text + inputPassword.text.Length);
        Debug.Log(correctPassword + correctPassword.Length);

        if (isPasswordCorrect(cleanedInputPassword))
        {
            passwordPanel.SetActive(false);
            wasDeletedPanel.SetActive(true);
            mainMenu.DeleteHighScores();
        }
        else
        {
            errorText.GetComponent<TextMeshProUGUI>().text = "Niepoprawne has³o";
        }
    }
}
