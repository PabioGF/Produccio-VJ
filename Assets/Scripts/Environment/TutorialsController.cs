using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialsController : MonoBehaviour
{
    [SerializeField] private float _transitionTime;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
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