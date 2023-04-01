using UnityEngine;

namespace Base
{
    public class Singleton<T> : MonoBehaviour where T : new()
    {
        public static T Instance
        {
            get
            {
                if (Instance == null) 
                    Instance = new T();
            
                return Instance;
            }
            private set => Instance = value;
        }

    }
}