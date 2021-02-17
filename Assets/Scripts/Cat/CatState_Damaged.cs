using UnityEngine;
using Common.FSM;

public class CatState_Damaged : State
{
    public Cat Cat { get { return (Cat)Machine; } }
    Color startingColor;

    public override void Enter()
    {
        base.Enter();
        startingColor = Cat.material.color;
    }

    public override void Update()
    {
        base.Update();
        if (Cat.damageable.invincible)
        {
            Cat.material.color = Color.red;
            Cat.material.color = startingColor;
        }
        else
        {
            Cat.ChangeState<CatState_Idle>();
        }
    }
}
