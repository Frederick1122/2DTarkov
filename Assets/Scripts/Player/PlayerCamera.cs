using System;
using UnityEditor;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
  [SerializeField] private GameObject _player;
  [SerializeField] private bool _isUseOffset;
  [SerializeField] private GameObject _offsetPoint;
  
  private void OnValidate()
  {
      if (_isUseOffset && _offsetPoint == null)
      {
          _offsetPoint = new GameObject();
          _offsetPoint.transform.position = transform.position;
          _offsetPoint.transform.parent = _player.transform;
          _offsetPoint.name = "CameraFollowerOffsetPoint";
          #if UNITY_EDITOR
          EditorUtility.SetDirty(this);
          #endif
      }
      else if( !_isUseOffset && _offsetPoint != null )
      {
          DestroyImmediate(_offsetPoint);
          #if UNITY_EDITOR
          EditorUtility.SetDirty(this);
          #endif
      }
  }

  private void LateUpdate()
  {
      if(_player == null)
          return;

      var target = new Vector3();
      if (_isUseOffset)
      {
          target = _offsetPoint.transform.position;
      }
      else
          target = _player.transform.position;
      
      transform.position = new Vector3(target.x, target.y, transform.position.z);
      transform.localRotation = _player.transform.localRotation;
  }
}
