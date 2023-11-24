using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackComponent : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private bool _upAttack;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerLifeComponent>(out var life))
        {
            life.ReceiveHit(_damage, _upAttack);
        }
    }
    private void SetAttackType(bool upAttack)
    {
        _upAttack = upAttack;
    }
}
