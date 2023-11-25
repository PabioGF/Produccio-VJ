using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackComponent : LifeComponent
{
    [SerializeField] private float _damage;
    private AttackTypes _attackType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerLifeComponent>(out var life))
        {
            life.ReceiveHit(_damage, _attackType);
        }
    }
    private void SetAttackType(AttackTypes attackType)
    {
        _attackType = attackType;
    }
}
