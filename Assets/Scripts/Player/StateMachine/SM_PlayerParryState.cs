using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_PlayerParryState : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerCombat playerCombat = animator.GetComponent<PlayerCombat>();

        playerCombat.IsParrying = false;
        playerCombat.Deflect = false;
    }
}
