using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _player;

    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _damping;

    private Vector3 _velocity;

    private void Start()
    {
        _velocity = Vector3.zero;
        if (_player == null )
        {
            Debug.LogError("[CameraController] La referència a Jugador és null");
        }
    }

    void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 desiredPosition = _player.position + _offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, _damping);
        transform.position = smoothedPosition;
    }
}
