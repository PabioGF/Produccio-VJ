using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Global Variables
    [SerializeField] private Transform _player;
    [SerializeField] private Rigidbody2D _playerRb;

    [Header("Camera Settings")]

    [SerializeField] private float _damping;

    [Range (0f, 3f)]
    [SerializeField] private float _velocitySmoother;

    [SerializeField] private float _lookAheadMinDistance;

    [SerializeField] private float _lookAheadMaxDistance;

    private PlayerInputActions _playerInputActions;
    private Vector3 _velocity;
    private float _lookAheadDesiredDistance;
    #endregion

    #region Unity Methods
    private void Start()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _velocity = Vector3.zero;
        if (_player == null )
        {
            Debug.LogError("[CameraController] La referència a Jugador és null");
        }
    }

    void LateUpdate()
    {
        HandleLookahead();
        FollowPlayer();
    }
    #endregion

    /// <summary>
    /// Follows the player position smoothly
    /// </summary>
    private void FollowPlayer()
    {
        //Gets the desired position
        Vector3 desiredPosition = _player.position;
        desiredPosition.z = transform.position.z;
        desiredPosition.x += _lookAheadDesiredDistance;

        //Lerps to the desired position and applies it to the camera transform
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _damping * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    /// <summary>
    /// Calculates the LookAhead offset
    /// </summary>
    private void HandleLookahead()
    {
        float playerVelocity = Mathf.Abs(_playerRb.velocity.x) * _velocitySmoother;
        float lookAheadOffset = _playerInputActions.Player.Move.ReadValue<float>() != 0 ? Mathf.Min(playerVelocity, _lookAheadMaxDistance) : Mathf.Max(playerVelocity, _lookAheadMinDistance);

        //Set its value to minimum 1 in case it manages to get below in certain situations
        if (lookAheadOffset < 1) lookAheadOffset = 1;
        _lookAheadDesiredDistance = _player.right.x == 1 ? lookAheadOffset : -lookAheadOffset;
    }
}
