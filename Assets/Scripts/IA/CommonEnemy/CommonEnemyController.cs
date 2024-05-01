using UnityEngine;

public class CommonEnemyController : IAController
{
    [Header("Common Enemy Params")]
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackDamage;
    [SerializeField] protected float _attackRadius;
    [SerializeField] private float _minPlayerDistance;
    [SerializeField] private AudioClip _moveSound;

    private AudioSource _audioSource;

    public float tiempoEntreCombos = 5.0f;
    
    protected override void Start()
    {
        base.Start();
        myAnimator = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
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
        if (DistanceToPlayer() <= _minPlayerDistance)
        {
            myVelocity.x = 0;
            myRB.velocity = myVelocity;
            return;
        }

        Vector3 direction = (_player.position - transform.position).normalized;
        myVelocity.x = velocidadMovimiento * direction.x;
        myRB.velocity = myVelocity;

        if (_moveSound != null && !_audioSource.isPlaying && Mathf.Abs(myVelocity.x) > 0.1f)
        {
            _audioSource.PlayOneShot(_moveSound);
        }
    }

    #region Attack

    public void Attack()
    {
        StandStill();
        myAnimator.SetInteger("Combo", Random.Range(0, 5));
        myAnimator.SetTrigger("Attack");
    }

    private void PerformAttack(int type)
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(_attackPoint.position, _attackRadius, LayerMask.GetMask("PlayerHitbox"));

        if (playerCollider != null)
        {
            playerCollider.GetComponent<PlayerLifeComponent>().ReceiveHit(_attackDamage, transform.position);
        }
    }
    #endregion
}