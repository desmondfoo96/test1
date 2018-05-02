using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Facebook.Unity;
using SimpleJSON;

public class MainMenuScene : MonoBehaviour
{
    private string[] cardName = new string[2];
    //User Interface refrences
    public Text display_name, striker_name, keeper_name;
    public InputField friend_input;
    public Text balance;
    public Text version;
    public Image rank, avatarImage;


    public GameObject loading_panel;
    public GameObject my_3D_avatar, opponent_3D_avatar;
    public GameObject entry_panel;
    public GameObject striker_panel;
    public GameObject keeper_panel;
    public GameObject right_panel;
    public GameObject friend_entry_panel;
    public GameObject list_panel;
    public GameObject scroll_contain;
    public GameObject scroll_contain2;
    public GameObject top_left_panel;
    public GameObject select_avatar_panel;
    public GameObject leaderboard_panel;
    public GameObject daily_panel;
    public GameObject create_oa_panel;
    public GameObject loadingImg;



    public GameObject[] avatar_3D;
    public Sprite[] rankImg;
    public Sprite[] avatarCard;
    public Sprite[] avatarImg;
    public Sprite[] statusImg;
    public Sprite[] button_sprite;
    public Sprite[] language_icon, avatar_icon;
    public Sprite[] keeper_icon;

    public GameObject[] settings_item;
    public GameObject[] daily_login_items;
    public GameObject[] vs_panel;
    public GameObject[] language_button, avatar_button;
    public GameObject[] profile_items;
    public GameObject[] dynamic_panel;
    public GameObject[] menu_panel;
    public GameObject[] striker_page;
    public GameObject[] keeper_page;
    public GameObject[] difficulty_page;
    public GameObject[] keeper_model;
    public GameObject[] striker_model;
    public GameObject[] menu_buttons;
    public GameObject[] timerUI;

    private Color temp;
    bool striker_started, keeper_started, setTime, vs_started, confirmClicked, checkSocket, playTick = false, clickedMatch;
    string prettyName;
    private float local_timer, beauti_timer, socket_timer = 5;

    Keeper selected_keeper;
    Striker selected_striker;
    static Language language_selected;

    create_oa _create_oa = new create_oa();
    send_user_data _send_user_data = new send_user_data();
    avatar_data _avatar_data = new avatar_data();
    add_friend _friend_data = new add_friend();
    delete_friend _delete_friend = new delete_friend();

    API _api;
    FbookAPI Fbook;
    API.onComplete callback;

    void Start()
    {
        version.text = "v" + Application.version;
        cardName[0] = "N";
        cardName[1] = "M";
        GameManager.manager.onInviteClicked = false;
        GameManager.manager.game_over = false;
        GameManager.manager.cancelled_game = false;
        _api = GetComponent<API>();
        Fbook = GetComponent<FbookAPI>();
        AssignButtons();
        //Debug.Log(GameManager.manager.getMatchMakingData());
        local_timer = 10;
        //pass pid and accesstoken to another body
        GameManager.manager.getMatchMakingData().setAccessToken(GameManager.manager.getPlayerData().data.getAccessToken());
        GameManager.manager.getMatchMakingData().setPlayerID(GameManager.manager.getPlayerData().data.getPlayerId());
        _send_user_data.setAccessToken(GameManager.manager.getPlayerData().data.getAccessToken());
        _send_user_data.setPlayerId(GameManager.manager.getPlayerData().data.getPlayerId());
        _send_user_data.setGameId(1);
        _create_oa.setMethod(2);
        _create_oa.setGameId(1);
        _create_oa.setPlayerId(GameManager.manager.getPlayerData().data.getPlayerId());
        _avatar_data.setPlayerId(GameManager.manager.getPlayerData().data.getPlayerId());
        _friend_data.setPlayerId(GameManager.manager.getPlayerData().data.getPlayerId());
        _delete_friend.setPlayerId(GameManager.manager.getPlayerData().data.getPlayerId());
        _delete_friend.setAccessToken(GameManager.manager.getPlayerData().data.getAccessToken());

        //set player id for body of classes
        GameManager.manager._timer_data.player_id = GameManager.manager.getPlayerData().data.getPlayerId();
        GameManager.manager._striker_data.player_id = GameManager.manager.getPlayerData().data.getPlayerId();
        GameManager.manager._keeper_data.player_id = GameManager.manager.getPlayerData().data.getPlayerId();


        string json = JsonUtility.ToJson(_send_user_data);
        callback += OnMenuLoad;
        _api.POST("/menu_api", json, callback);
        callback -= OnMenuLoad;



        if (PlayerPrefs.GetInt("language") == (int)Language.Chinese)
        {
            language_button[(int)Language.Chinese].GetComponent<Image>().sprite = language_icon[0];
        }
        else if (PlayerPrefs.GetInt("language") == (int)Language.English)
        {
            language_button[(int)Language.English].GetComponent<Image>().sprite = language_icon[2];
        }
        for (int j = 0; j < language_button.Length; j++)
        {
            int x = j;
            language_button[x].GetComponent<Button>().onClick.AddListener(() => OnLanguageClick(x));

        }



        if (!GameManager.manager.new_account && !GameManager.manager.daily_logined && !GameManager.manager.toVsScreen && !GameManager.manager.added_friend && !GameManager.manager.created_oa)
        {
            json = JsonUtility.ToJson(_send_user_data);
            callback += OnDailyClick;
            _api.POST("/check_daily_bonus", json, callback);
            callback -= OnDailyClick;
        }

        //for(int i = 0; i < striker_model.Length;i++)
        //{
        //   GameObject avatar = Instantiate(striker_model[i], new Vector3(999,999,999),  Quaternion.identity);
        //    avatar.name = "striker.avatar" + (i+1);
        //}
        //for (int i = 0; i < striker_model.Length; i++)
        //{
        //    GameObject avatar2 = Instantiate(striker_model[i], new Vector3(999, 999, 999), Quaternion.identity);
        //    avatar2.name = "striker.avatar2" + (i + 1);
        //}
        //for (int i = 0; i < keeper_model.Length; i++)
        //{
        //    GameObject avatar = Instantiate(keeper_model[i], new Vector3(999, 999, 999), Quaternion.identity);
        //    avatar.name = "keeper.avatar" + (i + 1);
        //}

        if (GameManager.manager.toVsScreen)
        {
            entry_panel.SetActive(false);
            right_panel.SetActive(false);
            //keeper_panel.SetActive(false);
            for (int i = 0; i < vs_panel.Length; i++)
            {
                vs_panel[i].SetActive(true);
            }
            for (int i = 0; i < avatar_3D.Length; i++)
            {
                if (GameManager.manager.getMenuData().data.getAvatar() - 1 == i)
                {
                    GameObject avatar = Instantiate(avatar_3D[i], vs_panel[0].transform.GetChild(1).GetChild(0));
                    vs_panel[1].transform.GetChild(0).GetComponent<Image>().sprite = avatarCard[i];
                    vs_panel[1].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = cardName[i];
                    vs_panel[1].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = GameManager.manager.getMenuData().data.getDisplayName().ToString();
                }
            }
            json = JsonUtility.ToJson(GameManager.manager.getMatchMakingData());
            // Debug.Log(json);
            GameManager.manager.SocketEmit("find_game", json);
            GameManager.manager.toVsScreen = false;
        }

        if (GameManager.manager.new_account)
        {
            entry_panel.SetActive(false);
            top_left_panel.SetActive(false);
            right_panel.SetActive(false);
            select_avatar_panel.SetActive(true);
            GameManager.manager.new_account = false;
        }





        if (GameManager.manager.added_friend)
        {
            OnFriendsClick();
            GameManager.manager.added_friend = false;
        }

        if (GameManager.manager.daily_logined)
        {
            entry_panel.SetActive(false);
            daily_panel.SetActive(true);
            entry_panel.SetActive(false);
            daily_login_items[2].GetComponent<Button>().enabled = false;
            daily_login_items[0].SetActive(true);
            daily_login_items[1].SetActive(false);
            daily_login_items[3].SetActive(true);
            GameManager.manager.daily_logined = false;
        }

    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Socket Stuff
        GameManager.manager.OpenConnection();
        GameManager.manager.OnSocketReceive();

    }

    #region settings screen

    public void OnLanguageClick(int button)
    {
        SoundManager.manager.playClick();
        for (int i = 0; i < language_button.Length; i++)
        {
            if (button == (int)Language.Chinese)
            {
                language_button[(int)Language.Chinese].GetComponent<Image>().sprite = language_icon[0];
                language_button[(int)Language.English].GetComponent<Image>().sprite = language_icon[3];
                language_selected = Language.Chinese;
                //PlayerPrefs.SetInt("language", 1);
            }
            if (button == (int)Language.English)
            {
                language_button[(int)Language.Chinese].GetComponent<Image>().sprite = language_icon[1];
                language_button[(int)Language.English].GetComponent<Image>().sprite = language_icon[2];
                language_selected = Language.English;
                //PlayerPrefs.SetInt("language", 0);
            }
            GameManager.manager.LoadLocalizedText((int)language_selected);
        }
    }
    #endregion

    #region Difficulty screen
    public void DifficultyPageChange(int page)
    {
        for (int i = 0; i < difficulty_page.Length; i++)
        {
            if (page == i)
            {
                temp = difficulty_page[i].transform.GetChild(0).GetComponent<Image>().color;
                temp.a = 1f;
                difficulty_page[i].transform.GetChild(0).GetComponent<Button>().enabled = true;
                difficulty_page[i].transform.GetChild(0).GetComponent<Image>().color = temp;
                difficulty_page[i].transform.GetChild(0).transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
            else
            {
                temp = difficulty_page[i].transform.GetChild(0).GetComponent<Image>().color;
                temp.a = 0.5f;
                difficulty_page[i].transform.GetChild(0).GetComponent<Button>().enabled = false;
                difficulty_page[i].transform.GetChild(0).GetComponent<Image>().color = temp;
                difficulty_page[i].transform.GetChild(0).transform.localScale = new Vector3(1, 1, 1);
            }

        }
    }

    //difficulty_page[i].transform.GetChild(0).GetComponent<Button>().onClick.AddListener(()=>);
    //void DifficultyButtonClick(int difficulty)
    //{
    //    for (int i = 1; i < difficulty_page.Length + 1; i++)
    //    {
    //        if (difficulty == i)
    //        {
    //            GameManager.manager.getMatchMakingData().setMatchType(i);
    //            string json = JsonUtility.ToJson(GameManager.manager.getMatchMakingData());

    //            //Debug.Log(json);
    //            GameManager.manager.SocketEmit("find_game", json);
    //            menu_panel[(int)Menu.match].SetActive(false);


    //        }
    //    }
    //}
    #endregion

    #region callbacks
    public void OnLeaderBoard(bool error, string data)
    {
        int x = 0;
        //Debug.Log(data);
        if (!error)
        {
            //save data to current game instance
            GameManager.manager.setLeaderBoardData(JsonUtility.FromJson<root_leaderboard_data>(data));
            //var test = JSON.Parse(data);
            //string jsonString = test["data"].ToString();

            Debug.Log(data);
            //friend_data friend_data = JsonUtility.FromJson<friend_data>(jsonString);


            if (GameManager.manager.getLeaderBoardData().error.getStatusCode() > 0)
            {
                //show error panel
                dynamic_panel[(int)DynamicPanel.error].SetActive(true);
                dynamic_panel[(int)DynamicPanel.error].GetComponentInChildren<Text>().text = GameManager.manager.getLeaderBoardData().error.getMessage();
                //Debug.Log("Error : " + GameManager.manager.getMenuData().error.getMessage());
            }
            else
            {
                //display menu data
                //Debug.Log("Data Retrieval Successful");  foreach (Transform obj1 in scrollList.transform)
                foreach (Transform obj1 in scroll_contain2.transform)
                {
                    GameObject.Destroy(obj1.gameObject);
                }
                foreach (leaderboard_list_data obj in GameManager.manager.getLeaderBoardData().data.list)
                {
                    x++;
                    GameObject lead;
                    lead = Instantiate(list_panel) as GameObject;
                    lead.transform.SetParent(scroll_contain2.transform, false);

                    Text friend_name = lead.transform.GetChild(0).GetComponent<Text>();
                    Text rank = lead.transform.GetChild(3).GetComponent<Text>();
                    Text winrate = lead.transform.GetChild(2).GetComponent<Text>();
                    Image avatar = lead.transform.GetChild(1).GetComponent<Image>();

                    rank.text = x + ".";
                    friend_name.text = obj.display_name.ToString();
                    winrate.text = obj.win_rate + "%";

                    for (int i = 0; i < 2; i++)
                    {
                        if (obj.avatar_id - 1 == i)
                        {
                            avatar.sprite = avatarImg[i];
                        }
                    }

                }

                leaderboard_panel.SetActive(true);

            }
        }
        else
        {
            Debug.Log(data);
        }
    }
    //void DisplayUsername(IResult result)
    //{
    //    display_name.text = "" + result.ResultDictionary["name"];
    //}

    //protected void HandleResult(IResult result)
    //{
    //    if (FB.IsLoggedIn)
    //    {
    //        FB.API("/me?fields=name", HttpMethod.GET, DisplayUsername);
    //    }
    //    else
    //    {

    //    }
    //    if (!string.IsNullOrEmpty(result.RawResult))
    //    {


    //    }

    //}

    //menu api callback
    public void OnMenuLoad(bool error, string data)
    {
        // Debug.Log(data);
        if (!error)
        {
            //save data to current game instance
            GameManager.manager.setMenuData(JsonUtility.FromJson<menu_data>(data));

            // Debug.Log(data);

            if (GameManager.manager.getMenuData().error.getStatusCode() > 0)
            {
                //show error panel
                dynamic_panel[(int)DynamicPanel.error].SetActive(true);
                dynamic_panel[(int)DynamicPanel.error].GetComponentInChildren<Text>().text = GameManager.manager.getMenuData().error.getMessage();
                //Debug.Log("Error : " + GameManager.manager.getMenuData().error.getMessage());
            }
            else
            {
                //display menu data
                //Debug.Log(GameManager.manager.getMenuData().data.getRank());
                for (int i = 0; i < rankImg.Length; i++)
                {
                    if (GameManager.manager.getMenuData().data.getRank() == i)
                    {
                        rank.sprite = rankImg[i];
                    }
                }
                display_name.text = GameManager.manager.getMenuData().data.getDisplayName().ToString();
                balance.text = GameManager.manager.getMenuData().data.getBalance().ToString();


                if (GameManager.manager.getMenuData().data.getAvatar() == 1)
                {
                    avatarImage.sprite = avatarImg[0];
                }
                if (GameManager.manager.getMenuData().data.getAvatar() == 2)
                {
                    avatarImage.sprite = avatarImg[1];
                }
                if (GameManager.manager.created_oa)
                {
                    OnSettingsClick();
                    GameManager.manager.created_oa = false;
                }

                for (int i = 0; i < avatar_3D.Length; i++)
                {
                    if (GameManager.manager.getMenuData().data.getAvatar() - 1 == i)
                    {
                        GameObject entryAvatar = Instantiate(avatar_3D[i], entry_panel.transform.GetChild(0));

                    }
                }

                GameManager.manager.isFb = false;

            }
        }
        else
        {
            Debug.Log(data);
        }
    }
    public void OnClaimLoad(bool error, string data)
    {
        if (!error)
        {
            //Debug.Log(data);
            GameManager.manager.daily_logined = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Debug.Log(data);
        }
    }

    public void OnDailyClick(bool error, string data)
    {
        SoundManager.manager.playClick();
        //Debug.Log(data);
        if (!error)
        {

            //save data to current game instance
            GameManager.manager.setDailyLogin(JsonUtility.FromJson<daily_login>(data));

            //Debug.Log(data);
            if (GameManager.manager.getDailyLogin().error.getStatusCode() == 3026)
            {
                daily_panel.SetActive(false);
                //entry_panel.SetActive(true);
                //daily_login_items[2].GetComponent<Button>().enabled = false;
                //daily_login_items[0].SetActive(true);
                //daily_login_items[1].SetActive(false);
                //daily_login_items[3].SetActive(true);
            }
            else
            {
                daily_panel.SetActive(true);
                entry_panel.SetActive(false);
                daily_login_items[2].GetComponent<Button>().enabled = true;
                daily_login_items[0].SetActive(false);
                daily_login_items[1].SetActive(true);
                daily_login_items[3].SetActive(false);
            }
        }
        else
        {
            Debug.Log(data);
        }
    }
    public void OnSettingsLoad(bool error, string data)
    {
        //Debug.Log(data);
        if (!error)
        {
            //save data to current game instance
            GameManager.manager.setSettingsData(JsonUtility.FromJson<root_settings_data>(data));

            //Debug.Log(data);
            if (GameManager.manager.getSettingsData().error.getStatusCode() > 0)
            {
                //show error panel
                dynamic_panel[(int)DynamicPanel.error].SetActive(true);
                dynamic_panel[(int)DynamicPanel.error].GetComponentInChildren<Text>().text = GameManager.manager.getSettingsData().error.getMessage();
                //Debug.Log("Error : " + GameManager.manager.getMenuData().error.getMessage());
            }
            else
            {
                if (GameManager.manager.getSettingsData().data.getIsGuest() == 1)
                {
                    settings_item[0].SetActive(true);
                    settings_item[1].SetActive(false);
                }
                if (GameManager.manager.getSettingsData().data.getIsGuest() == 0)
                {
                    settings_item[0].SetActive(false);
                    settings_item[1].SetActive(true);
                    settings_item[1].GetComponent<Text>().text = "Username : " + GameManager.manager.getSettingsData().data.getUsername();
                }

                if (GameManager.manager.getMenuData().data.getAvatar() == 1)
                {
                    avatar_button[0].GetComponent<Image>().sprite = avatar_icon[2];
                    avatar_button[1].GetComponent<Image>().sprite = avatar_icon[1];
                }
                if (GameManager.manager.getMenuData().data.getAvatar() == 2)
                {
                    avatar_button[0].GetComponent<Image>().sprite = avatar_icon[3];
                    avatar_button[1].GetComponent<Image>().sprite = avatar_icon[0];
                }
            }
        }
        else
        {
            Debug.Log(data);
        }
    }
    public void OnProfileLoad(bool error, string data)
    {
        //Debug.Log(data);
        if (!error)
        {
            //save data to current game instance
            GameManager.manager.setPlayerStats(JsonUtility.FromJson<statistics_data>(data));

            //Debug.Log(data);
            if (GameManager.manager.getPlayerStats().error.getStatusCode() > 0)
            {
                //show error panel
                dynamic_panel[(int)DynamicPanel.error].SetActive(true);
                dynamic_panel[(int)DynamicPanel.error].GetComponentInChildren<Text>().text = GameManager.manager.getPlayerStats().error.getMessage();
                //Debug.Log("Error : " + GameManager.manager.getMenuData().error.getMessage());
            }
            else
            {
                //display menu data
                //Debug.Log("Data Retrieval Successful");
                if (GameManager.manager.getPlayerStats().data.getRank() == (int)Rank.bronze)
                {
                    profile_items[0].transform.GetChild(4).GetComponent<Text>().text = (GameManager.manager.getPlayerStats().data.getExp().ToString());
                    profile_items[0].transform.GetChild(5).gameObject.SetActive(false);
                }
                else if (GameManager.manager.getPlayerStats().data.getRank() == (int)Rank.silver)
                {
                    profile_items[0].transform.GetChild(4).GetComponent<Text>().text = 30.ToString();
                    profile_items[1].transform.GetChild(4).GetComponent<Text>().text = ((GameManager.manager.getPlayerStats().data.getExp() - 30).ToString());
                    profile_items[0].transform.GetChild(5).gameObject.SetActive(false);
                    profile_items[1].transform.GetChild(5).gameObject.SetActive(false);
                }
                else if (GameManager.manager.getPlayerStats().data.getRank() == (int)Rank.gold)
                {
                    profile_items[0].transform.GetChild(4).GetComponent<Text>().text = 30.ToString();
                    profile_items[1].transform.GetChild(4).GetComponent<Text>().text = 50.ToString();
                    profile_items[2].transform.GetChild(4).GetComponent<Text>().text = ((GameManager.manager.getPlayerStats().data.getExp() - 80).ToString());
                    profile_items[0].transform.GetChild(5).gameObject.SetActive(false);
                    profile_items[1].transform.GetChild(5).gameObject.SetActive(false);
                    profile_items[2].transform.GetChild(5).gameObject.SetActive(false);
                }

                profile_items[3].GetComponent<Text>().text = GameManager.manager.getPlayerStats().data.getWinRate().ToString() + "%";
                profile_items[4].GetComponent<Text>().text = GameManager.manager.getPlayerStats().data.getTotalMatch().ToString();
                profile_items[5].GetComponent<Text>().text = GameManager.manager.getPlayerStats().data.getTotalGoal().ToString();
                profile_items[6].GetComponent<Text>().text = GameManager.manager.getPlayerStats().data.getTotalBlock().ToString();
            }
        }
        else
        {
            Debug.Log(data);
        }
    }
    public void OnFriendsLoad(bool error, string data)
    {
        //Debug.Log(data);
        if (!error)
        {
            //save data to current game instance
            GameManager.manager.setFriendData(JsonUtility.FromJson<root_friend_data>(data));
            //var test = JSON.Parse(data);
            //string jsonString = test["data"].ToString();

            //Debug.Log(jsonString);
            //friend_data friend_data = JsonUtility.FromJson<friend_data>(jsonString);

            Debug.Log(data);

            if (GameManager.manager.getFriendData().error.getStatusCode() > 0)
            {
                //show error panel
                dynamic_panel[(int)DynamicPanel.error].SetActive(true);
                dynamic_panel[(int)DynamicPanel.error].GetComponentInChildren<Text>().text = GameManager.manager.getFriendData().error.getMessage();
                //Debug.Log("Error : " + GameManager.manager.getMenuData().error.getMessage());
            }
            else
            {
                //display menu data
                //Debug.Log("Data Retrieval Successful");  foreach (Transform obj1 in scrollList.transform)
                foreach (Transform obj1 in scroll_contain.transform)
                {
                    GameObject.Destroy(obj1.gameObject);
                }
                foreach (friend_list_data obj in GameManager.manager.getFriendData().data.friend_list)
                {
                    GameObject scorePanel;
                    scorePanel = Instantiate(friend_entry_panel) as GameObject;
                    scorePanel.transform.SetParent(scroll_contain.transform, false);

                    Text friend_name = scorePanel.transform.GetChild((int)FriendPanel.Name).GetComponent<Text>();
                    Text winrate = scorePanel.transform.GetChild((int)FriendPanel.Winrate).GetComponent<Text>();
                    Image avatar = scorePanel.transform.GetChild((int)FriendPanel.Avatar).GetComponent<Image>();
                    //Image rank = scorePanel.transform.GetChild((int)FriendPanel.Rank).GetComponent<Image>();
                    Image status = scorePanel.transform.GetChild((int)FriendPanel.Status).GetComponent<Image>();
                    Button delete = scorePanel.transform.GetChild(5).GetComponent<Button>();
                    Button invite_to_play = scorePanel.transform.GetChild(7).GetComponent<Button>();

                    friend_name.text = obj.display_name;
                    winrate.text = obj.win_rate + "%";
                    delete.onClick.AddListener(() => DeleteFriend(obj.id));
                    invite_to_play.onClick.AddListener(() => OnInviteButton(obj.id, invite_to_play));

                    for (int i = 0; i < 3; i++)
                    {
                        if (obj.rank == i)
                        {
                            rank.sprite = rankImg[i];
                        }
                    }
                    for (int i = 0; i < avatarImg.Length; i++)
                    {
                        if ((obj.avatar_id - 1) == i)
                        {
                            avatar.sprite = avatarImg[i];
                        }
                    }
                    if (obj.status == "online")
                    {
                        status.sprite = statusImg[0];
                    }
                    else if (obj.status == "offline")
                    {
                        status.sprite = statusImg[1];
                    }

                }

                menuFunction((int)Menu.friends);
            }
        }
        else
        {
            Debug.Log(data);
        }
    }
    public void OnDeleteFriend(bool error, string data)
    {
        //Debug.Log(data);
        if (!error)
        {
            //save data to current game instance
            GameManager.manager.setRoot_oa(JsonUtility.FromJson<root_oa>(data));
            //var test = JSON.Parse(data);
            //string jsonString = test["data"].ToString();

            Debug.Log(data);
            //friend_data friend_data = JsonUtility.FromJson<friend_data>(jsonString);

            //Debug.Log(GameManager.manager.getFriendData().data.friend_list[0].id);

            if (GameManager.manager.getRoot_oa().error.getStatusCode() > 0)
            {
                //show error panel
                dynamic_panel[(int)DynamicPanel.error].SetActive(true);
                dynamic_panel[(int)DynamicPanel.error].GetComponentInChildren<Text>().text = GameManager.manager.getRoot_oa().error.getMessage();
                //Debug.Log("Error : " + GameManager.manager.getMenuData().error.getMessage());
            }
            else
            {
                GameManager.manager.added_friend = true;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else
        {
            Debug.Log(data);
        }
    }
    public void OnCreateOALoad(bool error, string data)
    {
        //Debug.Log(data);
        if (!error)
        {
            //save data to current game instance
            GameManager.manager.setRoot_oa(JsonUtility.FromJson<root_oa>(data));
            //var test = JSON.Parse(data);
            //string jsonString = test["data"].ToString();

            Debug.Log(data);
            //friend_data friend_data = JsonUtility.FromJson<friend_data>(jsonString);

            //Debug.Log(GameManager.manager.getFriendData().data.friend_list[0].id);

            if (GameManager.manager.getRoot_oa().error.getStatusCode() > 0)
            {
                //show error panel
                dynamic_panel[(int)DynamicPanel.error].SetActive(true);
                dynamic_panel[(int)DynamicPanel.error].GetComponentInChildren<Text>().text = GameManager.manager.getRoot_oa().error.getMessage();
                //Debug.Log("Error : " + GameManager.manager.getMenuData().error.getMessage());
            }
            else
            {
                GameManager.manager.created_oa = true;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else
        {
            Debug.Log(data);
        }
    }
    public void OnAddFriendsLoad(bool error, string data)
    {
        //Debug.Log(data);
        if (!error)
        {
            //save data to current game instance
            GameManager.manager.setFriendData(JsonUtility.FromJson<root_friend_data>(data));
            //var test = JSON.Parse(data);
            //string jsonString = test["data"].ToString();

            Debug.Log(data);
            //friend_data friend_data = JsonUtility.FromJson<friend_data>(jsonString);

            //Debug.Log(GameManager.manager.getFriendData().data.friend_list[0].id);

            if (GameManager.manager.getFriendData().error.getStatusCode() > 0)
            {
                //show error panel
                dynamic_panel[(int)DynamicPanel.error].SetActive(true);
                dynamic_panel[(int)DynamicPanel.error].GetComponentInChildren<Text>().text = GameManager.manager.getFriendData().error.getMessage();
                //Debug.Log("Error : " + GameManager.manager.getMenuData().error.getMessage());
            }
            else
            {
                GameManager.manager.added_friend = true;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else
        {
            Debug.Log(data);
        }
    }
    #endregion

    #region select striker and keeper

    public void StrikerPageChange(int page)
    {
        for (int i = 0; i < striker_page.Length; i++)
        {
            if (page == i)
            {
                selected_striker = (Striker)i + 1;
                GameManager.manager._striker_data.striker_id = (int)selected_striker;
                striker_name.text = GetPrettyName((int)i, (int)Role.Striker);
                temp = striker_page[i].transform.GetChild(0).GetComponent<Image>().color;
                temp.a = 1f;
                striker_page[i].transform.GetChild(0).GetComponent<Image>().color = temp;
                striker_page[i].transform.GetChild(0).transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                //Debug.Log(GameObject.Find("striker.avatar" + (i + 1)));
                GameObject.Find("striker.avatar" + (i + 1)).transform.parent = GameObject.Find("ModelPosition").transform;
                GameObject.Find("striker.avatar" + (i + 1)).transform.position = GameObject.Find("striker.avatar" + (i + 1)).transform.parent.position;
            }
            else
            {
                temp = striker_page[i].transform.GetChild(0).GetComponent<Image>().color;
                temp.a = 0.5f;
                striker_page[i].transform.GetChild(0).GetComponent<Image>().color = temp;
                striker_page[i].transform.GetChild(0).transform.localScale = new Vector3(1, 1, 1);
                GameObject.Find("striker.avatar" + (i + 1)).transform.parent = null;
                GameObject.Find("striker.avatar" + (i + 1)).transform.position = new Vector3(999, 999, 999);
            }

        }
    }

    public void KeeperPageChange(int page)
    {
        for (int i = 0; i < keeper_page.Length; i++)
        {
            if (page == i)
            {
                selected_keeper = (Keeper)i + 1;
                GameManager.manager._keeper_data.keeper_id = (int)selected_keeper;
                keeper_name.text = GetPrettyName((int)i, (int)Role.Keeper);
                temp = keeper_page[i].transform.GetChild(0).GetComponent<Image>().color;
                temp.a = 1f;
                keeper_page[i].transform.GetChild(0).GetComponent<Image>().color = temp;
                keeper_page[i].transform.GetChild(0).transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                GameObject.Find("keeper.avatar" + (i + 1)).transform.parent = GameObject.Find("ModelPosition").transform;
                GameObject.Find("keeper.avatar" + (i + 1)).transform.position = GameObject.Find("keeper.avatar" + (i + 1)).transform.parent.position;
            }
            else
            {
                temp = keeper_page[i].transform.GetChild(0).GetComponent<Image>().color;
                temp.a = 0.5f;
                keeper_page[i].transform.GetChild(0).GetComponent<Image>().color = temp;
                keeper_page[i].transform.GetChild(0).transform.localScale = new Vector3(1, 1, 1);
                GameObject.Find("keeper.avatar" + (i + 1)).transform.parent = null;
                GameObject.Find("keeper.avatar" + (i + 1)).transform.position = new Vector3(999, 999, 999);
            }

        }
    }
    #endregion

    #region AvatarSelect
    public void OnSettings1Click()
    {
        _avatar_data.setAvatarId(2);
        string json = JsonUtility.ToJson(_avatar_data);

        callback += OnAvatarSettingsLoad;
        _api.POST("/update_avatar", json, callback);
        callback -= OnAvatarSettingsLoad;
    }
    public void OnSettings2Click()
    {
        _avatar_data.setAvatarId(1);
        string json = JsonUtility.ToJson(_avatar_data);
        callback += OnAvatarSettingsLoad;
        _api.POST("/update_avatar", json, callback);
        callback -= OnAvatarSettingsLoad;
    }
    public void OnAvatar1Click()
    {
        SoundManager.manager.playSelect();
        _avatar_data.setAvatarId(2);
        string json = JsonUtility.ToJson(_avatar_data);

        callback += OnAvatarLoad;
        _api.POST("/update_avatar", json, callback);
        callback -= OnAvatarLoad;
    }
    public void OnAvatar2Click()
    {
        SoundManager.manager.playSelect();
        _avatar_data.setAvatarId(1);
        string json = JsonUtility.ToJson(_avatar_data);
        callback += OnAvatarLoad;
        _api.POST("/update_avatar", json, callback);
        callback -= OnAvatarLoad;
    }
    private void OnAvatarSettingsLoad(bool error, string data)
    {
        //Debug.Log(data);
        if (!error)
        {
            GameManager.manager.created_oa = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
        else
        {
            Debug.Log(data);
        }
    }
    private void OnAvatarLoad(bool error, string data)
    {
        //Debug.Log(data);
        if (!error)
        {
            Debug.Log(data);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //top_left_panel.SetActive(true);
            //right_panel.SetActive(true);
            //select_avatar_panel.SetActive(false);

            //string json = JsonUtility.ToJson(_send_user_data);
            //callback += OnDailyClick;
            //_api.POST("/check_daily_bonus", json, callback);
            //callback -= OnDailyClick;
        }
        else
        {
            Debug.Log(data);
        }
    }
    #endregion

    #region menu button
    private void menuFunction(int j)
    {
        for (int i = 0; i < menu_buttons.Length; i++)
        {
            if (i == j)
            {
                menu_panel[j].SetActive(true);
                menu_buttons[j].GetComponent<Image>().sprite = button_sprite[0];
            }
            else
            {
                daily_panel.SetActive(false);
                leaderboard_panel.SetActive(false);
                entry_panel.SetActive(false);
                menu_panel[i].SetActive(false);
                menu_buttons[i].GetComponent<Image>().sprite = button_sprite[1];
            }
        }
    }

    private void disablePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void OnMatchClick()
    {
        SoundManager.manager.playClick();
        if (GameManager.manager.getMenuData().data.getBalance() < 2600000)
        {
            dynamic_panel[(int)DynamicPanel.error].SetActive(true);
            dynamic_panel[(int)DynamicPanel.error].GetComponentInChildren<Text>().text = "Not enough money!!";
        }
        else
        {
            if (GameManager.manager.socket_ready == false)
            {
                loadingImg.SetActive(true);
                clickedMatch = true;
            }
            if (GameManager.manager.socket_ready == true && clickedMatch == false)
            {
                //GameManager.manager.getMatchMakingData().setCharacter(0);
                string json = JsonUtility.ToJson(GameManager.manager.getMatchMakingData());
                Debug.Log(json);
                GameManager.manager.SocketEmit("find_game", json);

                leaderboard_panel.SetActive(false);
                entry_panel.SetActive(false);
                daily_panel.SetActive(false);
                right_panel.SetActive(false);
                for (int i = 0; i < menu_panel.Length; i++)
                {
                    menu_panel[i].SetActive(false);
                }
                //keeper_panel.SetActive(false);
                for (int i = 0; i < vs_panel.Length; i++)
                {
                    vs_panel[i].SetActive(true);
                }
                for (int i = 0; i < avatar_3D.Length; i++)
                {
                    if (GameManager.manager.getMenuData().data.getAvatar() - 1 == i)
                    {
                        GameObject avatar = Instantiate(avatar_3D[i], vs_panel[0].transform.GetChild(1).GetChild(0));
                        vs_panel[1].transform.GetChild(0).GetComponent<Image>().sprite = avatarCard[i];
                        vs_panel[1].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = cardName[i];
                        vs_panel[1].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = GameManager.manager.getMenuData().data.getDisplayName().ToString();
                    }
                }
            }
        }
        //menuFunction((int)Menu.match);

    }
    public void OnInviteButton(int friend_id, Button myself)
    {
        if (GameManager.manager.getMenuData().data.getBalance() < 2600000)
        {
            dynamic_panel[(int)DynamicPanel.error].SetActive(true);
            dynamic_panel[(int)DynamicPanel.error].GetComponentInChildren<Text>().text = "Not enough money!!";
        }
        else
        {
            if (GameManager.manager.onInviteClicked == false)
            {
                Debug.Log("aaaaa");
                GameManager.manager.friend_match = true;
                _delete_friend.setFriendId(friend_id);
                _delete_friend.setDisplay(GameManager.manager.getMenuData().data.getDisplayName().ToString());
                string json = JsonUtility.ToJson(_delete_friend);
                GameManager.manager.SocketEmit("send_invite", json);
            }
            GameManager.manager.onInviteClicked = true;
        }
    }



    public void OnFriendsClick()
    {
        SoundManager.manager.playClick();
        string json = JsonUtility.ToJson(_send_user_data);
        Debug.Log(json);
        callback += OnFriendsLoad;
        _api.POST("/show_friend", json, callback);
        callback -= OnFriendsLoad;
    }

    public void OnLeaderBoardClick()
    {
        SoundManager.manager.playClick();
        menu_panel[2].SetActive(false);
        string json = JsonUtility.ToJson(_send_user_data);
        //Debug.Log(json);
        callback += OnLeaderBoard;
        _api.POST("/show_leaderboard", json, callback);
        callback -= OnLeaderBoard;
    }

    public void OnSettingsClick()
    {
        SoundManager.manager.playClick();
        menuFunction((int)Menu.setting);

        string json = JsonUtility.ToJson(_send_user_data);
        callback += OnSettingsLoad;
        _api.POST("/check_is_guest", json, callback);
        callback -= OnSettingsLoad;
    }

    public void OnProfileClick()
    {
        SoundManager.manager.playClick();
        menuFunction((int)Menu.profile);
        string json = JsonUtility.ToJson(_send_user_data);
        callback += OnProfileLoad;
        _api.POST("/get_statistics", json, callback);
        callback -= OnProfileLoad;

    }

    public void OnStoreClick()
    {
        SoundManager.manager.playClick();
        menuFunction((int)Menu.store);

    }
    public void OnClaimClick()
    {
        SoundManager.manager.playClick();
        string json = JsonUtility.ToJson(_send_user_data);
        callback += OnClaimLoad;
        _api.POST("/daily_bonus", json, callback);
        callback -= OnClaimLoad;

    }

    public void OnLogoutClick()
    {

        SoundManager.manager.playClick();
        dynamic_panel[(int)DynamicPanel.confirm].SetActive(true);

    }
    public void OnAddFriendClick()
    {
        SoundManager.manager.playClick();
        _friend_data.setFriendId(friend_input.text);
        string json = JsonUtility.ToJson(_friend_data);
        Debug.Log(json);
        callback += OnAddFriendsLoad;
        _api.POST("/add_friend", json, callback);
        callback -= OnAddFriendsLoad;


    }
    public void OnCOAClick()
    {
        SoundManager.manager.playClick();
        create_oa_panel.SetActive(true);
    }

    public void OnMatchMakingCancelClick()
    {
        SoundManager.manager.playClick();
        string json = JsonUtility.ToJson(GameManager.manager.getMatchMakingData());
        GameManager.manager.SocketEmit("cancel_match_making", json);
        Debug.Log(json);
    }
    public void OnCreateOnlineAccountClick()
    {
        SoundManager.manager.playClick();
        _create_oa.setUsername(create_oa_panel.transform.GetChild(0).GetChild(0).GetComponent<InputField>().text);
        _create_oa.setPassword(create_oa_panel.transform.GetChild(0).GetChild(1).GetComponent<InputField>().text);
        string json = JsonUtility.ToJson(_create_oa);
        Debug.Log(json);
        callback += OnCreateOALoad;
        _api.POST("/sign_up", json, callback);
        callback -= OnCreateOALoad;
        create_oa_panel.SetActive(false);
    }

    public void OnCreateCancelClick()
    {
        SoundManager.manager.playClick();
        create_oa_panel.SetActive(false);
    }

    public void OnAddButtonClick()
    {
        SoundManager.manager.playClick();
        dynamic_panel[(int)DynamicPanel.add_friend].SetActive(true);

    }
    public void OnStrikerConfirmClick()
    {
        SoundManager.manager.playClick();
        confirmClicked = true;
        string json = JsonUtility.ToJson(GameManager.manager._striker_data);
        GameManager.manager.SocketEmit("selected_striker", json);

    }
    public void OnKeeperConfirmClick()
    {
        SoundManager.manager.playClick();
        confirmClicked = true;
        string json = JsonUtility.ToJson(GameManager.manager._keeper_data);
        GameManager.manager.SocketEmit("selected_keeper", json);

    }
    public void OnConfirmClick()
    {
        if (FB.IsLoggedIn)
        {
            FB.LogOut();
        }
        GameManager.manager.logout = true;
        GameManager.manager.CloseConnection();
        SoundManager.manager.playClick();
        Fbook.Logout();
        string json = JsonUtility.ToJson(_send_user_data);
        _api.POST("/exit_game", json, DoNothing);
        //GameManager.manager.Delete();
        SceneManager.LoadScene("Login", LoadSceneMode.Single);
    }
    public void OnTEST1()
    {
        Debug.Log("AAAA");
    }

    public void OnBackClick(int panelType)
    {
        SoundManager.manager.playClick();
        for (int i = 0; i < dynamic_panel.Length; i++)
        {
            if (i == panelType)
            {
                disablePanel(dynamic_panel[i]);
            }

        }

    }
    #endregion
    void DoNothing(bool error, string data)
    {

    }
    #region General Functions
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            string json = JsonUtility.ToJson(_send_user_data);
            _api.POST("/exit_game", json, DoNothing);
        }
        else
        {

            string json = JsonUtility.ToJson(_send_user_data);
            _api.POST("/exit_game", json, DoNothing);

        }

    }
    void DeleteFriend(int id)
    {
        SoundManager.manager.playClick();
        _delete_friend.setFriendId(id);
        string json = JsonUtility.ToJson(_delete_friend);
        callback += OnDeleteFriend;
        _api.POST("/delete_friend", json, callback);
        callback -= OnDeleteFriend;
    }

    string GetPrettyName(int number, int role)
    {
        if (role == 1)
        {
            if (number == 0)
                prettyName = "Cristiano Ronaldo";
            else if (number == 1)
                prettyName = "Jordan Henderson";
            else if (number == 2)
                prettyName = "Lionel Messi";
            else if (number == 3)
                prettyName = "Neymar Junior";
            else if (number == 4)
                prettyName = "Paul Pogba";
            else if (number == 5)
                prettyName = "Wayne Rooney";
        }
        else if (role == 0)
        {
            if (number == 0)
                prettyName = "David De Gea";
            else if (number == 1)
                prettyName = "Gianluigi Buffon";
            else if (number == 2)
                prettyName = "Joe Hart";
        }
        return prettyName;
    }

    private void AssignButtons()
    {

        dynamic_panel[(int)DynamicPanel.confirm].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => OnBackClick((int)DynamicPanel.confirm));
        dynamic_panel[(int)DynamicPanel.error].transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(() => OnBackClick((int)DynamicPanel.error));
        dynamic_panel[(int)DynamicPanel.add_friend].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => OnBackClick((int)DynamicPanel.add_friend));
        //for (int test123 = 1; test123 < difficulty_page.Length + 1; test123++)
        //{
        //    int x24 = test123;
        //    difficulty_page[x24-1].transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => DifficultyButtonClick(x24));

        //}
    }

    void Update()
    {
        if (clickedMatch)
        {
            if (GameManager.manager.socket_ready == true)
            {
                loadingImg.SetActive(false);
                clickedMatch = false;
                OnMatchClick();
            }
        }
        vs_panel[0].transform.GetChild(2).GetChild(0).transform.rotation = vs_panel[0].transform.GetChild(1).GetChild(0).transform.rotation;
        if (!GameManager.manager.socket_ready)
        {
            socket_timer -= Time.deltaTime;
            {
                if (socket_timer <= 0)
                {
                    SceneManager.LoadScene("Login", LoadSceneMode.Single);
                    //dynamic_panel[DynamicPanel.error].
                }
            }

        }
        if (GameManager.manager.finding_game == true)
        {
            balance.text = GameManager.manager._root_new_balance.data.getNewBalance().ToString();
            GameManager.manager.finding_game = false;
        }
        if (GameManager.manager.cancelled_game == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.manager.cancelled_game = false;
        }
        if (striker_started == true)
        {
            if (GameManager.manager.timer_wait == true)
            {
                local_timer -= Time.deltaTime;
                striker_panel.transform.GetChild(5).GetChild(1).GetComponent<CircularTimer>().SetFillAmt(local_timer, 10);
                striker_panel.transform.GetChild(5).GetChild(2).GetComponent<Text>().text = ((int)local_timer).ToString();
                //Debug.Log(GameManager.manager._root_timer_data.data.time_left);
                if (GameManager.manager._root_timer_data.data.time_left <= 0 && confirmClicked == false)
                {
                    string json = JsonUtility.ToJson(GameManager.manager._striker_data);
                    GameManager.manager.SocketEmit("selected_striker", json);
                    //Debug.Log(json);
                    GameManager.manager.timer_wait = false;
                    striker_started = false;
                }
            }

        }

        if (keeper_started == true)
        {
            if (GameManager.manager.timer_wait == true)
            {

                local_timer -= Time.deltaTime;
                keeper_panel.transform.GetChild(5).GetChild(1).GetComponent<CircularTimer>().SetFillAmt(local_timer, 10);
                keeper_panel.transform.GetChild(5).GetChild(2).GetComponent<Text>().text = ((int)local_timer).ToString();
                //Debug.Log(GameManager.manager._root_timer_data.data.time_left);
                if (GameManager.manager._root_timer_data.data.time_left <= 0 && confirmClicked == false)
                {
                    string json = JsonUtility.ToJson(GameManager.manager._keeper_data);
                    GameManager.manager.SocketEmit("selected_keeper", json);

                    //Debug.Log(GameManager.manager._root_timer_data.data.time_left);
                    GameManager.manager.timer_wait = false;
                    keeper_started = false;
                }
            }

        }

        if (vs_started == true)
        {
            if (GameManager.manager.timer_wait == true)
            {
                if (GameManager.manager.game_over == true) 
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                local_timer -= Time.deltaTime;
                vs_panel[0].transform.GetChild(0).GetChild(1).GetComponent<CircularTimer>().SetFillAmt(local_timer, 10);
                vs_panel[0].transform.GetChild(0).GetChild(2).GetComponent<Text>().text = ((int)local_timer).ToString();
                //Debug.Log(GameManager.manager._root_timer_data.data.time_left);
                if (GameManager.manager._root_timer_data.data.time_left <= 0)
                {
                    playTick = false;
                    local_timer = GameManager.manager._root_timer_data.data.time_left;
                    GameManager.manager.timer_wait = false;
                    SceneManager.LoadScene("Game", LoadSceneMode.Single);
                    vs_started = false;
                }

                if (GameManager.manager._root_timer_data.data.time_left == 10)
                {
                    if (!playTick)
                    {
                        local_timer = GameManager.manager._root_timer_data.data.time_left;
                        SoundManager.manager.playTick();
                        playTick = true;
                    }
                }

                if (GameManager.manager._root_timer_data.data.time_left == 9)
                {
                    if (playTick)
                    {
                        local_timer = GameManager.manager._root_timer_data.data.time_left;
                        SoundManager.manager.playTick();
                        playTick = false;
                    }
                }
                if (GameManager.manager._root_timer_data.data.time_left == 8)
                {
                    if (!playTick)
                    {
                        local_timer = GameManager.manager._root_timer_data.data.time_left;
                        SoundManager.manager.playTick();
                        playTick = true;
                    }
                }
                if (GameManager.manager._root_timer_data.data.time_left == 7)
                {
                    if (playTick)
                    {
                        local_timer = GameManager.manager._root_timer_data.data.time_left;
                        SoundManager.manager.playTick();
                        playTick = false;
                    }
                }
                if (GameManager.manager._root_timer_data.data.time_left == 6)
                {
                    if (!playTick)
                    {
                        local_timer = GameManager.manager._root_timer_data.data.time_left;
                        SoundManager.manager.playTick();
                        playTick = true;
                    }
                }
                if (GameManager.manager._root_timer_data.data.time_left == 5)
                {
                    if (playTick)
                    {
                        local_timer = GameManager.manager._root_timer_data.data.time_left;
                        SoundManager.manager.playTick();
                        playTick = false;
                    }
                }
                if (GameManager.manager._root_timer_data.data.time_left == 4)
                {
                    if (!playTick)
                    {
                        local_timer = GameManager.manager._root_timer_data.data.time_left;
                        SoundManager.manager.playTick();
                        playTick = true;
                    }
                }
                if (GameManager.manager._root_timer_data.data.time_left == 3)
                {
                    if (playTick)
                    {
                        local_timer = GameManager.manager._root_timer_data.data.time_left;
                        SoundManager.manager.playTick();
                        playTick = false;
                    }
                }
                if (GameManager.manager._root_timer_data.data.time_left == 2)
                {
                    if (!playTick)
                    {
                        local_timer = GameManager.manager._root_timer_data.data.time_left;
                        SoundManager.manager.playTick();
                        playTick = true;
                    }
                }
                if (GameManager.manager._root_timer_data.data.time_left == 1)
                {
                    if (playTick)
                    {
                        local_timer = GameManager.manager._root_timer_data.data.time_left;
                        SoundManager.manager.playTick();
                        playTick = false;
                    }
                }



            }

        }
        //if (GameManager.manager.found_game == true)
        //{
        //    local_timer = 10;
        //    confirmClicked = false;
        //    GameManager.manager.timer_wait = false;
        //    striker_panel.SetActive(true);
        //    right_panel.SetActive(false);
        //    string timer = JsonUtility.ToJson(GameManager.manager._timer_data);
        //    GameManager.manager.SocketEmit("start_timer", timer);
        //    striker_started = true;
        //    GameManager.manager.found_game = false;
        //}

        if (GameManager.manager.striker_done == true)
        {
            for (int i = 0; i < striker_model.Length; i++)
            {
                GameObject.Find("striker.avatar" + (i + 1)).transform.parent = null;
                GameObject.Find("striker.avatar" + (i + 1)).transform.position = new Vector3(999, 999, 999);
            }
            striker_started = false;
            local_timer = 10;
            confirmClicked = false;
            GameManager.manager.timer_wait = false;
            striker_panel.SetActive(false);
            keeper_panel.SetActive(true);
            string timer = JsonUtility.ToJson(GameManager.manager._timer_data);
            GameManager.manager.SocketEmit("start_timer", timer);
            keeper_started = true;
            GameManager.manager.striker_done = false;
        }

        if (GameManager.manager.found_game == true)
        {
            GameManager.manager.waitResponse.SetActive(false);
            if (GameManager.manager.friend_match == false)
            {
                loading_panel.SetActive(false);
                vs_panel[0].transform.GetChild(2).gameObject.SetActive(true);
                vs_panel[1].transform.GetChild(1).gameObject.SetActive(true);
                for (int i = 0; i < avatar_3D.Length; i++)
                {
                    if (GameManager.manager.getMenuData().data.getAvatar() - 1 != i)
                    {
                        GameObject avatar = Instantiate(avatar_3D[i], vs_panel[0].transform.GetChild(2).GetChild(0));
                        vs_panel[1].transform.GetChild(1).GetComponent<Image>().sprite = avatarCard[i];
                        vs_panel[1].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = cardName[i];
                        vs_panel[1].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = GameManager.manager._root_match_data.data.opponent_name.ToString();
                    }
                }
                local_timer = 10;
                confirmClicked = false;
                keeper_started = false;
                GameManager.manager.timer_wait = false;

                //for (int i = 0; i < striker_model.Length; i++)
                //{
                //    if (GameManager.manager._root_opponent_data.data.opponent_striker == i + 1) 
                //    {
                //        //Debug.Log(GameObject.Find("striker.avatar" + (i + 1)));
                //        GameObject.Find("striker.avatar" + (i + 1)).transform.parent = vs_panel[0].transform.GetChild(2).GetChild(0);
                //        GameObject.Find("striker.avatar" + (i + 1)).transform.position = GameObject.Find("striker.avatar" + (i + 1)).transform.parent.position;
                //    }
                //    if ((int)selected_striker == i+1)
                //    {
                //        //Debug.Log("striker.avatar2" + (i + 1));
                //        GameObject.Find("striker.avatar2" + (i + 1)).transform.parent = vs_panel[0].transform.GetChild(1).GetChild(0);
                //        GameObject.Find("striker.avatar2" + (i + 1)).transform.position = GameObject.Find("striker.avatar2" + (i + 1)).transform.parent.position;
                //    }
                //}
                //for (int i = 0; i < keeper_icon.Length; i++)
                //{
                //    if (GameManager.manager._root_opponent_data.data.opponent_keeper == i + 1)
                //    {
                //        vs_panel[1].transform.GetChild(1).GetComponent<Image>().sprite = keeper_icon[i];
                //        vs_panel[1].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = GameManager.manager.GetLocalizedValue("keeper_" + (i + 1));
                //        vs_panel[1].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = GameManager.manager.GetLocalizedValue("keeper");
                //    }
                //    if ((int)selected_keeper == i + 1)
                //    {
                //        vs_panel[1].transform.GetChild(0).GetComponent<Image>().sprite = keeper_icon[i];
                //        vs_panel[1].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = GameManager.manager.GetLocalizedValue("keeper_" + (i + 1));
                //        vs_panel[1].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = GameManager.manager.GetLocalizedValue("keeper");
                //    }
                //}


                string timer = JsonUtility.ToJson(GameManager.manager._timer_data);
                Debug.Log(timer);
                GameManager.manager.SocketEmit("start_timer", timer);
                vs_started = true;
                GameManager.manager.found_game = false;
            }
            if (GameManager.manager.friend_match == true)
            {
                balance.text = GameManager.manager._root_match_data.data.new_balance.ToString();
                leaderboard_panel.SetActive(false);
                entry_panel.SetActive(false);
                daily_panel.SetActive(false);
                right_panel.SetActive(false);
                for (int i = 0; i < menu_panel.Length; i++)
                {
                    menu_panel[i].SetActive(false);
                }
                //keeper_panel.SetActive(false);
                for (int i = 0; i < vs_panel.Length; i++)
                {
                    vs_panel[i].SetActive(true);
                }
                for (int i = 0; i < avatar_3D.Length; i++)
                {
                    if (GameManager.manager.getMenuData().data.getAvatar() - 1 == i)
                    {
                        GameObject avatar = Instantiate(avatar_3D[i], vs_panel[0].transform.GetChild(1).GetChild(0));
                        vs_panel[1].transform.GetChild(0).GetComponent<Image>().sprite = avatarCard[i];
                        vs_panel[1].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = cardName[i];
                        vs_panel[1].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = GameManager.manager.getMenuData().data.getDisplayName().ToString();
                    }
                }

                loading_panel.SetActive(false);
                vs_panel[0].transform.GetChild(2).gameObject.SetActive(true);
                vs_panel[1].transform.GetChild(1).gameObject.SetActive(true);
                for (int i = 0; i < avatar_3D.Length; i++)
                {
                    if ((GameManager.manager._root_match_data.data.opponent_avatar - 1) == i)
                    {
                        GameObject avatar = Instantiate(avatar_3D[i], vs_panel[0].transform.GetChild(2).GetChild(0));
                        vs_panel[1].transform.GetChild(1).GetComponent<Image>().sprite = avatarCard[i];
                        vs_panel[1].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = cardName[i];
                        vs_panel[1].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = GameManager.manager._root_match_data.data.opponent_name.ToString();
                    }
                }
                local_timer = 10;
                confirmClicked = false;
                keeper_started = false;
                GameManager.manager.timer_wait = false;

                //for (int i = 0; i < striker_model.Length; i++)
                //{
                //    if (GameManager.manager._root_opponent_data.data.opponent_striker == i + 1) 
                //    {
                //        //Debug.Log(GameObject.Find("striker.avatar" + (i + 1)));
                //        GameObject.Find("striker.avatar" + (i + 1)).transform.parent = vs_panel[0].transform.GetChild(2).GetChild(0);
                //        GameObject.Find("striker.avatar" + (i + 1)).transform.position = GameObject.Find("striker.avatar" + (i + 1)).transform.parent.position;
                //    }
                //    if ((int)selected_striker == i+1)
                //    {
                //        //Debug.Log("striker.avatar2" + (i + 1));
                //        GameObject.Find("striker.avatar2" + (i + 1)).transform.parent = vs_panel[0].transform.GetChild(1).GetChild(0);
                //        GameObject.Find("striker.avatar2" + (i + 1)).transform.position = GameObject.Find("striker.avatar2" + (i + 1)).transform.parent.position;
                //    }
                //}
                //for (int i = 0; i < keeper_icon.Length; i++)
                //{
                //    if (GameManager.manager._root_opponent_data.data.opponent_keeper == i + 1)
                //    {
                //        vs_panel[1].transform.GetChild(1).GetComponent<Image>().sprite = keeper_icon[i];
                //        vs_panel[1].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = GameManager.manager.GetLocalizedValue("keeper_" + (i + 1));
                //        vs_panel[1].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = GameManager.manager.GetLocalizedValue("keeper");
                //    }
                //    if ((int)selected_keeper == i + 1)
                //    {
                //        vs_panel[1].transform.GetChild(0).GetComponent<Image>().sprite = keeper_icon[i];
                //        vs_panel[1].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = GameManager.manager.GetLocalizedValue("keeper_" + (i + 1));
                //        vs_panel[1].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = GameManager.manager.GetLocalizedValue("keeper");
                //    }
                //}


                string timer = JsonUtility.ToJson(GameManager.manager._timer_data);
                Debug.Log(timer);
                GameManager.manager.SocketEmit("start_timer", timer);
                vs_started = true;
                GameManager.manager.found_game = false;
                Debug.Log("AAAA");
            }
        }

    }



    #endregion
}
