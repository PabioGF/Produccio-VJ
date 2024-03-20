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
    public float tiempoEntreCombos = 5.0f;
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
        //Debug.Log(myRB.velocity);
    }

    public void Attack()
    {
        
        StandStill();
        myAnimator.SetInteger("Combo", Random.Range(0, 2));
        myAnimator.SetTrigger("attack");
    }
    private void PerformAttack(int type)
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(_attackPoint.position, _attackRadius, LayerMask.GetMask("Player"));

        if (playerCollider != null)
        {
            playerCollider.GetComponent<PlayerLifeComponent>().ReceiveHit(_attackDamage);
            Debug.Log(playerCollider.name + " has been hit");
        }
    }

    private void CambiaAtaque()
    {
        int num = Random.Range(0, 2);
        if (num == 1)
        {
            myAnimator.SetTrigger("cambiaAtaque");
        }
    }
}
