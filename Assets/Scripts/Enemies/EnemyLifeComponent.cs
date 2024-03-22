using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EnemyLifeComponent : LifeComponent
{
    #region Global Variables
    [SerializeField]
    private int _scoreAddWhenDead;

    private IAController _controller;
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    [Header("Audio")]
    [SerializeField] private AudioClip _punchSound;

    private AudioSource _audioSource;
    #endregion

    #region Unity methods
    protected override void Start()
    {
        base.Start();
        _controller = GetComponentInParent<IAController>();
        _rigidbody = GetComponentInParent<Rigidbody2D>();
        _animator = GetComponentInParent<Animator>();

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    #endregion
    public override void ReceiveHit(float amount)
    {
        _controller.GetHit();
        base.ReceiveHit(amount);

        if (_punchSound != null)
        {
            _audioSource.PlayOneShot(_punchSound);
        }

        if (_isDead)
        {
            GameController.Instance.AddScore(_scoreAddWhenDead);
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