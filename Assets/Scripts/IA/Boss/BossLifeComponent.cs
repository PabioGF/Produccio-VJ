using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLifeComponent : LifeComponent
{
    [SerializeField] private HealthBarComponent _healthBar;
    [SerializeField] private BossController _controller;
    public override void ReceiveHit(float amount)
    {
        _controller.ReceiveHit();
        base.ReceiveHit(amount);

        _healthBar.UpdateHealthBar(_currentLife, _maxLife);

        if (_currentLife < _maxLife / 2) _parent.GetComponent<BossController>().EnterSecondPhase();

        if (_isDead) _parent.GetComponent<BossController>().Die();
    }

}
