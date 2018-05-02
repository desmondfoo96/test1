using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FbookAPI : MonoBehaviour {

    IEnumerable<string> permissions = new List<string>() { "public_profile", "email", "user_friends" };

    string displayName;
    // Use this for initialization
    void Start () {


        
    }
 
    public void InitFB()
    {
        FB.Init(OnInitComplete, OnHideUnity);
    }

    private void OnInitComplete()
    {
       // Debug.Log("Init Success");
    }

    private void OnHideUnity(bool isGameShown)
    {
    }

    public void FBLogin(FacebookDelegate<ILoginResult> test1)
    {
        FB.LogInWithReadPermissions(permissions, test1);
       

    }

    public string GetID()
    {
        if (FB.IsLoggedIn)
        {

            return AccessToken.CurrentAccessToken.UserId;

        }
        else
        {
            return null;
        }
    }





    public void Logout()
    {
        if (FB.IsLoggedIn)
        {

            FB.LogOut();

        }
        
    }

    void RefreshCallback(IAccessTokenRefreshResult result)
    {
        
    }

   

    // Update is called once per frame
    void Update () {
		
	}
}
