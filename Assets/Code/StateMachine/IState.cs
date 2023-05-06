using System;

namespace Code.StateMachine
{
    public interface IState<TStates>
    where TStates : Enum
    {
        TStates Type { get; }
        
        void OnEnter();
        void OnExit();
        void Loop();
    }
}