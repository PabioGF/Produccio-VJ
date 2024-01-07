using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLifeComponent : LifeComponent
{
    public override void ReceiveHit(float amount)
    {
        base.ReceiveHit(amount);

        if (_currentLife < _maxLife / 2) _parent.GetComponent<BossController>().EnterSecondPhase();

        if (_isDead) _parent.GetComponent<BossController>().Die();
    }
}
