using UnityEngine;

namespace Base
{
    public class Singleton<T> : MonoBehaviour where T : new()
    {
        private static T _instance;

        public static T GetInstance()
        {
            if(_instance == null)
                _instance = new T();
            
            return _instance;
        }
    }
}