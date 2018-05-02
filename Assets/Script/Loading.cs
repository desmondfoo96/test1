using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour {
    Image fillImg;
    float timeAmt=1, time =1;
    bool minus;

    // Use this for initialization
    void Start () {
        fillImg = this.GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        if (time >= 1) 
        {
            minus = true;

        }
        if (time < 0) 
        {
            minus = false;
        }
        if(minus)
        {
            fillImg.fillClockwise = false;
            time -= Time.deltaTime;
        }
        else
        {
            fillImg.fillClockwise = true;
            time += Time.deltaTime;
        }



        fillImg.fillAmount = time / timeAmt;
    }
}
