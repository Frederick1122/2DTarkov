using System;
using UnityEngine;

public class ShootArea : MonoBehaviour
{
    public Action onEnter;
    public Action onExit;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"{other.gameObject.name} enter");
        if(other.GetComponent<Enemy>())
            onEnter?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"{other.gameObject.name} exit");
        if(other.GetComponent<Enemy>())
            onExit?.Invoke();
    }
}
