using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Common.Damageable;
using Common.FSM;

public class Cat : MachineBehaviour
{
    [HideInInspector]
    public Damageable damageable;

    [HideInInspector]
    public Material material;
    Color originalColor;

    [HideInInspector]
    public Leader leader;
    [HideInInspector]
    public bool followLeader;
    public bool followEnemy;
    GameObject followTarget;
    NavMeshAgent agent;
    [HideInInspector]
    public Rigidbody body;
    [HideInInspector]
    public float throwSpeed;
    //[HideInInspector]
    public float moveSpeed;
    [HideInInspector]
    public bool thrown;
    [HideInInspector]
    public bool isFollowing;
    [HideInInspector]
    public bool nearLeader;

    public float thrownWaitTime = 3;

    public override void AddStates()
    {
        AddState<CatState_Idle>();
        AddState<CatState_FollowLeader>();
        AddState<CatState_Thrown>();
        AddState<CatState_Damaged>();

        SetInitialState<CatState_Idle>();
    }

    public override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        body = GetComponent<Rigidbody>();
        damageable = GetComponent<Damageable>();
        material = GetComponent<MeshRenderer>().material;
        originalColor = material.color;

        leader = FindObjectOfType<Leader>();

        moveSpeed = 5;
        throwSpeed = 20;
    }

    public void ThrowCat(Vector3 startPosition, Vector3 direction)
    {
        // Put cat overhead to throw
        transform.position = startPosition;
        body.velocity = Vector3.zero;
        GetState<CatState_Thrown>().direction = direction;
        ChangeState<CatState_Thrown>();

        // Remove cat from leader inventory
        leader.AddRemoveCatInventory(this);
    }

    public void KillMe()
    {
        Destroy(this);
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
