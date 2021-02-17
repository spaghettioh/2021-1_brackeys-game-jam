using UnityEngine;

namespace Common.FSM
{
    public interface IStateInterface
    {
        void Initialize();

        void Enter();

        void Update();
        void FixedUpdate();
        void LateUpdate();

        void Exit();

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

        void OnAnimatorIK(int layerIndex);

        bool IsActive
        {
            get;
        }

        IMachineInterface Machine
        {
            get;
        }

        T GetMachine<T>() where T : IMachineInterface;
    }
}