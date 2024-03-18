using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyScript : SceneInventoryItem
{
    [SerializeField] private int _id;
    //[SerializeField] private AudioClip _disappearSound;

    //private AudioSource _audioSource;

    /*private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }*/

    protected override void PickUp()
    {
        _playerController.AddItem(new Key(_id));
        Destroy(gameObject);
    }

    /*private void OnDestroy()
    {
        // Reproducir el sonido cuando el objeto se destruye
        if (_disappearSound != null)
        {
            _audioSource.PlayOneShot(_disappearSound);
        }
    }*/
}