using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(SpriteMaskActivator))]
public class Humanoid : MonoBehaviour
{
    [Header("Start parameters")]
    [SerializeField] private float _hp = 100f;
    [SerializeField] private float _armor = 10f;

    public void GetDamage(float damage)
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
