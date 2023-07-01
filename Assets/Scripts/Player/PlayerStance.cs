using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerStance : MonoBehaviour
{
    [SerializeField] private Sprite _firstWeaponSprite;
    [SerializeField] private Sprite _secondWeaponSprite;
    [SerializeField] private Sprite _emptyWeaponSprite;

    [Space] [SerializeField] private SpriteRenderer _spriteRenderer;

    private void OnValidate()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnDisable()
    {
        Equipment.Instance.OnEquipmentChanged -= SetStance;
    }

    private void Start()
    {
        Equipment.Instance.OnEquipmentChanged += SetStance; 
        SetStance(Equipment.Instance.GetEquipment());
    }

    private void SetStance(EquipmentData equipmentData)
    {
        if (!equipmentData.isSecondWeapon)
        {
            var weapon = (Weapon) equipmentData.GetEquipment(EquipmentType.firstWeapon);
            if (weapon != null)
            {
                _spriteRenderer.sprite = _firstWeaponSprite;
                return;
            }
        }
        else
        {
            var weapon = (Weapon) equipmentData.GetEquipment(EquipmentType.secondWeapon);
            if (weapon != null)
            {
                _spriteRenderer.sprite = _secondWeaponSprite;
                return;
            }
        }

        _spriteRenderer.sprite = _emptyWeaponSprite;
    }
}
