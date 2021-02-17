using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.FSM
{
    public abstract class MachineBehaviour : MonoBehaviour, IMachineInterface
    {
        /// <summary>
        /// REQUIRES IMPL
        ///     
        ///     Add states to the machine with calls to AddState<>()
        ///     
        ///     When all states have been added notify the machine
        ///         which state to start in with SetInitialState<>()
        /// </summary>
        public abstract void AddStates();

        public virtual void Start()
        {
            Initialize();
        }

        public virtual void Initialize()
        {
            AddStates();

            currentState = InitialState;
            if (null == currentState)
            {
                ThrowException(".nextState", "Initialize()", "");
                throw new Exception("\n" + name + ".nextState is null on Initialize()!\tDid you forget to call SetInitialState()?\n");
            }

            foreach (KeyValuePair<Type, State> pair in states)
            {
                pair.Value.Initialize();
            }

            OnStateEnter = true;
            OnStateExit = false;
        }

        public virtual void Update()
        {
            if (OnStateExit)
            {
                currentState.Exit();
                currentState = NextState;
                NextState = null;

                OnStateEnter = true;
                OnStateExit = false;
            }

            if (OnStateEnter)
            {
                currentState.Enter();

                OnStateEnter = false;
            }

            try
            {
                currentState.Update();
            }
            catch (NullReferenceException e)
            {
                if (null == InitialState)
                {
                    throw new Exception("\n" + name + ".currentState is null when calling Update()!\tDid you set initial state?\n" + e.Message);
                }
                else
                {
                    throw new Exception("\n" + name + ".currentState is null when calling Update()!\tDid you change state to a valid state?\n" + e.Message);
                }
            }
        }

        public virtual void FixedUpdate()
        {
            if (!(OnStateEnter && OnStateExit))
            {
                try
                {
                    currentState.FixedUpdate();
                }
                catch (NullReferenceException e)
                {
                    if (null == InitialState)
                    {
                        throw new Exception("\n" + name + ".currentState is null when calling FixedUpdate()!\tDid you set initial state?\n" + e.Message);
                    }
                    else
                    {
                        throw new Exception("\n" + name + ".currentState is null when calling FixedUpdate()!\tDid you change state to a valid state?\n" + e.Message);
                    }
                }
            }
        }

        public virtual void LateUpdate()
        {
            if (!(OnStateEnter && OnStateExit))
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

        public virtual void OnCollisionEnter2D(Collision2D collision)
        {
            currentState.OnCollisionEnter2D(collision);
        }

        public virtual void OnCollisionStay2D(Collision2D collision)
        {
            currentState.OnCollisionStay2D(collision);
        }

        public virtual void OnCollisionExit2D(Collision2D collision)
        {
            currentState.OnCollisionExit2D(collision);
        }

        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            currentState.OnTriggerEnter2D(collider);
        }

        public virtual void OnTriggerStay2D(Collider2D collider)
        {
            currentState.OnTriggerStay2D(collider);
        }

        public virtual void OnTriggerExit2D(Collider2D collider)
        {
            currentState.OnTriggerExit2D(collider);
        }

        public virtual void OnCollisionEnter(Collision collision)
        {
            currentState.OnCollisionEnter(collision);
        }

        public virtual void OnCollisionStay(Collision collision)
        {
            currentState.OnCollisionStay(collision);
        }

        public virtual void OnCollisionExit(Collision collision)
        {
            currentState.OnCollisionExit(collision);
        }

        public virtual void OnTriggerEnter(Collider collider)
        {
            currentState.OnTriggerEnter(collider);
        }

        public virtual void OnTriggerStay(Collider collider)
        {
            currentState.OnTriggerStay(collider);
        }

        public virtual void OnTriggerExit(Collider collider)
        {
            currentState.OnTriggerExit(collider);
        }

        public void OnAnimatorIK(int layerIndex)
        {
            if (!(OnStateEnter && OnStateExit))
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

            OnStateExit = true;
        }

        public bool IsCurrentState<T>() where T : State
        {
            return (currentState.GetType() == typeof(T)) ? true : false;
        }

        public bool IsCurrentState(Type T)
        {
            return (currentState.GetType() == T) ? true : false;
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

        protected bool OnStateEnter
        {
            get;
            set;
        }

        protected bool OnStateExit
        {
            get;
            set;
        }

        protected Dictionary<Type, State> states = new Dictionary<Type, State>();

        void ThrowException(string method, string loop, string e)
        {
            throw new Exception("\n" + name + method + " is null when calling " + loop + "!\tDid you set initial state?\n" + e);
        }
    }
}