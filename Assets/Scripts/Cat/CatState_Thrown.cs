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

        // Throw the cat and release rotation lock
        Cat.body.freezeRotation = false;
        Cat.followLeader = false;
        // TODO make this work
        Cat.body.AddTorque(Vector3.one * Random.Range(0, 1) * 100, ForceMode.VelocityChange);
        Cat.body.AddForce(direction * Cat.throwForce, ForceMode.Impulse);
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
}
