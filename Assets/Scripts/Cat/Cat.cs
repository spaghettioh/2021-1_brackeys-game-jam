using System.Collections;
using UnityEngine;
using Common.Damageable;
using Common.FSM;

public class Cat : MachineBehaviour
{
    public float throwForce;
    public float moveSpeed;

    public Vector3Variable laserPointerHitWorldSpace;
    public string following;
    public Vector3 followPosition;

    [HideInInspector]
    public Damageable damageable;

    [HideInInspector]
    public Material material;

    [HideInInspector]
    public Leader leader;
    [HideInInspector]
    public Rigidbody body;

    public float thrownWaitTime = 3;

    public override void AddStates()
    {
        AddState<CatState_Idle>();
        AddState<CatState_Follow>();
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

        leader = FindObjectOfType<Leader>();
    }

    public override void Update()
    {
        base.Update();
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
    public void TakeDamage()
    {
        ChangeState<CatState_Damaged>();
    }

    public IEnumerator ChangeInvincibilityFrameColor()
    {
        while (damageable.invincible)
        {
            material.color = Color.red;
            yield return new WaitForSeconds(.15f);
            material.color = Color.yellow;
            yield return new WaitForSeconds(.15f);
        }
    }
}
