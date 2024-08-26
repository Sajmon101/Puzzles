using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class baseButton : MonoBehaviour
{
    private void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(ClickSound);
    }

    public void ClickSound()
    {
        AudioManager.Instance.Play(AudioManager.SoundName.ButtonSound);
    }
}
