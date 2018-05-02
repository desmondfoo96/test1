using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using Quobject.SocketIoClientDotNet.Client;
using Facebook.Unity;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    API _api;
    API.onComplete cb;
    //socket declaration
    Socket socket;
    string serverURL = "http://kick.fun1881.com";
    //string serverURL = "http://192.168.0.102:8080";
    //singleton
    [HideInInspector]
    public static GameManager manager = null;

    //localization
    private Dictionary<string, string> localizedText;
    private bool is_localization_ready = false;
    private string missingTextString = "Localized text not found";

    //data that cannot be destroyed
    private player_data _player_data;
    private daily_login _daily_login;
    private menu_data _menu_data;
    private statistics_data _statistics_data;
    private match_making_data _match_making_data;
    [HideInInspector]
    public root_invite_timer _root_invite_timer;
    [HideInInspector]
    public root_match_data _root_match_data;
    [HideInInspector]
    public root_opponent_data _root_opponent_data;
    [HideInInspector]
    public root_timer_data _root_timer_data;
    [HideInInspector]
    public root_round_data _root_round_data;
    [HideInInspector]
    public root_friend_data _root_friend_data;
    [HideInInspector]
    public root_leaderboard_data _root_leaderboard_data;
    [HideInInspector]
    public root_new_balance _root_new_balance;
    [HideInInspector]
    public root_settings_data _root_settings_data;
    [HideInInspector]
    public root_oa _root_oa;
    [HideInInspector]
    public check_ver _check_ver;
    [HideInInspector]
    public get_Ver _get_Ver;
    [HideInInspector]
    public root_receive_emoji _root_receive_emoji;


    [HideInInspector]
    public bool emmitted = false;
    [HideInInspector]
    public bool wait_result = true;
    [HideInInspector]
    public bool found_game = false;
    [HideInInspector]
    public bool striker_done = false;
    [HideInInspector]
    public bool keeper_done = false;
    [HideInInspector]
    public bool timer_wait = false;
    [HideInInspector]
    public bool start_round = false;
    [HideInInspector]
    public bool next_round = false;
    [HideInInspector]
    public bool toVsScreen = false;
    [HideInInspector]
    public bool game_over = false;
    [HideInInspector]
    public bool finding_game = false;
    [HideInInspector]
    public bool cancelled_game = false;
    [HideInInspector]
    public bool new_account = false;
    [HideInInspector]
    public bool added_friend = false;
    [HideInInspector]
    public bool daily_logined = false;
    [HideInInspector]
    public bool finishRoundTextAnimation = false;
    [HideInInspector]
    public bool created_oa = false;
    [HideInInspector]
    public bool socket_ready = false;
    [HideInInspector]
    public bool fail = false;
    [HideInInspector]
    public bool isFb = false;
    [HideInInspector]
    public bool logout = false;
    [HideInInspector]
    public bool cantInvite = false;
    [HideInInspector]
    public bool onInviteClicked = false;
    [HideInInspector]
    public bool friend_match = false;
    [HideInInspector]
    public bool spawn_emoji = false;


    [HideInInspector]
    public bool animation1stRound = false;
    [HideInInspector]
    public bool animation2ndRound = false;
    [HideInInspector]
    public bool animation3rdRound = false;

    [HideInInspector]
    public string display_name= "";

    
    public GameObject error_panel;
    public GameObject confirm_panel;
    public GameObject waitResponse;
    [HideInInspector]
    public timer_data _timer_data;
    [HideInInspector]
    public direction_data _direction_data;
    [HideInInspector]
    public striker_data _striker_data;
    [HideInInspector]
    public keeper_data _keeper_data;

    
    string filePath, errorMsg;
    float local_timer = 5, ping_timer = 1.5f, invite_timer1 = 10;
    bool invite_finding_game, problem;


    void Awake()
    {
        if (manager == null)
        {
            manager = this;
        }

        else if (manager != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        _daily_login = new daily_login();
        _player_data = new player_data();
        _menu_data = new menu_data();
        _match_making_data = new match_making_data();
        _statistics_data = new statistics_data();
        _root_match_data = new root_match_data();
        _root_opponent_data = new root_opponent_data();
        _root_timer_data = new root_timer_data();
        _root_friend_data = new root_friend_data();
        _root_round_data = new root_round_data();
        _root_new_balance = new root_new_balance();
        _root_leaderboard_data = new root_leaderboard_data();
        _root_settings_data = new root_settings_data();
        _root_invite_timer = new root_invite_timer();
        _root_oa = new root_oa();
        _get_Ver = new get_Ver();
        _check_ver = new check_ver();
        _root_receive_emoji = new root_receive_emoji();

         _timer_data = new timer_data();
        _striker_data = new striker_data();
        _keeper_data = new keeper_data();

        Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
    }

    private void Start()
    {
        Debug.Log(Application.version);
        _api = GetComponent<API>();
        

        _check_ver.setVer(Application.version);
        string json = JsonUtility.ToJson(_check_ver);
        cb += OnCheckVer;
        _api.POST("/check_version", json, cb);
        cb -= OnCheckVer;

        errorMsg = "You Disconnected From the Server!!";


        confirm_panel.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => OnOkClick(2));
        confirm_panel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => OnOkClick(3));

    }

    void OnCheckVer(bool error, string data)
    {
        if (!error)
        {
            GameManager.manager.setVer(JsonUtility.FromJson<get_Ver>(data));

            //Debug.Log(GameManager.manager.getPlayerData().error.getStatusCode());
            if (GameManager.manager.getVer().error.getStatusCode() == 2)
            {
                //show error panel
                error_panel.SetActive(true);
                errorMsg = "Invalid Version.\nPlease Update Game.";
                error_panel.GetComponentInChildren<Text>().text = errorMsg;
                error_panel.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(() => OnOkClick(1));
            }
            if (GameManager.manager.getVer().error.getStatusCode() == 3)
            {
                //show error panel
                error_panel.SetActive(true);
                errorMsg = "Invalid Version.";
                error_panel.GetComponentInChildren<Text>().text = errorMsg;
                error_panel.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(() => OnOkClick(1));
            }

        }
        else
        {
            Debug.Log(data);
        }
    }


    public void OpenConnection()
    {
        if (socket == null)
        {
            socket = IO.Socket(serverURL);

            socket.On(Socket.EVENT_CONNECT, () =>
            {
                Debug.Log("Socket.IO connected.");
               // wait_result = false;
                string json = JsonUtility.ToJson(_player_data.data);
                Debug.Log(json);
                socket.Emit("check_duplicate_login", json);
                socket_ready = true;
            });
            

            socket.On(Socket.EVENT_CONNECT_TIMEOUT, () =>
            {
                Debug.Log("Socket.IO Timeout.");
                errorMsg = "You Disconnected From the Server!!";
                fail = true;
                socket_ready = false;
            });

            socket.On(Socket.EVENT_DISCONNECT, () =>
            {
                Debug.Log("Socket.IO Disconnected.");
                errorMsg = "You Disconnected From the Server!!";
                fail = true;
                socket_ready = false;
            });

           

            socket.On(Socket.EVENT_ERROR, (data) =>
            {
                Debug.Log("Socket.IO Disconnected." + data);
                errorMsg = "You Disconnected From the Server!!";
                fail = true;
                socket_ready = false;
            });

            socket.On(Socket.EVENT_RECONNECT_FAILED, (data) =>
            {
                Debug.Log("Socket.IO reconnected fail." + data);
                errorMsg = "You Disconnected From the Server!!";
                fail = true;
                socket_ready = false;
            });

     

            socket.On(Socket.EVENT_RECONNECT_ATTEMPT, (data) =>
            {
                Debug.Log("Socket.IO reconnect attempt." + data);
                errorMsg = "You Disconnected From the Server!!";
                fail = true;
                socket_ready = false;
            });

          
        }
    }

    public void OnAcceptClick()
    {
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
        }
        string json = JsonUtility.ToJson(_player_data.data);
        SocketEmit("accept_invite", json);

    }
    public void OnRejectClick()
    {
        invite_finding_game = false;
        string json = JsonUtility.ToJson(_player_data.data);
        SocketEmit("reject_invite", json);
        waitResponse.SetActive(false);

    }

    public void OnOkClick(int x)
    {
        if (x == 0) 
        {
            error_panel.SetActive(false);
            problem = false;
        }
        if (x == 1)
        {
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.Kango.PenaltyKing");
        }
        if (x == 2)
        {
            Application.Quit();
        }
        if (x == 3)
        {
            confirm_panel.SetActive(false);
        }

    }

    void Ping()
    {
        ping_timer -= Time.deltaTime;
        if(ping_timer<=0)
        {if (socket != null)
            {
                socket.Emit("ping");
                wait_result = false;
                ping_timer = 1.5f;
            }
        }
    }

    void timeOut()
    {
        //Debug.Log(local_timer +" |  " + wait_result);
        if (local_timer <= 0)
        {
            if (wait_result == false)
            {
                fail = true;
                local_timer = 5;
            }
           
        }
        if (wait_result == true)
        {
            local_timer = 5;
        }
        if (wait_result == false)
        {
            local_timer -= Time.deltaTime;
        }
    }

    public void Update()
    {
       
        if (problem)
        {
            invite_finding_game = false;
            friend_match = false;
            onInviteClicked = false;
            waitResponse.SetActive(false);
            error_panel.SetActive(true);
            error_panel.GetComponentInChildren<Text>().text = errorMsg;
            error_panel.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(() => OnOkClick(0));
        }
        if(invite_finding_game)
        {
            waitResponse.SetActive(true);
            invite_timer1 -= Time.deltaTime;
            //waitResponse.transform.GetChild(1).GetChild(0).GetComponent<CircularTimer>().SetFillAmt(invite_timer1, 10);
            waitResponse.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = _root_invite_timer.data.getTimeLeft().ToString();
            invite_timer1 = _root_invite_timer.data.getTimeLeft();
            if(_root_invite_timer.data.getTimeLeft() == 0)
            {
                invite_timer1 = 10;
                invite_finding_game = false;
            }
            if (_root_invite_timer.data.getEndPoint() == 0)
            {
                waitResponse.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Waiting for response....";
                waitResponse.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            }
            if (_root_invite_timer.data.getEndPoint() == 1)
            {
                waitResponse.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "You are challenged to a match by : " + _root_invite_timer.data.getInviter();
                waitResponse.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            }
        }
        Ping();
        timeOut();
        if (emmitted == true)
        {
            wait_result = false;
            emmitted = false;
        }
       if(fail == true)
        {
            invite_finding_game = false;
            if (logout == false)
            {
                waitResponse.SetActive(false);
                error_panel.SetActive(true);
                error_panel.GetComponentInChildren<Text>().text = errorMsg;
                error_panel.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(() => OnOkClick(0));
            }
            waitResponse.SetActive(false);
            CloseConnection();
            SceneManager.LoadScene("Login", LoadSceneMode.Single);
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            
        }
    }
    public void CloseConnection()
    {
        if (socket != null)
        {
            socket.Disconnect();
            socket = null;
        }
    }

    public void OnSocketReceive()
    {
        socket.On("no_money", (data) =>
        {
            root_login_from_device err = new root_login_from_device();
            err = JsonUtility.FromJson<root_login_from_device>(data.ToString());
            Debug.Log(data.ToString());
            errorMsg = err.error.getMessage();
            invite_finding_game = false;
            problem = true;
            //Do Something
        });
        socket.On("friend_no_money", (data) =>
        {
            root_login_from_device err = new root_login_from_device();
            err = JsonUtility.FromJson<root_login_from_device>(data.ToString());
            Debug.Log(data.ToString());
            errorMsg = err.error.getMessage();
            invite_finding_game = false;
            problem = true;
            //Do Something
        });


        socket.On("receive_emoji", (data) =>
        {
            _root_receive_emoji = JsonUtility.FromJson<root_receive_emoji>(data.ToString());
            Debug.Log(data.ToString());
            spawn_emoji = true;
            //Do Something
        });

        socket.On("rejected_invite", (data) =>
        {
            root_login_from_device err = new root_login_from_device();
            err = JsonUtility.FromJson<root_login_from_device>(data.ToString());
            Debug.Log(data.ToString());
            errorMsg = err.error.getMessage();
            invite_finding_game = false;
            problem = true;
            //Do Something
        });
        socket.On("invite_timeout", (data) =>
        {
            root_login_from_device err = new root_login_from_device();
            err = JsonUtility.FromJson<root_login_from_device>(data.ToString());
            Debug.Log(data.ToString());
            errorMsg = err.error.getMessage();
            invite_finding_game = false;
            problem = true;
            //Do Something
        });
        socket.On("invite_timer", (data) =>
        {
            Debug.Log(data.ToString());
            _root_invite_timer = JsonUtility.FromJson<root_invite_timer>(data.ToString());
            friend_match = true;
            invite_finding_game = true;
           
            //Do Something
        });
        socket.On("friend_in_game", (data) =>
        {
            root_login_from_device err = new root_login_from_device();
            err = JsonUtility.FromJson<root_login_from_device>(data.ToString());
            Debug.Log(data.ToString());
            errorMsg = err.error.getMessage();
            invite_finding_game = false;
            problem = true;
            cantInvite = true;
            //Do Something
        });
        socket.On("friend_offline", (data) =>
        {
            root_login_from_device err = new root_login_from_device();
            err = JsonUtility.FromJson<root_login_from_device>(data.ToString());
            Debug.Log(data.ToString());
            errorMsg = err.error.getMessage();
            invite_finding_game = false;
            problem = true;
            cantInvite = true;
            //Do Something
        });

        socket.On("queue_full", (data) =>
        {
            root_login_from_device err = new root_login_from_device();
            err = JsonUtility.FromJson<root_login_from_device>(data.ToString());
            Debug.Log(data.ToString());
            errorMsg = err.error.getMessage();
            fail = true;
            //Do Something
        });

        socket.On("logged_in_another_device", (data) =>
        {
            root_login_from_device err = new root_login_from_device();
            err = JsonUtility.FromJson<root_login_from_device>(data.ToString());
            Debug.Log(data.ToString());
            errorMsg = err.error.getMessage();
            fail = true;
            //Do Something
        });

        socket.On("pong", (data) =>
        {
            Debug.Log(data.ToString());
            wait_result = true;
            //Do Something
        });
        socket.On("cancelled_match_making", (data) =>
        {
            Debug.Log(data.ToString());
            _root_new_balance = JsonUtility.FromJson<root_new_balance>(data.ToString());
            cancelled_game = true;
            //Do Something
        });

        socket.On("find_game_status", (data) =>
        {
            Debug.Log(data.ToString());
            _root_new_balance = JsonUtility.FromJson<root_new_balance>(data.ToString());
            //wait_result = true;
            finding_game = true;
            //Do Something
        });

        socket.On("found_game", (data) =>
        {
            Debug.Log(data.ToString());
            _root_match_data = JsonUtility.FromJson<root_match_data>(data.ToString());
            _timer_data.match_id = _root_match_data.data.match_id;
            _striker_data.match_id = _root_match_data.data.match_id;
            _keeper_data.match_id = _root_match_data.data.match_id;
            found_game = true;
            invite_finding_game = false;
            //Do Something
        });
        

        socket.On("timer", (data) =>
        {
           // Debug.Log(data.ToString());
            _root_timer_data = JsonUtility.FromJson<root_timer_data>(data.ToString());
            timer_wait = true;
            //Do Something
        });

        socket.On("to_keeper_selection", (data) =>
        {
            striker_done = true;
            //Debug.Log(data.ToString());
            //Do Something
        });

        socket.On("to_vs_screen", (data) =>
        {
            //Debug.Log(data.ToString());
            _root_opponent_data = JsonUtility.FromJson<root_opponent_data>(data.ToString());
            keeper_done = true;
            //Do Something
        });

        socket.On("round_timer", (data) =>
        {
          //  Debug.Log(data.ToString());
            _root_timer_data = JsonUtility.FromJson<root_timer_data>(data.ToString());
            start_round = true;
            //Do Something
        });
        socket.On("next_round", (data) =>
        {
            Debug.Log(data.ToString());
            _root_round_data = JsonUtility.FromJson<root_round_data>(data.ToString());
            next_round = true;
            //Do Something
        });
        socket.On("game_over", (data) =>
        {
            friend_match = false;
            next_round = true;
            game_over = true;
            Debug.Log(data.ToString());
            _root_round_data = JsonUtility.FromJson<root_round_data>(data.ToString());
            //Do Something
        });

    }

    public void SocketEmit(string key, string json)
    {

        socket.Emit(key, json);
        
    }
        

    #region localization
    public void LoadLocalizedText(int language)
    {
        localizedText = new Dictionary<string, string>();
        if (language == (int)Language.Chinese)
        {
            filePath = Path.Combine(Application.streamingAssetsPath, "en.json");
        }
        else if (language == (int)Language.English) 
        {
            filePath = Path.Combine(Application.streamingAssetsPath, "en.json");
        }
        string dataAsJson;

        if (Application.platform == RuntimePlatform.Android) //Need to extract file from apk first
        {
            WWW reader = new WWW(filePath);
            while (!reader.isDone) { }

            dataAsJson = reader.text;


        }
        else
        {
            dataAsJson = File.ReadAllText(filePath);
        }

        LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

        for (int i = 0; i < loadedData.items.Length; i++)
        {
            localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
        }

        //Debug.Log("Data loaded, dictionary contains: " + localizedText.Count + " entries");
        is_localization_ready = true;
    }

    public string GetLocalizedValue(string key)
    {
        string result = missingTextString;
        if (localizedText.ContainsKey(key))
        {
            result = localizedText[key];
        }

        return result;

    }

    public bool getIsLocalizationReady()
    {
        return is_localization_ready;
    }
    #endregion

    #region data_saving & loading
    public void Save(int id, string at)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerData.dat");

        user_data saveData = new user_data();
        saveData.setAccessToken(at);
        saveData.setPlayerId(id);

        bf.Serialize(file, saveData);
        file.Close();

    }

    public bool CheckAccessToken()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);
            _player_data.data = (user_data)bf.Deserialize(file);
            file.Close();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Delete()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {

            File.Delete(Application.persistentDataPath + "/playerData.dat");

        }
    }

    #endregion

    #region data set and gets
    public get_Ver getVer()
    {
        return this._get_Ver;
    }
    public void setVer(get_Ver get_Ver)
    {
        this._get_Ver = get_Ver;
    }

    public root_settings_data getSettingsData()
    {
        return this._root_settings_data;
    }
    public void setSettingsData(root_settings_data root_settings_data)
    {
        this._root_settings_data = root_settings_data;
    }
    public match_making_data getMatchMakingData()
    {
        return this._match_making_data;
    }
    public void setMatchMakingData(match_making_data match_making_data)
    {
        this._match_making_data = match_making_data;
    }
    public daily_login getDailyLogin()
    {
        return this._daily_login;
    }
    public void setDailyLogin(daily_login daily_login)
    {
        this._daily_login = daily_login;
    }
    public player_data getPlayerData()
    {
        return this._player_data;
    }
    public void setPlayerData(player_data player_data)
    {
        this._player_data = player_data;
    }
    public root_friend_data getFriendData()
    {
        return this._root_friend_data;
    }
    public void setFriendData(root_friend_data friend_data)
    {
        this._root_friend_data = friend_data;
    }

    public root_leaderboard_data getLeaderBoardData()
    {
        return this._root_leaderboard_data;
    }
    public void setLeaderBoardData(root_leaderboard_data leader_data)
    {
        this._root_leaderboard_data = leader_data;
    }

    public menu_data getMenuData()
    {
        return this._menu_data;
    }
    public void setMenuData(menu_data menu_data)
    {
        this._menu_data = menu_data;
    }

    public statistics_data getPlayerStats()
    {
        return this._statistics_data;
    }
    public void setPlayerStats(statistics_data data)
    {
        this._statistics_data = data;
    }
    public root_oa getRoot_oa()
    {
        return this._root_oa;
    }
    public void setRoot_oa(root_oa data)
    {
        this._root_oa = data;
    }


    #endregion

    void OnApplicationPause(bool pauseStatus)
    {
        if(pauseStatus)
        {
            if (FB.IsLoggedIn)
            {
                FB.LogOut();
            }
            if (isFb == false)
            {
                CloseConnection();
            }
            Debug.Log("Application ending after " + Time.time + " seconds");
        }
        else
        {
            if(FB.IsLoggedIn)
            {
                FB.LogOut();
            }
            if (isFb == false)
            {
                CloseConnection();
            }

        }
       
    }
    void DoNothing(bool error, string data)
    {
        
    }
    void OnApplicationQuit()
    {
        string json = JsonUtility.ToJson(_player_data.data);
        _api.POST("/exit_game", json, DoNothing);

        if (FB.IsLoggedIn)
        {
            FB.LogOut();
        }
        CloseConnection();
        Debug.Log("Application ending after " + Time.time + " seconds");


    }

}
