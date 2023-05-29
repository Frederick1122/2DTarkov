using System.IO;
using UnityEngine;

namespace Base
{
    public class SaveLoadManager<T, T2> : Singleton<T2> where T2 : MonoBehaviour
    {
        [SerializeField] internal string _secondPath = "";
        [SerializeField] protected string _path = "";
        
        internal T _saveData;

        protected void Load()
        {
            UpdatePath();
            _saveData = JsonUtility.FromJson<T>(File.ReadAllText(_path));
        }

        protected void Save()
        {
            UpdatePath();
            File.WriteAllText(_path, JsonUtility.ToJson(_saveData));
        }

        protected virtual void UpdatePath()
        {
            if (_path == "")
                _path = Application.streamingAssetsPath + _secondPath;
        }
    }
}