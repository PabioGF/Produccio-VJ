using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private bool _hasEntered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasEntered || !collision.CompareTag("Player")) return;

        _hasEntered = true;

        UIController.Instance.GameEnded();
    }
}
