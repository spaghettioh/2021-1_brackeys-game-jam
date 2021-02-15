using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cat : MonoBehaviour
{
    public Transform leader;
    public bool followLeader;
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

        if (followLeader)
        {
            cat.SetDestination(leader.position);

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Leader leader = other.gameObject.GetComponent<Leader>();
        if (leader != null)
        {
            followLeader = true;
            leader.AddCatToInventory(this);
        }
    }
}
