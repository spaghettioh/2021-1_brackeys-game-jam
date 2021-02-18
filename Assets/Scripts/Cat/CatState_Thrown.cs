using UnityEngine;
using Common.FSM;

public class CatState_Thrown : State
{
    public Cat Cat { get { return (Cat)Machine; } }
    float timesCollided;
    float timeInState;
    public Vector3 direction;

    public override void Enter()
    {
        base.Enter();
        Cat.material.color = Color.yellow;

        // Reset counters
        timesCollided = 0;
        timeInState = 0;

        // Ragdoll it
        Cat.anim.enabled = false;
        Cat.SetRigidBodyState(false);
        Cat.SetColliderState(true);

        // Throw the cat
        // TODO make this work
        //Cat.body.AddTorque(Vector3.one * Random.Range(0, 1) * 100, ForceMode.VelocityChange);
        //Cat.body.AddForce(direction * Cat.throwForce, ForceMode.Impulse);
        SetRigidBodyState();
        Cat.followLeader = false;


    }

    public override void Update()
    {
        base.Update();
        timeInState += Time.deltaTime;

        // Wait for the cat to "settle" or wait for seconds
        if (timesCollided >= 5 || timeInState > Cat.thrownWaitTime)
        {
            Cat.ChangeState<CatState_Idle>();
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        timesCollided += 1;
    }

    public void SetRigidBodyState()
    {
        Rigidbody[] rigidbodies = Cat.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rigidbodies)
        {
            rb.AddTorque(Vector3.one * Random.Range(0, 1) * 100, ForceMode.VelocityChange);
            rb.AddForce(direction * Cat.throwForce, ForceMode.Impulse);
        }
    }


}
