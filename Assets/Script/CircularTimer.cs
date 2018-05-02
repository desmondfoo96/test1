using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CircularTimer : MonoBehaviour
{
    Image fillImg;


    // Use this for initialization
    void Start()
    {
        fillImg = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void SetFillAmt(float time, float timeAmt)
    {
        fillImg.fillAmount = time / timeAmt;
    }
}