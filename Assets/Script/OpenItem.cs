using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
public class OpenItem : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Door;
    GameObject Effect;
    void Start()
    {
        Effect = transform.Find("Effect").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Red")
        {
            Effect.SetActive(true);
            Door.GetComponent<DG.Tweening.DOTweenAnimation>().DOPlay();
            StartCoroutine(EndRoutine());
            SoundManager.Instance.StartSound((int)SoundManager.SOUND_TYPE.Door);
        }
    }
    IEnumerator EndRoutine()
    {
        yield return new WaitForSeconds(0.2f);
        this.gameObject.SetActive(false);
    }
}
