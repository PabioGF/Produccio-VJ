using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyWalkState : StateMachineBehaviour
{
    private RangeEnemyController _enemyController;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _enemyController = animator.GetComponent<RangeEnemyController>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Move();

        if (_enemyController.DistanceToPlayer() < _enemyController.AttackRange)
        {
            _enemyController.Attack();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    private void Move()
    {
        _enemyController.EnemyBasicMovement();
    }
}
