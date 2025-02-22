using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyAttackState : StateMachineBehaviour
{
    private RangeEnemyController _enemyController;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _enemyController = animator.GetComponent<RangeEnemyController>();
        _enemyController.IsAttacking = true;
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _enemyController.StandStill();
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _enemyController.IsAttacking = false;
    }
}