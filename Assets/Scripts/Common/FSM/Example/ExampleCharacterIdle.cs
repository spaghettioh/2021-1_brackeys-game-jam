using UnityEngine;

public class ExampleCharacterIdle : Common.FSM.State
{
    public ExampleCharacter exampleCharacter { get { return (ExampleCharacter)Machine; } }
    public float timeInState { get; protected set; }

    public override void Enter()
    {
        base.Enter();

        timeInState = 0;
    }

    public override void Update()
    {
        base.Update();

        timeInState += Time.deltaTime;

        if (exampleCharacter.TimeToIdle <= timeInState)
        {
            Machine.ChangeState<ExampleCharacterWander>();
        }
    }
}
