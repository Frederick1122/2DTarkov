﻿using System;
using System.Collections.Generic;
using Base;
using Player.InputSystem;
using UI;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
    public class PlayerInteractZone : MonoBehaviour
    {
        private List<IInteract> _objectWithInteractions = new List<IInteract>();
        private PlayerHumanoid _playerHumanoid;
        private BaseUIWindowController _baseUIWindowController;

        private IInputSystem _inputSystem;
        private bool _isNewChange;
        
        private void Start()
        {
            _playerHumanoid = GameBus.Instance.PlayerHumanoid;
            _inputSystem = GameBus.Instance.PlayerInputSystem;
            _inputSystem.OnInteractAction += TryInteract;
            _baseUIWindowController = UIManager.Instance.GetWindow<BaseUIWindowController>();
        }

        private void OnDestroy()
        {
            if (_inputSystem != null)
            {
                _inputSystem.OnInteractAction -= TryInteract;
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var obj = col.GetComponent<IInteract>();
            if (obj != null)
                _objectWithInteractions.Add(obj);

            _isNewChange = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var obj = other.GetComponent<IInteract>();
            if (obj != null)
                _objectWithInteractions.Remove(obj);

            _isNewChange = true;
        }

        private void Update()
        {
            if(_isNewChange == false)
                return;

            _isNewChange = false;
            var canInteract = _objectWithInteractions.Count > 0 && !_playerHumanoid.isFreeze;
            
            _baseUIWindowController.SetActiveInteractButton(canInteract);
            
            if(canInteract != true)
                return;
            
            _baseUIWindowController.UpdateInteractButton(_objectWithInteractions[^1]);
        }

        private void TryInteract()
        {
            if (_objectWithInteractions.Count == 0)
                return;
            
            _objectWithInteractions[^1].Interact();
        }
    }