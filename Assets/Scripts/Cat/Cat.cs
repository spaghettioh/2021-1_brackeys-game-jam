using System.Collections;
using UnityEngine;
using Common.Damageable;
using Common.FSM;

public class Cat : MachineBehaviour
{
    public float throwForce;
    public float moveSpeed;

    public Vector3Variable laserPointerHitWorldSpace;
    public bool followLeader;
    public bool followLaser;
    public string previouslyFollowing;
    public Vector3 followPosition;

    [HideInInspector]
    public Damageable damageable;

    [HideInInspector]
    public Material material;

    [HideInInspector]
    public Leader leader;
    [HideInInspector]
    public Rigidbody body;
    [HideInInspector]
    public Animator anim;

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
        material = GetComponentInChildren<SkinnedMeshRenderer>().material;
        anim = GetComponent<Animator>();

        leader = FindObjectOfType<Leader>();
    }

    public void ThrowCat(Vector3 startPosition, Vector3 direction)
    {
        // Put cat overhead to throw
        transform.position = startPosition;
        body.velocity = Vector3.zero;

        GetState<CatState_Thrown>().direction = direction;
        ChangeState<CatState_Thrown>();
    }

    public void KillMe()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Should be used only by a Damageable event
    /// </summary>
    public void TakeDamage()
    {
        ChangeState<CatState_Damaged>();
    }

    /// <summary>
    /// Should be used only by a Damageable event
    /// </summary>
    public void RemoveFromInventory()
    {
        leader.AddRemoveCatInventory(this);
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

    public override void OnTriggerEnter(Collider collider)
    {
        // Follow player
        if (collider.gameObject.GetComponent<Leader>())
        {
            followLeader = true;
            leader.AddRemoveCatInventory(this);
        }

        // Follow laser
        if (collider.gameObject.GetComponent<LaserPointer>())
        {
            followLaser = true;
        }
    }

    public override void OnTriggerExit(Collider collider)
    {
        // Follow player
        if (collider.gameObject.GetComponent<Leader>())
        {
            followLeader = false;
            leader.AddRemoveCatInventory(this);
        }

        // Stop following laser
        if (collider.gameObject.GetComponent<LaserPointer>())
        {
            Vector3 laserLastKnownPosition = laserPointerHitWorldSpace.Value;
            followPosition = laserLastKnownPosition;
            followLaser = false;
        }
    }

    public void SetRigidBodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = state;
        }

        GetComponent<Rigidbody>().isKinematic = !state;
    }

    public void SetColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (var c in colliders)
        {
            c.enabled = state;
        }

        GetComponent<Collider>().enabled = !state;
    }

}
