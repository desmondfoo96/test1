using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkEffect : MonoBehaviour {
    Text blinkText;
    Color blinkColor;
    bool reverse = false;
	// Use this for initialization
	void Start () {
        blinkText = GetComponent<Text>();

        blinkColor = blinkText.color;
    }
	
	// Update is called once per frame
	void Update () {
        if(reverse)
        {
            if (blinkColor.a < 1)
            {
                blinkColor.a += 0.03f;
            }
            if (blinkColor.a >= 1)
            {
                reverse = false;
            }
        }
        else
        {
            if (blinkColor.a > 0)
            {
                blinkColor.a -= 0.03f;
            }
            if(blinkColor.a <= 0)
            {
                reverse = true;
            }
        }
      

        blinkText.color = blinkColor;

        //Debug.Log(blinkText.color.a);
    }
}
