using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cctv : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Effect1;
    public GameObject Effect2;
    public GameObject Area;
    public float Time;
    public float rewaindTime;
    public float Startime = 0;
    void Start()
    {
        //if(isSTartNow == true)
        {
            StartCoroutine(EffectEnd());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator EffectEnd()
    {
        yield return new WaitForSeconds(Startime);
        Effect1.SetActive(true);
        Effect2.SetActive(true);
        Area.SetActive(true);
        yield return new WaitForSeconds(Time);
        Effect1.SetActive(false);
        Effect2.SetActive(false);
        Area.SetActive(false);
        yield return new WaitForSeconds(rewaindTime);
        StartCoroutine(EffectEnd());
    }
}
