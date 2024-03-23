using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackComponent : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _movingForceUp;
    [SerializeField] private float _movingForceDown;

    [Header("Score Parameters")]
    [SerializeField] private int _hitScore;
    [SerializeField] private float _scoreHitMultiplier;

    private PlayerCombat _playerCombat;

    public PlayerAttackTypes AttackType;

    private void Awake()
    {
        _playerCombat = GetComponentInParent<PlayerCombat>();
    }

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

            // Add Score
            int scoreToAdd = Mathf.RoundToInt(_hitScore + ((_playerCombat.CurrComboLength - 1) * _scoreHitMultiplier));
            GameController.Instance.AddScore(scoreToAdd);
        }
    }

    public float Damage { get { return _damage; } set { _damage = value; } }
}