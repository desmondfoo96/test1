using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    public static SoundManager manager = null;

    AudioSource audioSource;
    
    float musicVolume;
    
    [HideInInspector]
    public Slider musicSlider = null;

    public AudioClip start;
    public AudioClip heartbeat, heartbeat2;
    public AudioClip round_tick_sound;
    public AudioClip fail;
    public AudioClip success;
    public AudioClip kick;
    public AudioClip payOut;
    public AudioClip click;
    public AudioClip save;

    public AudioClip select1;
    public AudioClip select2;
    public AudioClip select3;

    AudioClip mainMenuBgm2;
     AudioClip mainMenuBgm;

    Transform settings;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (manager == null)
        {
            manager = this;
        }
        else if (manager != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

        mainMenuBgm = Resources.Load("bg1") as AudioClip;
        mainMenuBgm2 = Resources.Load("bg2") as AudioClip;
        audioSource = GetComponent<AudioSource>();
        playMainMenuBg();

      

    }

    private void Update()
    {

        musicVolume = PlayerPrefs.GetFloat("volume")/100;
        audioSource.volume = musicVolume;

    }

    public void stopBGM()
    {
        audioSource.Stop();
    }
    public void playHeartBeat()
    {
        audioSource.clip = heartbeat2;
        audioSource.Play();

    }

    public void playPay()
    {
        audioSource.Stop();
        audioSource.clip = payOut;
        audioSource.Play();

    }

    public void playMainMenuBg()
    {
        audioSource.Stop();
        int a = Random.Range(0, 2);
        if (a == 0) 
        {
            audioSource.clip = mainMenuBgm;
        }
        else
        {
            audioSource.clip = mainMenuBgm2;
        }
        audioSource.Play();
    }


    public void playKick()
    {
        audioSource.PlayOneShot(kick, musicVolume);
    }

    public void playClick()
    {
        audioSource.PlayOneShot(click, musicVolume);
    }

    public void playSelect()
    {
        int a = Random.Range(0, 3);
        if (a == 0)
        {
            audioSource.PlayOneShot(select1, musicVolume);
        }
        if (a == 1)
        {
            audioSource.PlayOneShot(select2, musicVolume);
        }
        if (a == 2)
        {
            audioSource.PlayOneShot(select3, musicVolume);
        }
    }
    public void playStart()
    {
        audioSource.PlayOneShot(start, musicVolume);
    }

    public void playFail()
    {
        audioSource.PlayOneShot(fail, musicVolume);
    }
    public void playSuccess()
    {
        audioSource.PlayOneShot(success, musicVolume);
    }
    public void playTick()
    {
        audioSource.PlayOneShot(round_tick_sound, musicVolume);
    }
    public void playHeart ()
    {
        audioSource.PlayOneShot(heartbeat, musicVolume);
    }
    public void playSave()
    {
        audioSource.PlayOneShot(save, musicVolume);
    }


}
