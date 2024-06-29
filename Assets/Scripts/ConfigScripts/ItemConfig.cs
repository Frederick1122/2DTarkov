using System;
using UnityEngine;

namespace ConfigScripts
{
    
    public class ItemConfig : BaseConfig
    {
        protected string _directory = "";

        public Sprite icon;
        public Sprite dropIcon;
        [Space] public string itemName;
        public string description;
        [Space] public float weight = 1f;
        [Range(0, 100)] public int maxStack = 1;
        [Range(0, 100)] public int baseStack = 1;
        public string configPath = "";

        internal virtual void OnValidate()
        {
            configPath = $"Prototypes/{_directory}" + this.name;

            if (maxStack > baseStack)
                baseStack = maxStack;
        }
    }
}