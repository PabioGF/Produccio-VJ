using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonEnemyAirborneState : StateMachineBehaviour
{
    private CommonEnemyController _enemyController;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _enemyController = animator.GetComponent<CommonEnemyController>();
        _enemyController.IsFlying = true;
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _enemyController.StandStill();
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _enemyController.IsFlying = false;
    }
}
