using System;
using System.Collections.Generic;
using UnityEngine;

public class FsmHandler : MonoBehaviour
{
     [SerializeField] private List<FsmCase> _fsms = new List<FsmCase>();
}

 [Serializable]
 public class FsmCase
 {
      [SerializeReference, SubclassPicker] public Fsm Fsm;
 }