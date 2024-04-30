using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_MovingAttack : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<PlayerController>().IsOverride = false;
    }
}
