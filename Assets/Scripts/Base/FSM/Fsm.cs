using System;
using System.Collections.Generic;

public abstract class Fsm
{
   private FsmState<Fsm> _currentState;
   private Dictionary<Type, FsmState<Fsm>> _states = new Dictionary<Type, FsmState<Fsm>>();

   public void AddState(FsmState<Fsm> state)
   {
      _states.Add(state.GetType(), state);
   }

   public void SetState<T>() where T : FsmState<Fsm>
   {
      var type = typeof(T);
      
      if(_currentState.GetType() == type)
         return;

      if (_states.TryGetValue(type, out var newState))
      {
         _currentState?.Exit();
         _currentState = newState;
         _currentState.Enter();
      }
   }

   public void Update()
   {
      _currentState?.Update();
   }
}
