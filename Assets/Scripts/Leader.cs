using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Leader : MonoBehaviour
{
    NavMeshAgent leader;
    public LayerMask walkableLayer;
    //List<Vector3> hits = new List<Vector3>();
    public RectTransform walkTarget;
    public Canvas canvas;
    [SerializeField]
    List<Cat> catInventory = new List<Cat>();



    private void Start()
    {
        leader = GetComponent<NavMeshAgent>();

        walkTarget.gameObject.SetActive(false);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, walkableLayer))
            {
                walkTarget.position = ray.origin;
                walkTarget.gameObject.SetActive(true);
                leader.SetDestination(hit.point);
                //hits.Add(hit.point);
            }
        }

        //Debug.Log("Ray origin: " + ray.origin);
        //Debug.Log("Leader WorldToScreenPoint: " + Camera.main.WorldToScreenPoint(transform.position));
        //Debug.Log("ScreenPointToLocalPointInRectangle: " + worldToUISpace(canvas, transform.position));
        //transform.position = worldToUISpace(canvas, transform.position);
        if (transform.position == ray.origin)
        {
            walkTarget.gameObject.SetActive(false);
        }
    }

    public void AddCatToInventory(Cat cat)
    {
        catInventory.Add(cat);
    }


    public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    foreach (Vector3 hit in hits)
    //    {
    //        Gizmos.DrawSphere(hit, .1f);
    //    }
    //}
}
