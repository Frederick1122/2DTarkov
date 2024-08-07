﻿using System;
using System.Collections.Generic;
using Zenject;

namespace Base.FSM
{
    public class FsmManager : Singleton<FsmManager>
    {
        [Inject] private DiContainer _diContainer;
        
        private Dictionary<Type, GlobalFsm> _currentFsms = new ();
        private GlobalFsm _activeFsm;
        
        protected override void Awake()
        {
            base.Awake();
            foreach (var currentFsm in _currentFsms)
                currentFsm.Value.Init();
        }

        public void SetActiveFsm<T>() where T : GlobalFsm, new()
        {
            if (!_currentFsms.ContainsKey(typeof(T)))
            {
                var newFsm = _diContainer.Instantiate(typeof(T)) as T;
                newFsm.Init();
                _currentFsms.Add(typeof(T), newFsm);
            }
            
            _activeFsm?.Reset();
            _activeFsm = _currentFsms[typeof(T)];
            _activeFsm.SetStartState();
        }

        public Fsm TryGetFsm<T>() where T : GlobalFsm, new()
        {
            _currentFsms.TryGetValue(typeof(T), out var fsm);
            return fsm ?? AddNewFsm<T>();
        }

        private Fsm AddNewFsm<T>() where T : GlobalFsm, new()
        {
            if (_currentFsms.ContainsKey(typeof(T)))
                return _currentFsms[typeof(T)];

            var newFsm = new T();
            newFsm.Init();
            _currentFsms.Add(typeof(T), newFsm);
            return newFsm;
        }

        public void AddNewFsm(IFsm newFsm)
        {
            if (_currentFsms.ContainsKey(newFsm.GetType()))
                return;

            newFsm.Init();
            _currentFsms.Add(newFsm.GetType(), (GlobalFsm)newFsm);
        }

        public void RemoveFsm<T>() where T : GlobalFsm
        {
            _currentFsms.Remove(typeof(T));
        }

        public void RemoveFsm(IFsm fsm)
        {
            _currentFsms.Remove(fsm.GetType());
        }
    }
}