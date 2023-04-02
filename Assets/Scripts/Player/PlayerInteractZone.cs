using System;
using System.Collections.Generic;
using Base;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
    public class PlayerInteractZone : MonoBehaviour
    {
        [SerializeField] private Button _interactButton;

        private List<ObjectWithInteraction> _objectWithInteractions = new List<ObjectWithInteraction>();
        private Player _player;

        private void Start()
        {
            _player = GameManager.Instance.GetPlayer();
            _interactButton.onClick.AddListener(() =>
            {
                _objectWithInteractions[^1].Interact();
            });
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var obj = col.GetComponent<ObjectWithInteraction>();
            if (obj != null)
                _objectWithInteractions.Add(obj);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var obj = other.GetComponent<ObjectWithInteraction>();
            if (obj != null)
                _objectWithInteractions.Remove(obj);
        }

        private void Update()
        {
            var canInteract = _objectWithInteractions.Count > 0 && !_player.isFreeze;
            if (_interactButton.gameObject.activeSelf != canInteract)
            {
                _interactButton.gameObject.SetActive(canInteract);
            }
        }
        
        
    }