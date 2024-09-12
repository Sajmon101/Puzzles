using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddName : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = PlayerData.playerName;
    }
}
