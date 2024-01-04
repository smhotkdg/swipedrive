using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{    // Start is called before the first frame update    

    public AudioSource CarStart;
    public AudioSource Coin;
    public AudioSource Door;
    public AudioSource Fail;
    public AudioSource Man;
    public AudioSource Clear;
    public AudioSource BGM;
    private static SoundManager _instance = null;
    public enum SOUND_TYPE{
        CarStart,
        Coin,
        Door,
        Fail,
        Man,
        Clear
    };
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("cSingleton SoundManager == null");
            return _instance;
        }
    }
    private void Start()
    {
        if (GameManager.Instance.isStartSound == true)
        {
            BGM.Play();
        }
    }
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            //
            _instance = this;
            DontDestroyOnLoad(gameObject);     
        }
    }
    public void StartSound(int type)
    {
        if(GameManager.Instance.isStartSound ==true)
        {
            switch(type)
            {
                case (int)SOUND_TYPE.CarStart:
                    CarStart.Play();
                    break;
                case (int)SOUND_TYPE.Coin:
                    Coin.Play();
                    break;
                case (int)SOUND_TYPE.Door:
                    Door.Play();
                    break;
                case (int)SOUND_TYPE.Fail:
                    Fail.Play();
                    break;
                case (int)SOUND_TYPE.Man:
                    Man.Play();
                    break;
                case (int)SOUND_TYPE.Clear:
                    Clear.Play();
                    break;
            }
        }
    }
}
