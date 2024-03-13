using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialsController : MonoBehaviour
{
    [SerializeField] private GameObject _keyboardMessage;
    [SerializeField] private GameObject _controllerMessage;
    [SerializeField] private float _transitionTime;
    [SerializeField] private bool _adaptToDevice;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_adaptToDevice)
            {
                switch (PlayerInputsManager.Instance.InputDevice)
                {
                    case PlayerInputsManager.InputDevices.Keyboard:
                        _keyboardMessage.SetActive(true);
                        _controllerMessage.SetActive(false);
                        break;
                    case PlayerInputsManager.InputDevices.Controller:
                        _keyboardMessage.SetActive(false);
                        _controllerMessage.SetActive(true);
                        break;
                }
            }

            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > _transitionTime)
            {
                _animator.Play("MessageFadeIn");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > _transitionTime)
            {
                _animator.Play("MessageFadeOut");
            }
        }
    }
}