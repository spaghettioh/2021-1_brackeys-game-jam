using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.FSM
{
    [Serializable]
    public abstract class MachineState : State, IMachineInterface
    {
        /// <summary>
        /// REQUIRES IMPL
        /// </summary>
        public abstract void AddStates();

        public override void Initialize()
        {
            base.Initialize();

            name = Machine.name + "." + GetType().ToString();

            AddStates();

            currentState = InitialState;
            if (null == currentState)
            {
                throw new Exception("\n" + name + ".nextState is null on Initialize()!\tDid you forget to call SetInitialState()?\n");
            }

            foreach (KeyValuePair<Type, State> pair in states)
            {
                pair.Value.Initialize();
            }

            OnEnter = true;
            OnExit = false;
        }

        public override void Update()
        {
            base.Update();

            if (OnExit)
            {
                currentState.Exit();
                currentState = NextState;
                NextState = null;

                OnEnter = true;
                OnExit = false;
            }

            if (OnEnter)
            {
                currentState.Enter();

                OnEnter = false;
            }

            try
            {
                currentState.Update();
            }
            catch (NullReferenceException e)
            {
                if (null == InitialState)
                {
                    throw new Exception("\n" + name + ".currentState is null when calling Execute()!\tDid you set initial state?\n" + e.Message);
                }
                else
                {
                    throw new Exception("\n" + name + ".currentState is null when calling Execute()!\tDid you change state to a valid state?\n" + e.Message);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!(OnEnter && OnExit))
            {
                try
                {
                    currentState.FixedUpdate();
                }
                catch (NullReferenceException e)
                {
                    if (null == InitialState)
                    {
                        throw new Exception("\n" + name + ".currentState is null when calling Execute()!\tDid you set initial state?\n" + e.Message);
                    }
                    else
                    {
                        throw new Exception("\n" + name + ".currentState is null when calling Execute()!\tDid you change state to a valid state?\n" + e.Message);
                    }
                }
            }
        }

        public override void LateUpdate()
        {
            base.LateUpdate();

            if (!(OnEnter && OnExit))
            {
                try
                {
                    currentState.LateUpdate();
                }
                catch (NullReferenceException e)
                {
                    if (null == InitialState)
                    {
                        throw new Exception("\n" + name + ".currentState is null when calling Execute()!\tDid you set initial state?\n" + e.Message);
                    }
                    else
                    {
                        throw new Exception("\n" + name + ".currentState is null when calling Execute()!\tDid you change state to a valid state?\n" + e.Message);
                    }
                }
            }
        }

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            currentState.OnCollisionEnter2D(collision);
        }

        public override void OnCollisionStay2D(Collision2D collision)
        {
            currentState.OnCollisionStay2D(collision);
        }

        public override void OnCollisionExit2D(Collision2D collision)
        {
            currentState.OnCollisionExit2D(collision);
        }

        public override void OnTriggerEnter2D(Collider2D collider)
        {
            currentState.OnTriggerEnter2D(collider);
        }

        public override void OnTriggerStay2D(Collider2D collider)
        {
            currentState.OnTriggerStay2D(collider);
        }

        public override void OnTriggerExit2D(Collider2D collider)
        {
            currentState.OnTriggerExit2D(collider);
        }

        public override void OnCollisionEnter(Collision collision)
        {
            currentState.OnCollisionEnter(collision);
        }

        public override void OnCollisionStay(Collision collision)
        {
            currentState.OnCollisionStay(collision);
        }

        public override void OnCollisionExit(Collision collision)
        {
            currentState.OnCollisionExit(collision);
        }

        public override void OnTriggerEnter(Collider collider)
        {
            currentState.OnTriggerEnter(collider);
        }

        public override void OnTriggerStay(Collider collider)
        {
            currentState.OnTriggerStay(collider);
        }

        public override void OnTriggerExit(Collider collider)
        {
            currentState.OnTriggerExit(collider);
        }

        public override void OnAnimatorIK(int layerIndex)
        {
            base.OnAnimatorIK(layerIndex);

            if (!(OnEnter && OnExit))
            {
                try
                {
                    currentState.OnAnimatorIK(layerIndex);
                }
                catch (NullReferenceException e)
                {
                    if (null == InitialState)
                    {
                        throw new Exception("\n" + name + ".currentState is null when calling Execute()!\tDid you set initial state?\n" + e.Message);
                    }
                    else
                    {
                        throw new Exception("\n" + name + ".currentState is null when calling Execute()!\tDid you change state to a valid state?\n" + e.Message);
                    }
                }
            }
        }

        public void SetInitialState<T>() where T : State
        {
            InitialState = states[typeof(T)];
        }

        public void SetInitialState(Type T)
        {
            InitialState = states[T];
        }

        public void ChangeState<T>() where T : State
        {
            ChangeState(typeof(T));
        }

        public void ChangeState(Type T)
        {
            if (null != NextState)
            {
                throw new Exception(name + " is already changing states, you must wait to call ChangeState()!\n");
            }

            try
            {
                NextState = states[T];
            }
            catch (KeyNotFoundException e)
            {
                throw new Exception("\n" + name + ".ChangeState() cannot find the state in the machine!\tDid you add the state you are trying to change to?\n" + e.Message);
            }

            OnExit = true;
        }

        public bool IsCurrentState<T>() where T : State
        {
            if (currentState.GetType() == typeof(T))
            {
                return true;
            }

            return false;
        }

        public bool IsCurrentState(Type T)
        {
            if (currentState.GetType() == T)
            {
                return true;
            }

            return false;
        }

        public void AddState<T>() where T : State, new()
        {
            if (!ContainsState<T>())
            {
                State item = new T();
                item.Machine = this;

                states.Add(typeof(T), item);
            }
        }

        public void AddState(Type T)
        {
            if (!ContainsState(T))
            {
                State item = (State)Activator.CreateInstance(T);
                item.Machine = this;

                states.Add(T, item);
            }
        }

        public void RemoveState<T>() where T : State
        {
            states.Remove(typeof(T));
        }

        public void RemoveState(Type T)
        {
            states.Remove(T);
        }

        public bool ContainsState<T>() where T : State
        {
            return states.ContainsKey(typeof(T));
        }

        public bool ContainsState(Type T)
        {
            return states.ContainsKey(T);
        }

        public void RemoveAllStates()
        {
            states.Clear();
        }

        public T CurrentState<T>() where T : State
        {
            return (T)currentState;
        }

        public T GetState<T>() where T : State
        {
            return (T)states[typeof(T)];
        }

        public string name
        {
            get;
            set;
        }

        protected State currentState
        {
            get;
            set;
        }

        protected State NextState
        {
            get;
            set;
        }

        protected State InitialState
        {
            get;
            set;
        }

        protected bool OnEnter
        {
            get;
            set;
        }

        protected bool OnExit
        {
            get;
            set;
        }

        protected Dictionary<Type, State> states = new Dictionary<Type, State>();
    }
}