using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Walk : StateMachineBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _meleeAttackRange;
    [SerializeField] private float _shootRange;
    [SerializeField] private float _minPlayerDistance;

    private Transform _player;
    private Rigidbody2D _bossRb;
    private BossController _bossController;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _bossRb = animator.GetComponent<Rigidbody2D>();
        _bossController = animator.GetComponent<BossController>();
        _bossController.DisableJumpHitbox();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Move();

        if ((_bossRb.position.x < _bossController.leftPositionLimit || _bossRb.position.x > _bossController.rightPositionLimit) && _bossController.CanJump)
        {
            animator.SetTrigger("Jump");
        }

        if (Vector2.Distance(_player.position, _bossRb.position) <= _meleeAttackRange && _bossController.CanAttack)
        {
            _bossController.BeginCombo();
        }

        if (Vector2.Distance(_player.position, _bossRb.position) >= _shootRange && _bossController.CanShoot)
        {
            _bossController.StartShooting();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Shoot");
        animator.ResetTrigger("Jump");
    }

    private void Move()
    {
        _bossController.LookAtPlayer();

        if (Vector2.Distance(_player.position, _bossRb.position) < _minPlayerDistance) return;

        Vector2 target = new(_player.position.x, _bossRb.position.y);
        Vector2 newPosition = Vector2.MoveTowards(_bossRb.position, target, _speed * Time.fixedDeltaTime);

        _bossRb.MovePosition(newPosition);
    }
}