using UnityEngine;
using Common.FSM;

public class CatState_Follow : State
{
    public Cat Cat { get { return (Cat)Machine; } }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        Vector3 lookDirection = Cat.laserPointerHitWorldSpace.Value - Cat.transform.position;
        float rotation = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;
        Cat.transform.rotation = Quaternion.Euler(0, rotation, 0);

        if (Cat.body.velocity.magnitude > 0)
        {
            Cat.anim.Play("Run");
        }
        else
        {
            Cat.anim.Play("Idle");
        }

        // Laser takes priority
        if (Cat.followLaser)
        {
            Cat.material.color = Color.cyan;
            Chase(Cat.laserPointerHitWorldSpace.Value, 3);
        }
        else if (Cat.followLeader)
        {
            Cat.material.color = Color.blue;
            float distance = Vector3.Distance(Cat.transform.position, Cat.leader.transform.position);

            // Move towards leader but do not merge
            if (distance > 2)
            {
                Chase(Cat.leader.transform.position, 0);
            }
        }
        else
        {
            Cat.ChangeState<CatState_Idle>();
        }

    }

    void Chase(Vector3 position, float addMoveSpeed)
    {
        Cat.transform.position = Vector3.MoveTowards(Cat.transform.position, position, Time.deltaTime * (Cat.moveSpeed + addMoveSpeed));
    }

    public override void Exit()
    {
        base.Exit();
        Cat.body.freezeRotation = false;
    }
}
