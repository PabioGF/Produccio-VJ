using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_Idle : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<PlayerCombat>().DisableAttackAreas();
    }
}
