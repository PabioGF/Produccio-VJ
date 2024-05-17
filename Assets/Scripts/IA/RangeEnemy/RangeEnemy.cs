using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyController : IAController
{
    [Header("Range Enemy Params")]
    [SerializeField] private float minPlayerDistance;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackDamage;
    [SerializeField] protected float _attackRadius;
    [SerializeField] private AudioClip _attackSound;

    public float tiempoEntreCombos = 5.0f;

    private AudioSource _audioSource;

    private void Start()
    {
        base.Start();
        myAnimator = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public override void EnemyBasicMovement()
    {
        base.EnemyBasicMovement();

        if (!hasDetected) return;
        if (DistanceToPlayer() <= minPlayerDistance)
        {
            myVelocity.x = 0;
            myRB.velocity = myVelocity;
            return;
        };

        Vector3 direction = (_player.position - transform.position).normalized;
        myVelocity.x = velocidadMovimiento * direction.x;
        myRB.velocity = myVelocity;
    }

    public void Attack()
    {
        StandStill();
        myAnimator.SetInteger("Combo", Random.Range(0, 4));
        myAnimator.SetTrigger("attack");

        PlayAttackSound();
    }

    private void PerformAttack(int type)
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(_attackPoint.position, _attackRadius, LayerMask.GetMask("PlayerHitbox"));

        if (playerCollider != null)
        {
            playerCollider.GetComponent<PlayerLifeComponent>().ReceiveHit(_attackDamage, transform.position);
            Debug.Log(playerCollider.name + " has been hit");
        }
    }

    private void PlayAttackSound()
    {
        if (_attackSound != null)
        {
            _audioSource.PlayOneShot(_attackSound);
        }
    }
}
