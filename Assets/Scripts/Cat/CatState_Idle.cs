using UnityEngine;
using Common.FSM;

public class CatState_Idle : State
{
    public Cat Cat { get { return (Cat)Machine; } }

    public override void Enter()
    {
        base.Enter();

        Cat.material.color = Color.grey;

        // Reset the cat
        Cat.transform.rotation = Quaternion.Euler(0, 0, 0);
        Cat.body.velocity = Vector3.zero;
        Cat.body.freezeRotation = true;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void OnTriggerEnter(Collider other)
    {
        // Follow player
        if (other.gameObject.GetComponent<Leader>())
        {
            Cat.following = "leader";
            Cat.ChangeState<CatState_Follow>();
        }

        // Follow laser
        if (other.gameObject.GetComponent<LaserPointer>())
        {
            Cat.following = "laser";
            Cat.ChangeState<CatState_Follow>();
        }

    }
}
