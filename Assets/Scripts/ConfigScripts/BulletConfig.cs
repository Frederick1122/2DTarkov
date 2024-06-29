using System;
using UnityEngine;

namespace ConfigScripts
{
    [Serializable]
    [CreateAssetMenu(fileName = "NewBullet", menuName = "Item/Bullet")]
    public class BulletConfig : ItemConfig
    {
        [Space] public float speed;
        public float damage;
        public string bulletPrefabPath = "";

        internal override void OnValidate()
        {
            if (bulletPrefabPath == "")
                Debug.Log($"Incorrect bulletPrefabPath! Check {this.name}");

            if (_directory == "")
                _directory = "Bullets/";

            base.OnValidate();
        }
    }
}