using System;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
  [SerializeField] private GameObject _player;
  [SerializeField] private bool _isUseOffset;

  private GameObject _offsetPoint;

  private void Start()
  {
      if (_isUseOffset)
      {
          _offsetPoint = new GameObject();
          _offsetPoint.transform.position = transform.position;
          _offsetPoint.transform.parent = _player.transform;
          _offsetPoint.name = "CameraFollowerOffsetPoint";
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
