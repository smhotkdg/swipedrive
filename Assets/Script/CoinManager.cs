using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject NormalEffect;
    GameObject GetEffect;
    void Start()
    {
        NormalEffect = transform.Find("NormalEffect").gameObject;
        GetEffect = transform.Find("GetEffect").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Red")
        {
            NormalEffect.SetActive(false);
            GetEffect.SetActive(true);
            StartCoroutine(DestoryRoutine());
            SoundManager.Instance.StartSound((int)SoundManager.SOUND_TYPE.Coin);
            GameManager.Instance.totalGold++;
        }
    }
    IEnumerator DestoryRoutine()
    {
        GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(1.2f);
        this.gameObject.SetActive(false);
    }
}
