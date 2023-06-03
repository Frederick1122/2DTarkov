using System.IO;
using UnityEngine;

namespace Base
{
    public abstract class SaveLoadManager<T, T2> : Singleton<T2> where T2 : MonoBehaviour
    {
        [SerializeField] protected string _secondPath = "";
        [SerializeField] protected string _path = "";
        
        protected T _saveData;

        protected virtual void Start() => Load();

        protected virtual void OnApplicationQuit() => Save();
        
        protected virtual void OnApplicationPause(bool pauseStatus)
        {
            if(pauseStatus)
                Save();
        }
        
        protected virtual void Load()
        {
            UpdatePath();
            _saveData = DataSaver.LoadData<T>(_secondPath);
        }

        protected void Save()
        {
            UpdatePath();
            DataSaver.SaveData(_saveData, _secondPath);
        }

        protected virtual void UpdatePath()
        {
            if (_path == "")
                _path = Application.streamingAssetsPath + _secondPath;
        }
    }
}