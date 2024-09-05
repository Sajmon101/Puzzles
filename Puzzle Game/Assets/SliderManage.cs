using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderManage : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.SetFloat("SliderValue", 0f);
    }
    public void OnSliderValueChanged()
    {
        int sliderValue = Mathf.RoundToInt(gameObject.GetComponent<Slider>().value);

        PlayerPrefs.SetFloat("SliderValue", gameObject.GetComponent<Slider>().value);
    }

}
