using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EnemyLifeComponent : LifeComponent
{
    #region Variables
    private IAController _controller;
    #endregion

    #region Unity methods
    protected override void Start()
    {
        base.Start();
        _controller = GetComponent<IAController>();
    }
    #endregion
    public override void ReceiveHit(float amount)
    {
        _controller.GetHit();
        base.ReceiveHit(amount);
    }

    public override void SendFlyingUp(float force)
    {
        _parent.GetComponent<Rigidbody2D>().AddForce(new(0, force), ForceMode2D.Impulse);
    }

    public override void SendFlyingDown(float force)
    {
        _parent.GetComponent<Rigidbody2D>().AddForce(new(0, -force), ForceMode2D.Impulse);
    }

    public override void SendFlyingOutwards(float force)
    {
        _parent.GetComponent<Rigidbody2D>().AddForce(new(force, Mathf.Abs(force) / 2), ForceMode2D.Impulse);
    }
}