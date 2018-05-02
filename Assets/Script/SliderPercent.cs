using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using UnityEngine;
using System;

public class SliderPercent : MonoBehaviour {

    Text text;
    Single value;
    public Slider volumeSlider;
    int startValue;
	// Use this for initialization
	void Start () {
        text = this.GetComponent<Text>();
        startValue = (int)(PlayerPrefs.GetFloat("volume"));
        volumeSlider.value = (PlayerPrefs.GetFloat("volume"))/100;
        text.text = startValue.ToString() + "%";
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void ValueChange(Single slider)
    {
        slider *= 100;
        int value;
        value = (int)slider;
        text.text = value.ToString() + "%";
        PlayerPrefs.SetFloat("volume", (float)slider);
    }
}
