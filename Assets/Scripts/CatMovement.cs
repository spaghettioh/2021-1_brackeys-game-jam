using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatMovement : MonoBehaviour
{
    public Transform leader;
    NavMeshAgent cat;

    private void Start()
    {
        cat = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Debug.Log(Vector3.Distance(leader.position, transform.position));
        cat.SetDestination(leader.position);
        if (Vector3.Distance(leader.position, transform.position) < 2)
        {
            cat.ResetPath();
        }
    }
}
