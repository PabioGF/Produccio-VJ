using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class EnemyAttackComponent : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private PlayerLifeComponent.AttackTypes _attackType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerLifeComponent>(out var life))
        {
            life.ReceiveHit(_damage, _attackType);
        }
    }
    private void SetAttackType(PlayerLifeComponent.AttackTypes attackType)
    {
        _attackType = attackType;
    }
}
