using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class API : MonoBehaviour {
    
    public delegate void onComplete(bool error, string data);

    //string api_URL = "http://kick-api.fun1881.com";
    string api_URL = "http://192.168.0.121:8092";

    public WWW GET(string url, onComplete callback)
    {

        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www, callback));
        return www;
    }

    public WWW POST(string url, string jsonStr, onComplete callback)
    {
        WWWForm form = new WWWForm();

        //foreach (KeyValuePair<string, string> post_arg in post)
        //{
        //    form.AddField(post_arg.Key, post_arg.Value);
        //}
        Dictionary<string, string> postHeader = form.headers;
        if (postHeader.ContainsKey("Content-Type"))
            postHeader["Content-Type"] = "application/json";
        else
            postHeader.Add("Content-Type", "application/json");


        byte[] fromData = System.Text.Encoding.UTF8.GetBytes(jsonStr);

        WWW www = new WWW(api_URL+url, fromData, postHeader);

        StartCoroutine(WaitForRequest(www, callback));
        return www;
    }

    private IEnumerator WaitForRequest(WWW www, onComplete callback)
    {
        yield return www;
        string results;
        // check for errors
        if (www.error == null)
        {
            results = www.text;


            callback(false, results);
        }
        else
        {
            callback(true, www.error);
        }
    }
}
