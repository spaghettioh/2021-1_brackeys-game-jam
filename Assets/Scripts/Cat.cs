using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cat : MonoBehaviour
{
    Leader leader;
    public bool followLeader;
    NavMeshAgent agent;
    [HideInInspector]
    public Rigidbody body;
    public float throwSpeed;
    public float moveSpeed;
    public bool thrown;
    public bool isFollowing;
    public bool nearLeader;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        body = GetComponent<Rigidbody>();

        leader = FindObjectOfType<Leader>();
    }

    private void Update()
    {
        if (leader.catInventory.Contains(this))
        {
            isFollowing = true;

        }
        else
        {
            isFollowing = false;
        }

        if (followLeader)
        {
            //body.isKinematic = true;
            if (Vector3.Distance(transform.position, leader.transform.position) > 2 && !thrown)
            {
                transform.position = Vector3.MoveTowards(transform.position, leader.transform.position, .1f);

            }
        }
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

    public void ThrowCat(Vector3 target)
    {
        followLeader = false;


        //body.isKinematic = false;
        body.AddForce(target * throwSpeed, ForceMode.Impulse);
        thrown = true;
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
