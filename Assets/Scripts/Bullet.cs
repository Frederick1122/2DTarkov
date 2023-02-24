using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Bullet : MonoBehaviour
{
   [SerializeField] private float _speed;
   [SerializeField] private int _damage;

   private float _lifetime = 10f;
   private Rigidbody2D _rigidbody2D;
   private Vector3 _vectorMovement = new Vector3();

   private void OnValidate()
   {
      UpdateFields();
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if(other.GetComponent<Weapon>() || other.GetComponent<ShootArea>())
         return;
      
      if (other.GetComponent<Enemy>())
      {
         //do something
      }
      
      
      Debug.Log($"Bullet has been destroyed: {other.gameObject.name}");
      Destroy(gameObject);
   }

   private void Start()
   {
      UpdateFields();
      _vectorMovement = transform.TransformVector(0, 1, transform.position.z);
      StartCoroutine(LifeTime());
   }

   private void FixedUpdate()
   {
      _rigidbody2D.velocity = _vectorMovement * _speed * Time.fixedDeltaTime;
   }

   private IEnumerator LifeTime()
   {
      yield return new WaitForSeconds(_lifetime);
      Destroy(gameObject);
   }
   
   private void UpdateFields()
   {
      if (_rigidbody2D == null || _rigidbody2D == default)
         _rigidbody2D = GetComponent<Rigidbody2D>();
   }
}
