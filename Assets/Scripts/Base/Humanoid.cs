using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(SpriteMaskActivator))]
public class Humanoid : MonoBehaviour
{
    [Header("Start parameters")]
    [SerializeField] private int _hp = 100;
    [SerializeField] private int _armor = 10;

    public void GetDamage(int damage)
    {
        if (_armor != 0) 
            _armor = _armor - damage < 0 ? 0 : _armor - damage;
        else
            _hp = _hp - damage < 0 ? 0 : _hp - damage;

        if (_hp == 0) 
            Dying();
    }

    internal virtual void Dying()
    {
        Destroy(gameObject);
    }
}
