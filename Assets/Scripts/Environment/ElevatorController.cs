using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : InteractableObject
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip _doorOpenSound;
    [SerializeField] private float _doorOpenVolume = 1.0f;

    private bool _hasInteracted = false;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>(); 
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    protected override void Interact()
    {
        base.Interact();
        if (_hasInteracted) return;

        _hasInteracted = true;
        Debug.Log("ELEVATOR");

        _animator.SetTrigger("Open");
        PlayDoorOpenSound();
        Invoke(nameof(CompleteLevel), 0.3f);
    }

    private void PlayDoorOpenSound()
    {
        if (_doorOpenSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(_doorOpenSound, _doorOpenVolume);
        }
    }

    private void CompleteLevel()
    {
        UIController.Instance.LevelCompleted();

        LevelProgressController.Instance.LevelIndex += 1;
        LevelProgressController.Instance.IsCompleteScreen = true;
        LevelProgressController.Instance.HasSpawnPoint = false;
    }
}
