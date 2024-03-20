using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : InteractableObject
{
    [SerializeField] private Animator _animator;
    private bool _hasInteracted = false;

    protected override void Interact()
    {
        base.Interact();
        if (_hasInteracted) return;

        _hasInteracted = true;
        Debug.Log("ELEVATOR");

        _animator.SetTrigger("Open");
        Invoke(nameof(CompleteLevel), 0.3f);
    }

    private void CompleteLevel()
    {
        UIController.Instance.LevelCompleted();

        LevelProgressController.Instance.LevelIndex += 1;
        LevelProgressController.Instance.IsCompleteScreen = true;
        LevelProgressController.Instance.HasSpawnPoint = false;
    }
}
