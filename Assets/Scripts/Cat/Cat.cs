using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Common.Damageable;
using Common.FSM;

public class Cat : MachineBehaviour
{
    public float throwForce;
    public float moveSpeed;

    [HideInInspector]
    public Damageable damageable;

    [HideInInspector]
    public Material material;
    Color originalColor;

    [HideInInspector]
    public Leader leader;
    [HideInInspector]
    public Rigidbody body;

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
        body = GetComponent<Rigidbody>();
        damageable = GetComponent<Damageable>();
        material = GetComponent<MeshRenderer>().material;
        originalColor = material.color;

        leader = FindObjectOfType<Leader>();
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

    /// <summary>
    /// Should be used only by the Damageable OnDamage event
    /// </summary>
    public void Damaged()
    {
        ChangeState<CatState_Damaged>();
    }
}
