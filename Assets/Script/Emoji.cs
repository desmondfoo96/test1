
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Emoji : MonoBehaviour
{
    //public GameObject panel;
    public ParticleSystem[] emoji;
    public GameObject emoji_prefab, spawnLoc;
    public Sprite[] emojiS;
    bool open;
    Vector3 scale = new Vector3();
    float speed = 10;

    send_emoji _send_emoji = new send_emoji();

    private void Start()
    {
        //scale = new Vector3(0, 1, 1);
        _send_emoji.setPlayerId(GameManager.manager.getPlayerData().data.getPlayerId());
    }
    private void FixedUpdate()
    {
        //if (open == true)
        //{
        //    if (scale.x >= 1)
        //    {
        //        scale.x = 1;
        //    }
        //    if (scale.x < 1)
        //    {
        //        scale.x += (0.01f * speed);
        //    }
        //}
        //else
        //{
        //    if (scale.x <= 0)
        //    {
        //        scale.x = 0;
        //    }
        //    if (scale.x > 0)
        //    {
        //        scale.x -= (0.01f * speed);
        //    }

        //}
        //panel.transform.localScale = scale;

        if(GameManager.manager.spawn_emoji == true)
        {
            //float rand = Random.Range(0.3f, 0.9f);
            //GameObject emojic = Instantiate(emoji_prefab,spawnLoc.transform);
            //emojic.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, rand);
            //emojic.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, rand);
            //emojic.transform.localPosition = new Vector3(0, 0, 0);
            //for (int i = 0; i < emojiS.Length; i++)
            //{
            //    if (GameManager.manager._root_receive_emoji.data.getEmoji() == i)
            //    {
            //        emojic.GetComponent<Image>().sprite = emojiS[i];
            //    }
            //}
            for (int i = 0; i < emojiS.Length; i++)
            {
                if (GameManager.manager._root_receive_emoji.data.getEmoji() == i)
                {
                    emoji[i].Emit(1);
                }
            }
            
            GameManager.manager.spawn_emoji = false;
        }
    }

    public void onButtonCLick()
    {
        if (open == false)
        {
            open = true;
        }
        else
        {
            open = false;
        }
    }


    public void OnHappyClick()
    {
        _send_emoji.setEmoji(0);
        string json = JsonUtility.ToJson(_send_emoji);
        GameManager.manager.SocketEmit("send_emoji", json);
    }

    public void OnSadClick()
    {
        _send_emoji.setEmoji(1);
        string json = JsonUtility.ToJson(_send_emoji);
        GameManager.manager.SocketEmit("send_emoji", json);
    }
    public void OnDepressedClick()
    {
        _send_emoji.setEmoji(2);
        string json = JsonUtility.ToJson(_send_emoji);
        GameManager.manager.SocketEmit("send_emoji", json);
    }
   

}
