using UnityEngine;

public class CommonEnemyController : IAController
{
    [Header("Common Enemy Params")]
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackDamage;
    [SerializeField] protected float _attackRadius;
    [SerializeField] private float _minPlayerDistance;
    public float tiempoEntreCombos = 5.0f;
    
    protected override void Start()
    {
        base.Start();
        myAnimator = GetComponent<Animator>();
    }

    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null) return;

        Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
    }

    public override void EnemyBasicMovement()
    {
        base.EnemyBasicMovement();

        if (!hasDetected) return;
        if (DistanceToPlayer() <= _minPlayerDistance) return;

        Vector3 direction = (_player.position - transform.position).normalized;
        myVelocity.x = velocidadMovimiento * direction.x;
        myRB.velocity = myVelocity; 
    }

    #region Attack

    public void Attack()
    {
        StandStill();
        myAnimator.SetInteger("Combo", Random.Range(0, 3));
        myAnimator.SetTrigger("Attack");
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
    #endregion
}