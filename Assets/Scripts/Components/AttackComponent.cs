using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    [SerializeField] private float _damage;
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        LifeComponent life = collision.GetComponent<LifeComponent>();
        if (life != null)
        {
            life.ApplyDamage(_damage);
        }       
    }
}
