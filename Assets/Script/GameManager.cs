using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update    

    public List<GameObject> MapList;
    public Material DefaultMat;
    public GameObject Coin;
    public int CarType;
    public List<GameObject> CarList;
    List<MeshRenderer> carMat = new List<MeshRenderer>();
    public List<int> isBuyCar = new List<int>();
    public GameObject NormalEffect;
    public GameObject DesEffect;
    private static GameManager _instance = null;
    public int DrawType;
    public int Level;
    public bool isEnableGame;
    public bool isStop;
    Transform StartPoint;
    public int totalSceneCount;
    public bool isStartSound;
    public int totalGold;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("cSingleton GameManager == null");
            return _instance;
        }
    }
    public int InitLevel;
    public void SetGameSuccess()
    {
        NormalEffect.SetActive(false);
        DesEffect.GetComponent<ParticleSystem>().Play();
        if (Level < totalSceneCount)
            Level++;
        InitLevel = Level;
        UIManager.Instance.NextView();        
        
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
            InitData();
        }
    }
    void InitData()
    {
        DrawType = 0;
        Level = 1;
        InitLevel = 1;
        isEnableGame = true;
        isStop = false;
        CarType = 0;
        isStartSound = true;
        totalGold = 0;
        totalSceneCount = SceneManager.sceneCountInBuildSettings;
        totalSceneCount = 45+1;
        for(int i =0; i< 20; i++)
        {
            isBuyCar.Add(0);
        }
        isBuyCar[0] = 1;
        LoadData();
        
    }
  
    List<Transform> CoinPosTransform = new List<Transform>();

    public void SetParticleRand(int index)
    {
        //if (InitLevel == Level)
        {
            Vector3 cameraVec = Camera.main.transform.position;
            cameraVec.x = -1.65f + (70 * index);
            Camera.main.transform.position = cameraVec;
            Destroy(TempCar);
            for (int i = 0; i < CoinsList.Count; i++)
            {
                Destroy(CoinsList[i]);
            }
            CoinsList.Clear();
            CoinPosTransform.Clear();
            string strLevelEffect = "Level" + index.ToString() + "/DestRed/SwirlAuraYellow";
            NormalEffect = GameObject.Find(strLevelEffect).gameObject;
            NormalEffect.SetActive(true);
            string strLevelDesEffect = "Level" + index.ToString() + "/DestRed/BuffRed";
            DesEffect = GameObject.Find(strLevelDesEffect).gameObject;
            string strLevelStartPoint = "Level" + index.ToString() + "/StartPoints";
            StartPoint = GameObject.Find(strLevelStartPoint).gameObject.transform;
            string strLevelGoldPoint = "Level" + index.ToString() + "/GoldPoints";
            int children = GameObject.Find(strLevelGoldPoint).gameObject.transform.childCount;
            for (int i = 0; i < children; i++)
            {
                CoinPosTransform.Add(GameObject.Find(strLevelGoldPoint).gameObject.transform.GetChild(i));
            }
            string strLevelOpen = "Level" + index.ToString() + "/Open";
            string strLevelDoor = "Level" + index.ToString() + "/Door";
            string strLevelDoor2 = "Level" + index.ToString() + "/Door2";
            GameObject openItem = GameObject.Find(strLevelOpen);
            GameObject DoorItem = GameObject.Find(strLevelDoor);
            GameObject DoorItem2 = GameObject.Find(strLevelDoor2);
            if (openItem != null)
            {
                if (DoorItem != null)
                    DoorItem.GetComponent<DG.Tweening.DOTweenAnimation>().DORewind();
                if (DoorItem2 != null)
                    DoorItem2.GetComponent<DG.Tweening.DOTweenAnimation>().DORewind();

                int children_o = GameObject.Find(strLevelOpen).gameObject.transform.childCount;
                for (int i = 0; i < children_o; i++)
                {
                    openItem.gameObject.transform.GetChild(i).gameObject.SetActive(true);
                }
                //openItem.transform.GetChild(0).gameObject.SetActive(true);
            }

            SetCoin();
            SetCar();
            InitLevel = 0;
        }
    }
    public void SetParticle()
    {
        //if (InitLevel == Level)
        {
            Vector3 cameraVec = Camera.main.transform.position;
            cameraVec.x = -1.65f + (70 * Level);
            Camera.main.transform.position = cameraVec;
            Destroy(TempCar);
            for (int i = 0; i < CoinsList.Count; i++)
            {
                Destroy(CoinsList[i]);
            }
            CoinsList.Clear();
            CoinPosTransform.Clear();
            string strLevelEffect = "Level" + Level.ToString() + "/DestRed/SwirlAuraYellow";
            NormalEffect = GameObject.Find(strLevelEffect).gameObject;
            string strLevelDesEffect = "Level" + Level.ToString() + "/DestRed/BuffRed";
            DesEffect = GameObject.Find(strLevelDesEffect).gameObject;
            string strLevelStartPoint = "Level" + Level.ToString() + "/StartPoints";
            StartPoint = GameObject.Find(strLevelStartPoint).gameObject.transform;
            string strLevelGoldPoint = "Level" + Level.ToString() + "/GoldPoints";
            int children = GameObject.Find(strLevelGoldPoint).gameObject.transform.childCount;
            for (int i = 0; i < children; i++)
            {
                CoinPosTransform.Add(GameObject.Find(strLevelGoldPoint).gameObject.transform.GetChild(i));
            }
            string strLevelOpen = "Level" + Level.ToString() + "/Open";
            string strLevelDoor = "Level" + Level.ToString() + "/Door";
            string strLevelDoor2 = "Level" + Level.ToString() + "/Door2";
            GameObject openItem = GameObject.Find(strLevelOpen);
            GameObject DoorItem = GameObject.Find(strLevelDoor);
            GameObject DoorItem2 = GameObject.Find(strLevelDoor2);            
            if (openItem !=null)
            {
                if(DoorItem !=null)
                    DoorItem.GetComponent<DG.Tweening.DOTweenAnimation>().DORewind();
                if (DoorItem2 != null)
                    DoorItem2.GetComponent<DG.Tweening.DOTweenAnimation>().DORewind();

                int children_o= GameObject.Find(strLevelOpen).gameObject.transform.childCount;
                for (int i = 0; i < children_o; i++)
                {
                    openItem.gameObject.transform.GetChild(i).gameObject.SetActive(true);
                }
                //openItem.transform.GetChild(0).gameObject.SetActive(true);
            }

            SetCoin();
            SetCar();
            InitLevel = 0;
        }
    }
    GameObject TempCar;
    public void SetNewLevel()
    {        
        MapList[Level-1].SetActive(false);
        MapList[Level].SetActive(true);
    }
    public int GetActiveLevel()
    {
        for(int i=0; i< MapList.Count; i++)
        {
            if(MapList[i].activeSelf == true)
            {
                return i;
            }
        }
        return 0;
    }
    public void SetNewLevelRand(int prevlevel,int nowlevel)
    {
        MapList[prevlevel].SetActive(false);
        MapList[nowlevel].SetActive(true);
    }
    //
    void SetCar()
    {
        TempCar = Instantiate(CarList[CarType].gameObject);
        TempCar.GetComponent<NavMeshAgent>().enabled = false;
        //Temp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        TempCar.transform.position = StartPoint.position;
        TempCar.GetComponent<NavMeshAgent>().enabled = true;

    }

    public List<GameObject>CoinsList = new List<GameObject>();
    public void SetCoin()
    {
        for(int i=0; i< CoinPosTransform.Count; i++)
        {
            GameObject temp = Instantiate(Coin.gameObject);

            temp.transform.SetParent(CoinPosTransform[i]);
            temp.transform.localPosition = new Vector3(0, 0, 0);
            CoinsList.Add(temp);
        }
        
    }


    private void OnApplicationQuit()
    {
        SaveData();

    }
    private void OnApplicationPause(bool pause)
    {
        if (pause == true)
            SaveData();
    }
    void SaveData()
    {
        ES3.Save<int>("Stage", Level);
        ES3.Save<int>("Gold", totalGold);
        ES3.Save<List<int>>("IsBuyCar", isBuyCar);
        ES3.Save<int>("Cartype", CarType);
    }
    void LoadData()
    {
        if (ES3.KeyExists("Stage") == true)
        {            
            Level = ES3.Load<int>("Stage");
            
        }
        if (ES3.KeyExists("Gold") == true)
        {
            totalGold = ES3.Load<int>("Gold");
        }
        if (ES3.KeyExists("IsBuyCar") == true)
        {
            isBuyCar = ES3.Load<List<int>>("IsBuyCar");
        }
        if (ES3.KeyExists("Cartype") == true)
        {
            CarType = ES3.Load<int>("Cartype");
        }
    }
}
