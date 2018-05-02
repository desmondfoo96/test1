using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.SceneManagement;
using Facebook.Unity;

public class LoginScene : MonoBehaviour {

    //User Interface refrences
    public GameObject online_login;
    //public GameObject guest_login;
    public GameObject login_method;
    public GameObject tap_to_start;
    public GameObject error_panel;
    public InputField password_input;
    public InputField username_input;
    public InputField guest_username_input;
    public Text facebook_login_button;
    public Text online_login_button;
    public Text version;
    public Text guest_login_button;
    public GameObject loadingImg;

    public GameObject[] language_button;
    public Sprite[] language_icon;

    //temporary placeholder
    private string country, ipAdd;
    bool login_error;

    //instance of api and callback
    API _api;
    FbookAPI Fbook;
    API.onComplete callback;

    static Language language_selected;

    private void Start()
    {
        login_error = true;
        version.text = "v"+Application.version;
        GameManager.manager.logout = false;
        GameManager.manager.wait_result = true;
        //PlayerPrefs.DeleteAll();
        //Debug.Log(Application.persistentDataPath);
        GameManager.manager.fail = false;
        if(GameManager.manager.CheckAccessToken())
        {
            login_method.SetActive(false);

            tap_to_start.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetFloat("volume", 50);
            login_method.SetActive(true);
            login_method.transform.GetChild(3).gameObject.SetActive(false);
            tap_to_start.SetActive(false);
        }

        GameManager.manager.LoadLocalizedText(PlayerPrefs.GetInt("language"));
        _api = GetComponent<API>();
        Fbook = GetComponent<FbookAPI>();
        Fbook.InitFB();

        if(PlayerPrefs.GetInt("language") == (int)Language.Chinese)
        {
            language_button[(int)Language.Chinese].GetComponent<Image>().sprite = language_icon[0];
        }
        else if (PlayerPrefs.GetInt("language") == (int)Language.English)
        {
            language_button[(int)Language.English].GetComponent<Image>().sprite = language_icon[2];
        }

       

        //get ipaddress and country
        callback += OnGetCountry;
        _api.GET("http://ip-api.com/json", callback);
        callback -= OnGetCountry;

        //Assign localized text
        LoadLanguage();

        for (int j = 0; j < language_button.Length; j++)
        {
            int x = j;
            language_button[x].GetComponent<Button>().onClick.AddListener(() => OnLanguageClick(x));

        }

    }

        //void OnEnable()
    //{
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    //void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    //void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    //Socket Stuff
    //    //GameManager.manager.OpenConnection();
    //    //GameManager.manager.OnSocketReceive();
    //    Debug.Log("AAA");
    //}

    #region Online Method
    public void OnOnlineClick()
    {
        SoundManager.manager.playClick();
        online_login.SetActive(true);
        login_method.SetActive(false);
    }

    //Login button click
    public void OnLoginclick()
    {
        loadingImg.SetActive(true);
        SoundManager.manager.playClick();
        login_user login_user_data = new login_user();

        login_user_data.setPassword(password_input.text);
        login_user_data.setUsername(username_input.text);
        login_user_data.setIp(country);
        login_user_data.setCountry(ipAdd);

        string json = JsonUtility.ToJson(login_user_data);
        callback += OnLogin;
        Debug.Log(json);
        _api.POST("/login", json, callback);
        callback -= OnLogin;
    }

    //Login Callback
    public void OnLogin(bool error, string data)
    {
        if (!error)
        {
            GameManager.manager.setPlayerData(JsonUtility.FromJson<player_data>(data));

            //Debug.Log(GameManager.manager.getPlayerData().error.getStatusCode());
            if (GameManager.manager.getPlayerData().error.getStatusCode() > 0)
            {
                //show error panel
                login_error = false;
                error_panel.SetActive(true);
                error_panel.GetComponentInChildren<Text>().text = GameManager.manager.getPlayerData().error.getMessage();
                Debug.Log("Error : " + GameManager.manager.getPlayerData().error.getMessage());
            }
            else
            {
                Debug.Log("Login Successful");
                //save access token and player id to phone
                GameManager.manager.Save(GameManager.manager.getPlayerData().data.getPlayerId(), GameManager.manager.getPlayerData().data.getAccessToken());
                GameManager.manager.toVsScreen = false;
                SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
            }
        }
        else
        {

            error_panel.SetActive(true);
            error_panel.GetComponentInChildren<Text>().text = data;
        }
    }
    #endregion

    #region Guest Method
    //public void OnGuestClick()
    //{
    //    //guest_login.SetActive(true);
    //    login_method.SetActive(false);
    //}

    public void OnGuestLoginclick()
    {
        loadingImg.SetActive(true);
        SoundManager.manager.playClick();
        guest_user guest_user_data = new guest_user();
        
        //guest_user_data.setUsername(guest_username_input.text);
        guest_user_data.setCountry(country);
        guest_user_data.setIp(ipAdd);
        guest_user_data.setMethod(3);
        guest_user_data.setGame_Id(1);
        guest_user_data.setCharacter(1);

        string json = JsonUtility.ToJson(guest_user_data);
        callback += OnGuestLogin;
        Debug.Log(json);
        _api.POST("/sign_up", json, callback);

        callback -= OnGuestLogin;
    }


    //Guest Login Callback
    public void OnGuestLogin(bool error, string data)
    {

        if (!error)
        {
            GameManager.manager.setPlayerData(JsonUtility.FromJson<player_data>(data));
            //Debug.Log(GameManager.manager.getPlayerData().error.getStatusCode());
            if (GameManager.manager.getPlayerData().error.getStatusCode() > 0)
            {
                //show error panel
                error_panel.SetActive(true);
                error_panel.GetComponentInChildren<Text>().text = GameManager.manager.getPlayerData().error.getMessage();
                Debug.Log("Error : " + GameManager.manager.getPlayerData().error.getMessage());
            }
            else
            {
                Debug.Log("Login Successful");
                //save data to phone
                GameManager.manager.Save(GameManager.manager.getPlayerData().data.getPlayerId(),GameManager.manager.getPlayerData().data.getAccessToken());
                //Debug.Log(GameManager.manager.getPlayerData().data.getAccessToken());
                GameManager.manager.toVsScreen = false;
                GameManager.manager.new_account = true;
                SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
            }
        }
        else
        {

            error_panel.SetActive(true);
            error_panel.GetComponentInChildren<Text>().text = data;
        }
    }
    #endregion


    #region Facebook Method

    void SendAPI(IResult result)
    {
        facebook_user fb_user_data = new facebook_user();
        fb_user_data.setUsername(Fbook.GetID());
        fb_user_data.setDisplayName("" + result.ResultDictionary["name"]);
        fb_user_data.setCountry(country);
        fb_user_data.setIp(ipAdd);
        fb_user_data.setMethod(1);
        fb_user_data.setGame_Id(1);

        string json = JsonUtility.ToJson(fb_user_data);
        callback += OnFB;
        //Debug.Log(json);
        _api.POST("/sign_up", json, callback);
        callback -= OnFB;
    }

    protected void HandleResult(IResult result)
    {
        if (FB.IsLoggedIn)
        {
            //Debug.Log(AccessToken.CurrentAccessToken.UserId);

            FB.API("/me?fields=name", HttpMethod.GET, SendAPI);
           
        }
        else
        {

        }
        if (!string.IsNullOrEmpty(result.RawResult))
        {


        }

    }
    //facebook button click
    public void OnFacebookLoginClick()
    {
        GameManager.manager.isFb = true;
        loadingImg.SetActive(true);
        SoundManager.manager.playClick();
        Fbook.FBLogin(HandleResult);
       
    }

    //Login Callback
    public void OnFB(bool error, string data)
    {
        if (!error)
        {
            GameManager.manager.setPlayerData(JsonUtility.FromJson<player_data>(data));

            //Debug.Log(GameManager.manager.getPlayerData().error.getStatusCode());
            if (GameManager.manager.getPlayerData().error.getStatusCode() > 0)
            {
                //show error panel
                error_panel.SetActive(true);
                error_panel.GetComponentInChildren<Text>().text = GameManager.manager.getPlayerData().error.getMessage();
                Debug.Log("Error : " + GameManager.manager.getPlayerData().error.getMessage());
            }
            else
            {
                Debug.Log("Login Successful");
                //save access token and player id to phone
                GameManager.manager.Save(GameManager.manager.getPlayerData().data.getPlayerId(), GameManager.manager.getPlayerData().data.getAccessToken());
                GameManager.manager.toVsScreen = false;
                if (GameManager.manager.getPlayerData().error.getMessage() == "successfully created")
                {
                    GameManager.manager.new_account = true;
                }

                SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
            }
        }
        else
        {

            error_panel.SetActive(true);
            error_panel.GetComponentInChildren<Text>().text = data;
        }
    }
    #endregion

    #region AccessTokenExists

    public void OnTapStart()
    {
        loadingImg.SetActive(true);
        //Check If there is existing accessToken stored in device
        CheckToken();
    }

    //Check If there is existing accessToken stored in device
    void CheckToken()
{
        if (GameManager.manager.CheckAccessToken())
        {
            string json = JsonUtility.ToJson(GameManager.manager.getPlayerData().data);
            //Debug.Log(json);
            callback += OnAccessTokenLogin;
            _api.POST("/check_access_token", json, callback);
            callback -= OnAccessTokenLogin;
        }
        else
        {
            //Do Nothing
        }
    }

    //Access Token Login Callback
    public void OnAccessTokenLogin(bool error, string data)
    {

        if (!error)
        {
            GameManager.manager.setPlayerData(JsonUtility.FromJson<player_data>(data));

            //Debug.Log(GameManager.manager.getPlayerData().error.getStatusCode());
            if (GameManager.manager.getPlayerData().error.getStatusCode() > 0)
            {
                //delete the access token
                GameManager.manager.Delete();
                //show error panel
                error_panel.SetActive(true);
                error_panel.GetComponentInChildren<Text>().text = GameManager.manager.getPlayerData().error.getMessage();

               // Debug.Log("Error : " + GameManager.manager.getPlayerData().error.getMessage());
            }
            else
            {
                //Debug.Log("Login Successful");
                //Debug.Log(GameManager.manager.getPlayerData().data.getBalance());
                GameManager.manager.toVsScreen = false;
                SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
            }
        }
        else
        {
            error_panel.SetActive(true);
            error_panel.GetComponentInChildren<Text>().text = data;
        }
    }
    #endregion

    #region language
    private void LoadLanguage()
    {
        guest_login_button.text = GameManager.manager.GetLocalizedValue("guest_login");
        facebook_login_button.text = GameManager.manager.GetLocalizedValue("facebook_login");
        online_login_button.text = GameManager.manager.GetLocalizedValue("online_login");
    }
    public void OnLanguageClick(int button)
    {
        for (int i = 0; i < language_button.Length; i++)
        {
            if (button == (int)Language.Chinese)
            {
                language_button[(int)Language.Chinese].GetComponent<Image>().sprite = language_icon[0];
                language_button[(int)Language.English].GetComponent<Image>().sprite = language_icon[3];
                language_selected = Language.Chinese;
                PlayerPrefs.SetInt("language", 1);
            }
            if (button == (int)Language.English)
            {
                language_button[(int)Language.Chinese].GetComponent<Image>().sprite = language_icon[1];
                language_button[(int)Language.English].GetComponent<Image>().sprite = language_icon[2];
                language_selected = Language.English;
                PlayerPrefs.SetInt("language", 0);
            }

        }


        GameManager.manager.LoadLocalizedText((int)language_selected);
        LoadLanguage();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    #region General functions

    //GetCountry callback
    public void OnGetCountry(bool error, string data)
    {
        if (!error)
        {
            var result = JSON.Parse(data);
           // Debug.Log(data);
            country = result["country"];
            ipAdd = result["query"];
        }
        else
        {
            Debug.Log("Error : " + data);
        }
    }
    
    //Return to choosing login method
    public void OnBackClick()
    {
        SoundManager.manager.playClick();
        online_login.SetActive(false);
        //guest_login.SetActive(false);
        login_method.SetActive(true);
    }  
    //Return to accesstoken
    public void OnSwitchBackClick()
    {
        SoundManager.manager.playClick();
        login_method.SetActive(false);
        tap_to_start.SetActive(true);
    }

    public void OnErrorOkClick()
    {
        SoundManager.manager.playClick();
        error_panel.SetActive(false);
        if (login_error == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        login_error = true;
        loadingImg.SetActive(false);
    }

    public void OnSwitchAccount()
    {
        SoundManager.manager.playClick();
        login_method.SetActive(true);
        tap_to_start.SetActive(false);
    }
    #endregion


}
