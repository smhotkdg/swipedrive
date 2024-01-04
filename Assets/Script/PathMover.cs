using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathMover : MonoBehaviour
{
    private NavMeshAgent navmeshagent;
    private Queue<Vector3> pathPoints = new Queue<Vector3>();
    private Vector3 tmpContactPoint;
    private Vector3 tmpDirection;
    private GameObject tmpGamob;
    private Transform myTransf;
    public GameObject Effect;
    private void Awake()
    {
        navmeshagent = GetComponent<NavMeshAgent>();
        FindObjectOfType<PathCreator>().OnNewPathCreated += SetPoints;
        myTransf = transform;
    }
    void Start()
    {
        GameManager.Instance.isStop = false;

    }



    private void SetPoints(IEnumerable<Vector3> Points)
    {
      
        pathPoints = new Queue<Vector3>(Points);
               
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isStop == false)
        {
            UpdatePathing();
        }        
        if(GameManager.Instance.isStop ==true && pathPoints.Count >0)
        {
            pathPoints.Clear();
            if (navmeshagent.enabled == true) 
                navmeshagent.isStopped = true;
        }
    }

    public void SetPoints(List<Vector3> Points)
    {
        pathPoints = new Queue<Vector3>(Points);

    }
    bool endGame = false;
    private void UpdatePathing()
    {
        if(ShouldSetDestination())
        {
            navmeshagent.SetDestination(pathPoints.Dequeue());
        }
        if(navmeshagent.hasPath==false && GameManager.Instance.isEnableGame == false && isSuccess==false&& pathPoints.Count ==0)
        {
            UIManager.Instance.FailView();
            GameManager.Instance.isStop = true;
            if (navmeshagent.enabled == true)
                navmeshagent.isStopped = true;
            isSuccess = true;
        }
    }

    private bool ShouldSetDestination()
    {
        if(pathPoints.Count ==0)
        {     
            return false;
        }
       
        if(navmeshagent.hasPath == false || navmeshagent.remainingDistance < 2f)
        {
            return true;
        }
        return false;
    }
    bool isSuccess = false;
    private void OnTriggerEnter(Collider other)
    {
        if(this.tag == other.tag)
        {
            if (GameManager.Instance.isStop == false && isSuccess==false)
            {
                isSuccess = true;
                GameManager.Instance.SetGameSuccess();
                GameManager.Instance.isEnableGame = false;
                GameManager.Instance.isStop = true;
            }       
        }
        if (other.tag == "obstacle" && isSuccess == false)
        {
            UIManager.Instance.FailView();
            GameManager.Instance.isStop = true;
            navmeshagent.isStopped = true;
            isSuccess = true;
            
            tmpDirection = (other.transform.position - myTransf.position);
            tmpContactPoint = myTransf.position + tmpDirection;
            SetCollisionEffect(tmpContactPoint);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "obstacle" && isSuccess == false)
        {
            UIManager.Instance.FailView();
            GameManager.Instance.isStop = true;
            navmeshagent.isStopped = true;
            isSuccess = true;
            if(collision.contacts.Length >0)
            {
                SetCollisionEffect(collision.contacts[0].point);
            }
            
        }
    }
    void SetCollisionEffect(Vector3 pos)
    {
        Effect.transform.position = pos;
        Effect.SetActive(true);
    }
}
