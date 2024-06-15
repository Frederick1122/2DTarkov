using System;
using System.Collections.Generic;
using UnityEngine;

namespace Base.FSM
{
     public class FsmHandler : MonoBehaviour
     {
          [SerializeField] private List<FsmCase> _fsmCases = new List<FsmCase>();

          private void Start()
          {
               foreach (var fsmCase in _fsmCases)
               {
                    fsmCase.Fsm.Init();
               }
          }

          private void Update()
          {
               foreach (var fsmCase in _fsmCases)
               {
                    fsmCase.Fsm.Update();
               }
          }
     }

     [Serializable]
     public class FsmCase
     {
          [SerializeReference, SubclassPicker] public Fsm Fsm;
     }
}