using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackComponent : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _movingForceUp;
    [SerializeField] private float _movingForceDown;

    public PlayerAttackTypes AttackType;

    public enum PlayerAttackTypes
    {
        defaultAttack = 1,
        upwardsForceAttack = 2, 
        downwardsForceAttack = 3,
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<LifeComponent>(out var life))
        {
            life.ReceiveHit(_damage);
            if (AttackType == PlayerAttackTypes.upwardsForceAttack)
            {
                life.SendFlyingUp(_movingForceUp);
            }
            if (AttackType == PlayerAttackTypes.downwardsForceAttack)
            {
                life.SendFlyingDown(_movingForceDown);
            }
        }
    }
}
