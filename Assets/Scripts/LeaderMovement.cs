using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeaderMovement : MonoBehaviour
{
    NavMeshAgent leader;
    public LayerMask walkableLayer;
    List<Vector3> hits = new List<Vector3>();

    private void Start()
    {
        leader = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, walkableLayer))
            {
                leader.SetDestination(hit.point);
                hits.Add(hit.point);
            }
        }    
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 hit in hits)
        {
            Gizmos.DrawSphere(hit, .1f);
        }
    }
}
