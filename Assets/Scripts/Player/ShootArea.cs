using System;
using UnityEngine;

public class ShootArea : MonoBehaviour
{
    public event Action<GameObject> onEnter;
    public event Action<GameObject> onExit;
    
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
}
