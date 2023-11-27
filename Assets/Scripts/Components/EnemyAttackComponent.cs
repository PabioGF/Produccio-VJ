using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackComponent : MonoBehaviour
{
    [SerializeField] private float _damage;
    //Preguntar si aixo esta be
    [SerializeField] private LifeComponent.AttackTypes _attackType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerLifeComponent>(out var life))
        {
            life.ReceiveHit(_damage, _attackType);
        }
    }
    private void SetAttackType(LifeComponent.AttackTypes attackType)
    {
        _attackType = attackType;
    }
}
