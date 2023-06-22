using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerStance : MonoBehaviour
{
    [SerializeField] private Sprite _firstWeaponSprite;
    [SerializeField] private Sprite _secondWeaponSprite;

    [Space] [SerializeField] private SpriteRenderer _spriteRenderer;

    private void OnValidate()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Equipment.Instance.OnEquipmentChanged += SetStance; 
    }

    private void OnDisable()
    {
        Equipment.Instance.OnEquipmentChanged -= SetStance;
    }

    private void Start()
    {
        SetStance(Equipment.Instance.GetEquipment());
    }

    private void SetStance(EquipmentData equipmentData)
    {
        _spriteRenderer.sprite = equipmentData.isSecondWeapon ? _secondWeaponSprite : _firstWeaponSprite;
    }
}
