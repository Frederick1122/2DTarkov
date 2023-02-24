using UnityEngine;

public class CameraFollower : MonoBehaviour
{
  [SerializeField] private Transform _player;

  private void LateUpdate()
  {
      if(_player == null)
          return;
      
      transform.position = new Vector3(_player.position.x, _player.position.y, transform.position.z);
      transform.localRotation = _player.localRotation;
  }
}
