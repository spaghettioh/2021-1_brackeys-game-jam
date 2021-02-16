using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CatType { Slow, Average, Fast }

public class Cat : MonoBehaviour
{
    Leader leader;
    [HideInInspector]
    public bool followLeader;
    NavMeshAgent agent;
    [HideInInspector]
    public Rigidbody body;
    [HideInInspector]
    public float throwSpeed;
    [HideInInspector]
    public float moveSpeed;
    [HideInInspector]
    public bool thrown;
    [HideInInspector]
    public bool isFollowing;
    [HideInInspector]
    public bool nearLeader;
    public CatType catType = new CatType();

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        body = GetComponent<Rigidbody>();

        leader = FindObjectOfType<Leader>();

        if (catType == CatType.Slow)
        {
            moveSpeed = 3;
            throwSpeed = 10;
        }
        if (catType == CatType.Average)
        {
            moveSpeed = 6;
            throwSpeed = 20;
        }
        if (catType == CatType.Fast)
        {
            moveSpeed = 9;
            throwSpeed = 30;
        }
    }

    private void Update()
    {
        if (followLeader)
        {
            //body.isKinematic = true;
            if (Vector3.Distance(transform.position, leader.transform.position) > 2 && !thrown)
            {
                transform.position = Vector3.MoveTowards(transform.position, leader.transform.position, .1f);

            }
        }
    }

    public void ThrowCat(Vector3 target)
    {
        followLeader = false;


        thrown = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        thrown = false;

        //if (nearLeader)
        //{
        //    followLeader = true;
        //    leader.AddRemoveCatInventory(this);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Leader>() == leader)
        {
            followLeader = true;
            leader.AddRemoveCatInventory(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Leader>() == leader)
        {
            followLeader = false;
            //nearLeader = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Leader>() == leader)
        {
            //nearLeader = true;
        }
    }
}
