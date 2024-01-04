using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject Effect;
    void Start()
    {
        Effect = transform.Find("Effect").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag =="road" || collision.gameObject.tag == "Red")
        {
            Effect.transform.position = transform.position;
            Effect.SetActive(true);
            StartCoroutine(EndObject());
        }
        
    }
    IEnumerator EndObject()
    {
        yield return new WaitForSeconds(.5f);
        this.gameObject.SetActive(false);
    }
}
