using System;
using UnityEngine;

namespace Common.FSM
{
    public interface IMachineInterface
    {
        void SetInitialState<T>() where T : State;
        void SetInitialState(Type T);

        void ChangeState<T>() where T : State;
        void ChangeState(Type T);

        bool IsCurrentState<T>() where T : State;
        bool IsCurrentState(Type T);

        T CurrentState<T>() where T : State;
        T GetState<T>() where T : State;

        void AddState<T>() where T : State, new();
        void AddState(Type T);

        void RemoveState<T>() where T : State;
        void RemoveState(Type T);

        bool ContainsState<T>() where T : State;
        bool ContainsState(Type T);

        void OnCollisionEnter2D(Collision2D collision);
        void OnCollisionStay2D(Collision2D collision);
        void OnCollisionExit2D(Collision2D collision);

        void OnTriggerEnter2D(Collider2D collider);
        void OnTriggerStay2D(Collider2D collider);
        void OnTriggerExit2D(Collider2D collider);

        void OnCollisionEnter(Collision collision);
        void OnCollisionStay(Collision collision);
        void OnCollisionExit(Collision collision);

        void OnTriggerEnter(Collider collider);
        void OnTriggerStay(Collider collider);
        void OnTriggerExit(Collider collider);

        void RemoveAllStates();

        string name
        {
            get;
            set;
        }
    }
}