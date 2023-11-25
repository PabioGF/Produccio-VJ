using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackComponent : MonoBehaviour
{
    [SerializeField] private float _damage;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<LifeComponent>(out var life))
        {
            life.ReceiveHit(_damage);
        }       
    }
}
