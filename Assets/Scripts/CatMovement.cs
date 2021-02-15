using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatMovement : MonoBehaviour
{
    public Transform leader;
    public bool catSeesLeader;
    NavMeshAgent cat;

    private void Start()
    {
        cat = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Vector3.Distance(leader.position, transform.position) < 2)
        {
            cat.ResetPath();
            return;
        }

        if (catSeesLeader)
        {
            cat.SetDestination(leader.position);

        }

    }

    private void OnTriggerEnter(Collider other)
    {
            Debug.Log(other);
        if (other.gameObject.GetComponent<LeaderMovement>() != null)
        {
            catSeesLeader = true;
        }
    }
}
