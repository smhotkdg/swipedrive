using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCar : MonoBehaviour
{
    public List<GameObject> BoxList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartBox()
    {
        if(this.gameObject.transform.parent.gameObject.activeSelf == true)
        {
            if (BoxList.Count > 0)
            {
                StartCoroutine(BoxRoutine());
            }
        }
         
    }
    IEnumerator BoxRoutine()
    {
        for(int i=0; i< BoxList.Count;i++)
        {
            yield return new WaitForSeconds(0.5f);
            BoxList[i].GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 5));
            BoxList[i].transform.SetParent(transform.parent);
        }
    }
}
