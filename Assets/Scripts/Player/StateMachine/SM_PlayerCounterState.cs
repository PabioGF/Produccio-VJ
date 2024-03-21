using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_PlayerCounterState : StateMachineBehaviour
{
    private PlayerCombat _playerCombat;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerCombat = animator.GetComponent<PlayerCombat>();

        _playerCombat.IsParrying = true;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerCombat.IsParrying = false;
        _playerCombat.Hitbox.enabled = true;
    }
}