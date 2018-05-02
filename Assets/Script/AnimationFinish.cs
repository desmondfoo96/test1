using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFinish : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    public void finishAnimate(int s)
    {
        if(s == 1)
        {
            GameManager.manager.animation1stRound = true;
        }
        if (s == 2)
        {
            Debug.Log("aaa");
            GameManager.manager.animation2ndRound = true;
        }
        if (s == 3)
        {
            GameManager.manager.animation3rdRound = true;
        }

        if (s == 5)
        {
          
            GameManager.manager.finishRoundTextAnimation = true;
        }
        if (s == 6)
        {

            Destroy(this.gameObject);
        }
    }
    // Update is called once per frame
    void Update () {
        //Debug.Log(GameManager.manager.animation1stRound);
	}
}
