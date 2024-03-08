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
    private void Start()
    {
        base.Start();
        myAnimator = GetComponent<Animator>();
    }

    public override void EnemyBasicMovement()
    {
        base.EnemyBasicMovement();

        if (!hasDetected) return;
        if (DistanceToPlayer() <= minPlayerDistance) return;

        Vector3 direction = (_player.position - transform.position).normalized;
        myVelocity.x = velocidadMovimiento * direction.x;
        myRB.velocity = myVelocity;
    }

    public void Attack()
    {
        StandStill();
        myAnimator.SetTrigger("attack");
    }
    private void PerformAttack(int type)
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(_attackPoint.position, _attackRadius, LayerMask.GetMask("Player"));

        if (playerCollider != null)
        {
            PlayerLifeComponent.AttackTypes attackType = (PlayerLifeComponent.AttackTypes)type;
            playerCollider.GetComponent<PlayerLifeComponent>().ReceiveHit(_attackDamage, attackType);
            Debug.Log(playerCollider.name + " has been hit");
        }
    }
}
