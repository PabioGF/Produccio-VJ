using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingGround : MonoBehaviour
{
    [SerializeField] private GameObject _ground;
    [SerializeField] private GameObject _eventTrigger;
    private bool _isPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            _isPlayer = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            _isPlayer = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_isPlayer)
        {
            if (!_eventTrigger.activeSelf)
            {
                _ground.SetActive(false);
            }
        }
    }
}