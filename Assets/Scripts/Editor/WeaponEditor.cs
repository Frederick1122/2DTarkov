using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Script:", target, typeof(MonoScript), false);        
        EditorGUI.EndDisabledGroup();
        
        var data = (Weapon) target;

        data.topSprite = (Sprite)EditorGUILayout.ObjectField("Top Sprite", data.topSprite, typeof(Sprite), true);

        if (!data.isSecondWeapon)
            data.isKnife = EditorGUILayout.Toggle("Is Knife", data.isKnife);
        
        if (!data.isKnife)
            data.isSecondWeapon = EditorGUILayout.Toggle("Is Second Weapon", data.isSecondWeapon);

        data.rateOfFire = EditorGUILayout.FloatField("Rate Of Fire", data.rateOfFire);
        data.bulletDispersion = EditorGUILayout.FloatField("Bullet Dispersion", data.bulletDispersion);
        data.maxFiringDistance = EditorGUILayout.FloatField("Max Firing Distance", data.maxFiringDistance);

        data.noNeedAmmo = EditorGUILayout.Toggle("No Need Ammo", data.noNeedAmmo);

        if (!data.noNeedAmmo)
        {
            data.bullet = (Bullet)EditorGUILayout.ObjectField("Bullet", data.bullet, typeof(Bullet), false);
            data.maxAmmoInMagazine = EditorGUILayout.IntField("Max Ammo In Magazine", data.maxAmmoInMagazine);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(data);
        }
    }
}