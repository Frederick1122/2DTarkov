using UnityEngine;

namespace InteractObjects
{
    public class SaveInChunkTag : MonoBehaviour
    {
        private int _index;

        public void SetIndex(int index) => _index = index;
        
        public int GetIndex() => _index;
    }
}