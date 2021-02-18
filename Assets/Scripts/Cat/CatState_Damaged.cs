using UnityEngine;
using Common.FSM;

public class CatState_Damaged : State
{
    public Cat Cat { get { return (Cat)Machine; } }
    Color originalColor;

    public override void Enter()
    {
        base.Enter();

        originalColor = Cat.material.color;
        Cat.body.freezeRotation = false;
        Cat.body.AddTorque(Vector3.one * Random.Range(0, 1) * 100, ForceMode.VelocityChange);
        Cat.StartCoroutine(Cat.ChangeInvincibilityFrameColor());
    }

    public override void Update()
    {
        base.Update();
        if (!Cat.damageable.invincible)
        {
            Cat.material.color = originalColor;
            Cat.ChangeState<CatState_Follow>();
        }
    }
}
