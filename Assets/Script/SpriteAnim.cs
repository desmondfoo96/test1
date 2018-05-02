using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpriteAnim : MonoBehaviour {

    public Image image;
    public Sprite[] images;
    bool stop = false;
    int x;
	// Use this for initialization
	void Start () {
        StartCoroutine(StartAwesome());
	}
	
	// Update is called once per frame
	void FixedUpdate () {
       
	}

    IEnumerator StartAwesome()
    {
        while (!stop)
        {
            yield return new WaitForSeconds(0.04f);
            x++;
            if (x >= images.Length)
            {
                SceneManager.LoadScene("Login", LoadSceneMode.Single);
                stop = true;
            }
            image.GetComponent<Image>().sprite = images[x];
        }
    }
}
