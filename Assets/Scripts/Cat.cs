using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Common.Damageable;

public enum CatType { Slow, Average, Fast }

public class Cat : MonoBehaviour
{
    Damageable damageable;

    Material material;
    Color originalColor;

    Leader leader;
    [HideInInspector]
    public bool followLeader;
    public bool followEnemy;
    GameObject followTarget;
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
        damageable = GetComponent<Damageable>();
        material = GetComponent<MeshRenderer>().material;
        originalColor = material.color;

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
        if (followTarget)
        {
            if (followTarget == leader.gameObject)
            {
                // Move towards leader but do not merge
                if (Vector3.Distance(transform.position, followTarget.transform.position) > 2 && !thrown)
                {
                    transform.position = Vector3.MoveTowards(transform.position, leader.transform.position, .1f);

                }

            }

            if (followTarget.GetComponent<Dog>())
            {
                transform.position = Vector3.MoveTowards(transform.position, followTarget.transform.position, .1f);
            }
        }

        //while (damageable.invincible)
        //{
        //    material.color = Color.red;
        //    material.color = originalColor;
        //}
    }

    public void ThrowCat(Vector3 target)
    {
        //followLeader = false;
        followTarget = null;

        thrown = true;
    }

    public void KillMe()
    {
        Destroy(this);
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
        if (other.gameObject.GetComponent<Leader>() != null)
        {
            //followLeader = true;
            followTarget = other.gameObject;

            leader.AddRemoveCatInventory(this);
        }

        if (other.gameObject.GetComponent<Dog>() != null && thrown)
        {
            //followLeader = false;
            //followEnemy = true;
            followTarget = other.gameObject;
        }
    }
    //
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Leader>() == leader)
        {
            //followLeader = false;
            //nearLeader = false;
            followTarget = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Leader>() == leader)
        {
            //nearLeader = true;
        }
    }
    public void DamageFlash()
    {
        StartCoroutine(DamageFlasher());
    }

    IEnumerator DamageFlasher()
    {
        material.color = Color.red;
        yield return new WaitForSeconds(damageable.invincibleSeconds);
        material.color = originalColor;
    }
}
