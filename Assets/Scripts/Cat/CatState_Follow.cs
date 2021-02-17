using UnityEngine;
using Common.FSM;

public class CatState_FollowLeader : State
{
    public Cat Cat { get { return (Cat)Machine; } }

    public override void Enter()
    {
        base.Enter();
        Cat.material.color = Color.blue;
        Cat.leader.AddRemoveCatInventory(Cat);
    }

    public override void Update()
    {
        base.Update();
        float distance = Vector3.Distance(Cat.transform.position, Cat.leader.transform.position);

        // Move towards leader but do not merge
        if (distance > 2)
        {
            Cat.transform.position = Vector3.MoveTowards(Cat.transform.position, Cat.leader.transform.position, Time.deltaTime * Cat.moveSpeed);
        }
    }
}
