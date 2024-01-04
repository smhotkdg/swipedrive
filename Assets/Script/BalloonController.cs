using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Boom;
    List<GameObject> Booms = new List<GameObject>();
    public float Boomtime;
    void Start()
    {
        for(int i=0; i< 50;i++)
        {
            GameObject temp = Instantiate(Boom.gameObject);
            temp.transform.SetParent(Boom.transform.parent);
            temp.transform.position = Boom.transform.position;
            temp.transform.localScale = Boom.transform.localScale;
            Booms.Add(temp);
        }
        StartCoroutine(BoomRoutine());
    }
    private void OnEnable()
    {
        //StartCoroutine(BoomRoutine());
    }
    int count = 0;
    IEnumerator BoomRoutine()
    {
        yield return new WaitForSeconds(Boomtime);
        if (count >= Booms.Count)
            count = 0;
        Booms[count].SetActive(true);
        Booms[count].transform.SetParent(this.transform.parent);
        count++;
        StartCoroutine(BoomRoutine());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
