using UnityEngine;
using Common.FSM;

public class CatState_Follow : State
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

        if (Cat.following == "leader")
        {
            float distance = Vector3.Distance(Cat.transform.position, Cat.leader.transform.position);

            // Move towards leader but do not merge
            if (distance > 2)
            {
                Chase(Cat.leader.transform.position, 0);
            }
        }

        if (Cat.following == "laser")
        {
            Chase(Cat.laserPointerHitWorldSpace.Value, 3);
        }
    }

    void Chase(Vector3 position, float addMoveSpeed)
    {
        Cat.transform.position = Vector3.MoveTowards(Cat.transform.position, position, Time.deltaTime * (Cat.moveSpeed + addMoveSpeed));
    }

    public override void OnTriggerExit(Collider collider)
    {
        // Follow laser
        if (collider.gameObject.GetComponent<LaserPointer>())
        {
            Cat.following = "";
            Cat.ChangeState<CatState_Idle>();
        }
    }
}
