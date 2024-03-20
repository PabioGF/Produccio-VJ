using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : InteractableObject
{
    #region Global Variables
    [SerializeField] private GameObject _linkedObject;
    [SerializeField] private LinkedObjectType _objectType;
    [SerializeField] private AudioClip _toggleSound;

    private Animator _animator;
    private bool _toggled;
    private AudioSource _audioSource;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>(); // Inicializar el componente AudioSource
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    private enum LinkedObjectType
    {
        turret = 0,
        ground = 1
    } 

    #endregion

    protected override void Interact()
    {
        base.Interact();
        _animator.SetTrigger("Toggle");

        if (_toggleSound != null)
        {
            _audioSource.PlayOneShot(_toggleSound);
        }

        switch (_objectType)
        { 
            case LinkedObjectType.turret:
                _linkedObject.GetComponent<TurretScript>().DisarmTurret();
                break;
            case LinkedObjectType.ground:
                _linkedObject.GetComponent<Animator>().SetTrigger("Move");
                break;
        }

        _playerController.CanInteract(false);
        _playerController = null;
        _toggled = true;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (_toggled) return;

        if (collision.TryGetComponent(out PlayerController component))
        {
            _playerController = component;
            _playerController.CanInteract(true);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (_toggled) return;

        if (!collision.CompareTag("Player")) return;

        _playerController.CanInteract(false);
        _playerController = null;
    }
}
