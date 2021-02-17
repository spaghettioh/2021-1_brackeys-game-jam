﻿using UnityEngine;

public class ExampleCharacterWander : Common.FSM.State
{
    public ExampleCharacter exampleCharacter { get { return (ExampleCharacter)Machine; } }
    public float timeInState { get; protected set; }

    public override void Enter()
    {
        base.Enter();

        timeInState = 0;

        exampleCharacter.transform.rotation = Quaternion.Euler(0, 360 * Random.value, 0);
    }

    public override void Update()
    {
        base.Update();

        timeInState += Time.deltaTime;

        if (exampleCharacter.TimeToWander <= timeInState)
        {
            Machine.ChangeState<ExampleCharacterIdle>();
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (0 < collision.contacts.Length)
        {
            Vector3 N = collision.contacts[0].normal;
            
            if (!(1 <= Vector3.Dot(N, Vector3.up)))
            {
                N.Normalize();
                Vector3 P = collision.contacts[0].point;
                P.y = exampleCharacter.transform.position.y;
                Vector3 D = exampleCharacter.transform.forward;
                D.Normalize();
                float dot = Vector3.Dot(D, N);
                Vector3 R = D - 2 * dot * N;

                exampleCharacter.transform.rotation = Quaternion.LookRotation(R, Vector3.up);
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        exampleCharacter.rigidBody.MovePosition(exampleCharacter.rigidBody.position + exampleCharacter.transform.forward * exampleCharacter.Speed * Time.fixedDeltaTime);
    }
}
