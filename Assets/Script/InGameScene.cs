using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;


public class InGameScene : MonoBehaviour
{
    API _api;
    private Vector3 keeperCamPos, strikerCamPos, keeperCamRot, strikerCamRot, showResultPos, showResultRot;
    private bool canKick, startRoundAnimation, playHeart;
    private int currentScore, round;
    private GameObject[] timerUI, InGameAvatar= new GameObject[2];
    private Animator myAnimator, opponentAnimator;

    //enumeration of role and direction
    Role role1;
    Direction direct, OnKickDirect, realDirect;

    Vector3 temp1 = new Vector3();

    public Sprite[] Result, roundSprite;
    public Sprite[] buttonImg;
    //GameObject Refrences
    public GameObject[] keeperHighlight, strikerHighlight, timerUI1, timerUI2, avatar3D;
    
    public GameObject my_scoreboard, opponent_scoreboard, RoundImage, strikerBG, keeperBG;
    public GameObject joystick, StrikerUI;
    public GameObject OppTieRound, MeTieRound;
    public GameObject striker_avatar, keeper_avatar, kickBtn, footBall;
    public GameObject TopRight, TopLeft, BtmLeft, BtmRight, Middle;

    //onAnimationComplete animationCallback;

    //timer
    private float chooseTimer, animationTimer;

    void Start()
    {
        SoundManager.manager.stopBGM();
        _api = GetComponent<API>();
        GameManager.manager._direction_data.player_id = GameManager.manager._timer_data.player_id;
        GameManager.manager._direction_data.match_id = GameManager.manager._timer_data.match_id;
        //Preset Cam position for keeper and striker
        keeperCamPos = new Vector3(0f, 4.4f, 3.88f);
        keeperCamRot = new Vector3(21.66f, 0, 0);
        strikerCamPos = new Vector3(0f, 4.4f, -2f);
        strikerCamRot = new Vector3(25f, 0, 0);
        showResultPos = new Vector3(0f, 4.4f, -2f);
        showResultRot = new Vector3(25f, 0, 0);


        if (GameManager.manager.getMenuData().data.getAvatar() == 1)
        {
            InGameAvatar[0] = Instantiate(avatar3D[0], new Vector3(999, 999, 999), Quaternion.identity);
        }
        if (GameManager.manager.getMenuData().data.getAvatar() == 2)
        {
            InGameAvatar[0] = Instantiate(avatar3D[1], new Vector3(999, 999, 999), Quaternion.identity);
        }

        if (GameManager.manager._root_match_data.data.opponent_avatar == 1)
        {
            InGameAvatar[1] = Instantiate(avatar3D[0], new Vector3(999, 999, 999), Quaternion.identity);
        }
        if (GameManager.manager._root_match_data.data.opponent_avatar == 2)
        {
            InGameAvatar[1] = Instantiate(avatar3D[1], new Vector3(999, 999, 999), Quaternion.identity);
        }

        InGameAvatar[0].AddComponent<AnimationFinish>();
        InGameAvatar[1].AddComponent<AnimationFinish>();
        myAnimator = InGameAvatar[0].GetComponent<Animator>();
        opponentAnimator = InGameAvatar[1].GetComponent<Animator>();

        //Init Game settings    
        InitGame((Role)GameManager.manager._root_match_data.data.role);
        role1 = (Role)GameManager.manager._root_match_data.data.role;

        if (role1 == Role.Striker)
        {
            timerUI = (GameObject[])timerUI1.Clone();
        }
        else
        {
            timerUI = (GameObject[])timerUI2.Clone();
        }
        //Debug.Log(timerUI[0]);
        round = 1;
        //preset timer
        chooseTimer = 5;
        animationTimer = 3;

        // my_scoreboard.transform.GetChild(1).GetComponent<Text>().text = GameManager.manager.getMenuData().data.getUsername();
        opponent_scoreboard.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = GameManager.manager._root_match_data.data.opponent_name.ToString();
    }

    void FixedUpdate()
    {
        //Get Direction from Joystick
        //direct = (Direction)joystick.GetComponent<Joystick>().getDirection();

        //Debug.Log(direct);
        //SetHighlight for player
        SetHighlight();

        //Check if can kick
        checkKick();

        StartRound();
        

      // Debug.Log(direct);
    }

    public void OnKickClick()
    {
        OnKickDirect = direct;
        canKick = false;
    }

    private void InitGame(Role role)
    {
        direct = Direction.Centre;
        canKick = true;
        //round = 1;
        //if (!setCamera)
        //{
        round++;
        for (int i = 0; i < roundSprite.Length; i++)
        {
            if (round == (i + 1))
            {
                RoundImage.GetComponent<Image>().sprite = roundSprite[i];
            }
        }
        if (role == Role.Keeper)
        {

            InGameAvatar[0].transform.parent = keeper_avatar.transform;
            InGameAvatar[0].transform.position = keeper_avatar.transform.position;
            InGameAvatar[0].transform.rotation = keeper_avatar.transform.rotation;
            InGameAvatar[0].transform.localScale = new Vector3(1, 1, 1);
            myAnimator.runtimeAnimatorController = Resources.Load("KeeperNew") as RuntimeAnimatorController;

            InGameAvatar[1].transform.parent = striker_avatar.transform;
            InGameAvatar[1].transform.position = striker_avatar.transform.position;
            InGameAvatar[1].transform.rotation = striker_avatar.transform.rotation;
            opponentAnimator.runtimeAnimatorController = Resources.Load("StrikerNew") as RuntimeAnimatorController;

            InGameAvatar[1].transform.localScale = new Vector3(1, 1, 1);
            Camera.main.transform.position = keeperCamPos;
            Camera.main.transform.rotation = Quaternion.Euler(keeperCamRot);
            //KeeperUI.SetActive(true);
            StrikerUI.SetActive(true);
            strikerBG.SetActive(false);
            keeperBG.SetActive(true);
            kickBtn.GetComponent<Image>().sprite = buttonImg[0];
            
        }
        else if (role == Role.Striker)
        {
            InGameAvatar[0].transform.parent = striker_avatar.transform;
            InGameAvatar[0].transform.position = striker_avatar.transform.position;
            InGameAvatar[0].transform.rotation = striker_avatar.transform.rotation;
            InGameAvatar[0].transform.localScale = new Vector3(1, 1, 1);
            myAnimator.runtimeAnimatorController = Resources.Load("StrikerNew") as RuntimeAnimatorController;

            InGameAvatar[1].transform.parent = keeper_avatar.transform;
            InGameAvatar[1].transform.position = keeper_avatar.transform.position;
            InGameAvatar[1].transform.rotation = keeper_avatar.transform.rotation;
            InGameAvatar[1].transform.localScale = new Vector3(1, 1, 1);
            opponentAnimator.runtimeAnimatorController = Resources.Load("KeeperNew") as RuntimeAnimatorController;

            Camera.main.transform.position = strikerCamPos;
            Camera.main.transform.rotation = Quaternion.Euler(strikerCamRot);
            //KeeperUI.SetActive(false);
            strikerBG.SetActive(true);
            keeperBG.SetActive(false);
            StrikerUI.SetActive(true);
            kickBtn.GetComponent<Image>().sprite = buttonImg[1];
        }
        //setCamera = true;
        //  }

        temp1 = new Vector3(0, 0, 0);
        StartCoroutine(startTimer());

    }


    IEnumerator startTimer()
    {
        RoundImage.GetComponent<Animator>().SetBool("animate", true);
        yield return new WaitForSeconds(3f);
        RoundImage.GetComponent<Animator>().SetBool("animate", false);
        string timer = JsonUtility.ToJson(GameManager.manager._timer_data);
        GameManager.manager.SocketEmit("start_round_timer", timer);
        SoundManager.manager.playStart();
        striker_avatar.transform.GetComponent<Animator>().SetBool("start", true);
        striker_avatar.transform.GetChild(1).GetComponent<Animator>().SetTrigger("start");
    }

    private void SetHighlight()
    {
        if (role1 == Role.Keeper)
        {
            for (int i = 0; i < 6; i++)
            {
                if (direct == Direction.Centre)
                {
                    strikerHighlight[(int)Direction.Centre].SetActive(true);
                    joystick.transform.position = Middle.transform.position;
                }
                else if ((direct == Direction.TopLeft))
                {
                    strikerHighlight[(int)Direction.TopLeft].SetActive(true);
                    joystick.transform.position = TopLeft.transform.position;
                }
                else if ((direct == Direction.TopRight))
                {
                    strikerHighlight[(int)Direction.TopRight].SetActive(true);
                    joystick.transform.position = TopRight.transform.position;
                }
                else if ((direct == Direction.BottomLeft))
                {
                    strikerHighlight[(int)Direction.BottomLeft].SetActive(true);
                    joystick.transform.position = BtmLeft.transform.position;
                }
                else if ((direct == Direction.BottomRight))
                {
                    strikerHighlight[(int)Direction.BottomRight].SetActive(true);
                    joystick.transform.position = BtmRight.transform.position;
                }
                if (i < 5)
                {
                    strikerHighlight[i].SetActive(false);
                }
            }

        }
        else if (role1 == Role.Striker)
        {
            for (int i = 0; i < 6; i++)
            {

                if (direct == Direction.Centre)
                {
                    strikerHighlight[(int)Direction.Centre].SetActive(true);
                    joystick.transform.position = Middle.transform.position;
                }
                else if ((direct == Direction.TopLeft))
                {
                    strikerHighlight[(int)Direction.TopLeft].SetActive(true);
                    joystick.transform.position = TopLeft.transform.position;
                }
                else if ((direct == Direction.TopRight))
                {
                    strikerHighlight[(int)Direction.TopRight].SetActive(true);
                    joystick.transform.position = TopRight.transform.position;
                }
                else if ((direct == Direction.BottomLeft))
                {
                    strikerHighlight[(int)Direction.BottomLeft].SetActive(true);
                    joystick.transform.position = BtmLeft.transform.position;
                }
                else if ((direct == Direction.BottomRight))
                {
                    strikerHighlight[(int)Direction.BottomRight].SetActive(true);
                    joystick.transform.position = BtmRight.transform.position;

                }

                if (i < 5)
                {
                    strikerHighlight[i].SetActive(false);
                }
            }
        }


    }

    private void checkKick()
    {
        if (!canKick)
        {
            TopRight.transform.GetChild(0).GetComponent<Button>().enabled = false;
            TopLeft.transform.GetChild(0).GetComponent<Button>().enabled = false;
            BtmLeft.transform.GetChild(0).GetComponent<Button>().enabled = false;
            BtmRight.transform.GetChild(0).GetComponent<Button>().enabled = false;
            Middle.transform.GetChild(0).GetComponent<Button>().enabled = false;
            kickBtn.GetComponent<Button>().interactable = false;

        }
        else
        {
            TopRight.transform.GetChild(0).GetComponent<Button>().enabled = true;
            TopLeft.transform.GetChild(0).GetComponent<Button>().enabled = true;
            BtmLeft.transform.GetChild(0).GetComponent<Button>().enabled = true;
            BtmRight.transform.GetChild(0).GetComponent<Button>().enabled = true;
            Middle.transform.GetChild(0).GetComponent<Button>().enabled = true;
            kickBtn.GetComponent<Button>().interactable = true;
        }

    }

    private void finishRound()
    {
        //RoundImage.GetComponent<Animator>().SetBool("animate", false);
        chooseTimer = 5;

    }

    private void StartRound()
    {
        //Debug.Log(GameManager.manager.finishRoundTextAnimation);
        if(startRoundAnimation == true)
        {
            //RoundImage.GetComponent<Animator>().SetBool("animate", true);
            startRoundAnimation = false;
        }
        if (GameManager.manager.finishRoundTextAnimation == true)
        {
            finishRound();
            GameManager.manager.finishRoundTextAnimation = false;
        }
        // Debug.Log(GameManager.manager.start_round);
        if (GameManager.manager.start_round == true)
        {
            // Debug.Log(chooseTimer);

            chooseTimer -= Time.deltaTime;
            //change the timer UI
            timerUI[round-1].GetComponent<CircularTimer>().SetFillAmt(chooseTimer, 5f);
            //TimerUI.transform.GetChild(1).GetComponent<CircularTimer>().SetFillAmt(chooseTimer, 5);
           // TimerUI.transform.GetChild(2).GetComponent<Text>().text = ((int)chooseTimer + 1).ToString();
            //Debug.Log(GameManager.manager._root_timer_data.data.time_left);
            if (GameManager.manager._root_timer_data.data.time_left <= 0)
            {
                timerUI[round - 1].GetComponent<CircularTimer>().SetFillAmt(0, 5f);
                playHeart = false;
                chooseTimer = GameManager.manager._root_timer_data.data.time_left;
                canKick = false;
                realDirect = direct;
                GameManager.manager._direction_data.direction = (int)realDirect;
                string json = JsonUtility.ToJson(GameManager.manager._direction_data);
                //Debug.Log(json);
                GameManager.manager.SocketEmit("send_direction", json);
                GameManager.manager.start_round = false;
            }
            if (GameManager.manager._root_timer_data.data.time_left == 9)
            {
                if(!playHeart)
                {
                    SoundManager.manager.playHeart();
                    chooseTimer = GameManager.manager._root_timer_data.data.time_left;
                    playHeart = true;
                }
            }
            if (GameManager.manager._root_timer_data.data.time_left == 8)
            {
                if (playHeart)
                {
                    SoundManager.manager.playHeart();
                    chooseTimer = GameManager.manager._root_timer_data.data.time_left;
                    playHeart = false;
                }
            }
            if (GameManager.manager._root_timer_data.data.time_left == 7)
            {
                if (!playHeart)
                {
                    SoundManager.manager.playHeart();
                    chooseTimer = GameManager.manager._root_timer_data.data.time_left;
                    playHeart = true;
                }
            }
            if (GameManager.manager._root_timer_data.data.time_left == 6)
            {
                if (playHeart)
                {
                    SoundManager.manager.playHeart();
                    chooseTimer = GameManager.manager._root_timer_data.data.time_left;
                    playHeart = false;
                }
            }
            if (GameManager.manager._root_timer_data.data.time_left == 5)
            {
                if (!playHeart)
                {
                    SoundManager.manager.playHeart();
                    chooseTimer = GameManager.manager._root_timer_data.data.time_left;
                    playHeart = true;
                }
            }
            if (GameManager.manager._root_timer_data.data.time_left == 4)
            {
                if (playHeart)
                {
                    SoundManager.manager.playHeart();
                    chooseTimer = GameManager.manager._root_timer_data.data.time_left;
                    playHeart = false;
                }

            }
            if (GameManager.manager._root_timer_data.data.time_left == 3)
            {
                if (!playHeart)
                {
                    SoundManager.manager.playHeart();
                    chooseTimer = GameManager.manager._root_timer_data.data.time_left;
                    playHeart = true;
                }
            }
            if (GameManager.manager._root_timer_data.data.time_left == 2)
            {
                if (playHeart)
                {
                    SoundManager.manager.playHeart();
                    chooseTimer = GameManager.manager._root_timer_data.data.time_left;
                    playHeart = false;
                }
            }
            if (GameManager.manager._root_timer_data.data.time_left == 1)
            {
                if (!playHeart)
                {
                    SoundManager.manager.playHeart();
                    chooseTimer = GameManager.manager._root_timer_data.data.time_left;
                    playHeart = true;
                }
            }

        }
        if (GameManager.manager.next_round == true)
        {
            //do additional stuff
            //do animation
            DoAnimation();
            //do animation
            GameManager.manager.next_round = false;
        }
        if(GameManager.manager.animation1stRound == true)
        {
           
            
            if (role1 == Role.Striker)
            {
                if (GameManager.manager._root_round_data.data.round_result == 0)
                {
                    SoundManager.manager.playSuccess();
                    //Handheld.Vibrate();
                }
                else
                {
                    SoundManager.manager.playFail();
                }
                if (GameManager.manager._root_round_data.data.round_result == 0)
                {
                    striker_avatar.transform.GetChild(1).transform.GetComponent<Animator>().SetTrigger("success");
                    keeper_avatar.transform.GetChild(1).transform.GetComponent<Animator>().SetTrigger("fail");
                }
                if (GameManager.manager._root_round_data.data.round_result == 1)
                {
                    striker_avatar.transform.GetChild(1).transform.GetComponent<Animator>().SetTrigger("fail");
                    keeper_avatar.transform.GetChild(1).transform.GetComponent<Animator>().SetTrigger("success");
                }
            }

            if (role1 == Role.Keeper)
            {
                if (GameManager.manager._root_round_data.data.round_result == 0)
                {
                    SoundManager.manager.playSave();
                }
                else
                {
                    SoundManager.manager.playFail();
                }
                if (GameManager.manager._root_round_data.data.round_result == 0)
                {
                    striker_avatar.transform.GetChild(1).transform.GetComponent<Animator>().SetTrigger("fail");
                    keeper_avatar.transform.GetChild(1).transform.GetComponent<Animator>().SetTrigger("success");
                }
                if (GameManager.manager._root_round_data.data.round_result == 1)
                {
                    striker_avatar.transform.GetChild(1).transform.GetComponent<Animator>().SetTrigger("success");
                    keeper_avatar.transform.GetChild(1).transform.GetComponent<Animator>().SetTrigger("fail");
                }
            }
            GameManager.manager.animation1stRound = false;
        }
        if (GameManager.manager.animation2ndRound == true)
        {

            keeper_avatar.transform.localScale = new Vector3(14, 14, 14);
            striker_avatar.transform.GetComponent<Animator>().SetBool("start", false);
            striker_avatar.transform.GetComponent<Animator>().SetBool("kick", false);
            striker_avatar.transform.GetChild(1).GetComponent<Animator>().SetTrigger("kick");
            keeper_avatar.transform.GetComponent<Animator>().SetTrigger("reset");
            keeper_avatar.transform.GetChild(1).transform.GetComponent<Animator>().SetTrigger("reset");
            footBall.transform.GetComponent<Animator>().SetTrigger("reset");
            if(GameManager.manager.game_over)
            {
                GameOver();
            }
            else
            {
                NextRound();
            }
            GameManager.manager.animation2ndRound = false;
        }
        if (GameManager.manager.animation3rdRound == true)
        {
            Vector3 temp = new Vector3();
            temp = keeper_avatar.transform.localScale;
            SoundManager.manager.playKick();
            if (role1 == Role.Striker)
            {
                if ((int)realDirect == 0)
                {
                    if (GameManager.manager._root_round_data.data.round_result == 0)
                    {
                        footBall.GetComponent<Animator>().SetTrigger("middle");
                    }
                    else
                    {
                        footBall.GetComponent<Animator>().SetTrigger("middleBlock");
                    }
                }
                if ((int)realDirect == 1)
                {
                    if (GameManager.manager._root_round_data.data.round_result == 0)
                    {
                        footBall.GetComponent<Animator>().SetTrigger("top1");
                    }
                    else
                    {
                        footBall.GetComponent<Animator>().SetTrigger("top1Block");
                    }
                }
                if ((int)realDirect == 2)
                {
                    if (GameManager.manager._root_round_data.data.round_result == 0)
                    {
                        footBall.GetComponent<Animator>().SetTrigger("top2");
                    }
                    else
                    {
                        footBall.GetComponent<Animator>().SetTrigger("top2Block");
                    }
                }
                if ((int)realDirect == 3)
                {
                    if (GameManager.manager._root_round_data.data.round_result == 0)
                    {
                        footBall.GetComponent<Animator>().SetTrigger("bottom3");
                    }
                    else
                    {
                        footBall.GetComponent<Animator>().SetTrigger("bottom3Block");
                    }
                }
                if ((int)realDirect == 4)
                {
                    if (GameManager.manager._root_round_data.data.round_result == 0)
                    {
                        footBall.GetComponent<Animator>().SetTrigger("bottom4");
                    }
                    else
                    {
                        footBall.GetComponent<Animator>().SetTrigger("bottom4Block");
                    }
                }
                if (GameManager.manager._root_round_data.data.opponent_direction == 0)
                { 
                    keeper_avatar.GetComponent<Animator>().SetTrigger("middle");
                    keeper_avatar.transform.GetChild(1).GetComponent<Animator>().SetTrigger("bcenter");
                }
                if (GameManager.manager._root_round_data.data.opponent_direction == 1)
                {                    keeper_avatar.GetComponent<Animator>().SetTrigger("top1");
                    keeper_avatar.transform.GetChild(1).GetComponent<Animator>().SetTrigger("bside");
                }
                if (GameManager.manager._root_round_data.data.opponent_direction == 2)
                {
                    keeper_avatar.GetComponent<Animator>().SetTrigger("top2");
                    keeper_avatar.transform.localScale = new Vector3(-temp.x, temp.y, temp.z);
                    keeper_avatar.transform.GetChild(1).GetComponent<Animator>().SetTrigger("bside");
                }
                if (GameManager.manager._root_round_data.data.opponent_direction == 3)
                {
                    keeper_avatar.GetComponent<Animator>().SetTrigger("bottom3");
                    keeper_avatar.transform.GetChild(1).GetComponent<Animator>().SetTrigger("bside");
                }
                if (GameManager.manager._root_round_data.data.opponent_direction == 4)
                {
                    keeper_avatar.GetComponent<Animator>().SetTrigger("bottom4");
                    keeper_avatar.transform.localScale = new Vector3(-temp.x, temp.y, temp.z);
                    keeper_avatar.transform.GetChild(1).GetComponent<Animator>().SetTrigger("bside");
                }
            }
            else if (role1 == Role.Keeper)
            {
                if (GameManager.manager._root_round_data.data.opponent_direction == 0)
                {
                    if (GameManager.manager._root_round_data.data.round_result == 0)
                    {
                        footBall.GetComponent<Animator>().SetTrigger("middleBlock");
                    }
                    else
                    {
                        footBall.GetComponent<Animator>().SetTrigger("middle");
                    }
                }
                if (GameManager.manager._root_round_data.data.opponent_direction == 1)
                {
                    if (GameManager.manager._root_round_data.data.round_result == 0)
                    {
                        footBall.GetComponent<Animator>().SetTrigger("top1Block");
                    }
                    else
                    {
                        footBall.GetComponent<Animator>().SetTrigger("top1");
                    }
                }
                if (GameManager.manager._root_round_data.data.opponent_direction == 2)
                {
                    if (GameManager.manager._root_round_data.data.round_result == 0)
                    {
                        footBall.GetComponent<Animator>().SetTrigger("top2Block");
                    }
                    else
                    {
                        footBall.GetComponent<Animator>().SetTrigger("top2");
                    }
                }
                if (GameManager.manager._root_round_data.data.opponent_direction == 3)
                {
                    if (GameManager.manager._root_round_data.data.round_result == 0)
                    {
                        footBall.GetComponent<Animator>().SetTrigger("bottom3Block");
                    }
                    else
                    {
                        footBall.GetComponent<Animator>().SetTrigger("bottom3");
                    }
                }
                if (GameManager.manager._root_round_data.data.opponent_direction == 4)
                {
                    if (GameManager.manager._root_round_data.data.round_result == 0)
                    {
                        footBall.GetComponent<Animator>().SetTrigger("bottom4Block");
                    }
                    else
                    {
                        footBall.GetComponent<Animator>().SetTrigger("bottom4");
                    }
                }

                if ((int)realDirect == 0)
                {
                    keeper_avatar.GetComponent<Animator>().SetTrigger("middle");
                    keeper_avatar.transform.GetChild(1).GetComponent<Animator>().SetTrigger("bcenter");
                }
                if ((int)realDirect == 1)
                {
                    keeper_avatar.GetComponent<Animator>().SetTrigger("top1");
                    keeper_avatar.transform.GetChild(1).GetComponent<Animator>().SetTrigger("bside");
                }
                if ((int)realDirect == 2)
                {
                    keeper_avatar.GetComponent<Animator>().SetTrigger("top2");
                    keeper_avatar.transform.localScale = new Vector3(-temp.x, temp.y, temp.z);
                    keeper_avatar.transform.GetChild(1).GetComponent<Animator>().SetTrigger("bside");
                }
                if ((int)realDirect == 3)
                {
                    keeper_avatar.GetComponent<Animator>().SetTrigger("bottom3");
                    keeper_avatar.transform.GetChild(1).GetComponent<Animator>().SetTrigger("bside");
                }
                if ((int)realDirect == 4)
                {
                    keeper_avatar.GetComponent<Animator>().SetTrigger("bottom4");
                    keeper_avatar.transform.localScale = new Vector3(-temp.x, temp.y, temp.z);
                    keeper_avatar.transform.GetChild(1).GetComponent<Animator>().SetTrigger("bside");
                }

            }
            GameManager.manager.animation3rdRound = false;
        }

    }
    
    private void DoAnimation()
    {
        canKick = false;
        Camera.main.transform.position = strikerCamPos;
        Camera.main.transform.rotation = Quaternion.Euler(strikerCamRot);
        strikerBG.SetActive(true);
        keeperBG.SetActive(false);
        striker_avatar.transform.GetComponent<Animator>().SetBool("kick", true);
        striker_avatar.transform.GetChild(1).GetComponent<Animator>().SetTrigger("kick");
       

    }
   
    void NextRound()
    {
        if (round > 9)
        {
            my_scoreboard.transform.GetChild(2).gameObject.SetActive(false);
            opponent_scoreboard.transform.GetChild(2).gameObject.SetActive(false);
        }
        chooseTimer = 5;
        role1 = (Role)GameManager.manager._root_round_data.data.role;
        InitGame((Role)GameManager.manager._root_round_data.data.role);
        MeTieRound.GetComponent<Image>().sprite = Result[GameManager.manager._root_round_data.data.score.round[5].round_score];
        OppTieRound.GetComponent<Image>().sprite = Result[GameManager.manager._root_round_data.data.opponent_score.round[5].round_score];
        if (my_scoreboard != null) 
        {
            my_scoreboard.transform.GetChild(0).GetComponent<Text>().text = GameManager.manager._root_round_data.data.score.total.ToString();
            opponent_scoreboard.transform.GetChild(0).GetComponent<Text>().text = GameManager.manager._root_round_data.data.opponent_score.total.ToString();

            for (int i = 0; i < GameManager.manager._root_round_data.data.score.round.Length - 1; i++)
            {
                for (int j = 1; j < 3; j++)
                {
                    if (GameManager.manager._root_round_data.data.score.round[i].round_score == j)
                    {
                        //Debug.Log(GameManager.manager._root_round_data.data.score.round[i].round_score);
                        // my_scoreboard.transform.GetChild(2).GetChild(4 - i).GetComponent<Image>().sprite = Result[j];
                        my_scoreboard.transform.GetChild(2).GetChild(i).GetComponent<Image>().sprite = Result[j];
                    }

                }
            }
            for (int i = 0; i < GameManager.manager._root_round_data.data.opponent_score.round.Length - 1; i++)
            {
                for (int j = 1; j < 3; j++)
                {
                    if (GameManager.manager._root_round_data.data.opponent_score.round[i].round_score == j)
                    {
                        opponent_scoreboard.transform.GetChild(2).GetChild(i).GetComponent<Image>().sprite = Result[j];
                        // Debug.Log(GameManager.manager._root_round_data.data.opponent_score.round[i].round_score);
                    }


                }
            }
        }
       

        GameManager.manager.next_round = false;

    }

    void GameOver()
    {

        MeTieRound.GetComponent<Image>().sprite = Result[GameManager.manager._root_round_data.data.score.round[5].round_score];
        OppTieRound.GetComponent<Image>().sprite = Result[GameManager.manager._root_round_data.data.opponent_score.round[5].round_score];
        if (my_scoreboard != null)
        {
            my_scoreboard.transform.GetChild(0).GetComponent<Text>().text = GameManager.manager._root_round_data.data.score.total.ToString();
            opponent_scoreboard.transform.GetChild(0).GetComponent<Text>().text = GameManager.manager._root_round_data.data.opponent_score.total.ToString();

            for (int i = 0; i < GameManager.manager._root_round_data.data.score.round.Length - 1; i++)
            {
                for (int j = 1; j < 3; j++)
                {
                    if (GameManager.manager._root_round_data.data.score.round[i].round_score == j)
                    {
                        //Debug.Log(GameManager.manager._root_round_data.data.score.round[i].round_score);
                        // my_scoreboard.transform.GetChild(2).GetChild(4 - i).GetComponent<Image>().sprite = Result[j];
                        my_scoreboard.transform.GetChild(2).GetChild(i).GetComponent<Image>().sprite = Result[j];
                    }

                }
            }
            for (int i = 0; i < GameManager.manager._root_round_data.data.opponent_score.round.Length - 1; i++)
            {
                for (int j = 1; j < 3; j++)
                {
                    if (GameManager.manager._root_round_data.data.opponent_score.round[i].round_score == j)
                    {
                        opponent_scoreboard.transform.GetChild(2).GetChild(i).GetComponent<Image>().sprite = Result[j];
                        // Debug.Log(GameManager.manager._root_round_data.data.opponent_score.round[i].round_score);
                    }


                }
            }
        }

        SceneManager.LoadScene("Result", LoadSceneMode.Single);
        GameManager.manager.next_round = false;
        GameManager.manager.game_over = false;

    }

    public void OnTopLeftClick()
    {
        SoundManager.manager.playClick();
        direct = Direction.TopLeft;
    }
    public void OnTopRightClick()
    {
        SoundManager.manager.playClick();
        direct = Direction.TopRight;
    }
    public void OnBtmRight()
    {
        SoundManager.manager.playClick();
        direct = Direction.BottomRight;
    }
    public void OnBtmLeft()
    {
        SoundManager.manager.playClick();
        direct = Direction.BottomLeft;
    }
    public void OnMiddleClick()
    {
        SoundManager.manager.playClick();
        direct = Direction.Centre;
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

