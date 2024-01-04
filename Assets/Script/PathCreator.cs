using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class PathCreator : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<Vector3> Points = new List<Vector3>();
    public Action<IEnumerable<Vector3>> OnNewPathCreated = delegate { };
    //public PathMover myPath;
    //public int StepNumber;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void OnEnable()
    {
        lineRenderer.positionCount = 0;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
        if(GameManager.Instance.isEnableGame == true)
        {
            //  delete line        
            if (Input.GetButtonDown("Fire1"))
            {
                UIManager.Instance.disableTutorial();
                Points.Clear();
            }
            if (Input.GetButton("Fire1"))
            {               
                int layerMask = 1 << LayerMask.NameToLayer("PathCreator");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
                {
                    if (DistanceToLastPoint(hitInfo.point) > 0.1f)
                    {
                        Vector3 myvec = hitInfo.point;
                        myvec.y = 0.1f;
                        hitInfo.point = myvec;
                        Points.Add(hitInfo.point);

                        lineRenderer.positionCount = Points.Count;
                        lineRenderer.SetPositions(Points.ToArray());
                    }
                }
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                OnNewPathCreated(Points);
                SoundManager.Instance.StartSound((int)SoundManager.SOUND_TYPE.CarStart);
                GameManager.Instance.isEnableGame = false;    

            }

        }

    
      
    }
    private float DistanceToLastPoint(Vector3 point)
    {
        if(!Points.Any())
        {
            return Mathf.Infinity;
        }
        return Vector3.Distance(Points.Last(), point);
    }

}
