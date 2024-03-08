using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInventoryItem : MonoBehaviour
{
    protected PlayerController _playerController;

    #region Unity Methods
    protected virtual void Awake()
    {
    }

    private void OnDisable()
    {
    }
    #endregion

    protected virtual void PickUp()
    {
        _playerController.DesiredInteraction = false;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out var component))
        {
            _playerController = component;
        }        
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        _playerController = null;
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (_playerController != null && _playerController.DesiredInteraction)
        {
            PickUp();
        }
    }
}
