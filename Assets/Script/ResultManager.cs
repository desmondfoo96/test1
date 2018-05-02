using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour {

    API _api;
    public Sprite[] bgS, cupS, confettiS;
    private string[] youT = new string[3], moneyAmtT = new string[3];

    public Image bg, cup, confetti;
    public Text you, moneyAmt, score, saved, missed;
    public GameObject money, particle;
    public GameObject[] InGameAvatar;

    ParticleSystem.MainModule psMain;

    // Use this for initialization
    void Start () {

       _api = GetComponent<API>();

        youT[0] = "YOU WON";
        youT[1] = "YOU LOST";
        youT[2] = "YOU DRAW";


        moneyAmtT[0] = "4,000,000";
        moneyAmtT[2] = "2,600,000";

        

        for (int i = 0; i < bgS.Length; i++)
        {
            if (GameManager.manager._root_round_data.data.result == i)
            {
                bg.sprite = bgS[i];
                cup.sprite = cupS[i];
                confetti.sprite = confettiS[i];
                you.text = youT[i];
                if (money != null)
                {
                    moneyAmt.text = moneyAmtT[i];
                }
            }
        }
        if (GameManager.manager._root_round_data.data.result == 0)
        {
            SoundManager.manager.playPay();
            if (GameManager.manager.getMenuData().data.getAvatar() == 1)
            {
                InGameAvatar[0].transform.localPosition = new Vector3(0, 0, 0);
                InGameAvatar[0].GetComponent<Animator>().SetTrigger("win");
            }
            else if (GameManager.manager.getMenuData().data.getAvatar() == 2)
            {
                InGameAvatar[1].transform.localPosition = new Vector3(0, 0, 0);
                InGameAvatar[1].GetComponent<Animator>().SetTrigger("win");
            }
        }
        if (GameManager.manager._root_round_data.data.result == 1)
        {
            if (GameManager.manager.getMenuData().data.getAvatar() == 1)
            {
                InGameAvatar[0].transform.localPosition = new Vector3(0, 0, 0);
                InGameAvatar[0].GetComponent<Animator>().SetTrigger("lose");
            }
            else if (GameManager.manager.getMenuData().data.getAvatar() == 2)
            {
                InGameAvatar[1].transform.localPosition = new Vector3(0, 0, 0);
                InGameAvatar[1].GetComponent<Animator>().SetTrigger("lose");
            }
            money.SetActive(false);
            particle.SetActive(false);
        }
        if (GameManager.manager._root_round_data.data.result == 2)
        {
            money.SetActive(false);
            particle.SetActive(false);
        }

        score.text = GameManager.manager._root_round_data.data.score.total.ToString();
        saved.text = GameManager.manager._root_round_data.data.saved.ToString();
        missed.text = GameManager.manager._root_round_data.data.miss.ToString();



    }

    public void OnReturnToLobbyClick()
    {
        SoundManager.manager.playMainMenuBg();
        GameManager.manager.toVsScreen = false;
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    public void OnFindOpponent()
    {
        SoundManager.manager.playMainMenuBg();
        GameManager.manager.toVsScreen = true;
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    private void Update()
    {

        if (particle != null)
        {
            var main = particle.GetComponent<ParticleSystem>().main;
            main.startColor = new Color(Random.value, Random.value, Random.value, 1);
        }
        
    }
    void DoNothing(bool error, string data)
    {

    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            string timer = JsonUtility.ToJson(GameManager.manager._timer_data);
            _api.POST("/exit_game", timer, DoNothing);
        }
        else
        {
            string timer = JsonUtility.ToJson(GameManager.manager._timer_data);
            _api.POST("/exit_game", timer, DoNothing);

        }

    }
}
