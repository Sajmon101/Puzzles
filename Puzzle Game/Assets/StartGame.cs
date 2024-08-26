using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] GameObject timer;
    [SerializeField] GameObject instructionPanel;

    public void PlayGame()
    {
        timer.SetActive(true);
        instructionPanel.SetActive(false);
    }
}
