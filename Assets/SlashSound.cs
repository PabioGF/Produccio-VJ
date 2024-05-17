using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    public void PlaySound()
    {
        _audioSource.Play();
    }
}
