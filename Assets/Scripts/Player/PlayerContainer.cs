using UnityEngine;

public class PlayerContainer : MonoBehaviour
{
    [SerializeField] private PlayerHumanoid _playerHumanoid;

    public PlayerHumanoid GetPlayer() => _playerHumanoid;
}
