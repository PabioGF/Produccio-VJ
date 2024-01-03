using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLifeComponent : LifeComponent
{
    public override void ReceiveHit(float amount, AttackTypes attackType = AttackTypes.DefaultAttack)
    {
        base.ReceiveHit(amount, attackType);

        if (_isDead) _parent.GetComponent<BossController>().Die();
    }
}
