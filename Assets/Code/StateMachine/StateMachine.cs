using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.StateMachine
{
    public class StateMachine<TStates> : MonoBehaviour
        where TStates : Enum
    {
        [SerializeField] private TStates firstState;
        [SerializeField] private List<TStates> noExitStates;

        [field: SerializeField] private TStates currentStateType;
        private List<IState<TStates>> _states;
        private IState<TStates> _currentState;

        public TStates CurrentState
        {
            get => _currentState.Type;

            set
            { if (NoExitStateCheck())
                  return;
              if (!ValidateState(value))
                  return;
              _currentState?.OnExit();
              _currentState = _states.First(s => CompareGenericEnums(value, s.Type));
              _currentState.OnEnter();
              currentStateType = _currentState.Type; }
        }

        private void Awake()
        {
            _states = gameObject.GetComponents<IState<TStates>>().ToList();

            CurrentState = firstState;
        }

        public bool CurrentStateIs(TStates state)
        {
            if (_currentState == null)
                return false;

            return CompareGenericEnums(CurrentState, state);
        }

        private bool ValidateState(TStates state)
        {
            if (_currentState == null)
                return true;

            return !CompareGenericEnums(_currentState.Type, state);
        }

        private bool NoExitStateCheck()
        {
            if (_currentState == null)
                return false;

            return noExitStates.Contains(_currentState.Type);
        }

        private bool CompareGenericEnums(TStates enum1, TStates enum2)
        {
            return EqualityComparer<TStates>.Default.Equals(enum1, enum2);
        }

        private void Update()
        {
            _currentState?.Loop();
        }
    }
}