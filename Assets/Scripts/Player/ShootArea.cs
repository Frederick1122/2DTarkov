using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ShootArea : MonoBehaviour
{
    public event Action<GameObject> onEnter;
    public event Action<GameObject> onExit;
    
    [SerializeField] private BoxCollider2D _boxCollider2D;

    private void OnValidate()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"{other.gameObject.name} enter");
        if(other.GetComponent<Enemy>())
            onEnter?.Invoke(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"{other.gameObject.name} exit");
        if(other.GetComponent<Enemy>())
            onExit?.Invoke(other.gameObject);
    }

    public void SetDistance(float distance, Vector3 startPosition)
    {
        transform.position = startPosition;
        transform.localScale = new Vector3(distance, transform.localScale.y);
        transform.localPosition += new Vector3(distance / 2, 0);
    }
}
