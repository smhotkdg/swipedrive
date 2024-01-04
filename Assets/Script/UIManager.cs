using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public GameObject CommingSoonUI;
    public GameObject TutorialObj;
    public Text GoldText;
    public List<GameObject> TopCarList;    
    public List<GameObject> ButtomCarList;
    public GameObject LoadingUI;
    public GameObject StartUI;
    public GameObject NextUI;
    public GameObject EndGameUI;
    public GameObject ClearEffect;
    public GameObject GameoverEffect;
    
    public Text NextText;
    public Text FailText;
    private static UIManager _instance = null;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("cSingleton UIManager == null");
            return _instance;
        }
    }
    public void SetCamera()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
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
    private void Start()
    {
        
    }
    bool start = false;
    public void NextView()
    {
        FunGameSDKManager.Instance.LevelComplete();
        SoundManager.Instance.StartSound((int)SoundManager.SOUND_TYPE.Clear);
        NextUI.SetActive(true);
        ClearEffect.SetActive(true);
        if (GameManager.Instance.Level < GameManager.Instance.totalSceneCount)
        {
            NextText.text = "Level " + GameManager.Instance.Level.ToString();
        }
        else
        {
            NextText.text = "Event Level";
        }
    }
    public void FailView()
    {
        FunGameSDKManager.Instance.LevelFail();
        SoundManager.Instance.StartSound((int)SoundManager.SOUND_TYPE.Fail);
        EndGameUI.SetActive(true);
        GameoverEffect.SetActive(true);
        if (GameManager.Instance.Level < GameManager.Instance.totalSceneCount)
        {
            FailText.text = "Level " + GameManager.Instance.Level.ToString();
        }
        else
        {
            FailText.text = "Event Level";
        }
        
    }
    public void disableTutorial()
    {
        if(TutorialObj.activeSelf == true)
            TutorialObj.SetActive(false);
    }
    int prevLevel = 0;
    int NowLevel;
    int randLevel =0;
    public void LoadSecen(bool isFail)
    {        
        string scenename = "Level" + GameManager.Instance.Level.ToString();
        if(GameManager.Instance.MapList[0].activeSelf==true)
            GameManager.Instance.MapList[0].SetActive(false);
        StartUI.SetActive(false);
        NextUI.SetActive(false);
        EndGameUI.SetActive(false);
        ClearEffect.SetActive(false);
        GameoverEffect.SetActive(false);
        start = false;
        if(GameManager.Instance.Level < GameManager.Instance.totalSceneCount)
            LoadLevel(scenename);
        else
        {
            prevLevel = GameManager.Instance.GetActiveLevel();
            if(isFail == true)
            {
                randLevel = Random.Range(2, GameManager.Instance.totalSceneCount);
                //randLevel = 2;
            }
                
            NowLevel = randLevel;
            string scenenameRand = "Level" + randLevel.ToString();
            //CommingSoonUI.SetActive(true);
            LoadLevel(scenenameRand,true);
            return;
        }
        
    }

    public void LoadLevel(string nameScene,bool isRand = false)
    {
        if (start == false)
        {
            LoadingUI.SetActive(true);
            StartCoroutine(LoadAsynchronously(nameScene,isRand));
            start = true;
        }
    }

    int gamecount = 0;
    IEnumerator LoadAsynchronously(string nameScen,bool isRand = false)
    {
        if(isRand ==false)
        {
            GameManager.Instance.SetNewLevel();
        }
        else
        {
            GameManager.Instance.SetNewLevelRand(prevLevel, NowLevel);
        }
        yield return new WaitForSeconds(0.5f);
        if (GameManager.Instance.Level == 1 || GameManager.Instance.Level == 2)
        {
            TutorialObj.SetActive(true);
        }
        if(isRand == false)
        {
            GameManager.Instance.SetParticle();
        }
        else
        {
            GameManager.Instance.SetParticleRand(NowLevel);
        }
        
        //AdManager.Instance.ShowRewardedAds(0);
        if(gamecount > 1)
        {
            //AdManager.Instance.ShowINterstitialAds();
            //Debug.Log("광고");

        }
            
        LoadingUI.SetActive(false);
        GameManager.Instance.isEnableGame = true;
        GameManager.Instance.isStop = false;
        gamecount++;
        FunGameSDKManager.Instance.StartLevel();
    }
    public void SelectCar(int index)
    {
        for (int i = 0; i < ButtomCarList.Count; i++)
        {
            if (GameManager.Instance.isBuyCar[1] == 1)
            {
                ButtomCarList[i].SetActive(true);
            }
        }
        //if (GameManager.Instance.isBuyCar[index] == 1)
        {
            for (int i = 0; i < TopCarList.Count; i++)
            {
                if (index == i)
                {
                    TopCarList[i].SetActive(true);
                }
                else
                {
                    if (TopCarList[i].activeSelf == true)
                    {
                        TopCarList[i].SetActive(false);
                    }
                }
            }
           
            //GameManager.Instance.CarType = index;
        }
        if(GameManager.Instance.isBuyCar[index] ==1)
        {
            GameManager.Instance.CarType = index;
        }
        SelectCarNumber = index;
    }
       
    public void InitCar()
    {
        for (int i = 0; i < ButtomCarList.Count; i++)
        {
            if (GameManager.Instance.isBuyCar[i] == 1)
            {
                ButtomCarList[i].SetActive(true);
            }
        }
        for (int i = 0; i < TopCarList.Count; i++)
        {
            TopCarList[i].SetActive(false);
        }
        TopCarList[GameManager.Instance.CarType].SetActive(true);
        
    }
    public int SelectCarNumber = 0;
    public void SetGold()
    {
        GoldText.text = GameManager.Instance.totalGold.ToString();
    }
    public void BuyCar()
    {
        if (GameManager.Instance.totalGold >= 100)
        {
            GameManager.Instance.isBuyCar[SelectCarNumber] = 1;
            GameManager.Instance.totalGold -= 100;
            SetGold();
            InitCar();
        }
        else
        {            
            return;
        }
    }
}   
