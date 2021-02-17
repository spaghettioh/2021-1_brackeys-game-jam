// Comment to silence
//#define VERBOSE
//#define VERY_VERBOSE

using System;
using System.Reflection;
using UnityEngine;

namespace Common.FSM
{
    [Serializable]
    public abstract class State : IStateInterface
    {
        public virtual void Update()
        {
#if (VERY_VERBOSE)
            Log(MethodBase.GetCurrentMethod().Name);
#endif // VERY_VERBOSE
        }

        public virtual void FixedUpdate()
        {
#if (VERY_VERBOSE)
            Log(MethodBase.GetCurrentMethod().Name);
#endif // VERY_VERBOSE
        }

        public virtual void LateUpdate()
        {
#if (VERY_VERBOSE)
            Log(MethodBase.GetCurrentMethod().Name);
#endif // VERY_VERBOSE
        }

        public virtual void OnCollisionEnter2D(Collision2D collision)
        {
#if (VERBOSE)
            Log(MethodBase.GetCurrentMethod().Name);
#endif // VERBOSE
        }
        public virtual void OnCollisionStay2D(Collision2D collision)
        {
#if (VERY_VERBOSE)
            Log(MethodBase.GetCurrentMethod().Name);
#endif // VERY_VERBOSE
        }

        public virtual void OnCollisionExit2D(Collision2D collision)
        {
#if (VERBOSE)
            Log(MethodBase.GetCurrentMethod().Name);
#endif // VERBOSE
        }

        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
#if (VERBOSE)
            Log(MethodBase.GetCurrentMethod().Name);
#endif // VERBOSE
        }

        public virtual void OnTriggerStay2D(Collider2D collider)
        {
#if (VERY_VERBOSE)
            Log(MethodBase.GetCurrentMethod().Name);
#endif // VERY_VERBOSE
        }

        public virtual void OnTriggerExit2D(Collider2D collider)
        {
#if (VERBOSE)
            Log(MethodBase.GetCurrentMethod().Name);
#endif // VERBOSE
        }

        public virtual void OnCollisionEnter(Collision collision)
        {
#if (VERBOSE)
            Log(MethodBase.GetCurrentMethod().Name);
#endif // VERBOSE
        }

        public virtual void OnCollisionStay(Collision collision)
        {
#if (VERY_VERBOSE)
            Log(MethodBase.GetCurrentMethod().Name);
#endif // VERY_VERBOSE
        }

        public virtual void OnCollisionExit(Collision collision)
        {
#if (VERBOSE)
            Log(MethodBase.GetCurrentMethod().Name);
#endif // VERBOSE
        }

        public virtual void OnTriggerEnter(Collider collider)
        {
#if (VERBOSE)
            Log(MethodBase.GetCurrentMethod().Name);
#endif // VERBOSE
        }

        public virtual void OnTriggerStay(Collider collider)
        {
#if (VERY_VERBOSE)
            Log(MethodBase.GetCurrentMethod().Name);
#endif // VERY_VERBOSE
        }

        public virtual void OnTriggerExit(Collider collider)
        {
#if (VERBOSE)
            Log(MethodBase.GetCurrentMethod().Name);
#endif // VERBOSE
        }

        public virtual void OnAnimatorIK(int layerIndex)
        {
#if (VERBOSE)
            Log(MethodBase.GetCurrentMethod().Name);
#endif // VERBOSE
        }

        public virtual void Initialize()
        {
#if (VERBOSE)
            Log(MethodBase.GetCurrentMethod().Name);
#endif // VERBOSE
        }

        public virtual void Enter()
        {
#if (VERBOSE)
            Log(MethodBase.GetCurrentMethod().Name);
#endif // VERBOSE
        }

        public virtual void Exit()
        {
#if (VERBOSE)
            Log(MethodBase.GetCurrentMethod().Name);
#endif // VERBOSE
        }

        public T GetMachine<T>() where T : IMachineInterface
        {
            try
            {
                return (T)Machine;
            }
            catch (InvalidCastException e)
            {
                if (typeof(T) == typeof(MachineState) || typeof(T).IsSubclassOf(typeof(MachineState)))
                {
                    throw new Exception(Machine.name + ".GetMachine() cannot return the type you requested!\tYour machine is derived from MachineBehaviour not MachineState!" + e.Message);
                }
                else if (typeof(T) == typeof(MachineBehaviour) || typeof(T).IsSubclassOf(typeof(MachineBehaviour)))
                {
                    throw new Exception(Machine.name + ".GetMachine() cannot return the type you requested!\tYour machine is derived from MachineState not MachineBehaviour!" + e.Message);
                }
                else
                {
                    throw new Exception(Machine.name + ".GetMachine() cannot return the type you requested!\n" + e.Message);
                }
            }
        }

        public IMachineInterface Machine
        {
            get;
            internal set;
        }

        public bool IsActive
        {
            get
            {
                return Machine.IsCurrentState(GetType());
            }
        }

        void Log(string methodName)
        {
            Debug.Log(Machine.name + "." + GetType().Name + "::" + methodName + "()");
        }
    }
}