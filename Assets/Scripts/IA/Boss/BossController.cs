using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _meleeAttackCd;
    [SerializeField] private float _rangedAttackCd;

    private Animator _animator;
    private bool _isFlipped;
    private bool _canAttack;
    private bool _canShoot;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _canAttack = true;
        _canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LookAtPlayer()
    {
        if (transform.position.x > _player.position.x && !_isFlipped)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            _isFlipped = true;
        }
        else if (transform.position.x < _player.position.x && _isFlipped)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            _isFlipped = false;
        }
    }

    public void Attack()
    {
        _animator.SetTrigger("Attack");
        _canAttack = false;
        Invoke(nameof(ResetMeleeAttack), _meleeAttackCd);
    }

    public void Shoot()
    {
        _animator.SetTrigger("Shoot");
        _canShoot = false;
        Invoke(nameof(ResetRangedAttack), _rangedAttackCd);
    }

    private void ResetMeleeAttack()
    {
        _canAttack = true;
    }

    private void ResetRangedAttack()
    {
        _canShoot = true;
    }

    public bool CanAttack { get { return _canAttack; } }
    public bool CanShoot { get { return _canShoot; } }
}