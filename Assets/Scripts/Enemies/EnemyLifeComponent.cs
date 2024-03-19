using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EnemyLifeComponent : LifeComponent
{
    #region Variables
    private IAController _controller;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    #endregion

    #region Unity methods
    protected override void Start()
    {
        base.Start();
        _controller = GetComponentInParent<IAController>();
        _rigidbody = GetComponentInParent<Rigidbody2D>();
        _animator = GetComponentInParent<Animator>();
    }
    #endregion
    public override void ReceiveHit(float amount)
    {
        _controller.GetHit();
        base.ReceiveHit(amount);

        if (_isDead)
        {
            _parent.gameObject.SetActive(false);
        }
    }

    public override void SendFlyingUp(float force)
    {
        if (_controller.IsAttacking || _controller.IsFlying) return;

        _rigidbody.AddForce(new(0, force), ForceMode2D.Impulse);
        _animator.SetTrigger("GoFlying");
    }

    public override void SendFlyingDown(float force)
    {
        if (_controller.IsAttacking || !_controller.IsFlying) return;
        _parent.GetComponent<Rigidbody2D>().AddForce(new(0, -force), ForceMode2D.Impulse);
    }

    public override void SendFlyingOutwards(float force)
    {
        if (_controller.IsAttacking || _controller.IsFlying) return;
        _parent.GetComponent<Rigidbody2D>().AddForce(new(force, Mathf.Abs(force) / 2), ForceMode2D.Impulse);
        _animator.SetTrigger("GoFlying");
    }
}