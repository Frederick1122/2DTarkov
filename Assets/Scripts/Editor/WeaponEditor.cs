using ConfigScripts;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponConfig))]
public class WeaponEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Script:", target, typeof(MonoScript), false);        
        EditorGUI.EndDisabledGroup();
        
        var data = (WeaponConfig) target;

        data.topSprite = (Sprite)EditorGUILayout.ObjectField("Top Sprite", data.topSprite, typeof(Sprite), true);
        data.isMelee = EditorGUILayout.Toggle("Is Melee", data.isMelee);
        data.rateOfFire = EditorGUILayout.FloatField("Rate Of Fire", data.rateOfFire);

        if (!data.isMelee)
        {
            data.isSecondWeapon = EditorGUILayout.Toggle("Is Second Weapon", data.isSecondWeapon);
            data.bulletDispersion = EditorGUILayout.FloatField("Bullet Dispersion", data.bulletDispersion);
            data.maxFiringDistance = EditorGUILayout.FloatField("Max Firing Distance", data.maxFiringDistance);
            data.noNeedAmmo = EditorGUILayout.Toggle("No Need Ammo", data.noNeedAmmo);
            
            if (!data.noNeedAmmo)
            {
                data.bulletConfig = (BulletConfig)EditorGUILayout.ObjectField("Bullet", data.bulletConfig, typeof(BulletConfig), false);
                data.maxAmmoInMagazine = EditorGUILayout.IntField("Max Ammo In Magazine", data.maxAmmoInMagazine);
            }
        }

        GUILayout.Space(5);
        GUILayout.Label("Base Item Settings");

        data.configKey = EditorGUILayout.TextField("Config Key", data.configKey);

        GUILayout.Space(5);

        data.icon = (Sprite)EditorGUILayout.ObjectField("Icon", data.icon, typeof(Sprite), true);
        data.dropIcon = (Sprite)EditorGUILayout.ObjectField("Drop Icon", data.dropIcon, typeof(Sprite), true);

        data.itemName = EditorGUILayout.TextField("Item Name", data.itemName);
        data.description = EditorGUILayout.TextField("Item Description", data.description);
        
        data.weight = EditorGUILayout.FloatField("Weight", data.weight);
        
        var maxStack = EditorGUILayout.IntField("Max Stack", data.maxStack);
        var baseStack = EditorGUILayout.IntField("Base Stack", data.baseStack);

        if (maxStack > baseStack)
            baseStack = maxStack;

        data.maxStack = maxStack;
        data.baseStack = baseStack;
        
        data.configPath = EditorGUILayout.TextField("Сonfig Path", data.configPath);

        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(data);
        }
    }
}