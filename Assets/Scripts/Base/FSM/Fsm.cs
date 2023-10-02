using System;
using System.Collections.Generic;

[Serializable]
public abstract class Fsm
{
   protected FsmState _currentState;
   protected Dictionary<Type, FsmState> _states = new Dictionary<Type, FsmState>();

   public void AddState<T>(FsmState state) where T : FsmState
   {
      _states.Add(typeof(T), state);
   }

   public void SetState<T>() where T : FsmState
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
