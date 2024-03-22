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

    [Header("Audio")]
    [SerializeField] private AudioClip _impactSound;
    [SerializeField] private AudioClip _punchSound;
    [SerializeField][Range(0f, 1f)] private float _impactSoundVolume = 1f;

    private AudioSource _audioSource;

    private PlayerCombat _playerCombat;

    public PlayerAttackTypes AttackType;

    private void Awake()
    {
        _playerCombat = GetComponentInParent<PlayerCombat>();
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
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

            if (_impactSound != null)
            {
                _audioSource.volume = _impactSoundVolume;
                _audioSource.PlayOneShot(_impactSound);
            }
        }
        else {
            _audioSource.volume = _impactSoundVolume;
            _audioSource.PlayOneShot(_punchSound);
        }
    }

    public float Damage { get { return _damage; } set { _damage = value; } }
}